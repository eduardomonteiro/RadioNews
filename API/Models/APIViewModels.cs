using API.Util;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class CategoriaAudioViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
    }
    public class CentralAudioViewModel
    {
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public virtual List<AudioViewModel> Audios { get; set; }
    }
    public class AudioViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Titulo { get; set; }
        public DateTime? Data { get; set; }
    }

    public class NoticiaViewModel
    {
        private string _chamada;
        private string _titulo;

        public int Id { get; set; }
        public string Chamada
        {
            get { return StringUtils.Strip(_chamada); }
            set { _chamada = value; }
        }
        public string Titulo
        {
            get { return StringUtils.Strip(_titulo); }
            set { _titulo = value; }
        }
        public string Texto { get; set; }
        public string Foto { get; set; }
        public string Url { get; set; }
        public string NomeAutor { get; set; }
        public string NomeCategoria { get; set; }
        public DateTime? Data { get; set; }
    }
    public class NoticiaMaisLidasViewModel
    {
        public int Id { get; set; }
        public string Chamada { get; set; }
        public string Foto { get; set; }
        public DateTime? Data { get; set; }
    }

    public class PostagemViewModel
    {
        private string _chamada;
        private string _titulo;

        public int Id { get; set; }
        public string Chamada
        {
            get { return StringUtils.Strip(_chamada); }
            set { _chamada = value; }
        }
        public string Titulo
        {
            get { return StringUtils.Strip(_titulo); }
            set { _titulo = value; }
        }
        public string Texto { get; set; }
        public string Foto { get; set; }
        public string Url { get; set; }
        public string NomeColunista { get; set; }
        public DateTime? Data { get; set; }
    }

    public class ColunistaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Descricao { get; set; }
        public string Foto { get; set; }
        public string Sexo { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
    }

    public class PublicidadeViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Imagem { get; set; }
    }

    public class HomeViewModel
    {
        public List<AudioViewModel> Audios { get; set; }
        public List<NoticiaViewModel> Noticias { get; set; }
        public List<NoticiaViewModel> Esportes { get; set; }
        public List<ColunistaViewModel> Colunistas { get; set; }
        public List<PublicidadeViewModel> Publicidade { get; set; }    
    }

    public class BuscarViewModel
    {
        public List<AudioViewModel> Audios { get; set; }
        public List<NoticiaViewModel> Noticias { get; set; }
        public List<PostagemViewModel> Postagens { get; set; }
        public List<ColunistaViewModel> Colunistas { get; set; }    
    }
    
}