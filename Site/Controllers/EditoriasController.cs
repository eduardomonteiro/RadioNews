using PagedList;
using Site.Enums;
using Site.Models;
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
    public class EditoriasController : BaseController
    {
        public EditoriasController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: Editorias
        [OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "chave;page;palavraChave;dataInicio;dataFinal;categoria")]
        public async Task<ActionResult> Index(string chave, int? page, string palavraChave, string dataInicio, string dataFinal, int categoria = 0)
        {
            var hoje = DateTime.Now;
            var queryDestaques = db.Noticias.Include(x => x.Editoriais).Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now && x.idColunista == null && !x.plantao && x.Editoriais.Any(y => y.chave == chave && y.ativo)).OrderByDescending(p => p.dataAtualizacao)
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
                }).OrderByDescending(y => y.DataCadastro).Take(30);

            var destaques = await DestaquesEditoria(queryDestaques, chave, new int[] { });

            var idsDestaques = new int[] { };
            if (destaques != null && destaques.Any())
            {
                idsDestaques = destaques.Select(x => x.Id).ToArray();
            }

            var editorial = await db.Editoriais.Include(x => x.Noticias).Where(x => !x.excluido && x.chave == chave && x.ativo)

                .Select(e => new EditorialViewModel
                {
                    ativo = e.ativo,
                    excluido = e.excluido,
                    chave = e.chave,
                    nome = e.nome,
                    id = e.id,
                    especial = e.especial,
                    esportes = e.esportes,
                    Noticias = e.Noticias.Where(x => x.liberado && !x.excluido && x.dataAtualizacao < DateTime.Now && x.idColunista == null && !idsDestaques.Contains(x.id)).OrderByDescending(p => p.dataAtualizacao).Select(noticia => new NoticiasViewModel
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
                        PorAutor = noticia.porAutor,
                        Categorias = noticia.Categorias.Select(cat => new CategoriasViewModel
                        {
                            Id = cat.Id
                        })
                    }).OrderByDescending(y => y.DataCadastro).Take(100).ToList()
                }).FirstOrDefaultAsync();

            var banner1 = await db.Banners.Include(x => x.AreaBanner).FirstOrDefaultAsync(x => x.Liberado && !x.Excluido && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-editoria-interna" && w.Ativo.Value));

            var bannerPrincipal = await db.Banners.Include(x => x.AreaBanner).Where(x => x.Liberado && !x.Excluido && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => (w.chave == "banner-editoria-super" || w.chave == "banner-editoria-leaderboard" || w.chave == "banner-editoria-maxiboard") && w.Ativo.Value)).Select(banner => new BannerViewModel
            {
                AreaBanner = banner.AreaBanner,
                Arquivos = banner.Arquivo,
                Id = banner.Id,
                Link = banner.Link,
                Titulo = banner.Titulo,
                Anunciante = banner.Anunciante,
                Chave = banner.AreaBanner.FirstOrDefault().chave

            }).FirstOrDefaultAsync();


            if (editorial == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var query = editorial.Noticias;

            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(palavraChave))
            {
                query = editorial.Noticias.Where(n => (n.Titulo != null && n.Titulo.Contains(palavraChave.ToLower())) || (n.Chamada != null && n.Chamada.Contains(palavraChave)) || (n.Subtitulo != null && n.Subtitulo.Contains(palavraChave))).ToList();
            }

            //if (categoria > 0)
            //{
            //    query = query.Where(n => n.Categorias.Any(x => x.Id == categoria)).ToList();
            //}

            if (!string.IsNullOrEmpty(dataInicio))
            {
                DateTime DataInicio = DateTime.Parse(dataInicio).AddHours(0).AddMinutes(0).AddSeconds(0);
                query = query.Where(d => d.DataCadastro >= DataInicio).ToList();
            }

            if (!string.IsNullOrEmpty(dataFinal))
            {
                DateTime DataFim = DateTime.Parse(dataFinal).AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(d => d.DataCadastro <= DataFim).ToList();
            }

            var viewModel = new EditoriasViewModel
            {
                Editoria = editorial,
                Destaques = destaques,
                Banner = banner1,
                BannerPrincipal = bannerPrincipal,
                paginacao = query.Where(q => !idsDestaques.Contains(q.Id)).ToPagedList(pageNumber, 8)
            };

            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.EditoriaId == editorial.id), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));

            return View(viewModel);
        }


        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "chave;page;palavraChave;dataInicio;dataFinal;categoria")]
        public async Task<ActionResult> Ouvintes(int? page, string palavraChave, string dataInicio, string dataFinal, int categoria = 0)
        {
            var denuncias = await db.Ouvintes.Include(x => x.Regioes).Where(x => !x.excluido && x.liberado).ToListAsync();

            var query = denuncias.AsQueryable();

            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(palavraChave))
            {
                query = query.Where(n => n.nome.Contains(palavraChave.ToLower()) || n.assunto.Contains(palavraChave) || n.comentario.Contains(palavraChave) || n.Regioes.titulo.Contains(palavraChave));
            }


            if (!string.IsNullOrEmpty(dataInicio))
            {
                DateTime DataInicio = DateTime.Parse(dataInicio).AddHours(0).AddMinutes(0).AddSeconds(0);
                query = query.Where(d => d.data >= DataInicio);
            }

            if (!string.IsNullOrEmpty(dataFinal))
            {
                DateTime DataFim = DateTime.Parse(dataFinal).AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(d => d.data <= DataFim);
            }

            return View(query.ToPagedList(pageNumber, 8));

        }

        public async Task<List<NoticiasViewModel>> DestaquesEditoria(IQueryable<NoticiasViewModel> queryPrincipal, string chave, IEnumerable<int> idDestaques)
        {
            var destaques = new List<NoticiasViewModel>();

            queryPrincipal = queryPrincipal.Where(noticia => !idDestaques.Contains(noticia.Id));

            if (chave == "esporte")
            {
                queryPrincipal = queryPrincipal.Where(x => x.Editoriais.Take(1).Any(p => !p.Excluido && !p.Especial && p.Ativo && (p.Chave == chave || p.Esporte)));
            }
            else if (chave == "noticia")
            {
                queryPrincipal = queryPrincipal.Where(x => x.Editoriais.Take(1)
                    .Any(p => !p.Excluido
                        && p.Ativo
                        && p.Chave != "politica"
                        && p.Chave != "entretenimento"
                        && p.Chave != "cidade"
                        && p.Chave != "esporte"
                        && p.Chave != "parlamento-aberto"
                        && !p.Esporte));
            }
            else
            {
                queryPrincipal = queryPrincipal.Where(x => x.Editoriais.Take(1).Any(p => !p.Excluido && p.Ativo && p.Chave == chave));
            }

            queryPrincipal = queryPrincipal.Take(30);
            var skip = 0;

            var destaque1 = await queryPrincipal.OrderByDescending(y => y.DataCadastro).FirstOrDefaultAsync(noticia => noticia.TipoDestaque == TipoDestaque.Interna1);
            if (destaque1 == null)
            {
                destaque1 = await queryPrincipal.OrderByDescending(y => y.DataCadastro).Skip(skip).FirstOrDefaultAsync();
                skip++;
            }

            var destaque2 = await queryPrincipal.OrderByDescending(y => y.DataCadastro).FirstOrDefaultAsync(noticia => noticia.TipoDestaque == TipoDestaque.Interna2);
            if (destaque2 == null)
            {
                destaque2 = await queryPrincipal.OrderByDescending(y => y.DataCadastro).Skip(skip).FirstOrDefaultAsync();
                skip++;
            }

            var destaque3 = await queryPrincipal.OrderByDescending(y => y.DataCadastro).FirstOrDefaultAsync(noticia => noticia.TipoDestaque == TipoDestaque.Interna3);
            if (destaque3 == null)
            {
                destaque3 = await queryPrincipal.OrderByDescending(y => y.DataCadastro).Skip(skip).FirstOrDefaultAsync();
                skip++;
            }

            destaques.Add(destaque1);
            destaques.Add(destaque2);
            destaques.Add(destaque3);

            destaques.RemoveAll(destaque => destaque == null);

            var destaquesId = new int[] { };

            if (destaques == null && !destaques.Any())
            {
                destaquesId = destaques.Select(destaque => destaque.Id).ToArray();
                destaques.AddRange(await queryPrincipal.Where(destaque => (destaque.TipoDestaque == null || destaque.TipoDestaque == TipoDestaque.NenhumDestaque)
                    && !destaquesId.Contains(destaque.Id)).OrderByDescending(y => y.DataCadastro).Take(3).ToListAsync());
            }


            return destaques;

        }
    }
}