using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Util;
using System.Data.Entity;
using Site.ViewModels;

namespace API.Services
{
    public static class AudiosService
    {
        static string primeKey = "audios:";
        public static List<AudioViewModel> ObterAudios(string chave = "")
        {
            string key = primeKey + "ObterAudios:" + chave + ":TNoticias:TAudios";

            List<AudioViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<AudioViewModel>> funcao = t => ObterAudios(db, chave);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 60);
            }

            return retorno;
        }

        public static List<AudioViewModel> ObterAudios(ModeloDados db, string chave = "")
        {
            var url = ServerSettings.Url;

            var audios = db.Noticias
                .Where(audio => !string.IsNullOrEmpty(audio.audio)
                && chave == null || audio.audio.Contains(chave)
                && (chave == null || audio.titulo.Contains(chave) || audio.texto.Contains(chave) || audio.subtitulo.Contains(chave) || (audio.Colunista == null ? true : audio.Colunista.nome.Contains(chave))))
                .OrderByDescending(noticia => noticia.dataAtualizacao).Take(10)
                .Select(noticia => new AudioViewModel { Url = url + "/Admin/conteudo/noticias/" + noticia.id + "/audio/" + noticia.audio, Titulo = noticia.titulo, Data = noticia.dataAtualizacao, Id = noticia.id })
                .AsParallel().ToList();

            audios.AddRange(db.Audios.Where(audio => !audio.Excluido && audio.Liberado && (chave == null || audio.Legenda.Contains(chave)))
                .Select(audio => 
                new AudioViewModel
                {
                    Data = audio.DataCadastro,
                    Url = url + "/Admin/conteudo/audios/" + audio.Id + "/" + audio.Url,
                    Id = audio.Id,
                    Titulo = audio.Legenda
                }).OrderByDescending(noticia => noticia.Data).Take(10).ToList());

            return audios;
        }

        public static List<CategoriaAudioViewModel> ObterCategorias()
        {
            string key = primeKey + "ObterCategorias:TCategoriasAudios";

            Func<object, List<CategoriaAudioViewModel>> funcao = t => ObterCategoriasDB();
            List<CategoriaAudioViewModel> retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 60);

            return retorno;
        }

        private static List<CategoriaAudioViewModel> ObterCategoriasDB()
        {
            using (var db = new ModeloDados())
            {
                return db.CategoriasAudios
                    .Where(x => x.Liberado && !x.Excluido)
                    .Select(categoria => new CategoriaAudioViewModel
                    {
                        Id = categoria.Id,
                        Descricao = categoria.Descricao,
                        DataCadastro = categoria.DataCadastro
                    })
                    .OrderBy(x => x.Descricao).ToList();
            }
        }

        public static CentralAudioViewModel ObterAudioCategoria(int? categoriaId) 
        {
            string key = primeKey + "ObterAudioCategoria:" + categoriaId + ":TCategoriasAudios:TNoticias";

            Func<object, CentralAudioViewModel> funcao = t => ObterAudioCategoriaDB(categoriaId);
            CentralAudioViewModel retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 60);

            return retorno;
        }
        private static CentralAudioViewModel ObterAudioCategoriaDB(int? categoriaId)
        {
            var url = ServerSettings.Url;
            using (var db = new ModeloDados())
            {
                var categoria = db.CategoriasAudios.Where(cat => cat.Id == categoriaId).FirstOrDefault();
                var audios = db.Audios.Where(audio => !audio.Excluido && audio.Liberado
                                                && audio.Colecao.Categoria.Id == categoriaId)
                    .OrderByDescending(audio => audio.DataCadastro)
                    .Take(20)
                    .Select(audio => new AudioViewModel
                    {
                        Url = url + "/Admin/conteudo/audios/" + audio.Id + "/" + audio.Url,
                        Titulo = audio.Colecao.Titulo,
                        Data = audio.DataCadastro,
                        Id = audio.Id
                    }).AsParallel()
                    .ToList();
                var central = new CentralAudioViewModel();
                central.Audios = audios;
                central.DataCadastro = categoria.DataCadastro;
                central.Descricao = categoria.Descricao;

                return central;
            }
        }
    }
}