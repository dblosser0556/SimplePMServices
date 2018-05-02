using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePMServices.Data;
using SimplePMServices.Models.Entities;
using SimplePMServices.ViewModels;
using Microsoft.AspNet.OData;
using System.Diagnostics;

namespace SimplePMServices.Controllers
{
    [Produces("application/json")]
    [Route("api/Projects")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [EnableQuery]
        [HttpGet]
        public IQueryable<ProjectList> GetProjects()
        {



            List<ProjectList> projects = new List<ProjectList>();

            var query = from p in _context.Projects
                        join s in _context.Status on p.StatusId equals s.StatusId
                        join g in _context.Groups on p.GroupId equals g.GroupId
                        join u in _context.AppUsers on p.ProjectManager equals u.Id

                        select new
                        {
                            ProjectId = p.ProjectId,
                            IsTemplate = p.IsTemplate,
                            ProjectName = p.ProjectName,
                            ProjectDesc = p.ProjectDesc,
                            ProjectManagerName = u.LastName + ", " + u.FirstName,
                            ProjectManager = p.ProjectManager,
                            PlannedStartDate = p.PlannedStartDate.ToString("yyyy-MM-dd"),
                            PlannedStartYear = p.PlannedStartDate.Year.ToString(),
                            ActualStartDate = p.ActualStartDate.HasValue ? p.ActualStartDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                            ActualStartYear = p.ActualStartDate.HasValue ? p.ActualStartDate.Value.Year.ToString() : string.Empty,
                            FilterYear = p.ActualStartDate.HasValue ? p.ActualStartDate.Value.Year.ToString() : p.PlannedStartDate.Year.ToString(),
                            GroupId = g.GroupId,
                            GroupName = g.GroupName,
                            GroupManager = g.GroupManager,
                            StatusId = s.StatusId,
                            StatusName = s.StatusName


                        };


            foreach (var item in query)
            {

                ICollection<Month> months = _context.Projects.Where(p => p.ProjectId == item.ProjectId)
                         .SelectMany(p => p.Months).ToList();


                var totalPlannedExpense = months.Sum(m => m.TotalPlannedExpense);
                var totalActualExpense = months.Sum(m => m.TotalActualExpense);
                var totalPlannedCapital = months.Sum(m => m.TotalPlannedCapital);
                var totalActualCapital = months.Sum(m => m.TotalActualCapital);


                ICollection<Budget> budgets = _context.Projects.Where(p => p.ProjectId == item.ProjectId)
                    .SelectMany(p => p.Budgets).ToList();

                var totalCapitalBudget = budgets.Where(b => b.BudgetType == BudgetType.Capital)
                    .Sum(b => b.Amount);
                var totalExpenseBudget = budgets.Where(b => b.BudgetType == BudgetType.Expense)
                    .Sum(b => b.Amount);


                var project = new ProjectList
                {
                    ProjectId = item.ProjectId,
                    IsTemplate = item.IsTemplate,
                    ProjectName = item.ProjectName,
                    ProjectDesc = item.ProjectDesc,
                    ProjectManager = item.ProjectManager,
                    ProjectManagerName = item.ProjectManagerName,
                    PlannedStartDate = item.PlannedStartDate,
                    PlannedStartYear = item.PlannedStartYear,
                    ActualStartDate = item.ActualStartDate,
                    ActualStartYear = item.ActualStartYear,
                    FilterYear = item.FilterYear,
                    GroupId = item.GroupId,
                    StatusId = item.StatusId,
                    GroupName = item.GroupName,
                    GroupManager = item.GroupManager,
                    StatusName = item.StatusName,
                    TotalPlannedExpense = totalPlannedExpense,
                    TotalActualExpense = totalActualExpense,
                    TotalActualCapital = totalActualCapital,
                    TotalPlannedCapital = totalPlannedCapital,
                    TotalCapitalBudget = totalCapitalBudget,
                    TotalExpenseBudget = totalExpenseBudget
                };
                projects.Add(project);
            }

            return projects.AsQueryable();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {


                var project = await _context.Projects
                                            .Include(p => p.Budgets)
                                            .Include(p => p.Vendors)
                                                .ThenInclude(v => v.Invoices)
                                            .Include(p => p.Resources)
                                                .ThenInclude(pr => pr.ResourceMonths)
                                            .Include(p => p.Months)
                                            .Include(p => p.FixedPriceCosts)
                                                .ThenInclude(fp => fp.FixedPriceMonths)
                                            .Include(p => p.Milestones)
                                            .Where(p => p.ProjectId == id)
                                               .ToListAsync();
                // the default sorting order is the id 
                // but we need the items to be sorted by month no so the item line up
                // in the grid.
                foreach (var p in project)
                {
                    p.Months = p.Months.OrderBy(m => m.MonthNo).ToList();
                    p.Budgets = p.Budgets.OrderBy(b => b.ApprovedDateTime).ToList();
                    foreach (var resource in p.Resources)
                    {
                        resource.ResourceMonths = resource.ResourceMonths.OrderBy(rm => rm.MonthNo).ToList();
                    }
                    foreach (var f in p.FixedPriceCosts)
                    {
                        f.FixedPriceMonths = f.FixedPriceMonths.OrderBy(fm => fm.MonthNo).ToList();
                    }

                }



                if (project == null)
                {
                    return NotFound();
                }



                return Ok(project);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return BadRequest(e.Message);
            }

        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject([FromRoute] int id, [FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectId)
            {
                return BadRequest();
            }
            return await UpdateProject(id, project);
        }

        private async Task<IActionResult> UpdateProject(int id, Project project)
        {

            var existingProject = _context.Projects.Include(p => p.Resources)
                                            .ThenInclude(pr => pr.ResourceMonths)
                                         .Include(p => p.Vendors)
                                            .ThenInclude(v => v.Invoices)
                                        .Include(p => p.Months)
                                        .Include(p => p.FixedPriceCosts)
                                            .ThenInclude(fp => fp.FixedPriceMonths)
                                        .Include(p => p.Budgets)
                                        .Include(p => p.Milestones)
                                        .Where(p => p.ProjectId == id)
                                        .SingleOrDefault();
            if (existingProject != null)
            {
                try
                {
                    _context.Entry(existingProject).CurrentValues.SetValues(project);

                    //Find Deleted Resources
                    if (existingProject.Resources != null)
                    {
                        foreach (var existingResource in existingProject.Resources.ToList())
                        {
                            if (!project.Resources.Any(r => r.ResourceId == existingResource.ResourceId))
                                _context.Resources.Remove(existingResource);
                        }
                    }
                    //Update and Insert resources
                    foreach (var resource in project.Resources)
                    {
                        //check to see if the resource already exists.
                        //new resources have a resourceId of -1
                        var existingResource = existingProject.Resources
                            .Where(r => r.ResourceId == resource.ResourceId && resource.ResourceId > 0)
                            .SingleOrDefault();
                        if (existingResource != null)
                        {
                            //update resource
                            _context.Entry(existingResource).CurrentValues.SetValues(resource);

                            // remove any months that no longer exist.
                            foreach (var existingMonth in existingResource.ResourceMonths.ToList())
                            {
                                if (!resource.ResourceMonths.Any(r => r.ResourceMonthId == existingMonth.ResourceMonthId))
                                    _context.ResourceMonths.Remove(existingMonth);
                            }

                            // update existing resource months
                            foreach (var resourceMonth in resource.ResourceMonths)
                            {
                                var existingResourceMonth = existingResource.ResourceMonths
                                    .Where(r => r.ResourceMonthId == resourceMonth.ResourceMonthId && resourceMonth.ResourceMonthId > 0)
                                    .SingleOrDefault();

                                //work through the set of resource months and update or add.
                                if (existingResourceMonth != null)
                                    _context.Entry(existingResourceMonth).CurrentValues.SetValues(resourceMonth);
                                else
                                {
                                    var newMonth = GetResourceMonth(resourceMonth);

                                    //ensure there are months for the existing resource
                                    if (existingResource.ResourceMonths == null)
                                        existingResource.ResourceMonths = new List<ResourceMonth>();
                                    existingResource.ResourceMonths.Add(newMonth);
                                }
                            }
                        }
                        else
                        {
                            // add resource
                            var newResource = new Resource
                            {
                                Rate = resource.Rate,
                                ResourceName = resource.ResourceName,
                                RoleId = resource.RoleId,
                                ResourceTypeId = resource.ResourceTypeId,
                                Vendor = resource.Vendor,
                                TotalActualEffort = resource.TotalActualEffort,
                                TotalPlannedEffort = resource.TotalPlannedEffort,


                            };

                            //add the resource months
                            newResource.ResourceMonths = new List<ResourceMonth>();
                            foreach (var month in resource.ResourceMonths)
                            {
                                var newMonth = GetResourceMonth(month);
                                newResource.ResourceMonths.Add(newMonth);
                            }
                            existingProject.Resources.Add(newResource);

                        }
                    }

                    //find deleted vendors
                    foreach (var existingVendor in existingProject.Vendors.ToList())
                    {

                        if (!project.Vendors.Any(v => v.VendorId == existingVendor.VendorId))
                            _context.Vendors.Remove(existingVendor);

                    }

                    // Update and Insert vendors
                    foreach (var vendor in project.Vendors)
                    {
                        //check to see if the Vendor already exists.
                        //new vendors have a vendorId of -1
                        var existingVendor = existingProject.Vendors
                            .Where(r => r.VendorId == vendor.VendorId && vendor.VendorId > 0)
                            .SingleOrDefault();
                        if (existingVendor != null)
                        {
                            //update vendor
                            _context.Entry(existingVendor).CurrentValues.SetValues(vendor);

                            // update existing vendor invoices
                            foreach (var vendorInvoice in vendor.Invoices)
                            {
                                var existingInvoice = existingVendor.Invoices
                                    .Where(r => r.VendorInvoiceId == vendorInvoice.VendorInvoiceId)
                                    .SingleOrDefault();

                                //work through the set of vendor invoices and update or add.
                                if (existingInvoice != null)
                                    _context.Entry(existingInvoice).CurrentValues.SetValues(vendorInvoice);
                                else
                                {
                                    var newMonth = GetInvoice(vendorInvoice);

                                    //ensure there are invoices for the existing vendor
                                    if (existingVendor.Invoices == null)
                                        existingVendor.Invoices = new List<VendorInvoice>();
                                    existingVendor.Invoices.Add(newMonth);
                                }
                            }
                        }
                        else
                        {
                            // add vendor
                            var newVendor = new Vendor
                            {
                                Contact = vendor.Contact,
                                ContactEmail = vendor.ContactEmail,
                                ContactPhone = vendor.ContactPhone,
                                ContractAmount = vendor.ContractAmount,
                                ContractEndDate = vendor.ContractEndDate,
                                ContractIdentifier = vendor.ContractIdentifier,
                                ContractTerms = vendor.ContractTerms,
                                VendorName = vendor.VendorName
                            };

                            //add the vendor invoices
                            newVendor.Invoices = new List<VendorInvoice>();
                            foreach (var invoice in vendor.Invoices)
                            {
                                var newInvoice = GetInvoice(invoice);
                                newVendor.Invoices.Add(newInvoice);
                            }
                            existingProject.Vendors.Add(newVendor);

                        }
                    }



                    //Find Deleted Budgets
                    foreach (var existingBudget in existingProject.Budgets.ToList())
                    {
                        if (!project.Budgets.Any(r => r.BudgetId == existingBudget.BudgetId))
                            _context.Budgets.Remove(existingBudget);
                    }

                    //Update and insert Budgets
                    foreach (var budget in project.Budgets)
                    {
                        // update existing budget entries  
                        // new budgets have an id of -1
                        var existingBudget = existingProject.Budgets
                            .Where(r => r.BudgetId == budget.BudgetId && budget.BudgetId > 0)
                            .SingleOrDefault();
                        if (existingBudget != null)
                        {
                            //update resource
                            _context.Entry(existingBudget).CurrentValues.SetValues(budget);

                        }
                        else
                        {
                            // add budget
                            var newBudget = new Budget
                            {
                                Amount = budget.Amount,
                                ApprovedDateTime = budget.ApprovedDateTime,
                                BudgetType = budget.BudgetType
                            };
                            existingProject.Budgets.Add(newBudget);

                        }
                    }


                    //Find Deleted Months
                    foreach (var existingMonth in existingProject.Months.ToList())
                    {
                        if (!project.Months.Any(r => r.MonthId == existingMonth.MonthId))
                            _context.Months.Remove(existingMonth);
                    }

                    //Update and Insert months
                    foreach (var month in project.Months)
                    {
                        var existingMonth = existingProject.Months
                            .Where(r => r.MonthId == month.MonthId && month.MonthId > 0)
                            .SingleOrDefault();
                        if (existingMonth != null)
                        {
                            //update resource
                            _context.Entry(existingMonth).CurrentValues.SetValues(month);


                        }
                        else
                        {
                            // add resource
                            var newMonth = new Month
                            {

                                MonthNo = month.MonthNo,
                                PhaseId = month.PhaseId,
                                TotalActualCapital = month.TotalActualCapital,
                                TotalActualExpense = month.TotalActualExpense,


                            };
                            existingProject.Months.Add(newMonth);

                        }
                    }

                    //Find Deleted fixed costs
                    foreach (var existingCost in existingProject.FixedPriceCosts.ToList())
                    {
                        if (!project.FixedPriceCosts.Any(r => r.FixedPriceId == existingCost.FixedPriceId))
                            _context.FixedPrices.Remove(existingCost);
                    }

                    //Update and Insert months
                    foreach (var cost in project.FixedPriceCosts)
                    {
                        var existingCost = existingProject.FixedPriceCosts
                            .Where(f => f.FixedPriceId == cost.FixedPriceId)
                            .SingleOrDefault();
                        if (existingCost != null)
                        {
                            //update resource
                            _context.Entry(existingCost).CurrentValues.SetValues(cost);

                            // remove any months that no longer exist.
                            foreach (var existingMonth in existingCost.FixedPriceMonths.ToList())
                            {
                                if (!cost.FixedPriceMonths.Any(r => r.FixedPriceMonthId == existingMonth.FixedPriceMonthId))
                                    _context.FixedPriceMonths.Remove(existingMonth);
                            }

                            // update existing fixed price and new months
                            foreach (var fixedMonth in cost.FixedPriceMonths)
                            {
                                var existingFixedMonth = existingCost.FixedPriceMonths
                                    .Where(r => r.FixedPriceMonthId == fixedMonth.FixedPriceMonthId && fixedMonth.FixedPriceMonthId > 0)
                                    .SingleOrDefault();

                                //work through the set of resource months and update or add.
                                if (existingFixedMonth != null)
                                    _context.Entry(existingFixedMonth).CurrentValues.SetValues(fixedMonth);
                                else
                                {
                                    var newMonth = GetFixedPriceMonth(fixedMonth);

                                    //ensure there are months for the existing fixed price row
                                    if (existingCost.FixedPriceMonths == null)
                                        existingCost.FixedPriceMonths = new List<FixedPriceMonth>();
                                    existingCost.FixedPriceMonths.Add(newMonth);
                                }
                            }
                        }
                        else
                        {
                            // add fixed price record
                            var newFixedPrice = new FixedPrice
                            {

                                FixedPriceName = cost.FixedPriceName,
                                FixedPriceTypeId = cost.FixedPriceTypeId,
                                ResourceTypeId = cost.ResourceTypeId,
                                Vendor = cost.Vendor,
                                TotalActualCost = cost.TotalActualCost,
                                TotalPlannedCost = cost.TotalPlannedCost
                            };

                            //add the months
                            newFixedPrice.FixedPriceMonths = new List<FixedPriceMonth>();
                            foreach (var month in cost.FixedPriceMonths)
                            {
                                var newFixedPriceMonth = GetFixedPriceMonth(month);


                                newFixedPrice.FixedPriceMonths.Add(newFixedPriceMonth);
                            }
                            existingProject.FixedPriceCosts.Add(newFixedPrice);

                        }
                    }


                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    if (!ProjectExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Debug.Write(e.InnerException);
                        throw;
                    }
                }


                existingProject.Months = existingProject.Months.OrderBy(m => m.MonthNo).ToList();
                existingProject.Budgets = existingProject.Budgets.OrderBy(b => b.ApprovedDateTime).ToList();
                foreach (var resource in existingProject.Resources)
                {
                    resource.ResourceMonths = resource.ResourceMonths.OrderBy(rm => rm.MonthNo).ToList();
                }
                foreach (var f in existingProject.FixedPriceCosts)
                {
                    f.FixedPriceMonths = f.FixedPriceMonths.OrderBy(fm => fm.MonthNo).ToList();
                }



                return Ok(existingProject);

            }
            else
            {
                return NotFound();
            }






        }

        // POST: api/Projects
        [HttpPost]
        public async Task<IActionResult> PostProject([FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //this add the project header information
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                //now add the monthly details.
                //await UpdateProject(project.ProjectId, project);

                return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.SingleOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }

        // Create new instance of resource month in order to add to database.
        private ResourceMonth GetResourceMonth(ResourceMonth month)
        {
            var newMonth = new ResourceMonth
            {
                ActualEffort = month.ActualEffort,
                MonthNo = month.MonthNo,
                ActualEffortCapPercent = month.ActualEffortCapPercent,
                ActualEffortStyle = month.ActualEffortStyle,
                PlannedEffort = month.PlannedEffort,
                PlannedEffortCapPercent = month.PlannedEffortCapPercent,
                PlannedEffortStyle = month.PlannedEffortStyle
            };
            return newMonth;
        }

        // Create new instance of FixedPriceMonth to be added to the database.
        private FixedPriceMonth GetFixedPriceMonth(FixedPriceMonth month)
        {
            var newMonth = new FixedPriceMonth
            {
                ActualCost = month.ActualCost,
                ActualCostCapPercent = month.ActualCostCapPercent,
                ActualCostStyle = month.ActualCostStyle,
                MonthNo = month.MonthNo,
                PlannedCost = month.PlannedCost,
                PlannedCostCapPercent = month.PlannedCostCapPercent,
                PlannedCostStyle = month.PlannedCostStyle
            };
            return newMonth;
        }

        private VendorInvoice GetInvoice(VendorInvoice invoice)
        {
            var newInvoice = new VendorInvoice
            {
                Amount = invoice.Amount,
                Comments = invoice.Comments,
                InvoiceDate = invoice.InvoiceDate
            };
            return newInvoice;
        }
    }
}