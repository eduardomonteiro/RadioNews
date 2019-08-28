using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Administrativo.Models;
using Administrativo.Commons;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Trabalhe Conosco")]
    public class TrabalheConoscoController : BaseController
    {

        //
        // GET: /TrabalheConosco/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

			ViewBag.TrabalheConoscoActive = "active";
            ViewBag.TrabalheConoscoSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));

        }


		public void allClassOffSort()
        {

		            ViewBag.SortClassid = "sorting";
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClasscelular = "sorting";
            ViewBag.SortClasstelefone = "sorting";
            ViewBag.SortClasssexo = "sorting";
            ViewBag.SortClassestado = "sorting";
            ViewBag.SortClasscidade = "sorting";
            ViewBag.SortClassareaPretencao = "sorting";
            ViewBag.SortClassdataNascimento = "sorting";
            ViewBag.SortClasscurriculo = "sorting";
            ViewBag.SortClassemail = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

		public IPagedList<TrabalheConosco> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.TrabalheConosco;

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = 0;
                if (int.TryParse(searchParam, out outDate))
                {
                    int.Parse(searchParam);
                    pagedList = pagedList.Where(g => g.id == id);
                }
                else
                {
                    pagedList = pagedList.Where(g => g.nome.Contains(searchParam) || g.areaPretencao.Contains(searchParam));
                };

            }


            switch (sortField)
            {
				case "id":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.id);
						ViewBag.SortClassid = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.id);
						ViewBag.SortClassid = "sorting_desc";
					}
					break;
				case "nome":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.nome);
						ViewBag.SortClassnome = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.nome);
						ViewBag.SortClassnome = "sorting_desc";
					}
					break;
				case "celular":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.celular);
						ViewBag.SortClasscelular = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.celular);
						ViewBag.SortClasscelular = "sorting_desc";
					}
					break;
				case "telefone":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.telefone);
						ViewBag.SortClasstelefone = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.telefone);
						ViewBag.SortClasstelefone = "sorting_desc";
					}
					break;
				case "sexo":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.sexo);
						ViewBag.SortClasssexo = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.sexo);
						ViewBag.SortClasssexo = "sorting_desc";
					}
					break;
				case "estado":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.estado);
						ViewBag.SortClassestado = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.estado);
						ViewBag.SortClassestado = "sorting_desc";
					}
					break;
				case "cidade":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.cidade);
						ViewBag.SortClasscidade = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.cidade);
						ViewBag.SortClasscidade = "sorting_desc";
					}
					break;
				case "areaPretencao":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.areaPretencao);
						ViewBag.SortClassareaPretencao = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.areaPretencao);
						ViewBag.SortClassareaPretencao = "sorting_desc";
					}
					break;
				case "dataNascimento":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.dataNascimento);
						ViewBag.SortClassdataNascimento = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.dataNascimento);
						ViewBag.SortClassdataNascimento = "sorting_desc";
					}
					break;
				case "curriculo":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.curriculo);
						ViewBag.SortClasscurriculo = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.curriculo);
						ViewBag.SortClasscurriculo = "sorting_desc";
					}
					break;
				case "email":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.email);
						ViewBag.SortClassemail = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.email);
						ViewBag.SortClassemail = "sorting_desc";
					}
					break;
				case "dataCadastro":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.dataCadastro);
						ViewBag.SortClassdataCadastro = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.dataCadastro);
						ViewBag.SortClassdataCadastro = "sorting_desc";
					}
					break;
                default:
                    pagedList = pagedList.OrderByDescending(t => t.id);
					ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<TrabalheConosco>(pageNumber, 10);
        }

        //
        // GET: /TrabalheConosco/Details/5

        public ActionResult Details(int id = 0)
        {
            TrabalheConosco trabalheconosco = db.TrabalheConosco.Find(id);
            if (trabalheconosco == null)
            {
                return HttpNotFound();
            }
            return View(trabalheconosco);
        }


        //
        // GET: /TrabalheConosco/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TrabalheConosco trabalheconosco = db.TrabalheConosco.Find(id);
            if (trabalheconosco == null)
            {
                return HttpNotFound();
            }
            return View(trabalheconosco);
        }

        //
        // POST: /TrabalheConosco/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TrabalheConosco trabalheconosco = db.TrabalheConosco.Find(id);
            db.TrabalheConosco.Remove(trabalheconosco);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileResult Download(int id)
        {

            var path = "~/Conteudo/Curriculos/";

            var arquivo = db.TrabalheConosco.Find(id);

            var path2 = Server.MapPath(path + arquivo.curriculo);

            byte[] fileBytes = System.IO.File.ReadAllBytes(path2);

            string fileName = arquivo.curriculo;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

    }
}




