using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NRI.DB.CST.Contexts;
using NRI.DB.CST.Repositories;

namespace HowToUseRepository
{
    public class CustomerInfoRepository : Repository<CustomerInfoRepository, Guid>
    {
        public CustomerInfoRepository(NRIContext context) : base(context)
        {
        }
    }
}