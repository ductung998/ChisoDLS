using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassChung;
using System.IO;

namespace Chisoyhoc_MVC.Controllers
{
    public class ProductController : Controller
    {
        private static List<Observation> Obs = new List<Observation>();
        KetnoiDB db = new KetnoiDB();
        public ActionResult Index(string strSearch, string filterType, string filterPN,
            string filterCQ, int? page, string sortOrder)
        {
            List<chisoyhoc> query = db.filterCSYH(filterType, filterPN, filterCQ);

            if (!String.IsNullOrEmpty(strSearch))
            {
                query = query.Where(x => x.tenchiso.ToUpper().Contains(strSearch.ToUpper())).ToList();
            }

            switch (sortOrder)
            {
                case "tenchiso_desc":
                    query = query.OrderByDescending(s => s.tenchiso).ToList();
                    break;
                case "tenchiso_asc":
                    query = query.OrderBy(s => s.tenchiso).ToList();
                    break;
                case "machiso_desc":
                    query = query.OrderByDescending(s => s.machiso).ToList();
                    break;
                case "machiso_asc":
                    query = query.OrderBy(s => s.machiso).ToList();
                    break;
                default:
                    // Original order or default sorting
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<chisoyhoc> obj = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            int totalCount = query.Count();

            ViewBag.StrSearch = strSearch;
            ViewBag.FilterType = filterType;
            ViewBag.FilterPN = filterPN;
            ViewBag.FilterCQ = filterCQ;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalCount = totalCount;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "tenchiso_asc" ? "tenchiso_asc" : "tenchiso_desc";
            ViewBag.MachisoSortParm = sortOrder == "machiso_asc" ? "machiso_asc" : "machiso_desc";

            return View(obj);
        }
        public ActionResult TinhCSYH(string machiso)
        {
            Chisoyhoc kq = db.GetCSYHtheoIDchiso(machiso);
            List<Bien> DSbien = db.GetDSbien(machiso);
            int socot = 1 + DSbien.Count();

            List<string> DSketqua2 = new List<string>();
            DSketqua2.Add("ID");

            List<string> DStencot = new List<string>();
            foreach (Bien i in DSbien)
            {
                DStencot.Add(i.tendaydu);
                DSketqua2.Add(i.tenbien.Trim());
            }
            string tenchiso = db.GetTenchiso(machiso);

            List<string> DSketqua = new List<string>();
            DSketqua.AddRange(DStencot);
            DSketqua.AddRange(db.GetDSKQNCKH(machiso));

            DSketqua2.Add(db.GetDStenKQNCKH(machiso)[0]);

            string checkTD = machiso.Substring(0, 1);
            //if (checkTD == "T")
            //{
            //    DSketqua.Add(db.GetBienKQ(machiso).TendayduKQ);
            //    DSketqua2.Add(db.GetBienKQ(machiso).TenbienKQ);
            //}
                

            DSketqua2.AddRange(db.GetDStenKQNCKH(machiso).Skip(1));
            string DStenKQ = "";
            foreach (string i in DSketqua2)
            {
                DStenKQ = DStenKQ + i + "-";
            }
            DStenKQ = DStenKQ.TrimEnd('-');

            ViewBag.Socot = socot;
            ViewBag.DSTencot = DStencot;
            ViewBag.Machiso = machiso;
            ViewBag.Tenchiso = tenchiso;
            ViewBag.DSKetqua = DSketqua;
            ViewBag.DSTenKQ = DStenKQ;

            // Retrieve products from the static variable
            return View(Obs);
        }

        [HttpPost]
        public ActionResult AddRow()
        {
            // Find the maximum ID in the existing products
            int maxId = Obs.Max(p => p.Id);

            // Increment the maximum ID by one to get the new ID for the next product
            int nextId = maxId + 1;

            // Add an empty product row with the incremented ID
            Obs.Add(new Observation { Id = nextId });

            // Return a partial view for the newly added row, passing the product with the incremented ID
            return PartialView("_ProductRow", Obs.First(p => p.Id == nextId));
        }

        public ActionResult DownloadExcel(string machiso)
        {
            // Validate machiso parameter if necessary

            // Get the file path
            string filePath = Path.Combine(Server.MapPath("~/Temp/Filemau"), machiso.Substring(0, 3), machiso + ".xlsx");

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound(); // Or any other appropriate action
            }

            // Provide the file for download
            return File(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", machiso + ".xlsx");
        }
        [HttpPost]
        public ActionResult ProcessData(string machiso, Observation model)
        {
            model.setdata();
            // Process the inputData using your method
            List<List<string>> processedData = Tuongtac.tinhCSYHnhieu(machiso, model.data);

            // Return the processed data
            return Json(processedData);
        }
    }
}
