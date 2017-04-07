using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NRI.DB.CST.Contexts;
using NRI.DB.CST.Models;
using NRI.DB.CST.Repositories;

namespace HowToUseRepository
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var context = new NRIContext(); // Should use the simpleinjection for using in the real-world project instead of new context object.
                                            //Add(context);
                                            // FileTest s = new FileTest();
                                            // var xx = s.ColumnProperty.Name;
                                            //AddRange(context);
                                            // Update(context);

            //var result = (from items in context.CustomerInfoes
            //              where items.ID == Guid.NewGuid()
            //              select items).FirstOrDefault();

            // CustomerInfoRepository repo = new CustomerInfoRepository(context);

            Repository<FileInfo, Guid> fileInfo = new Repository<FileInfo, Guid>(context);

            CustomUpdate(fileInfo);

            //  var counts = fileInfo.GetCounts();

            /// var xvfx = fileInfo.GetAll().ToList();

            //  var DSC04158 = fileInfo.GetCounts(oo => oo.OriginalName == "DSC04158.JPG");
            var result1 = fileInfo.GetOrderByDescending<DateTime>(oo => oo.CreateDate, 1).ToList();

            // var result2 = fileInfo.GetOrderByDescending<String>(oo => oo.FileName, 1).ToList();

            // var result3 = fileInfo.GetPageByDescending<String>(oo => oo.FileName, 1, 3).ToList();

            //  var result3 = fileInfo.GetItemsOrderByObject(oo => oo.IsActive, 5);

            //  var result4 = fileInfo.GetItemsOrderByString(oo => oo.FileName, 5);

            //  var result1 = fileInfo.GetItemsByPagesCount(2, 3, d => d.CreateDate);
            // var result23 = fileInfo.GetPage(bb => bb, 1, 3);
            // var result2 = fileInfo.PaginateAndOrderByDesc(2, 3, d => d.ID);

            var xx = result1;

            //  var result = from  repo.Table;
            // var result = from a in repo.Table.
        }

        private static void CustomUpdate(Repository<FileInfo, Guid> fileInfo)
        {
            var list = new List<FileInfo>();
            var fileUpdate = new FileInfo
            {
                ID = Guid.Parse("24946E57-B640-4DEE-8F3F-DCBD55105171")
            };

            fileUpdate.FileName = "Test111";
            fileUpdate.FilePath = "-";
            fileUpdate.OriginalName = "-";

            var fileUpdate2 = new FileInfo
            {
                ID = Guid.Parse("DA583697-D59D-487C-A204-EBCCEC096F99")
            };

            fileUpdate2.FileName = "Test222";
            fileUpdate2.FilePath = "-";
            fileUpdate2.OriginalName = "-";

            list.Add(fileUpdate);
            list.Add(fileUpdate2);

            //listProperties.AddRange(fileUpdate.OriginalName, fileUpdate.IsActive);

            ////fileInfo.Update(fileUpdate, p => p.FileName);

            fileInfo.Updates(list, i => i.FileName);

            fileInfo.SaveChanges();
        }

        private static void Add(NRIContext context)
        {
            FileInfoRepository repo = new FileInfoRepository(context);

            //add operation into context
            repo.Add(new FileInfo
            {
                ID = Guid.NewGuid(),
                CreateBy = "System",
                ModifyBy = "System",
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                EnterpriseFK = Guid.Parse("7E01C680-9C26-4268-A183-407BB2B2D7B7"),
                FileName = "Test Repository",
                FilePath = "Test Repository Path",
                IsActive = true,
                OriginalName = "Test Repository Name"
            });

            //save all state changed
            repo.SaveChanges();
        }

        private static void AddRange(NRIContext context)
        {
            FileInfoRepository repo = new FileInfoRepository(context);
            var addList = new List<FileInfo>();

            for (int i = 0; i < 5; i++)
            {
                addList.Add(new FileInfo
                {
                    ID = Guid.NewGuid(),
                    CreateBy = "System",
                    ModifyBy = "System",
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    EnterpriseFK = Guid.Parse("7E01C680-9C26-4268-A183-407BB2B2D7B7"),
                    FileName = "Test Repository" + new Random().Next(1, 3),
                    FilePath = "Test Repository Path" + new Random().Next(1, 3),
                    IsActive = true,
                    OriginalName = "Test Repository Name" + new Random().Next(1, 3)
                });
            }

            //add operation to context
            repo.AddRange(addList);

            //save all state changed
            repo.SaveChanges();
        }

        private static void Update(NRIContext context)
        {
            FileInfoRepository repo = new FileInfoRepository(context);
            var list = new List<FileInfo>();

            //for (int i = 0; i < 5; i++)
            //{
            //    addList.Add(new FileInfo
            //    {
            //        ID = Guid.NewGuid(),
            //        CreateBy = "System",
            //        ModifyBy = "System",
            //        CreateDate = DateTime.Now,
            //        ModifyDate = DateTime.Now,
            //        EnterpriseFK = Guid.Parse("7E01C680-9C26-4268-A183-407BB2B2D7B7"),
            //        FileName = "Test Repository" + new Random().Next(1, 3),
            //        FilePath = "Test Repository Path" + new Random().Next(1, 3),
            //        IsActive = true,
            //        OriginalName = "Test Repository Name" + new Random().Next(1, 3)
            //    });
            //}

            //var updatedItem = new FileInfo
            //{
            //    ID = Guid.Parse("FAFD22AC-4947-41FA-9BE5-200950A188E2"),
            //    IsActive = false,
            //    FileName = "new updated"
            //};

            // var xx = repo.GetItemsByCount(o => o.ID, 3);

            var updatedItem1 = repo.Get(Guid.Parse("FAFD22AC-4947-41FA-9BE5-200950A188E2"));
            updatedItem1.IsActive = false;
            updatedItem1.FilePath = "new updated1111";

            var updatedItem2 = repo.Get(Guid.Parse("60277FC7-821D-4375-A560-61C544EC192B"));
            updatedItem2.IsActive = false;
            updatedItem2.FilePath = "new updated2222";

            list.Add(updatedItem1);
            list.Add(updatedItem2);

            repo.Updates(list);
            // repo.UpdateWithProprty(updatedItem, aa => aa.IsActive, aa => aa.FileName);

            // repo.Update(updatedItem);

            //save all state changed
            repo.SaveChanges();
        }
    }
}