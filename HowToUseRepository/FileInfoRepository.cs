using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NRI.DB.CST.Contexts;
using NRI.DB.CST.Models;
using NRI.DB.CST.Repositories;

namespace HowToUseRepository
{
    public class FileInfoRepository : Repository<FileInfo, Guid>
    {
        public FileInfoRepository(NRIContext context) : base(context)
        {
        }
    }
}