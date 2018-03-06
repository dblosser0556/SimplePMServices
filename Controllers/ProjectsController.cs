﻿using System;
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
        public IQueryable <ProjectList> GetProjects()
        {

           

            List<ProjectList> projects = new List<ProjectList>();

            var query = from p in _context.Projects
                        join s in _context.Status on p.StatusId equals s.StatusId
                        join g in _context.Groups on p.GroupId equals g.GroupId
                        select new
                        {
                            ProjectId = p.ProjectId,
                            ProjectName = p.ProjectName,
                            ProjectDesc = p.ProjectDesc,
                            ProjectManager = p.ProjectManager,
                            PlannedStartDate = p.PlannedStartDate.ToString("yyyy-MM-dd"),
                            ActualStartDate = p.ActualStartDate.HasValue ? p.ActualStartDate.Value.ToString("yyyy-MM-dd"): string.Empty,
                            GroupId = g.GroupId,
                            GroupName = g.GroupName,
                            GroupManager = g.GroupManager,
                            StatusId = s.StatusId,
                            StatusName = s.StatusName
                            
                        };

            foreach (var item in query)
            {
                var project = new ProjectList
                {
                    ProjectId = item.ProjectId,
                    ProjectName = item.ProjectName,
                    ProjectDesc = item.ProjectDesc,
                    ProjectManager = item.ProjectManager,
                    PlannedStartDate = item.PlannedStartDate,
                    ActualStartDate = item.ActualStartDate,
                    GroupId = item.GroupId,
                    StatusId = item.StatusId,
                    GroupName = item.GroupName,
                    GroupManager = item.GroupManager,
                    StatusName = item.StatusName
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
                //var project = await _context.Projects.SingleOrDefaultAsync(m => m.ProjectId == id);
                var project = await _context.Projects
                                            .Include(p => p.Resources)
                                                .ThenInclude(pr => pr.ResourceMonths)
                                            .Include(p => p.Months)
                                            .Include(p => p.FixedPriceCosts)
                                                .ThenInclude(fp => fp.FixedPriceMonths)
                                            .Where(p => p.ProjectId == id)
                                            .ToListAsync();
                if (project == null)
                {
                    return NotFound();
                }

                return Ok(project);
            }
            catch(Exception e)
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

            var existingProject = _context.Projects.Include(p => p.Resources)
                                                .ThenInclude(pr => pr.ResourceMonths)
                                            .Include(p => p.Months)
                                            .Include(p => p.FixedPriceCosts)
                                                .ThenInclude(fp => fp.FixedPriceMonths)
                                            .Where(p => p.ProjectId == id)
                                            .SingleOrDefault();
            if (existingProject != null)
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
                    var existingResource = existingProject.Resources
                        .Where(r => r.ResourceId == resource.ResourceId)
                        .SingleOrDefault();
                    if (existingResource != null)
                    {
                        //update resource
                        _context.Entry(existingResource).CurrentValues.SetValues(resource);

                        // update existing resource months
                        foreach (var resourceMonth in resource.ResourceMonths)
                        {
                            var existingResourceMonth = existingResource.ResourceMonths
                                .Where(r => r.ResourceMonthId == resourceMonth.ResourceMonthId)
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
                        .Where(r => r.MonthId == month.MonthId)
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

                //Find Deleted Months
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


                        // update existing fixed price and new months
                        foreach (var fixedMonth in cost.FixedPriceMonths)
                        {
                            var existingFixedMonth = existingCost.FixedPriceMonths
                                .Where(r => r.FixedPriceMonthId == fixedMonth.FixedPriceMonthId)
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
                        // add resource
                        var newFixedPrice = new FixedPrice
                        {

                            FixedPriceName = cost.FixedPriceName,
                            FixedPriceTypeId = cost.FixedPriceTypeId,
                            ResourceTypeId = cost.ResourceTypeId,
                            Vendor = cost.Vendor
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

                try
                {
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

                return NoContent();

            } else
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
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

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

        private ResourceMonth GetResourceMonth (ResourceMonth month)
        {
            var newMonth = new ResourceMonth
            {
                ActualEffort = month.ActualEffort,
                MonthNo = month.MonthNo,
                ActualEffortCapPercent = month.ActualEffortCapPercent,
                PlannedEffort = month.PlannedEffort,
                PlannedEffortCapPercent = month.PlannedEffortCapPercent,
               
            };
            return newMonth;
        }

        private FixedPriceMonth GetFixedPriceMonth (FixedPriceMonth month)
        {
            var newMonth = new FixedPriceMonth
            {
                ActualCost = month.ActualCost,
                ActualCostCapPercent = month.ActualCostCapPercent,
                MonthNo = month.MonthNo,
                PlannedCost = month.PlannedCost,
                PlannedCostCapPercent = month.PlannedCostCapPercent
            };
            return newMonth;
        } 
    }
}