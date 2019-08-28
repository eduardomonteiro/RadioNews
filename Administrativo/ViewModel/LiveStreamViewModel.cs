using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Administrativo.ViewModel
{
    public class LiveStreamViewModel
    {
        public int Id { get; set; }
        public string Legenda { get; set; }

        [DisplayName("Código da transmissão")]
        public string CodigoTransmissao { get; set; }

        [DisplayName("Data de cadastro")]
        public DateTime DataCadastro { get; set; }

        [DisplayName("Data de finalização")]
        public DateTime? DataFinalizacao { get; set; }
        public bool Excluido { get; set; }
        public bool Ativo { get; set; }
        public int IdNoticia { get; set; }
    }
}