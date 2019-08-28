using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Enums;

namespace Site.ViewModels
{
    public class HomeViewModel
    {
        public NoticiasViewModel DestaqueEspecial { get; set; }
        public List<NoticiasViewModel> Destaques { get; set; }
        public List<NoticiasViewModel> EditoriaCidade { get; set; }
        public List<NoticiasViewModel> EditoriaPolitica { get; set; }
        public List<NoticiasViewModel> EditoriaEntretenimento { get; set; }
        public List<NoticiasViewModel> EditoriaEsportes { get; set; }
        public List<NoticiasViewModel> Podcast { get; set; }
        public List<NoticiasViewModel> Noticias { get; set; }
        public BannerHomeViewModel Banner1 { get; set; }
        public BannerHomeViewModel BannerMobile { get; set; }
        public BannerHomeViewModel Banner2 { get; set; }
        public BannerHomeViewModel BannerMobilePrincipal { get; set; }
        public BannerHomeViewModel BannersPrincipal { get; set; }


    }

    public class SideBarHomeViewModel
    {
        public RedesSociaisHomeViewModel RedeSociais { get; set; }

        public List<PlantaoViewModel> PlantaoBH { get; set; }
        public List<NoticiasDetalhesViewModel> MaisLidas { get; set; }
        public Midias Videos { get; set; }

        public List<ColunistaViewModel> Colunistas { get; set; }

        public BannerHomeViewModel Banner { get; set; }
        public BannerHomeViewModel Banner2 { get; set; }
        public BannerHomeViewModel BannerUltimoSidebar { get; set; }

        public List<PodcastViewModel> Podcast { get; set; }

        public EnqueteViewModel Enquete { get; set; }

    }

    public class PlantaoViewModel
    {
        public string Titulo { get; set; }
        public DateTime Hora { get; set; }

    }

    public class ColunistaViewModel
    {
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public string Chave { get; set; }
        public string Foto { get; set; }
        public string Url { get; set; }
        public DateTime UltimaAtualizacao { get; set; }

    }

    public class NoticiasViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public TipoDestaque? TipoDestaque { get; set; }
        public string FotoCredito { get; set; }
        public string Chamada { get; set; }
        public string Foto { get; set; }
        public string Titulo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Subtitulo { get; set; }
        public bool Plantao { get; set; }
        public int? IdColunista { get; set; }
        public string Audio { get; set; }
        public string VideoYoutube { get; set; }
        public int Visualizacao { get; set; }
        public bool Excluido { get; set; }
        public bool Liberado { get; set; }
        public string TituloCapa { get; set; }
        public IEnumerable<EditorialNoticiaViewModel> Editoriais { get; set; }
        public string PorAutor { get; set; }
        public int? idGaleria { get; set; }
        public IEnumerable<CategoriasViewModel> Categorias { get; set; }

        public virtual Galeria Galeria { get; set; }

    }
    public class NoticiasDetalhesViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public TipoDestaque? TipoDestaque { get; set; }
        public bool CreditoFotoNoTexto { get; set; }
        public string FotoCredito { get; set; }
        public string FotoDescricao { get; set; }
        public string Chamada { get; set; }
        public string Foto { get; set; }
        public bool exibirImagemInterna { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Subtitulo { get; set; }
        public bool Plantao { get; set; }
        public int? IdColunista { get; set; }
        public string Audio { get; set; }
        public string VideoYoutube { get; set; }
        public int Visualizacao { get; set; }
        public bool Excluido { get; set; }
        public bool Liberado { get; set; }
        public string TituloCapa { get; set; }
        public string PorAutor { get; set; }
        public int? idGaleria { get; set; }
        public string chaveGaleria { get; set; }
        public bool galeriaExcluida { get; set; }
        public List<string> Tags { get; set; }
        public BannerHomeViewModel Banner1 { get; set; }
    }

    public class CategoriasViewModel
    {
        public int Id { get; set; }
    }

    public class PodcastViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Audio { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Titulo { get; set; }
        public List<Editoriais> Editoriais { get; set; }
    }

    public class BannerViewModel
    {
        public int Id { get; set; }
        public string Anunciante { get; set; }
        public string Titulo { get; set; }
        public string Arquivos { get; set; }
        public string Link { get; set; }
        public string Chave { get; set; }
        public ICollection<AreaBanner> AreaBanner { get; set; }
    }

    public class BannerHomeViewModel
    {
        public int Id { get; set; }
        public string Anunciante { get; set; }
        public string Titulo { get; set; }
        public string Arquivos { get; set; }
        public string Link { get; set; }
        public string Chave { get; set; }
        public string AreaBannerTamanho { get; set; }
        public string AreaBannerDescricao { get; set; }
        public bool? AreaBannerAtivo { get; set; }
    }

    public class EditorialNoticiaViewModel
    {
        public string Nome { get; set; }
        public bool Excluido { get; set; }
        public bool Ativo { get; set; }
        public bool Especial { get; set; }
        public string Chave { get; set; }
        public bool Esporte { get; set; }
    }

}