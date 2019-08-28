using PagedList;
using Site.Services;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class EspeciaisController : BaseController
    {
        string primeKey = "especiais:";

        public EspeciaisController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Metodo parlamento aberto
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> ParlamentoAberto()
        {

            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var parlamento = await db.Noticias.Include(x=>x.Editoriais).Where(x => !x.excluido && x.liberado &&
            x.dataAtualizacao >= hoje && x.dataAtualizacao < DateTime.Now &&
            x.Editoriais.Any(p => !p.excluido && p.especial && p.chave == "parlamento-aberto")).OrderByDescending(x => x.dataAtualizacao).ToListAsync();

            return View(parlamento);
        }
        
        ///Especiais/MaisParlamento/
        [HttpPost, ActionName("MaisParlamento")]
        public PartialViewResult MaisParlamento(int ultimoId, int primeiroId, bool antigo = true)
        {
            var itens = db.Noticias.Include(x => x.Editoriais).Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now &&
            x.Editoriais.Any(p => !p.excluido && p.especial && p.chave == "parlamento-aberto"));


            if (!antigo)
            {
                itens = itens.Where(x => x.id > primeiroId);
            }
            else
            {
                itens = itens.Where(x => x.id < ultimoId);
            }

            var lista = itens.OrderByDescending(x => x.dataAtualizacao).ToList();

            return PartialView("MaisParlamento", lista);


        }


        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page;palavraChave;dataInicio;dataFinal;categoria")]
        public ActionResult VendaIngressos(int? page, string palavraChave, string dataInicio, string dataFinal, int categoria = 0)
        {
            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            var editoria = db.Editoriais.FirstOrDefault(x => x.ativo && !x.excluido && x.especial && x.chave == "venda-de-ingressos");

            if (editoria == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var noticias = db.Noticias.Include(x => x.Editoriais).Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now &&
            x.Editoriais.Any(p => p.ativo && !p.excluido && p.chave == "venda-de-ingressos")
            && x.data.Value >= hoje
            ).OrderByDescending(x => x.dataAtualizacao).AsQueryable();

            int pageNumber = (page ?? 1);


            if (!string.IsNullOrEmpty(palavraChave))
            {
                noticias = noticias.Where(n => n.titulo.ToLower().Contains(palavraChave.ToLower()) || n.chamada.Contains(palavraChave)
                || n.subtitulo.ToLower().Contains(palavraChave));

            }

            if (categoria > 0)
            {
                noticias = noticias.Where(n => n.Categorias.Any(p => p.Id == categoria));
            }


            if (!string.IsNullOrEmpty(dataInicio))
            {
                DateTime DataInicio = DateTime.Parse(dataInicio).AddHours(0).AddMinutes(0).AddSeconds(0);
                noticias = noticias.Where(d => d.dataCadastro >= DataInicio);
            }

            if (!string.IsNullOrEmpty(dataFinal))
            {
                DateTime DataFim = DateTime.Parse(dataFinal).AddHours(23).AddMinutes(59).AddSeconds(59);

                noticias = noticias.Where(d => d.dataCadastro <= DataFim);
            }

            var pagination = noticias.ToPagedList(pageNumber, 12);

            var viewModel = new ModeloEspecialViewModel
            {
                Editoria = editoria,
                paginacao = pagination
            };


            ViewBag.categoria = new SelectList(db.Categorias.Include(x => x.Editoriais).Where(x => !x.Excluido && x.Editoriais.chave == "venda-de-ingressos"), "Id", "Titulo", (categoria > 0 ? categoria : 0));

            return View(viewModel);


        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page;palavraChave;dataInicio;dataFinal;categoria")]
        public ActionResult FeiraVeiculos(int? page, string palavraChave, string dataInicio, string dataFinal, int categoria = 0)
        {
            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            var editoria = db.Editoriais.FirstOrDefault(x => x.ativo && !x.excluido && x.especial && x.chave == "feira-de-veiculos");

            if (editoria == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var noticias = db.Noticias.Include(x => x.Editoriais).Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now &&
            x.Editoriais.Any(p => p.ativo && !p.excluido && p.chave == "feira-de-veiculos")
            && x.data.Value >= hoje
            ).OrderByDescending(x => x.dataAtualizacao).AsQueryable();

            int pageNumber = (page ?? 1);


            if (!string.IsNullOrEmpty(palavraChave))
            {
                noticias = noticias.Where(n => n.titulo.ToLower().Contains(palavraChave.ToLower()) || n.chamada.Contains(palavraChave)
                || n.subtitulo.ToLower().Contains(palavraChave));

            }

            if (categoria > 0)
            {
                noticias = noticias.Include(x => x.Categorias).Where(n => n.Categorias.Any(p => p.Id == categoria));
            }

            if (!string.IsNullOrEmpty(dataInicio))
            {
                DateTime DataInicio = DateTime.Parse(dataInicio).AddHours(0).AddMinutes(0).AddSeconds(0);
                noticias = noticias.Where(d => d.dataCadastro >= DataInicio);
            }

            if (!string.IsNullOrEmpty(dataFinal))
            {
                DateTime DataFim = DateTime.Parse(dataFinal).AddHours(23).AddMinutes(59).AddSeconds(59);

                noticias = noticias.Where(d => d.dataCadastro <= DataFim);
            }


            var pagination = noticias.ToPagedList(pageNumber, 12);

            var viewModel = new ModeloEspecialViewModel
            {
                Editoria = editoria,
                paginacao = pagination
            };


            ViewBag.categoria = new SelectList(db.Categorias.Include(x => x.Editoriais).Where(x => !x.Excluido && x.Editoriais.chave == "feira-de-veiculos"), "Id", "Titulo", (categoria > 0 ? categoria : 0));

            return View(viewModel);


        }

        /// <summary>
        /// Metodo que sera usado para gastronomia/turismo
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "chave")]
        public async Task<ActionResult> ModeloSecoes(string chave)
        {
            var editoria = await db.Editoriais.FirstOrDefaultAsync(x => x.ativo && !x.excluido && x.especial && x.chave == chave);

            if (editoria == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var hoje = DateTime.Now;

            var banner = await db.Banners.Include(x => x.AreaBanner).Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-especial-interna" && w.Ativo.Value)).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

            var noticias = await db.Noticias.Include(x => x.Editoriais).Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now &&
            x.Editoriais.Any(p => p.ativo && !p.excluido && p.chave == chave))
            .OrderByDescending(x => x.dataAtualizacao)
            .ToListAsync();

            var view = new ModeloEspecialSecoesViewModel
            {
                Banner = banner,
                Editoria = editoria,
                Noticias = noticias
            };

            return View(view);
        }

        /// <summary>
        /// Metodo BH fiscaliza
        /// </summary>
        /// <param name="page"></param>
        /// <param name="palavraChave"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFinal"></param>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page;palavraChave;dataInicio;dataFinal;categoria")]
        public async Task<ActionResult> BHFiscaliza(int? page, string palavraChave, string dataInicio, string dataFinal, int categoria = 0)
        {  
            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            //var editorial = await db.Editoriais.Where(x => !x.excluido && x.chave =="bh-fiscaliza" && x.ativo).FirstOrDefaultAsync();
            var destaques = await db.Noticias.Include(x => x.Editoriais).Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now && !x.plantao && x.destaqueEditoria && x.Editoriais.Any(y => y.chave ==  "bh-fiscaliza" && y.ativo)).OrderByDescending(p => p.dataAtualizacao)
                .Select(noticia => new NoticiasViewModel
                {
                    Chamada = noticia.chamada,
                    DataCadastro = noticia.dataCadastro,
                    Editoriais = noticia.Editoriais.Select(ed => new EditorialNoticiaViewModel
                    {
                        Ativo = ed.ativo,
                        Chave = ed.chave,
                        Especial = ed.especial,
                        Esporte = ed.esportes,
                        Excluido = ed.excluido,
                        Nome = ed.nome

                    }),
                    Foto = noticia.foto,
                    FotoCredito = noticia.fotoCredito,
                    Id = noticia.id,
                    Subtitulo = noticia.subtitulo,
                    TipoDestaque = noticia.TipoDestaque,
                    Titulo = noticia.titulo,
                    TituloCapa = noticia.TituloCapa,
                    Url = noticia.url,
                    Audio = noticia.audio,
                    Liberado = noticia.liberado,
                    Excluido = noticia.excluido,
                    IdColunista = noticia.idColunista,
                    Plantao = noticia.plantao,
                    PorAutor = noticia.porAutor
                })
                .Take(5).ToListAsync();
            var idsDestaques = destaques.Select(x => x.Id).ToArray();

            var banner1 = await db.Banners.Include(x => x.AreaBanner).Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-editoria-interna" && w.Ativo.Value)).FirstOrDefaultAsync();

            var editorial = await db.Editoriais.Include(x => x.Noticias).Where(x => !x.excluido && x.chave == "bh-fiscaliza" && x.ativo)
                .Select(e => new EditorialViewModel
                {
                    ativo = e.ativo,
                    excluido = e.excluido,
                    chave = e.chave,
                    nome = e.nome,
                    id = e.id,
                    especial = e.especial,
                    esportes = e.esportes,
                    Noticias = e.Noticias.Where(x => x.liberado && !x.excluido && x.idColunista == null && !idsDestaques.Contains(x.id)).Select(noticia => new NoticiasViewModel

                    {
                        Chamada = noticia.chamada,
                        DataCadastro = noticia.dataCadastro,
                        Editoriais = noticia.Editoriais.Select(ed => new EditorialNoticiaViewModel
                        {
                            Ativo = ed.ativo,
                            Chave = ed.chave,
                            Especial = ed.especial,
                            Esporte = ed.esportes,
                            Excluido = ed.excluido,
                            Nome = ed.nome

                        }),
                        Foto = noticia.foto,
                        FotoCredito = noticia.fotoCredito,
                        Id = noticia.id,
                        Subtitulo = noticia.subtitulo,
                        TipoDestaque = noticia.TipoDestaque,
                        Titulo = noticia.titulo,
                        TituloCapa = noticia.TituloCapa,
                        Url = noticia.url,
                        Audio = noticia.audio,
                        Liberado = noticia.liberado,
                        Excluido = noticia.excluido,
                        IdColunista = noticia.idColunista,
                        Plantao = noticia.plantao
                    }).Take(1000).ToList()
                }).FirstOrDefaultAsync();

            if (editorial == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var query = editorial.Noticias.Where(x => x.Liberado && !x.Excluido && !idsDestaques.Contains(x.Id));

            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(palavraChave))
            {
                query = query.Where(n => n.Titulo.Contains(palavraChave.ToLower()) || n.Chamada.Contains(palavraChave) || n.Subtitulo.Contains(palavraChave));
            }

            //if (categoria > 0)
            //{
            //    query = query.Where(n => n.Categorias.Any(x => x.Id == categoria));
            //}



            if (!string.IsNullOrEmpty(dataInicio))
            {
                DateTime DataInicio = DateTime.Parse(dataInicio).AddHours(0).AddMinutes(0).AddSeconds(0);                
                query = query.Where(d => d.DataCadastro >= DataInicio);
            }

            if (!string.IsNullOrEmpty(dataFinal))
            {
                DateTime DataFim = DateTime.Parse(dataFinal).AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(d => d.DataCadastro <= DataFim);
            }


            var viewModel = new EditoriasViewModel
            {
                Editoria = editorial,
                Destaques = destaques,
                Banner = banner1,
                paginacao = query.ToList().ToPagedList(pageNumber, 8)
            };


            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.EditoriaId == editorial.id), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));

            return View(viewModel);

        }


        [HttpPost, ActionName("VerMaisBloco")]
        public PartialViewResult VerMaisBloco(int id)
        {
            var itens = db.Blocos.Where(x => !x.Excluido && x.Ativo && x.Id == id).FirstOrDefault();
            
            return PartialView("VerMaisBloco", itens);


        }


       [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page;bloco")]
        public ActionResult Carnaval(int? page, string bloco)
        {
            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            
            var banner = db.Banners.Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-especial-carnaval" && w.Ativo.Value)).FirstOrDefault();

            var noticiasDestaque = db.Noticias.Where(x => !x.excluido  && x.liberado && x.dataAtualizacao < DateTime.Now && x.dataAtualizacao < DateTime.Now && x.destaqueEditoria && 
            x.Editoriais.Any(p => p.ativo && !p.excluido && p.chave == "carnaval-2017"))
            .OrderByDescending(x => x.dataAtualizacao)
            .ToList();
            

            var blocos = db.Blocos.Where(x => !x.Excluido && x.Ativo).OrderBy(x => x.Data).AsQueryable();

            var noticias = db.Noticias.Where(x => !x.excluido && x.liberado && !x.destaqueEditoria &&
            x.Editoriais.Any(p => p.ativo && !p.excluido && p.chave == "carnaval-2017"))
            .OrderByDescending(x => x.dataAtualizacao)
            .ToPagedList(page?? 1, 3);

            if (blocos == null)
            {
                return RedirectToAction("Index", "Home");
            }

            
            int pageNumber = (page ?? 1);

            
            if (!string.IsNullOrEmpty(bloco))
            {
                blocos = blocos.Where(n => n.Chave == bloco);
            }

            var pagination = blocos.ToPagedList(pageNumber, 8);

            var viewModel = new ModeloEspecialCarnavalViewModel
            {
                Banner = banner,
                NoticiasDestaques = noticiasDestaque,
                Noticias = noticias,
                paginacao = pagination
            };


            ViewBag.bloco = new SelectList(db.Blocos.Where(x => !x.Excluido && x.Ativo), "Chave", "Nome", (!string.IsNullOrEmpty(bloco) ? bloco : null));

            return View(viewModel);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult T1(string chave, int? page)
        {
            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            string key = primeKey + "T1:Noticias:" + chave;

            Func<object, List<NoticiaEspecialViewModel>> funcao = t => NoticiasEditoriaCached(chave);
            var noticiasEditoria = RedisService.GetOrSetToRedis(key, funcao, 60);

            key = primeKey + "T1:Editoria:" + chave;

            Func<object, EditoriaEspecialViewModel> funcao2 = t => EditoriaEspecial(chave);
            var editoria = RedisService.GetOrSetToRedis(key, funcao2, 600);

            var noticiasDestaque = noticiasEditoria.Where(x => x.Destaque)
                .Take(1).ToList();

            int pageNumber = (page ?? 1);

            var pagination = noticiasEditoria.Where(q => !noticiasDestaque.Select(n => n.Id).Contains(q.Id));

            var viewModel = new EspeciaisViewModel
            {
                Editoria = editoria,
                destaques = noticiasDestaque,
                noticias = pagination.ToPagedList(pageNumber, 6)
            };
            return View(viewModel);
        }

        /// <summary>
        /// Metodo Pampulha
        /// </summary>
        /// <returns></returns>
        /// 
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> Pampulha()
        {
            var editoria = await db.Editoriais.FirstOrDefaultAsync(x => x.ativo && !x.excluido && x.especial && x.chave == "pampulha");

            if (editoria == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            var banner = await db.Banners.Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-especial-interna" && w.Ativo.Value)).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

            var noticiasDestaque = await db.Noticias.Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now && x.destaqueEditoria &&
            x.Editoriais.Any(p => p.ativo && !p.excluido && p.chave == "pampulha"))
            .OrderByDescending(x => x.dataAtualizacao)
            .ToListAsync();


            var noticias = await db.Noticias.Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now && !x.destaqueEditoria &&
            x.Editoriais.Any(p => p.ativo && !p.excluido && p.chave == "pampulha"))
            .OrderByDescending(x => x.dataAtualizacao)
            .ToListAsync();

            var view = new ModeloEspecialPampulhaViewModel
            {
                Banner = banner,
                Editoria = editoria,
                Noticias = noticias,
                NoticiasDestaques = noticiasDestaque
            };

            return View(view);
            
            
        }

        public ActionResult ObterMaisNoticias(string chave, int? page)
        {
            var tiraBotaoVerMais = false;

            string key = primeKey + "ObterMaisNoticias:" + chave;

            Func<object, List<NoticiaEspecialViewModel>> funcao = t => NoticiasEditoriaCached(chave);
            var noticiasEspeciais = RedisService.GetOrSetToRedis(key, funcao, 60);

            var noticias = noticiasEspeciais.Where(n => !n.Destaque)
            .OrderByDescending(x => x.dataAtualizacao)
            .ToPagedList(page ?? 1, 6);

            if (noticias.Count == 6)
            {
                var temNovaPagina = noticias.Count > 0;
                if (!temNovaPagina)
                {
                    tiraBotaoVerMais = true;
                }
            } else
            {
                tiraBotaoVerMais = true;
            }

            var retorno = RenderRazorViewToString("_RioDasVelhasNoticiasPartial", noticias);

            if (tiraBotaoVerMais)
            {
                retorno += "<script>$('#btnVerMais').hide();</script>";
            }

            return Json(new { data = retorno });
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult ReportagensEspeciais()
        {
            string key = primeKey + "ReportagensEspeciais";

            Func<object, List<ReportagemEspecialViewModel>> funcao = t => ReportagensEspeciaisDB();
            var reportagensEspeciais = RedisService.GetOrSetToRedis(key, funcao, 60);

            var retorno = new ReportagensEspeciaisViewModel {
                Reportagens = reportagensEspeciais
            };

            return View(retorno);
        }

        private List<ReportagemEspecialViewModel> ReportagensEspeciaisDB()
        {
            return db.Editoriais.Where(e => e.ativo && !e.excluido && e.especial && !e.canal)
                .Select(e => new ReportagemEspecialViewModel {
                    Id = e.id,
                    Action = e.Especiais_Modelos.Action,
                    Descricao = e.Descricao,
                    FotoCapa = e.FotoCapa,
                    nome = e.nome,
                    Chave = e.chave
                }).ToList();
        }

        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult T2(string chave, int? page, string palavraChave = "", string dataInicio = "", string dataFinal = "")
        {
            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            string key = primeKey + "T2:Noticias:" + chave;

            Func<object, List<NoticiaEspecialViewModel>> funcao = t => NoticiasEditoriaCached(chave);
            var noticiasEditoria = RedisService.GetOrSetToRedis(key, funcao, 60);

            key = primeKey + "T2:Editoria:" + chave;

            Func<object, EditoriaEspecialViewModel> funcao2 = t => EditoriaEspecial(chave);
            var editoria = RedisService.GetOrSetToRedis(key, funcao2, 600);

            if (!string.IsNullOrEmpty(palavraChave))
            {
                noticiasEditoria = noticiasEditoria.Where(n =>
                    n.titulo.ToLower().Contains(palavraChave.ToLower()) ||
                    n.texto.ToLower().Contains(palavraChave.ToLower()) ||
                    n.subTitulo.ToLower().Contains(palavraChave.ToLower()) ||
                    n.tituloCapa.ToLower().Contains(palavraChave.ToLower()) ||
                    n.chamada.ToLower().Contains(palavraChave.ToLower())
                ).ToList();
            }
            if (!string.IsNullOrEmpty(dataInicio))
            {
                DateTime DataInicio = DateTime.Parse(dataInicio).AddHours(0).AddMinutes(0).AddSeconds(0);
                noticiasEditoria = noticiasEditoria.Where(d => d.dataCadastro >= DataInicio).ToList();
            }

            if (!string.IsNullOrEmpty(dataFinal))
            {
                DateTime DataFim = DateTime.Parse(dataFinal).AddHours(23).AddMinutes(59).AddSeconds(59);
                noticiasEditoria = noticiasEditoria.Where(d => d.dataCadastro <= DataFim).ToList();
            }

            var noticiasDestaque = noticiasEditoria.Where(x => x.Destaque)
                .Take(2).ToList();

            int pageNumber = (page ?? 1);

            var pagination = noticiasEditoria.Where(q => !noticiasDestaque.Select(n => n.Id).Contains(q.Id));

            var viewModel = new EspeciaisViewModel
            {
                Editoria = editoria,
                destaques = noticiasDestaque,
                noticias = pagination.ToPagedList(pageNumber, 9)
            };

            return View(viewModel);
        }

        private List<NoticiaEspecialViewModel> NoticiasEditoriaCached(string chave)
        {
            return db.Noticias.Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now && x.dataAtualizacao < DateTime.Now &&
                x.Editoriais.Any(p => p.ativo && !p.excluido && p.chave == chave)).Select(n => new NoticiaEspecialViewModel
                {
                    Destaque = n.destaqueEditoria,
                    Id = n.id,
                    chamada = n.chamada,
                    foto = n.foto,
                    titulo = n.titulo,
                    videoYoutube = n.videoYoutube,
                    subTitulo = n.subtitulo,
                    texto = n.texto,
                    dataCadastro = n.dataCadastro,
                    dataAtualizacao = n.dataAtualizacao,
                    Url = n.url,
                    FotoCredito = n.fotoCredito,
                    tituloCapa = n.TituloCapa,
                }).OrderByDescending(x => x.dataAtualizacao).ToList();
        }
        private EditoriaEspecialViewModel EditoriaEspecial(string chave)
        {
            return db.Editoriais.Where(p => p.ativo && !p.excluido && p.chave == chave)
                .Select(e => new EditoriaEspecialViewModel
                {
                    Id = e.id,
                    nome = e.nome,
                    action = e.Especiais_Modelos.Action,
                    Chave = e.chave
                }).FirstOrDefault();
        }
    }

    public class MaisItens
    {
        public int id { get; set; }
        public string imagem { get; set; }
        public string texto { get; set; }
    }
}