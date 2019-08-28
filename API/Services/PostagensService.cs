using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Util;

namespace API.Services
{
    public static class PostagensService
    {
        static string url = ServerSettings.Url;
        static string primeKey = "postagens:";

        public static List<PostagemViewModel> ObterPostagens(int? idEditoria = null, int? idColunista = null, string chave = "")
        {
            string key = primeKey + "ObterPostagens:" + idEditoria + ":" + idColunista + ":" + chave + ":TNoticias";

            List<PostagemViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<PostagemViewModel>> funcao = t => ObterPostagens(db, idEditoria, idColunista, chave);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 60);
            }

            return retorno;
        }

        public static List<PostagemViewModel> ObterPostagens(ModeloDados db, int? idEditoria = null, int? idColunista = null, string chave = "")
        {
            var postagensQuery = db.Noticias.Where(postagem =>

                            postagem.Colunista != null
                            && !postagem.excluido
                            && postagem.liberado
                            );

            if (idColunista != null)
            {
                postagensQuery = postagensQuery.Where(noticia => noticia.idColunista == idColunista);
            }
            else if (!string.IsNullOrWhiteSpace(chave))
            {
                postagensQuery = postagensQuery.Where(postagem => (chave == null || postagem.titulo.Contains(chave) || postagem.texto.Contains(chave) || postagem.subtitulo.Contains(chave) || (postagem.Colunista == null ? true : postagem.Colunista.nome.Contains(chave))));
            }
            else
            {
                postagensQuery = postagensQuery.Where(noticia => noticia.Editoriais.Any(editoria => idEditoria == null || editoria.id == idEditoria));
            }

            var postagens = postagensQuery.OrderByDescending(noticia => noticia.dataAtualizacao).Take(20)
                 .Select(postagem => new PostagemViewModel
                 {
                     Id = postagem.id,
                     Titulo = postagem.titulo,
                     Chamada = postagem.chamada,
                     Texto = string.Empty,
                     Foto = string.IsNullOrEmpty(postagem.foto) ? null : url + "/admin/Conteudo/noticias/" + postagem.id + "/744x500/" + postagem.foto,
                     Data = postagem.dataAtualizacao,
                     NomeColunista = postagem.Colunista.nome,
                     Url = url + "/noticia/" + postagem.url

                 }).ToList();

            return postagens;
        }


        public static PostagemViewModel ObterPostagem(int? idPostagem = null)
        {
            string key = primeKey + "ObterPostagens:" + idPostagem + ":TNoticias";

            PostagemViewModel retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, PostagemViewModel> funcao = t => ObterPostagem(db, idPostagem);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 60);
            }

            return retorno;
        }

        public static PostagemViewModel ObterPostagem(ModeloDados db, int? idPostagem = null)
        {
            var postagemViewModel = db.Noticias.Where(postagem =>

                            postagem.Colunista != null
                            && !postagem.excluido
                            && postagem.liberado && postagem.id == idPostagem
                            ).OrderByDescending(postagem => postagem.dataAtualizacao)

                 .Select(postagem => new PostagemViewModel
                 {
                     Id = postagem.id,
                     Titulo = postagem.titulo,
                     Chamada = postagem.chamada,
                     Texto = postagem.texto,
                     Foto = string.IsNullOrEmpty(postagem.foto) ? null : url + "/admin/Conteudo/noticias/" + postagem.id + "/744x500/" + postagem.foto,
                     Data = postagem.dataAtualizacao,
                     NomeColunista = postagem.Colunista.nome,
                     Url = url + "/noticia/" + postagem.url

                 }).FirstOrDefault();

            return postagemViewModel;
        }

        public static List<PostagemViewModel> ObterPostagensRelacionadas(int? idPostagem = null)
        {
            string key = primeKey + "ObterPostagensRelacionadas:" + idPostagem + ":TNoticias";

            List<PostagemViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<PostagemViewModel>> funcao = t => ObterPostagensRelacionadas(db, idPostagem);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 60);
            }

            return retorno;
        }

        public static List<PostagemViewModel> ObterPostagensRelacionadas(ModeloDados db, int? idPostagem = null)
        {
            var colunistaId = db.Noticias.FirstOrDefault(noticia => noticia.id == idPostagem).idColunista;

            var postagemViewModel = db.Noticias.Where(postagem => postagem.id != idPostagem && !postagem.excluido && postagem.dataAtualizacao < DateTime.Now && postagem.liberado && postagem.idColunista != null && postagem.idColunista == colunistaId).OrderByDescending(postagem => postagem.dataAtualizacao).Take(3)
                .Select(
                postagem => new PostagemViewModel
                {
                    Id = postagem.id,
                    Titulo = postagem.titulo,
                    Chamada = postagem.chamada,
                    Texto = string.Empty,
                    Foto = string.IsNullOrEmpty(postagem.foto) ? string.Empty : url + "/admin/Conteudo/noticias/" + postagem.id + "/744x500/" + postagem.foto,
                    Data = postagem.dataAtualizacao,
                    NomeColunista = postagem.Colunista.nome,
                    Url = url + "/noticia/" + postagem.url
                }).ToList();

            return postagemViewModel;
        
        }

    }
}