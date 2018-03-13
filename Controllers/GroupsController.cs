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
        [HttpGet]
        public IEnumerable<Group> GetGroups()
        {
            return _context.Groups;
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup([FromRoute] int id)
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

            return Ok(group);
        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup([FromRoute] int id, [FromBody] Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.GroupId)
            {
                return BadRequest();
            }

            _context.Entry(group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Groups
        [HttpPost]
        public async Task<IActionResult> PostGroup([FromBody] Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            await UpdateHierarchy();

            return CreatedAtAction("GetGroup", new { id = group.GroupId }, group);
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

        //update the hierachy each time an entry is made to the groups table.
        private async Task<ActionResult> UpdateHierarchy()
        {
            List<Group> groups = await GetHierarchyOrder();
            for (int i = 0; i < groups.Count; i++)
            {
                _context.Entry(groups[i]).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }

            return NoContent();
        }

        private async Task<List<Group>> GetHierarchyOrder()
        {
            List<Group> groups = new List<Group>();

            //get an updated list of the hierarchy order by hierarchy.
            var sql = "With GroupList AS" +
                "(SELECT Parent.GroupId, Parent.GroupName, Parent.GroupDesc, Parent.ParentId, Parent.LevelDesc, 1 as Level " +
                "FROM group AS Parent " +
                "WHERE Parent.ParentId is NULL " +
                "UNION ALL " +
                "SELECT Child.GroupId, Child.GroupName, Child.GroupDesc, Child.ParentId, Child.LevelDesc, GL.Level + 1 " +
                "FROM group AS Child " +
                "INNER JOIN GroupList AS GL " +
                "ON Child.ParentId = GL.GroupId " +
                "WHERE Child.ParentId is NULL)" +
                "SELECT * FROM GroupList";

            var results = await _context.Groups.FromSql(sql).ToListAsync();

            foreach( var item in results)
            {
                var group = new Group();

                group.GroupId = item.GroupId;
                group.ParentId = item.ParentId;
                group.GroupManager = item.GroupManager;
                group.Level = item.Level;
                group.LevelDesc = item.LevelDesc;
                groups.Add(group);
            }

            // calculate left and right values
            int counter = 1;
            for (int i = 0; i < groups.Count; i++)
            {
              
                    Group _group = new Group();
                    _group = ChildCount(groups[i], groups, counter);
                    counter = (int)_group.Right + 1;
                    groups[i].Left = _group.Left;
                    groups[i].Right = _group.Right;
                
            }
            return groups;
        }

        private Group ChildCount(Group group, List<Group> groups, int counter )
        {

            group.Left = counter++;
            foreach (Group _group in groups )
            {
                if (_group.ParentId == group.GroupId)
                {
                    counter++;
                    group = ChildCount(_group, groups, counter);
                }
            }
            group.Right = ++counter;
            return group;
        }
    }
}