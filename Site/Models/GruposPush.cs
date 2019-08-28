using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    [Table("GruposPush")]
    public partial class GruposPush
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GruposPush()
        {
            Notificacoes = new HashSet<NotificacoesPush>();
        }
        public int Id { get; set; }

        public bool Excluido { get; set; }
        public bool Liberado { get; set; }

        [Required]
        [MaxLength(30,ErrorMessage = "O título não pode ter mais do que 30 caracteres.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Data do Cadastro")]
        public DateTime DataCadastro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotificacoesPush> Notificacoes { get; set; }
        public virtual ICollection<Tags> Tags { get; set; }
    }
}