using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePMServices.Models.Entities
{
    public class Characteristic
    {

        public int CharacteristicId { get; set; }
        public int? ParentId { get; set; }

        //indicator of the organization level withe level 1 being the top level.
        public int Level { get; set; }
     

        // this indicates the position in the hierarchy.  
        // these are calculated fields.
        public int? Lft { get; set; }
        public int? Rgt { get; set; }
        [Required]
        [StringLength(50)]
        public string CharacteristicName { get; set; }
        public string CharacteristicDesc { get; set; }
 

    }
}
