namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Editoriais
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Editoriais()
        {
            Categorias = new HashSet<Categorias>();
            Editoriais_Equipe = new HashSet<Editoriais_Equipe>();
            Especiais_Secoes = new HashSet<Especiais_Secoes>();
            Programacao = new HashSet<Programacao>();
            Galeria = new HashSet<Galeria>();
            Noticias = new HashSet<Noticias>();
            Times = new HashSet<Times>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string nome { get; set; }

        [StringLength(512)]
        public string Descricao { get; set; }

        [StringLength(512)]
        public string FotoCapa { get; set; }

        [StringLength(200)]
        public string Action { get; set; }

        public bool especial { get; set; }

        public int? modeloEspecial { get; set; }

        public bool ativo { get; set; }

        public bool esportes { get; set; }

        public bool excluido { get; set; }
        public bool canal { get; set; }

        [Required]
        [StringLength(300)]
        public string chave { get; set; }

        public DateTime DataCadastro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categorias> Categorias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Editoriais_Equipe> Editoriais_Equipe { get; set; }

        public virtual Especiais_Modelos Especiais_Modelos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Especiais_Secoes> Especiais_Secoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Programacao> Programacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Galeria> Galeria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Noticias> Noticias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Times> Times { get; set; }
    }
}
