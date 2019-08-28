namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserProfile")]
    public partial class UserProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserProfile()
        {
            Promocoes_Resultados = new HashSet<Promocoes_Resultados>();
        }

        [Key]
        public int UserId { get; set; }

        [StringLength(150)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(56)]
        public string UserName { get; set; }

        public DateTime? DataNascimento { get; set; }

        [StringLength(1)]
        public string Sexo { get; set; }

        [StringLength(50)]
        public string Identidade { get; set; }

        [StringLength(20)]
        public string CPF { get; set; }

        [StringLength(50)]
        public string Telefone { get; set; }

        [StringLength(50)]
        public string Celular { get; set; }

        [StringLength(50)]
        public string CEP { get; set; }

        public int? Estado { get; set; }

        public int? Cidade { get; set; }

        [StringLength(150)]
        public string Bairro { get; set; }

        [StringLength(50)]
        public string Número { get; set; }

        [StringLength(500)]
        public string Complemento { get; set; }

        public int? ColunistaId { get; set; }

        public bool? Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Promocoes_Participantes Promocoes_Participantes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Promocoes_Resultados> Promocoes_Resultados { get; set; }
    }
}
