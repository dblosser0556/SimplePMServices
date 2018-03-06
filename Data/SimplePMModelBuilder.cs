using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using SimplePMServices.Models.Entities;

namespace SimplePMServices.Data
{
    public class SimplePMModelBuilder : ODataConventionModelBuilder
    {
        public SimplePMModelBuilder(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            EntitySet<Phase>(nameof(ApplicationDbContext.Phases));
            EntitySet<Code>(nameof(ApplicationDbContext.Codes));
        }
    }
}
