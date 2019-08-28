namespace Site.Models
{
    using Site.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Noticias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Noticias()
        {
            Categorias = new HashSet<Categorias>();
            Editoriais = new HashSet<Editoriais>();
            Tags = new HashSet<Tags>();
            migrado = false;
        }

        [StringLength(90)]
        public string TituloCapa { get; set; }

        public int id { get; set; }

        public int? idColunista { get; set; }

        [StringLength(250)]
        public string audio { get; set; }

        public int? CategoriaMapaId { get; set; }

        [Required]
        [StringLength(300)]
        public string titulo { get; set; }

        [StringLength(350)]
        public string subtitulo { get; set; }

        [StringLength(350)]
        [Display(Name = "Local")]
        public string local { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string texto { get; set; }

        [StringLength(500)]
        public string foto { get; set; }

        [StringLength(100)]
        public string fotoDescricao { get; set; }

        [StringLength(500)]
        public string fotoMini { get; set; }

        [StringLength(250)]
        public string link { get; set; }

        public bool destaque { get; set; }

        public bool destaqueEditoria { get; set; }

        public bool destaqueHome { get; set; }

        public bool DestaqueHomeMenor { get; set; }

        public TipoDestaque? TipoDestaque { get; set; }

        public bool assuntoSemana { get; set; }

        [StringLength(500)]
        public string chamada { get; set; }

        public DateTime? data { get; set; }

        public TimeSpan? hora { get; set; }

        public int? idGaleria { get; set; }

        [StringLength(200)]
        public string latitude { get; set; }

        [StringLength(200)]
        public string longitude { get; set; }

        [StringLength(100)]
        public string videoYoutube { get; set; }

        public bool videoDestaqueHome { get; set; }

        public bool transito { get; set; }

        public int? RegiaoId { get; set; }

        public DateTime dataAtualizacao { get; set; }

        [StringLength(600)]
        public string url { get; set; }

        public bool plantao { get; set; }

        public int visualizacao { get; set; }

        public bool projeto { get; set; }

        public bool liberado { get; set; }

        [StringLength(200)]
        public string fotoCredito { get; set; }

        [StringLength(100)]
        public string porAutor { get; set; }

        public bool excluido { get; set; }

        public bool ExibirImagemInterna { get; set; }

        public bool CreditoFotoNoTexto { get; set; }

        public bool? migrado { get; set; }

        public DateTime dataCadastro { get; set; }

        public virtual CategoriasMapa CategoriasMapa { get; set; }

        public virtual Colunista Colunista { get; set; }

        public virtual Galeria Galeria { get; set; }

        public virtual NoticiasRegioes NoticiasRegioes { get; set; }

        public virtual LiveStreaming LiveStreaming { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comentario> Comentarios { get; set; }

        public virtual IEnumerable<Comentario> ComentariosRespostas
        {
            get
            {
                return Comentarios.Where(comentario => comentario.Resposta && !comentario.Bloqueado);
            }
        }

        public virtual IEnumerable<Comentario> ComentariosRaiz
        {
            get
            {
                return Comentarios.Where(comentario => !comentario.Resposta && !comentario.Bloqueado);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categorias> Categorias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Editoriais> Editoriais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tags> Tags { get; set; }
    }
}
