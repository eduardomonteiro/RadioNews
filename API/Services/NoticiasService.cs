using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Util;
using Site.Enums;

namespace API.Services
{
    public static class NoticiasService
    {
        static string url = ServerSettings.Url;
        static string primeKey = "noticias:";

        public static List<NoticiaViewModel> ObterNoticias(int? idEditoria, string chave = "")
        {
            string key = primeKey + "ObterNoticias:" + idEditoria + ":" + chave + ":TNoticias:TEditoriais";

            List<NoticiaViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<NoticiaViewModel>> funcao = t => ObterNoticias(db, idEditoria, chave);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 10);
            }

            return retorno;
        }

        public static List<NoticiaViewModel> ObterNoticias(ModeloDados db, int? idEditoria = null, string chave = "")
        {
            var noticiasQuery = db.Noticias
                .Where(noticia =>
                    noticia.Colunista == null
                    && !noticia.excluido
                    && noticia.liberado
                    && noticia.dataAtualizacao < DateTime.Now);

            if (idEditoria != null)
            {
                noticiasQuery = noticiasQuery.Where(noticia => (noticia.Editoriais.Any(editoria => editoria.id == idEditoria)));
            }
            else if (string.IsNullOrWhiteSpace(chave))
            {
                noticiasQuery = noticiasQuery.Where(noticia => (noticia.titulo.Contains(chave) || noticia.texto.Contains(chave) || noticia.subtitulo.Contains(chave) || (noticia.Colunista == null ? true : noticia.Colunista.nome.Contains(chave))));
            }

            var noticias = noticiasQuery.OrderByDescending(noticia => noticia.dataAtualizacao).Take(20).Select(noticia =>
                new NoticiaViewModel
                {
                    Id = noticia.id,
                    Titulo = noticia.titulo,
                    Chamada = noticia.chamada,
                    Texto = string.Empty,
                    Foto = string.IsNullOrEmpty(noticia.foto) ? null : url + "/admin/Conteudo/noticias/" + noticia.id + "/744x500/" + noticia.foto,
                    Data = noticia.dataAtualizacao,
                    NomeAutor = noticia.porAutor,
                    NomeCategoria = noticia.Categorias.FirstOrDefault() == null ? string.Empty : noticia.Categorias.FirstOrDefault().Titulo,
                    Url = url + "/noticia/" + noticia.url
                }).ToList();

            return noticias;
        }

        public static List<NoticiaViewModel> ObterNoticiasHome(int? idEditoria = null, string chave = "")
        {
            string key = primeKey + "ObterNoticiasHome:" + idEditoria + ":" + chave + ":TNoticias:TEditoriais";

            List<NoticiaViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<NoticiaViewModel>> funcao = t => ObterNoticiasHome(db, idEditoria, chave);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 10);
            }

            return retorno;
        }

        public static List<NoticiaViewModel> ObterNoticiasHome(ModeloDados db, int? idEditoria = null, string chave = "")
        {
            var noticiasEsportes = db.Noticias
                .Where(noticia =>
                    noticia.Colunista == null
                    && !noticia.excluido
                    && noticia.liberado
                    && (noticia.Editoriais.Any(editoria => editoria.id == 1))
                    && !(noticia.foto == null)
                    && (noticia.TipoDestaque == TipoDestaque.Interna1
                        || noticia.TipoDestaque == TipoDestaque.Interna2
                        || noticia.TipoDestaque == TipoDestaque.Interna3))
                    .OrderByDescending(noticia => noticia.dataAtualizacao)
                    .ThenByDescending(noticia => noticia.TipoDestaque)
                    .Take(3);

            var noticiasGerais = db.Noticias
                .Where(noticia =>
                    noticia.Colunista == null
                    && !noticia.excluido
                    && noticia.liberado
                    && !(noticia.Editoriais.Any(editoria => editoria.id == 1))
                    && !(noticia.foto == null)
                    && (noticia.TipoDestaque == TipoDestaque.Normal1
                        || noticia.TipoDestaque == TipoDestaque.Normal2
                        || noticia.TipoDestaque == TipoDestaque.Normal3))
                    .OrderByDescending(noticia => noticia.dataAtualizacao)
                    .ThenByDescending(noticia => noticia.TipoDestaque)
                    .Take(3);

            //if (idEditoria != null)
            //{
            //    noticiasQuery = noticiasQuery.Where(noticia => (noticia.Editoriais.Any(editoria => editoria.id == idEditoria)));
            //}
            //else if (string.IsNullOrWhiteSpace(chave))
            //{
            //    noticiasQuery = noticiasQuery.Where(noticia => (noticia.titulo.Contains(chave) || noticia.texto.Contains(chave) || noticia.subtitulo.Contains(chave) || (noticia.Colunista == null ? true : noticia.Colunista.nome.Contains(chave))));
            //}

            var noticiashome = noticiasEsportes.ToList();
            noticiashome.AddRange(noticiasGerais.ToList());
            
            var noticias = noticiashome.Select(noticia =>
                new NoticiaViewModel
                {
                    Id = noticia.id,
                    Titulo = noticia.titulo,
                    Chamada = noticia.chamada,
                    Texto = string.Empty,
                    Foto = string.IsNullOrEmpty(noticia.foto) ? null : url + "/admin/Conteudo/noticias/" + noticia.id + "/744x500/" + noticia.foto,
                    Data = noticia.dataAtualizacao,
                    NomeAutor = noticia.porAutor,
                    NomeCategoria = noticia.Categorias.FirstOrDefault() == null ? string.Empty : noticia.Categorias.FirstOrDefault().Titulo,
                    Url = url + "/noticia/" + noticia.url

                }).ToList();

            return noticias;
        }

        public static List<NoticiaMaisLidasViewModel> ObterNoticiasMaisVistas(int? idEditoria)
        {
            string key = primeKey + "ObterNoticiasMaisVistas:" + idEditoria + ":TNoticias:TEditoriais";

            List<NoticiaMaisLidasViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<NoticiaMaisLidasViewModel>> funcao = t => ObterNoticiasMaisVistas(db, idEditoria);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 10);
            }

            return retorno;
        }

        public static List<NoticiaMaisLidasViewModel> ObterNoticiasMaisVistas(ModeloDados db, int? idEditoria)
        {
            var lastweek = DateTime.Now.Date.AddDays(-7);
            var noticias = db.Noticias.Include("Editoriais")
                .Where(noticia =>
                        noticia.Colunista == null
                        && !noticia.excluido
                        && noticia.liberado
                        && noticia.dataCadastro >= lastweek
                        && noticia.Editoriais.Any(editoria => editoria.ativo && !editoria.excluido))
                    .OrderByDescending(noticia => noticia.visualizacao).Take(7)

                .Select(noticia => new NoticiaMaisLidasViewModel
                {
                    Id = noticia.id,
                    Chamada = noticia.chamada,
                    Foto = string.IsNullOrEmpty(noticia.foto) ? string.Empty : url + "/admin/Conteudo/noticias/" + noticia.id + "/744x500/" + noticia.foto,
                    Data = noticia.dataCadastro

                }).ToList();

            return noticias;
        }

        public static NoticiaViewModel ObterNoticia(int idNoticia)
        {
            string key = primeKey + "ObterNoticia:" + idNoticia + ":TNoticias";

            NoticiaViewModel retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, NoticiaViewModel> funcao = t => ObterNoticia(db, idNoticia);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 10);
            }

            return retorno;
        }

        public static NoticiaViewModel ObterNoticia(ModeloDados db, int idNoticia)
        {
            var noticiaViewModel = db.Noticias.Where(noticia => noticia.id == idNoticia)
                .Select(
                noticia => new NoticiaViewModel
                {
                    Id = noticia.id,
                    Titulo = noticia.titulo,
                    Chamada = noticia.chamada,
                    Texto = noticia.texto,
                    Foto = string.IsNullOrEmpty(noticia.foto) ? string.Empty : url + "/admin/Conteudo/noticias/" + noticia.id + "/744x500/" + noticia.foto,
                    Data = noticia.dataAtualizacao,
                    NomeAutor = noticia.porAutor,
                    NomeCategoria = noticia.Categorias.FirstOrDefault() == null ? string.Empty : noticia.Categorias.FirstOrDefault().Titulo,
                    Url = url + "/noticia/" + noticia.url

                }).FirstOrDefault();

            return noticiaViewModel;
        }

        public static List<NoticiaViewModel> ObterNoticiasRelacionadas(int idNoticia)
        {
            string key = primeKey + "ObterNoticiasRelacionadas:" + idNoticia + ":TNoticias";

            List<NoticiaViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<NoticiaViewModel>> funcao = t => ObterNoticiasRelacionadas(db, idNoticia);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 10);
            }

            return retorno;
        }

        public static List<NoticiaViewModel> ObterNoticiasRelacionadas(ModeloDados db, int idNoticia)
        {
            var editoriaisIds = db.Noticias.Where(noticia => noticia.id == idNoticia).SelectMany(noticia => noticia.Editoriais.Select(editorial => editorial.id)).ToArray();

            var noticiaViewModel = db.Noticias.Where(noticia => noticia.id != idNoticia && noticia.Editoriais.Any(editorial => editoriaisIds.Contains(editorial.id))).OrderByDescending(noticia => noticia.dataAtualizacao).Take(3)
                .Select(
                noticia => new NoticiaViewModel
                {
                    Id = noticia.id,
                    Titulo = noticia.titulo,
                    Chamada = noticia.chamada,
                    Texto = string.Empty,
                    Foto = string.IsNullOrEmpty(noticia.foto) ? string.Empty : url + "/admin/Conteudo/noticias/" + noticia.id + "/744x500/" + noticia.foto,
                    Data = noticia.dataAtualizacao,
                    NomeAutor = noticia.porAutor,
                    NomeCategoria = noticia.Categorias.FirstOrDefault() == null ? string.Empty : noticia.Categorias.FirstOrDefault().Titulo,
                    Url = url + "/noticia/" + noticia.url

                }).ToList();

            return noticiaViewModel;
        }
    }
}