using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Models;

namespace Site.ViewModels
{
    public class MenuViewModel
    {
        public List<RadiosMenuViewModel> Radios { get; set; }
        public List<EditoriasLayoutViewModel> Editorias { get; set; }
        public List<EditoriasLayoutViewModel> Campeonatos { get; set; }
        public List<CategoriasAudiosMenuViewModel> CategoriasAudios { get; set; }
        public List<ProgramacaoMenuViewModel> Programacao { get; set; }

    }
    public class RadiosMenuViewModel
    {
        public string Chave { get; set; }
        public string Titulo { get; set; }
        public string Stream1 { get; set; }
        public string Stream2 { get; set; }
    }
    public class CategoriasAudiosMenuViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
    
    public class ProgramacaoMenuViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Horario { get; set; }
        public string FimHorario { get; set; }
    }
}