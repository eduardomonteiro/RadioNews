using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(ComentariosMetaData))]
    public partial class Comentarios
    {
        public string Area
        {
            get
            {
                if (Noticias.Colunista != null)
                {
                    return "Coluna do(a) " + Noticias.Colunista.nome;
                }

                if (Noticias.Editoriais.Any())
                {
                    var area = "Editoria ";

                    foreach (var editoria in Noticias.Editoriais)
                    {
                        area += editoria.nome + " - ";
                    }

                    return area.Substring(0, area.Length - 3);
                }

                return "Não identificado.";
            }
        }
    }

    public class ComentariosMetaData
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string IPAcesso { get; set; }
        public string UrlFacebook { get; set; }

        public int Gostei { get; set; }
        public int NaoGostei { get; set; }

        [DisplayName("Data de cadastro")]
        public DateTime DataCadastro { get; set; }

        public virtual Noticias Noticias { get; set; }
        public virtual ICollection<Comentarios> Comentarios1 { get; set; }
        public virtual Comentarios Comentarios2 { get; set; }
    }
}