using FlickrNet;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{

    public class EventoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Imagem { get; set; }

    }


    public class DetalhesEventoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Imagem { get; set; }
        
        public IPagedList<AcontecimentoViewModel> Encerrados { get; set; }

    }


    public class AcontecimentoViewModel
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public string EventoUrl { get; set; }
        public string EventoTitulo { get; set; }
        public string Titulo { get; set; }
        public DateTime Data { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFim { get; set; }
        public string Local { get; set; }
        public string Url { get; set; }

        public PhotosetPhotoCollection Imagens { get; set; }

    }


}