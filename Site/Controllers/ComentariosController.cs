using PagedList;
using Site.Models;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System.Threading.Tasks;
using Site.Services;

namespace Site.Controllers
{
    public class ComentariosController : BaseController
    {
        private string primeKey = "comentarios:";

        public ComentariosController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        [HttpPost]
        public ActionResult Comentar(ComentarioViewModel comentarioVM)
        {
            var comentario = new Comentario
            {
                CPF = comentarioVM.CPFUser,
                Nome = comentarioVM.NameUser,
                Texto = comentarioVM.MensagemUser,
                DataCadastro = DateTime.Now,
                UrlFacebook = comentarioVM.Link,
                IPAcesso = Request.UserHostAddress,
                IdComentarioRaiz = comentarioVM.ComentarioId,
                IdNoticia = comentarioVM.NoticiaId
            };

            db.Comentarios.Add(comentario);
            db.SaveChanges();

            return Json(new { MomentoPostagem = "postado em " + comentario.DataCadastro.ToString(@"dd MMM, yyyy à\s hh:mm t"), Id = comentario.Id });
        }

        [HttpPost]
        public ActionResult GostarComentario(int id)
        {
            var comentario = db.Comentarios.Find(id);

            comentario.Gostar();
            db.Entry(comentario).State = EntityState.Modified;

            db.SaveChanges();

            return Json("Ok");
        }

        [HttpPost]
        public ActionResult RemoverGostarComentario(int id)
        {
            var comentario = db.Comentarios.Find(id);

            comentario.RemoverGostar();
            db.Entry(comentario).State = EntityState.Modified;

            db.SaveChanges();

            return Json("Ok");
        }

        [HttpPost]
        public ActionResult RemoverNaoGostarComentario(int id)
        {
            var comentario = db.Comentarios.Find(id);

            comentario.RemoverNaoGostar();
            db.Entry(comentario).State = EntityState.Modified;

            db.SaveChanges();

            return Json("Ok");
        }

        [HttpPost]
        public ActionResult NaoGostarComentario(int id)
        {
            var comentario = db.Comentarios.Find(id);

            comentario.NaoGostar();
            db.Entry(comentario).State = EntityState.Modified;

            db.SaveChanges();

            return Json("Ok");
        }

        [OutputCache(Duration = 30, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "pagina;noticiaId;ordem")]
        public ActionResult CarregarComentarios(int pagina, int noticiaId, OrdemComentario? ordem)
        {
            var comentarios = CarregarComentariosCached(pagina, noticiaId, ordem);

            return Json(new { Comentarios = comentarios, Pagina = pagina }, JsonRequestBehavior.AllowGet);
        }

        private List<ComentarioPagedListViewModel> CarregarComentariosCached(int pagina, int noticiaId, OrdemComentario? ordem)
        {
            string key = primeKey + "CarregarComentarios:TNoticias:TComentario:" + pagina + ":" + noticiaId;

            List<ComentarioPagedListViewModel> retorno = null;

            Func<object, List<ComentarioPagedListViewModel>> funcao = t => CarrregarComentariosDB(pagina,noticiaId,ordem);
            retorno = RedisService.GetOrSetToRedis(key, funcao, 30, pagina, noticiaId, ordem);

            return retorno;
        }
        private List<ComentarioPagedListViewModel> CarrregarComentariosDB(int pagina, int noticiaId, OrdemComentario? ordem)
        {
            var noticia = db.Noticias.Include(n => n.Comentarios).Include(n => n.Comentarios.Select(coment => coment.Respostas)).FirstOrDefault(n => n.id == noticiaId);

            IOrderedEnumerable<Comentario> comentariosRaiz;

            switch (ordem)
            {
                case OrdemComentario.DataCadastro:
                    comentariosRaiz = noticia.ComentariosRaiz.OrderByDescending(comentario => comentario.DataCadastro);
                    break;
                case OrdemComentario.Relevancia:
                    comentariosRaiz = noticia.ComentariosRaiz.OrderByDescending(comentario => comentario.Gostei);
                    break;
                case OrdemComentario.Alfabetica:
                    comentariosRaiz = noticia.ComentariosRaiz.OrderBy(comentario => comentario.Nome);
                    break;
                default:
                    comentariosRaiz = noticia.ComentariosRaiz.OrderByDescending(comentario => comentario.DataCadastro);
                    break;
            }

            var comentarios = comentariosRaiz.ToPagedList(pagina, 3).Select(coment =>
                new ComentarioPagedListViewModel {
                    Nome = coment.Nome,
                    DataCadastro = "postado em " + coment.DataCadastro.ToString(@"dd MMM, yyyy à\s HH:mm t"),
                    Texto = coment.Texto,
                    Id = coment.Id,
                    Gostei = coment.Gostei,
                    NaoGostei = coment.NaoGostei,
                    Respostas = coment.Respostas.Select(resp =>
                            new RespostaPagedListViewModel{
                                Nome = resp.Nome,
                                DataCadastro = "postado em " + resp.DataCadastro.ToString(@"dd MMM, yyyy à\s HH:mm t"),
                                Texto = resp.Texto,
                                Id = resp.Id,
                                Gostei = resp.Gostei,
                                NaoGostei = resp.NaoGostei
                            }).ToList()
                }).ToList();
            return comentarios;
        }

        public ActionResult ComentarioCompartilhado(int? idComentario)
        {
            if (idComentario == null || idComentario == 0)
            {
                return RedirectToActionPermanent("Index", controllerName: "Home");
            }

            var comentario = db.Comentarios.Include(coment => coment.Noticia).FirstOrDefault(coment => coment.Id == idComentario);

            return View(comentario);
        }

        [OutputCache(Duration = 30, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "idNoticia")]
        public ActionResult QuantidadeComentarios(int idNoticia)
        {
            return Json(QuantidadeComentariosCached(idNoticia).ToString(), JsonRequestBehavior.AllowGet);
        }
        private int QuantidadeComentariosCached(int noticiaId)
        {
            string key = primeKey + "CarregarComentarios:TNoticias:TComentario:" + noticiaId;

            int retorno = 0;

            Func<object, int> funcao = t => QuantidadeComentariosDB(noticiaId);
            retorno = RedisService.GetOrSetToRedis(key, funcao, 30, noticiaId);

            return retorno;
        }
        private int QuantidadeComentariosDB(int noticiaId)
        {
            var noticiaObj = db.Noticias.Include(noticia => noticia.Comentarios).FirstOrDefault(noticia => noticia.id == noticiaId);

            if (noticiaObj.Comentarios != null)
            {
                return noticiaObj.Comentarios.Where(comentario => !comentario.Bloqueado && (!comentario.Resposta || !comentario.ComentarioRaiz.Bloqueado)).Count();
            }
            return 0;
        }
    }

    public enum OrdemComentario
    {
        DataCadastro = 0,
        Relevancia = 1,
        Alfabetica = 2
    }
}