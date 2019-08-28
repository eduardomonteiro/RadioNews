using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Site
{
    public class RouteConfig
    {
        public static object MVC { get; private set; }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "PaginaAudios",
                url: "audios/{tipo}/{id}",
                defaults: new { controller = "Audios", action = "Index" }
            );

            #region colunistas
            routes.MapRoute(
               name: "ColunistaDetalhes",
               url: "blog/{colunistaSlug}",
              defaults: new { controller = "Colunistas", action = "Detalhes", colunistaSlug = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "ColunistaMateria",
             url: "blog/{colunistaSlug}/{chave}",
             defaults: new { controller = "Colunistas", action = "Materia", chave = UrlParameter.Optional, colunistaSlug = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "Colunistas",
              url: "colunistas",
              defaults: new { controller = "Colunistas", action = "Index" }
           );

            #endregion

            #region central de audio
            routes.MapRoute(
               name: "horoscopo",
               url: "central-de-audio/horoscopo",
               defaults: new { controller = "Podcast", action = "Horoscopo", page = UrlParameter.Optional }
            );



            routes.MapRoute(
               name: "podcast",
               url: "central-de-audio/{page}",
               defaults: new { controller = "Podcast", action = "Index", page = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "categoriaAudios",
            url: "central-de-audio/{id}/{slug}",
            defaults: new { controller = "Podcast", action = "CategoriaAudio", id = UrlParameter.Optional, slug = UrlParameter.Optional }
            );
            #endregion

            #region live streaming
            routes.MapRoute(
            name: "liveStreaming",
            url: "live/",
            defaults: new { controller = "LiveStreaming", action = "Index" }
            );
            #endregion

            #region sobre
            routes.MapRoute(
                name: "sobre",
                url: "sobre/trofeu-guara/",
                defaults: new { controller = "Sobre", action = "TrofeuGuara" }
             );
            routes.MapRoute(
                name: "copa-CompanyName",
                url: "sobre/copa-CompanyName/",
                defaults: new { controller = "Sobre", action = "CopaCompanyName" }
             );

            routes.MapRoute(
                name: "radio-CompanyName",
                url: "sobre/radio-CompanyName/",
                defaults: new { controller = "Sobre", action = "QuemSomos" }
             );

            routes.MapRoute(
                name: "midiakit",
                url: "sobre/midia-kit/",
                defaults: new { controller = "Sobre", action = "MidiaKit" }
             );

            routes.MapRoute(
               name: "equipe",
               url: "sobre/equipe/",
               defaults: new { controller = "Sobre", action = "Equipe" }
            );
            #endregion

            #region Editoria


            routes.MapRoute(
    name: "EditoriaOuvintes",
    url: "editoria/ouvinte-no-ar/{page}",
    defaults: new { controller = "Editorias", action = "Ouvintes", page = UrlParameter.Optional }
     );


            routes.MapRoute(
           name: "EditoriaSemPage",
           url: "editoria/{chave}",
           defaults: new { controller = "Editorias", action = "Index", chave = UrlParameter.Optional }
            );


            routes.MapRoute(
            name: "Editoria",
            url: "editoria/{chave}/{page}",
            defaults: new { controller = "Editorias", action = "Index", chave = UrlParameter.Optional, page = UrlParameter.Optional }
             );

            #endregion

            #region bastidores
            routes.MapRoute(
            name: "bastidoresvideos",
            url: "bastidores-da-redacao/videos",
            defaults: new { controller = "Bastidores", action = "Videos" }
             );

            routes.MapRoute(
            name: "bastidoresfoto",
            url: "bastidores-da-redacao/{chaveBastidor}",
            defaults: new { controller = "Bastidores", action = "Fotos", chaveBastidor = UrlParameter.Optional }
             );
            #endregion

            #region programacao
            routes.MapRoute(
            name: "programacao",
            url: "programacao/{dia}",
            defaults: new { controller = "Programacao", action = "Index", dia = UrlParameter.Optional }
             );

            #endregion
            
            #region faleconosco


            routes.MapRoute(
             name: "DenunciaDetails",
             url: "denuncia/{id}/{slug}",
             defaults: new { controller = "FaleConosco", action = "DetalhesOuvintes", id = UrlParameter.Optional, slug = UrlParameter.Optional }
             );

            routes.MapRoute(
                 name: "faleConosco",
                 url: "contato",
                 defaults: new { controller = "FaleConosco", action = "Index" }
             );

            routes.MapRoute(
             name: "Denuncia",
             url: "envie-sua-denuncia",
             defaults: new { controller = "FaleConosco", action = "Denuncia" }
             );




            #endregion

            #region noticias


            routes.MapRoute(
              name: "noticiaEsportes",
              url: "noticias/esporte",
              defaults: new { controller = "Noticias", action = "esportes", page = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "NoticiaIndex",
              url: "noticias/jornalismo/{page}",
              defaults: new { controller = "Noticias", action = "Index", page = UrlParameter.Optional }
           );


            routes.MapRoute(
              name: "NoticiaDetalhes",
              url: "noticia/{chave}",
              defaults: new { controller = "Noticias", action = "Detalhes", chave = UrlParameter.Optional }
           );

            #endregion

            #region especiais


            // routes.MapRoute(
            //   name: "EspecialIndex",
            //   url: "especial-/{chave}",
            //   defaults: new { controller = "Especiais", action = "Index", chave = UrlParameter.Optional }
            //);


            routes.MapRoute(
              name: "EspecialParlamento",
              url: "especial/parlamento-aberto",
              defaults: new { controller = "Especiais", action = "ParlamentoAberto" }
           );

            routes.MapRoute(
              name: "BHFiscaliza",
              url: "especial/bh-fiscaliza",
              defaults: new { controller = "Especiais", action = "BHFiscaliza" }
           );

            routes.MapRoute(
              name: "Carnaval",
              url: "especial/carnaval",
              defaults: new { controller = "Especiais", action = "Carnaval" }
           );

            routes.MapRoute(
             name: "pampulha",
             url: "especial/pampulha",
             defaults: new { controller = "Especiais", action = "Pampulha" }
          );

            routes.MapRoute(
                name: "VendaIngresso",
                url: "especial/venda-de-ingressos",
                defaults: new { controller = "Especiais", action = "VendaIngressos" }
             );

            routes.MapRoute(
                name: "FeiraVeiculos",
                url: "especial/feirao-de-domingo",
                defaults: new { controller = "Especiais", action = "FeiraVeiculos" }
             );


            routes.MapRoute(
                name: "ajaxBlocoDetalhes",
                url: "especiais/VerMaisBloco/{id}",
                defaults: new { controller = "Especiais", action = "VerMaisBloco", id = UrlParameter.Optional }
             );



            routes.MapRoute(
                name: "ajaxParlamento",
                url: "especiais/MaisParlamento",
                defaults: new { controller = "Especiais", action = "MaisParlamento" }
             );


            //routes.MapRoute(
            //    name: "modelosSimples",
            //    url: "especiais/{chave}",
            //    defaults: new { controller = "Especiais", action = "ModeloSimples", chave = UrlParameter.Optional }
            // );

            routes.MapRoute(
              name: "modelosSecos",
              url: "especial/{chave}",
              defaults: new { controller = "Especiais", action = "ModeloSecoes", chave = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "reportagensEspeciais",
              url: "reportagens-especiais",
              defaults: new { controller = "Especiais", action = "ReportagensEspeciais" }
            );

            routes.MapRoute(
              name: "reportagemEspecial",
              url: "reportagem-especial/{action}/{chave}",
              defaults: new { controller = "Especiais", chave = UrlParameter.Optional }
            );


            #endregion

            #region Busca

            routes.MapRoute(
              name: "Busca",
              url: "busca/{busca}/{page}",
              defaults: new { controller = "Busca", action = "Index", page = UrlParameter.Optional, busca = UrlParameter.Optional }
           );
            #endregion

            #region esportes

            routes.MapRoute(
            name: "cruzeiro",
            url: "futebol/cruzeiro",
            defaults: new { controller = "Clubes", action = "Index", id = "cruzeiro" }
            );


            routes.MapRoute(
            name: "atletico",
            url: "futebol/atletico",
            defaults: new { controller = "Clubes", action = "Index", id = "atletico" }
            );


            routes.MapRoute(
            name: "america",
            url: "futebol/america",
            defaults: new { controller = "Clubes", action = "Index", id = "america" }
            );


            routes.MapRoute(
            name: "brasil",
            url: "futebol/brasil",
            defaults: new { controller = "Clubes", action = "Index", id = "brasil" }
            );

            routes.MapRoute(
            name: "amistoso",
            url: "futebol/campeonato/amistoso",
            defaults: new { controller = "Campeonatos", action = "Amistoso" }
            );

            routes.MapRoute(
            name: "serieA",
            url: "futebol/campeonato/campeonato-brasileiro-serie-a",
            defaults: new { controller = "Campeonatos", action = "SerieA" }
            );

            routes.MapRoute(
            name: "serieB",
            url: "futebol/campeonato/campeonato-brasileiro-serie-b",
            defaults: new { controller = "Campeonatos", action = "SerieB" }
            );

            routes.MapRoute(
            name: "mineiro",
            url: "futebol/campeonato/campeonato-mineiro",
            defaults: new { controller = "Campeonatos", action = "Mineiro" }
            );

            routes.MapRoute(
            name: "copaAmerica",
            url: "futebol/campeonato/copa-america",
            defaults: new { controller = "Campeonatos", action = "CopaAmerica" }
            );

            routes.MapRoute(
            name: "copaBrasil",
            url: "futebol/campeonato/copa-do-brasil",
            defaults: new { controller = "Campeonatos", action = "CopaDoBrasil" }
            );

            routes.MapRoute(
            name: "liberta",
            url: "futebol/campeonato/copa-libertadores",
            defaults: new { controller = "Campeonatos", action = "Libertadores" }
            );

            routes.MapRoute(
            name: "liga",
            url: "futebol/campeonato/primeira-liga",
            defaults: new { controller = "Campeonatos", action = "PrimeiraLiga" }
            );


            routes.MapRoute(
            name: "eliminatorias",
            url: "futebol/campeonato/eliminatorias-da-copa",
            defaults: new { controller = "Campeonatos", action = "Eliminatorias" }
            );



            routes.MapRoute(
            name: "olimpiadas",
            url: "futebol/campeonato/olimpiadas",
            defaults: new { controller = "Campeonatos", action = "Olimpiadas" }
            );



            #endregion

            #region CompanyName-no-ponto

            routes.MapRoute(
            name: "CompanyNamenoponto",
            url: "CompanyName-no-ponto",
            defaults: new { controller = "Eventos", action = "Index" }
            );

            routes.MapRoute(
            name: "evento",
            url: "CompanyName-no-ponto/evento/{id}/{slug}",
            defaults: new { controller = "Eventos", action = "Detalhes", id = UrlParameter.Optional, slug = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "acontecimento",
            url: "CompanyName-no-ponto/acontecimento/{id}/{slug}",
            defaults: new { controller = "Eventos", action = "Acontecimento", id = UrlParameter.Optional, slug = UrlParameter.Optional }
            );

            #endregion
            
            #region galerias
            routes.MapRoute(
            name: "galeriasvideos",
            url: "galerias/videos",
            defaults: new { controller = "Galerias", action = "Videos" }
             );

            routes.MapRoute(
            name: "GaleriasFotos",
            url: "galeria-de-fotos/{chave}",
            defaults: new { controller = "Galerias", action = "Index", chave = UrlParameter.Optional }
             );
            #endregion

            #region Comentarios
            //routes.MapRoute(
            //    name: "CompartilharFacebook",
            //    url: "{controller}/{action}/{idComentario}",
            //    defaults: new { controller = "Comentarios", action = "ComentarioCompartilhado", idComentario = UrlParameter.Optional }
            //);
            #endregion

            #region notFound


            #endregion

            #region SiteAntigo
            routes.MapRoute(
                name: "AoVivo",
                url: "aovivo/",
                defaults: new { controller = "Home", action = "Player", chave = "belo-horizonte" }
            );
            routes.MapRoute(
                name: "Cadu Done Feed",
                url: "cadudone/feed",
                defaults: new { controller = "Colunistas", action = "Detalhes", colunistaSlug = "cadu-done" }
            );
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
