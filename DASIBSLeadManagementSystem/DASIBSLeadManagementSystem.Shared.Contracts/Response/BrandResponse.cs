using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSystem.Shared.Contracts.Response
{
    public class BrandResponse
    {
        public int Id { get; set; }

        public int BrandId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string SubBrand { get; set; }

        public bool Active { get; set; }
    }
}
