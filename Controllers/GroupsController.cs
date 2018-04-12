using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePMServices.Data;
using SimplePMServices.Models.Entities;
using SimplePMServices.ViewModels;

namespace SimplePMServices.Controllers
{
    [Produces("application/json")]
    [Route("api/Groups")]
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [EnableQuery]
        [HttpGet]
        public IEnumerable<Group> GetGroups()
        {

            var groups =  _context.Groups
                             .Include(g => g.GroupBudgets)
                             .OrderBy(g => g.Lft);

            foreach (Group group in groups) {
                group.GroupBudgets = group.GroupBudgets.OrderBy(gb => gb.BudgetYear).ToList();
            }

            return groups.AsQueryable();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var group = await _context.Groups
                              .Include(g => g.GroupBudgets)
                              .OrderBy(g => g.Lft)
                              .SingleOrDefaultAsync(g => g.GroupId == id);



            group.GroupBudgets = group.GroupBudgets.OrderBy(gb => gb.BudgetYear).ToList();


            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup([FromRoute] int id, [FromBody] Group group)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != group.GroupId)
                {
                    return BadRequest();
                }

                await UpdateBudgets(id, group);
                await RebuildHierarchyOrder();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
           
                }
            }

            return NoContent();
        }

        // POST: api/Groups
        [HttpPost]
        public async Task<IActionResult> PostGroup([FromBody] Group group)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // can't pass a null but can't add a value
                _context.Groups.Add(group);
                _context.SaveChanges();

                await UpdateBudgets(group.GroupId, group);

                await RebuildHierarchyOrder();

                return CreatedAtAction("GetGroup", new { id = group.GroupId }, group);
            } catch (Exception e)
            {
                Console.Write(e.Message, e.InnerException);
                return BadRequest();
            }
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = await _context.Groups.SingleOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return Ok(group);
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }

        private async Task<IActionResult> UpdateBudgets(int id, Group group) {



            var existingGroup = await _context.Groups
                                         .Include(g => g.GroupBudgets)
                                         .Where(g => g.GroupId == id)
                                         .SingleOrDefaultAsync();

            if (existingGroup != null)
            {

                try
                {
                    _context.Entry(existingGroup).CurrentValues.SetValues(group);

                    //Find Deleted Resources
                    if (existingGroup.GroupBudgets != null)
                    {
                        foreach (var existingBudget in existingGroup.GroupBudgets.ToList())
                        {
                            if (!group.GroupBudgets.Any(r => r.GroupBudgetId == existingBudget.GroupBudgetId))
                                _context.GroupBudgets.Remove(existingBudget);
                        }
                    }
                    //Update and Insert resources
                    foreach (var budget in group.GroupBudgets)
                    {
                        //check to see if the resource already exists.
                        //new resources have a resourceId of -1
                        var existingBudget = existingGroup.GroupBudgets
                            .Where(gb => gb.GroupBudgetId == budget.GroupBudgetId && budget.GroupBudgetId > 0)
                            .SingleOrDefault();
                        if (existingBudget != null)
                        {
                            //update budget
                            _context.Entry(existingBudget).CurrentValues.SetValues(budget);
                        }
                        else
                        {
                            //add the group budget
                            // add resource
                            var newBudget = new GroupBudget
                            {
                                Amount = budget.Amount,
                                ApprovedDateTime = budget.ApprovedDateTime,
                                BudgetType = budget.BudgetType,
                                BudgetYear = budget.BudgetYear,
                                GroupId = budget.GroupId
                            };

                            //add the resource months

                            existingGroup.GroupBudgets.Add(newBudget);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    if (!GroupExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Debug.Write(e.InnerException);
                        return BadRequest();
                    }
                }

                return NoContent();

            }
            else
            {
                return NotFound();
            }

        }
        

        private async Task<ActionResult> RebuildHierarchyOrder()
        {
            try
            {
                List<Group> groups = new List<Group>();

                // get an updated list of the hierarchy order by hierarchy.
                var sql = "With GroupList AS " +
                    "(SELECT Parent.GroupId, Parent.ParentId, Parent.LevelDesc, Parent.LevelId, Parent.Lft, " +
                    "Parent.Rgt, Parent.GroupName, Parent.GroupDesc, Parent.GroupManager, 1 as Level " +
                    "FROM Groups AS Parent " +
                    "WHERE Parent.ParentId is NULL OR Parent.ParentId = 0 " +
                    "UNION ALL " +
                    "SELECT Child.GroupId, Child.ParentId, Child.LevelDesc, Child.LevelId, Child.Lft, " +
                    "Child.Rgt, Child.GroupName, Child.GroupDesc, Child.GroupManager, GL.Level + 1 " +
                    "FROM Groups AS Child " +
                    "INNER JOIN GroupList AS GL " +
                    "ON Child.ParentId = GL.GroupId " +
                    "WHERE Child.ParentId is NOT NULL or Child.ParentId > 0) " +
                    "SELECT * FROM GroupList";



     

                var results = await _context.Groups.FromSql(sql).ToListAsync();
                
                int right = 1;
                foreach (var item in results)
                {
                    int left = right;
                    if (item.Level == 1)
                    {
                        right = await RebuildTree(item, left, 1);
                    }
                  
                }
                return Ok();
                //return groups;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }


        //count the children 
        private async Task<int> RebuildTree(Group group, int left, int level)
        {
            int right = left + 1;
            
            var results = await _context.Groups.FromSql(
                "Select * from groups WHERE ParentId={0}", group.GroupId).ToListAsync();

            // go through and get all children
            foreach (Group item in results)
            {
                right = await RebuildTree(item, right, level + 1);
            }
            _context.Attach(group);
            group.Lft = left;
            group.Rgt = right;
            group.Level = level;
            await _context.SaveChangesAsync();

            return right + 1;
        }
    }
}