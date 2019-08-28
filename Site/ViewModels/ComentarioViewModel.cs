using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class ComentarioViewModel
    {
        public string Link { get; set; }
        public string NameUser { get; set; }
        public string EmailUser { get; set; }
        public string IdUser { get; set; }
        public string MensagemUser { get; set; }
        public string CPFUser { get; set; }
        public int? ComentarioId { get; set; }
        public int NoticiaId { get; set; }
    }
    public class ComentarioPagedListViewModel : RespostaPagedListViewModel
    {
        public List<RespostaPagedListViewModel> Respostas { get; set; }
    }
    public class RespostaPagedListViewModel
    {
        public string Nome { get; set; }
        public string DataCadastro { get; set; }
        public string Texto { get; set; }
        public int Id { get; set; }
        public int Gostei { get; set; }
        public int NaoGostei { get; set; }
    }
}