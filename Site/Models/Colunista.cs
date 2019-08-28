namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Colunista")]
    public partial class Colunista
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Colunista()
        {
            ColunistaSeguidores = new HashSet<ColunistaSeguidores>();
            Noticias = new HashSet<Noticias>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string nome { get; set; }

        public int Ordem { get; set; }

        [StringLength(150)]
        public string email { get; set; }

        [StringLength(1)]
        public string sexo { get; set; }

        public string descricao { get; set; }

        [StringLength(200)]
        public string foto { get; set; }

        [StringLength(200)]
        public string fotoMini { get; set; }

        [Required]
        [StringLength(250)]
        public string chave { get; set; }

        public bool liberado { get; set; }

        public bool excluido { get; set; }

        public DateTime dataCadastro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ColunistaSeguidores> ColunistaSeguidores { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Noticias> Noticias { get; set; }
    }
}
