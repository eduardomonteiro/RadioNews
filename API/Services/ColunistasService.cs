using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Util;
using System.Collections.ObjectModel;

namespace API.Services
{
    public static class ColunistasService
    {
        static string primeKey = "colunistas:";
        public static List<ColunistaViewModel> ObterColunistas(int? idColunista = null, string chave = "")
        {
            string key = primeKey + "ObterColunistas:" + idColunista + ":" + chave + ":TColunista:TNoticias";

            List<ColunistaViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<ColunistaViewModel>> funcao = t => ObterColunistas(db, idColunista, chave);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 120);
            }

            return retorno;
        }

        public static List<ColunistaViewModel> ObterColunistas(ModeloDados db, int? idColunista = null, string chave = "")
        {
            var url = ServerSettings.Url;

            var colunistasL = db.Colunista.Include("Noticias").Where(colunista =>
                    !colunista.excluido
                    && colunista.liberado
                    && (chave == null || colunista.nome.Contains(chave)))
                    .ToList();

            if (idColunista != null)
                return colunistasL.Where(colunista => colunista.id == idColunista).Select(colunista => new ColunistaViewModel { Id = colunista.id, Nome = colunista.nome, Email = colunista.email, Descricao = colunista.descricao, Foto = string.IsNullOrEmpty(colunista.foto) ? null : url + "/Admin/Conteudo/Colunistas/Foto/" + colunista.foto, Sexo = colunista.sexo }).ToList();
            else
            {
                var colunistasOrdenado = new ObservableCollection<ColunistaViewModel>(colunistasL.Select(colunista => new ColunistaViewModel { Id = colunista.id, Nome = colunista.nome, Email = colunista.email, Descricao = "", Foto = string.IsNullOrEmpty(colunista.foto) ? null : url + "/Admin/Conteudo/Colunistas/Foto/" + colunista.foto, Sexo = colunista.sexo, UltimaAtualizacao = colunista.Noticias.Count > 0 ? colunista.Noticias.LastOrDefault().dataAtualizacao : DateTime.MinValue })
                    .OrderByDescending(c => c.UltimaAtualizacao)
                    .ToList());
                var indexCadu = colunistasOrdenado.IndexOf(colunistasOrdenado.Where(e => e.Nome.Equals("Cadu Doné")).FirstOrDefault());
                if (indexCadu > -1)
                    colunistasOrdenado.Move(indexCadu, 0);
                var indexEmanuel = colunistasOrdenado.IndexOf(colunistasOrdenado.Where(e => e.Nome.Equals("Emanuel Carneiro")).FirstOrDefault());
                if (indexEmanuel > -1)
                    colunistasOrdenado.Move(indexEmanuel, 0);
                return colunistasOrdenado.ToList();
            }
        }

    }
}