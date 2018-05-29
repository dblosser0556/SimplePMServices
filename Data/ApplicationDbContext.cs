using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SimplePMServices.Models.Entities;
using SimplePMServices.ViewModels;

namespace SimplePMServices.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Query<ProjectMonthlyProjection>().ToTable("V_ProjectMonthlyProjections");
            builder.Query<ProjectMilestone>().ToTable("V_ProjectMilestones");
        }


        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        public DbSet<Code> Codes { get; set; }
        public DbSet<CodeType> CodeTypes { get; set; }

        public DbSet<FixedPrice> FixedPrices { get; set; }
        public DbSet<FixedPriceMonth> FixedPriceMonths { get; set; }
        public DbSet<FixedPriceType> FixedPriceTypes { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupBudget> GroupBudgets { get; set; }

        public DbSet<Month> Months { get; set; }

        public DbSet<Phase> Phases { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceMonth> ResourceMonths { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }

        public DbSet<Role> ProjectRoles { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorInvoice> Invoices { get; set; }
        public DbSet<VendorPeriod> VendorPeriods { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }

        public DbQuery<ProjectMonthlyProjection> ProjectMonthlyProjections { get; set; }
        public DbQuery<ProjectMilestone> ProjectMilestones { get; set; }






    }
}
