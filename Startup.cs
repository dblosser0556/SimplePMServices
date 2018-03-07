using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using SimplePMServices.Data;
using SimplePMServices.Models.Entities;
using SimplePMServices.Models;
using SimplePMServices.Helpers;
using SimplePMServices.Extensions;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimplePMServices.Auth;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNet.OData.Extensions;


namespace SimplePMServices
{
    public class Startup
    {


        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                 b => b.MigrationsAssembly("SimplePMServices")));

            services.AddSingleton<IJwtFactory, JwtFactory>();

            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            var builder = services.AddIdentityCore<AppUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);

            builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
                options.AddPolicy("Admin", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.Admin));
                options.AddPolicy("EditProgram", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.EditPrograms));
                options.AddPolicy("EditProjects", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.EditProjects));

            });
            
            services.AddTransient<SimplePMModelBuilder>();
            services.AddTransient<SimplePMDBContextSeedData>();
            services.AddOData();
            services.AddCors();
            services.AddAutoMapper();
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SimplePMDBContextSeedData seedData)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            
            app.UseExceptionHandler(
            builder =>
            {
              builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                context.Response.AddApplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            }
                        });
            });
            app.UseCors(
                options => options.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseDefaultFiles();
            // app.UseMvc();
            app.UseMvc(async builder => {

                builder
                    .Count()
                    .Expand()
                    .Filter()
                    .OrderBy()
                    .Select()
                    ;

                await seedData.SeedAminUser();

                var edmModel = app.ApplicationServices.GetRequiredService<SimplePMModelBuilder>().GetEdmModel();
                builder.MapODataServiceRoute("ODataRoute", null, edmModel);
                builder.EnableDependencyInjection();
                //builder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());
                builder.MapRoute("default", "api/{controller}/{action}");
            });

        }
    }
}
