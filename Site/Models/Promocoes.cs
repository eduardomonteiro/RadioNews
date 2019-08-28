namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Promocoes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Promocoes()
        {
            Promocoes_Participantes = new HashSet<Promocoes_Participantes>();
            Promocoes_Resultados = new HashSet<Promocoes_Resultados>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        [Required]
        [StringLength(200)]
        public string Premio { get; set; }

        public int Quantidade { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        [StringLength(255)]
        public string Imagem { get; set; }

        public string Regulamento { get; set; }

        public string EmailTexto { get; set; }

        public bool ResultadoAutomatico { get; set; }

        public int TipoPromocao { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime? DataCadastro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Promocoes_Participantes> Promocoes_Participantes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Promocoes_Resultados> Promocoes_Resultados { get; set; }
    }
}
