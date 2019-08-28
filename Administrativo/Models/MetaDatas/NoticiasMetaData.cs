using Administrativo.Models.CustomValidation;
using Administrativo.Enums;
using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(NoticiasMetaData))]
    public partial class Noticias
    {
    }

    public class NoticiasMetaData
    {
        public int id { get; set; }

        [Display(Name = "Título da Capa")]
        [StringLength(90)]
        public string TituloCapa { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Colunista")]
        public Nullable<int> idColunista { get; set; }

        [ScaffoldInList(false)]
        [RequiredIf("transito", true, ErrorMessage = "Escolha a categoria desta noticia")]
        public Nullable<int> CategoriaMapaId { get; set; }

        [StringLength(135, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Versal")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string titulo { get; set; }

        [StringLength(350, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Subtítulo")]
        public string subtitulo { get; set; }

        [AllowHtml]
        [ScaffoldInList(false)]
        [Display(Name = "Título da Interna")]
        [StringLength(500, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        public string chamada { get; set; }

        [AllowHtml]
        [ScaffoldInList(false)]
        [Display(Name = "Texto")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string texto { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Foto")]
        [RequiredIf("destaqueHome", true)]
        //[AllowedExtensions(".jpg|.png|.jpeg",ErrorMessage="Tipo de arquivo não permitido.")]
        public string foto { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Legenda da Foto")]
        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        public string fotoDescricao { get; set; }
        
        [AllowHtml]
        [ScaffoldInList(false)]
        [Display(Name = "Crédito da Foto")]
        [StringLength(200, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        public string fotoCredito { get; set; }

        [Display(Name = "Tipo de Destaque")]
        public TipoDestaque? TipoDestaque { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Miniatura")]
        //[AllowedExtensions(".jpg|.png|.jpeg", ErrorMessage = "Tipo de arquivo não permitido.")]
        public string fotoMini { get; set; }

        [Display(Name = "Destaque (Editorial)")]
        [ScaffoldInList(false)]
        public bool destaque { get; set; }

        [Display(Name = "Destaque (Home)")]
        public bool destaqueHome { get; set; }

        [Display(Name = "Destaque (Home Menor)")]
        public bool DestaqueHomeMenor { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Assunto da Semana")]
        public bool assuntoSemana { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Plantão")]
        public bool plantao { get; set; }

        [ScaffoldInList(false)]
        public bool excluido { get; set; }

        [Display(Name = "Data de Cadastro")]
        public System.DateTime dataCadastro { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Galeria")]
        public Nullable<int> idGaleria { get; set; }

        [ScaffoldInList(false)]
        [RequiredIf("transito", true)]
        public string latitude { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Por Autor")]
        public string porAutor { get; set; }


        [ScaffoldInList(false)]
        [RequiredIf("transito", true)]
        public string longitude { get; set; }

        [ScaffoldInList(false)]
        public Nullable<byte> transito { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Região")]
        [RequiredIf("transito", true, ErrorMessage = "Escolha a região onde isto ocorreu")]
        public Nullable<int> RegiaoId { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Data de Alteração")]
        public Nullable<System.DateTime> dataAtualizacao { get; set; }

        [ScaffoldInList(false)]
        [RequiredIf("videoDestaqueHome", true)]
        [Display(Name = "Código do Vídeo")]
        public string videoYoutube { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Vídeo na Home")]
        public bool videoDestaqueHome { get; set; }
        [StringLength(600, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        public string url { get; set; }

        [Display(Name = "Acessos")]
        public int visualizacao { get; set; }

        [Display(Name = "Ativo")]
        [ScaffoldInList(false)]
        public bool liberado { get; set; }

        [Display(Name = "Exibir imagem na interna")]
        public bool ExibirImagemInterna { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Áudio da Notícia")]
        //[AllowedExtensions(".mp3", ErrorMessage = "Tipo de arquivo não permitido.")]
        public string audio { get; set; }
    }
}