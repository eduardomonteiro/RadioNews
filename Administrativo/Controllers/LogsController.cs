using Administrativo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Controllers
{
    public class LogsController : BaseController
    {
        //
        // GET: /Logs/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "", int AreaId = 0, int UserId = 0)
        {

            ViewBag.EditoriaisActive = "active";
            ViewBag.EditoriaisSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            ViewBag.AreaIdVB = AreaId;
            ViewBag.UserIdVB = UserId;
            allClassOffSort();

            ViewBag.AreaId = new SelectList(db.AreasADM.Where(c=>c.Id>1 ), "Id", "Area");
            ViewBag.UserId = new SelectList(_db.UserProfiles, "UserId", "UserName");

            return View(GetListItens(page, search, sortField, order, AreaId, UserId));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassid = "sorting";
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<Logs> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "", int AreaId = 0, int UserId = 0)
        {
            var list = db.Logs;

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);


            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;
                pagedList = pagedList.Where(e => e.Id == id || e.AreasADM.Area.ToLower().Contains(searchParam.ToLower()));
            }

            if (AreaId > 0)
            {
                pagedList = pagedList.Where(x => x.AreaId == AreaId);
            }


            if (UserId > 0)
            {
                pagedList = pagedList.Where(x => x.UserId == UserId);
            }

            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.AreasADM.Area);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.AreasADM.Area);
                        ViewBag.SortClassnome = "sorting_desc";
                    }
                    break;
                case "DataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Data);
                        ViewBag.SortClassDataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Data);
                        ViewBag.SortClassDataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(e => e.Data);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Logs>(pageNumber, 10);
        }

        //
        // GET: /Logs/Details/5

        public ActionResult Details(int id)
        {
            var log = db.Logs.FirstOrDefault(x => x.Id == id);

            if (log == null)
            {
                return HttpNotFound();
            }


            return View(log);
        }


        //
        // GET: /Logs/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Logs/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public string getUserName(int UserId)
        {
            var name = _db.UserProfiles.FirstOrDefault(x => x.UserId == UserId).UserName;
            return name;
        }
    }
}
