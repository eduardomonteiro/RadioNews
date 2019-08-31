using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Site.Services.Classes
{
	public abstract class Campeonato
	{
		//public int Id { get; set; }
		public string Titulo { get; set; }
		public List<Fase> Fases { get; set; }
		public List<Clube> Clubes { get; set; }
		public List<Grupo> Grupos { get; set; }
		public List<Artilheiros> Artilharia { get; set; }
		public abstract void Popular();

		public void PopularClubes(string caminho)
		{
			#if (RELEASE)
				XDocument doc = XDocument.Load(caminho);
	            //Id = (from id in doc.Descendants("standings") select (int)id.Attribute("id")).First();
	            Clubes = new List<Clube>();
	            Clubes = (from linha
	                      in doc.Descendants("team")
	                      select new Clube
	                      {
	                          UID = (string)linha.Attribute("uid"),
	                          Id = (string)linha.Attribute("id"),
	                          Logo = (string)linha.Attribute("uid") + ".gif",
	                          Nome = (string)linha.Attribute("name")
	                      }).ToList();
			#endif
		}

		public void PopularClassificacao(string caminho)
		{
			//XDocument doc = XDocument.Load(caminho);

			//Grupos = new List<Grupo>();
			//Grupos = (from grupo
			//		  in doc.Descendants("table")
			//		  select new Grupo
			//		  {
			//			  Abreviacao = (string)grupo.Attribute("short") + "",
			//			  Id = (string)grupo.Attribute("group"),
			//			  Nome = (string)grupo.Attribute("name") + "",
			//			  Classificacao = (from time
			//				 in grupo.Descendants("team")
			//							   select new Classificacao
			//							   {
			//								   Aproveitamento = (double)time.Attribute("per"),
			//								   Clube = Clubes.FirstOrDefault(c => c.Id == (string)time.Attribute("id")),
			//								   Derrotas = (int)time.Attribute("l"),
			//								   Empates = (int)time.Attribute("d"),
			//								   GolsContra = (int)time.Attribute("ga"),
			//								   GolsPro = (int)time.Attribute("gf"),
			//								   Jogos = (int)time.Attribute("pld"),
			//								   Pontos = (int)time.Attribute("pts"),
			//								   Posicao = (int)time.Attribute("pos"),
			//								   SaldoGols = (int)time.Attribute("gd"),
			//								   Vitorias = (int)time.Attribute("w")
			//							   }).ToList(),
			//		  }).ToList();

		}

		public void PopularArtilharia(string caminho)
		{
			//XDocument doc = XDocument.Load(caminho);
			//Artilharia = new List<Artilheiros>();
			//Artilharia = (from linha
			//			 in doc.Descendants("player")
			//			  select new Artilheiros
			//			  {
			//				  Clube = Clubes.FirstOrDefault(c => c.Id == (string)linha.Attribute("teamid")),
			//				  Gols = (int)linha.Attribute("goals"),
			//				  Nome = (string)linha.Attribute("name")
			//			  }).Take(5).ToList();
		}

		public void PopularFaseseRodadas(string caminho)
		{
			//XDocument doc = XDocument.Load(caminho);
			//Fases = null;
			//Fases = (from linha
			//		 in doc.Descendants("stage")
			//		 select new Fase
			//		 {
			//			 Nome = (string)linha.Attribute("name"),
			//			 Tipo = (string)linha.Attribute("type"),
			//			 Rodadas = (from rodada
			//						in linha.Descendants("round")
			//						select new Rodada
			//						{
			//							Abreviado = (string)rodada.Attribute("short"),
			//							Atual = (bool)rodada.Attribute("current"),
			//							DataFim = DateTime.Now,
			//							DataInicio = DateTime.Now,
			//							//DataFim = DateTime.Parse((string)rodada.Attribute("enddate")),
			//							//DataInicio = DateTime.Parse((string)rodada.Attribute("startdate")),
			//							Proxima = (bool)rodada.Attribute("next"),
			//							Titulo = rodada.Value,
			//							Id = (int)rodada.Attribute("id")
			//						}).ToList()

			//		 }).ToList();
		}

		public void PopularJogos(string caminho)
		{
			//XDocument doc = XDocument.Load(caminho);
			//foreach (var fase in Fases)
			//{
			//	foreach (var rodada in fase.Rodadas)
			//	{
			//		rodada.Jogos = (from linha
			//					   in doc.Descendants("match")
			//						where ((int)linha.Attribute("round") == rodada.Id)
			//						select new Jogo
			//						{
			//							Campeonato = Titulo,
			//							Grupo = (string)linha.Attribute("group"),
			//							Data = DateTime.Now,
			//							//Data = DateTime.Parse((string)linha.Attribute("date") + " " + (string)linha.Attribute("time")),
			//							Local = (string)linha.Attribute("venue"),
			//							Mandante = (from clube
			//									   in linha.Descendants("team")
			//										where (bool)clube.Attribute("home")
			//										select Clubes.FirstOrDefault(c => c.Id == (string)clube.Attribute("code"))).FirstOrDefault(),
			//							Visitante = (from clube
			//									   in linha.Descendants("team")
			//										 where !(bool)clube.Attribute("home")
			//										 select Clubes.FirstOrDefault(c => c.Id == (string)clube.Attribute("code"))).FirstOrDefault(),
			//							PlacarMandante = (from clube
			//											  in linha.Descendants("team")
			//											  where (bool)clube.Attribute("home")
			//											  select (string)clube.Attribute("score")).FirstOrDefault(),
			//							PlacarVisitante = (from clube
			//											   in linha.Descendants("team")
			//											   where !(bool)clube.Attribute("home")
			//											   select (string)clube.Attribute("score")).FirstOrDefault()
			//						}).ToList();
			//	}
			//}

		}
	}
}