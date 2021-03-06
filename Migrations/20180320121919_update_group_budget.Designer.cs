﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SimplePMServices.Data;
using SimplePMServices.Models.Entities;
using System;

namespace SimplePMServices.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180320121919_update_group_budget")]
    partial class update_group_budget
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Budget", b =>
                {
                    b.Property<int>("BudgetId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<DateTime>("ApprovedDateTime");

                    b.Property<int>("BudgetType");

                    b.Property<int?>("GroupId");

                    b.Property<int>("ProjectId");

                    b.HasKey("BudgetId");

                    b.HasIndex("GroupId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Code", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CodeDesc");

                    b.Property<string>("CodeName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("CodeTypeID");

                    b.HasKey("ID");

                    b.ToTable("Codes");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.CodeType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TypeDesc");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("CodeTypes");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.FixedPrice", b =>
                {
                    b.Property<int>("FixedPriceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FixedPriceName");

                    b.Property<int?>("FixedPriceTypeId");

                    b.Property<int>("ProjectId");

                    b.Property<int?>("ResourceTypeId");

                    b.Property<double>("TotalActualCost");

                    b.Property<double>("TotalPlannedCost");

                    b.Property<string>("Vendor");

                    b.HasKey("FixedPriceId");

                    b.HasIndex("ProjectId");

                    b.ToTable("FixedPrices");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.FixedPriceMonth", b =>
                {
                    b.Property<int>("FixedPriceMonthId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("ActualCost");

                    b.Property<double>("ActualCostCapPercent");

                    b.Property<int?>("ActualCostStyle");

                    b.Property<int>("FixedPriceId");

                    b.Property<int>("MonthNo");

                    b.Property<double>("PlannedCost");

                    b.Property<double>("PlannedCostCapPercent");

                    b.Property<int?>("PlannedCostStyle");

                    b.HasKey("FixedPriceMonthId");

                    b.HasIndex("FixedPriceId");

                    b.ToTable("FixedPriceMonths");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.FixedPriceType", b =>
                {
                    b.Property<int>("FixedPriceTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FixedPriceTypeDesc");

                    b.Property<string>("FixedPriceTypeName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("FixedPriceTypeId");

                    b.ToTable("FixedPriceTypes");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GroupDesc");

                    b.Property<string>("GroupManager");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Level");

                    b.Property<string>("LevelDesc");

                    b.Property<int?>("LevelId");

                    b.Property<int?>("Lft");

                    b.Property<int?>("ParentId");

                    b.Property<int?>("Rgt");

                    b.HasKey("GroupId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.GroupBudget", b =>
                {
                    b.Property<int>("GroupBudgetId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<DateTime>("ApprovedDateTime");

                    b.Property<int>("BudgetType");

                    b.Property<int>("BudgetYear");

                    b.Property<int>("GroupId");

                    b.HasKey("GroupBudgetId");

                    b.ToTable("GroupBudgets");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Month", b =>
                {
                    b.Property<int>("MonthId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MonthNo");

                    b.Property<int>("PhaseId");

                    b.Property<int>("ProjectId");

                    b.Property<double>("TotalActualCapital");

                    b.Property<double>("TotalActualExpense");

                    b.Property<double>("TotalPlannedCapital");

                    b.Property<double>("TotalPlannedExpense");

                    b.HasKey("MonthId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Months");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Phase", b =>
                {
                    b.Property<int>("PhaseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PhaseDesc");

                    b.Property<string>("PhaseName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("PhaseId");

                    b.ToTable("Phases");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ActualStartDate");

                    b.Property<int>("GroupId");

                    b.Property<bool>("IsTemplate");

                    b.Property<DateTime>("PlannedStartDate");

                    b.Property<string>("ProjectDesc");

                    b.Property<string>("ProjectManager");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("StatusId");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Resource", b =>
                {
                    b.Property<int>("ResourceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProjectId");

                    b.Property<double>("Rate");

                    b.Property<string>("ResourceName")
                        .IsRequired();

                    b.Property<int>("ResourceTypeId");

                    b.Property<int>("RoleId");

                    b.Property<double>("TotalActualEffort");

                    b.Property<double>("TotalPlannedEffort");

                    b.Property<string>("Vendor");

                    b.HasKey("ResourceId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RoleId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.ResourceMonth", b =>
                {
                    b.Property<int>("ResourceMonthId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("ActualEffort");

                    b.Property<double>("ActualEffortCapPercent");

                    b.Property<int?>("ActualEffortStyle");

                    b.Property<int>("MonthNo");

                    b.Property<double>("PlannedEffort");

                    b.Property<double>("PlannedEffortCapPercent");

                    b.Property<int?>("PlannedEffortStyle");

                    b.Property<int>("ResourceId");

                    b.HasKey("ResourceMonthId");

                    b.HasIndex("ResourceId");

                    b.ToTable("ResourceMonths");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.ResourceType", b =>
                {
                    b.Property<int>("ResourceTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ResourceTypeDesc");

                    b.Property<string>("ResourceTypeName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ResourceTypeId");

                    b.ToTable("ResourceTypes");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RoleDesc");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("RoleId");

                    b.ToTable("ProjectRoles");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Status", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StatusDesc");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("StatusId");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.AppUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.ToTable("AppUser");

                    b.HasDiscriminator().HasValue("AppUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Budget", b =>
                {
                    b.HasOne("SimplePMServices.Models.Entities.Group")
                        .WithMany("GroupBudgets")
                        .HasForeignKey("GroupId");

                    b.HasOne("SimplePMServices.Models.Entities.Project")
                        .WithMany("Budgets")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.FixedPrice", b =>
                {
                    b.HasOne("SimplePMServices.Models.Entities.Project")
                        .WithMany("FixedPriceCosts")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.FixedPriceMonth", b =>
                {
                    b.HasOne("SimplePMServices.Models.Entities.FixedPrice")
                        .WithMany("FixedPriceMonths")
                        .HasForeignKey("FixedPriceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Month", b =>
                {
                    b.HasOne("SimplePMServices.Models.Entities.Project")
                        .WithMany("Months")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.Resource", b =>
                {
                    b.HasOne("SimplePMServices.Models.Entities.Project")
                        .WithMany("Resources")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimplePMServices.Models.Entities.Role")
                        .WithMany("Resources")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimplePMServices.Models.Entities.ResourceMonth", b =>
                {
                    b.HasOne("SimplePMServices.Models.Entities.Resource")
                        .WithMany("ResourceMonths")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
