using Administrativo.Models;
using Administrativo.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Controllers
{
    public class ComentariosController : BaseController
    {

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "", string especial = "", int? noticiaId = null, int? colunistaId = 0, int? editoriaId = 0)
        {
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Noticia = noticiaId;
            ViewBag.Page = page;
            ViewBag.Colunista = colunistaId;
            ViewBag.Editoria = editoriaId;
            AllClassOffSort();

            var comentarioVM = new ComentarioViewModel();

            comentarioVM.Colunistas = new SelectList(db.Colunista.Where(c => !c.excluido && c.liberado)
                .OrderBy(c => c.nome)
                .Select(c => new
                {
                    Id = c.id,
                    Nome = c.nome
                }).ToList(), "Id", "Nome");

            comentarioVM.Editorias = new SelectList(db.Editoriais.Where(e => !e.excluido && e.ativo)
                .OrderBy(e => e.nome)
                .Select(e => new
                {
                    Id = e.id,
                    Nome = e.nome
                }).ToList(),"Id","Nome");

            ViewBag.Comentarios = comentarioVM;

            return View(GetListItens(page, search, sortField, order, especial, noticiaId, colunistaId, editoriaId));
        }

        public void AllClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassNome = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
            ViewBag.SortClassStatus = "sorting";
        }

        public IPagedList<Comentarios> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "", string especial = "", int? noticiaId = null, int? colunistaId = 0, int? editoriaId = 0)
        {
            var query = db.Comentarios.AsQueryable();

            if (colunistaId > 0)
            {
                query = query.Include(com => com.Noticias)
                    .Where(com => com.Noticias.idColunista == colunistaId)
                    .AsQueryable();
            }
            if (editoriaId > 0)
            {
                query = query.Include(com => com.Noticias).Include(com => com.Noticias.Editoriais)
                    .Where(com => com.Noticias.Editoriais.Contains(
                        db.Editoriais.Where(e => e.id == editoriaId).FirstOrDefault()
                        )
                    ).AsQueryable();
            }

            var list = query.ToList();

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            int id;

            if (!string.IsNullOrEmpty(searchParam))
            {
                int.TryParse(searchParam, out id);

                pagedList = pagedList.Where(e => e.Id == id || e.Texto.ToLower().Contains(searchParam.ToLower()) || e.Nome.ToLower().Contains(searchParam.ToLower()));
            }

            if (noticiaId != null)
            {
                pagedList = pagedList.Where(e => e.IdNoticia == noticiaId.Value);
            }

            switch (sortField)
            {
                case "Id":
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
                case "Nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Nome);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Nome);
                        ViewBag.SortClassnome = "sorting_desc";
                    }
                    break;
                case "DataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_asc";
                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_desc";
                    }
                    break;
                case "Status":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Bloqueado);
                        ViewBag.SortClassStatus = "sorting_asc";
                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Bloqueado);
                        ViewBag.SortClassStatus = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(e => e.Id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList(pageNumber, 10);
        }

        public ActionResult Detalhes(int id)
        {
            var comentario = db.Comentarios.Find(id);

            return View(comentario);
        }

        [HttpPost]
        public ActionResult Bloquear(int id)
        {
            var comentario = db.Comentarios.Find(id);

            comentario.Bloqueado = !comentario.Bloqueado;

            db.Entry(comentario).State = EntityState.Modified;

            db.SaveChanges();

            return Json(true);
        }


    }
}