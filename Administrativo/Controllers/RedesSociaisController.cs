using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Site.Enums;
using System.Globalization;
using System.Threading.Tasks;
using Site.Helpers;
using Site.Controllers;
using Site.Models;

namespace Administrativo.Controllers
{
    public class RedesSociaisController : BaseController
    {
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.Server)]
        public JsonResult AtualizaRedesSociais()
        {
            Task<string> str = Task.Run(async () =>
            {
                string msg = await getPostsSocialAsync();
                return msg;
            });

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #region async
        private Task<string> getPostsSocialAsync()
        {
            return Task.Run<string>(() => getPostsSocial());
        }
        #endregion

        #region metodoGeral
        public string getPostsSocial()
        {
            #region RedesSociaisApi


            List<Site.Controllers.RedeSocialModel> listaPosts = new List<Site.Controllers.RedeSocialModel>();

            //Instagram
            //try
            //{
            //    listaPosts.AddRange(getJsonInstagram(ConfigurationManager.AppSettings["instagramAccessToken"]));
            //}
            //catch { }


            //Facebook
            try
            {

                listaPosts.AddRange(getJsonFacebook(ConfigurationManager.AppSettings["facebookUserId"]));
            }
            catch { }


            //Twitter
            try
            {
                listaPosts.AddRange(getJsonTwitter(ConfigurationManager.AppSettings["twitterUser"]));
            }
            catch (Exception ex)
            {
                var x = ex;

            }


            //youtube

            /*
                try
                {
                    listaPosts.AddRange(getJsonYoutube(ConfigurationManager.AppSettings["YoutubeUser"]));
                }
                catch (Exception ex)
                {
                    var x = ex;

                }
            */

            var lista = listaPosts.OrderByDescending(d => d.DataPublicacaoOrder);


            foreach (var item in lista)
            {
                var obj = new RedesSociais();
                obj.Imagem = item.Picture;
                obj.PostId = item.PostId;
                obj.Link = item.Link;
                obj.Texto = item.Mensagem;
                obj.TipoRede = item.TipoRedeSocial.GetHashCode();
                obj.Video = item.Video;
                obj.Data = item.DataPublicacaoOrder;
                obj.DataCadastro = DateTime.Now;


                var db2 = new ModeloDados();

                var exists = db2.RedesSociais.FirstOrDefault(x => x.PostId == obj.PostId && x.TipoRede == obj.TipoRede);

                if (exists == null)
                {
                    db2.RedesSociais.Add(obj);
                    db2.SaveChanges();
                }


            }

            return "";

            #endregion
        }
        #endregion

        #region metodos individuais

        public List<RedeSocialModel> getJsonInstagram(string accessToken)
        {
            var url2 = "https://api.instagram.com/v1/users/self/media/recent/?access_token=" + accessToken;

            List<RedeSocialModel> retorno = new List<RedeSocialModel>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url2);
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string retornoInsta = reader.ReadToEnd();
                var requests = JsonConvert.DeserializeObject<dynamic>(retornoInsta);

                if (requests.data.Count > 0)
                {
                    foreach (var item in requests.data)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        dateTime = dateTime.AddSeconds(Convert.ToDouble(item.created_time.ToString())).ToLocalTime();

                        string data = String.Format("{0:dd/MM/yyyy HH:mm:ss}", dateTime);

                        retorno.Add(new RedeSocialModel()
                        {
                            TipoRedeSocial = RedeSocialTipo.Instagram,
                            PostId = item.id,
                            Mensagem = item.caption.text,
                            Picture = item.images.low_resolution.url,
                            Video = item.type == "video" ? item.videos.low_resolution.url : "",
                            Link = item.link,
                            dataPublicacao = data
                        });

                    }
                }




            }
            return retorno;

        }

        public List<RedeSocialModel> getJsonFacebook(string userid)
        {
            //Parkshopping - 141048155937027
            string apiKey = ConfigurationManager.AppSettings["facebookAccessToken"];
            List<RedeSocialModel> retorno = new List<RedeSocialModel>();
            //https://graph.facebook.com/141048155937027/posts?access_token=914701921932420|kb3JkWmBZlCXaBK9kPh8HJnqyIA&fields=message,picture,source,link,created_time
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://graph.facebook.com/" + userid + "/posts?access_token=" + apiKey + "&fields=message,full_picture,source,link,created_time,type,permalink_url");
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string retornoFace = reader.ReadToEnd();
                var requests = JsonConvert.DeserializeObject<dynamic>(retornoFace);

                //requests.AddRange(requestFb);

                if (requests.data.Count > 0)
                {
                    foreach (var item in requests.data)
                    {
                        retorno.Add(new RedeSocialModel()
                        {
                            TipoRedeSocial = RedeSocialTipo.Facebook,
                            PostId = item.id,
                            Mensagem = item.message,
                            Picture = item.full_picture,
                            Video = item.type == "video" ? item.source : "",
                            Link = item.permalink_url,
                            dataPublicacao = Convert.ToDateTime(item.created_time.ToString()).ToString()
                        });
                    }
                }


            }

            return retorno;
        }

        public List<RedeSocialModel> getJsonTwitter(string userid)
        {
            List<RedeSocialModel> retornoPosts = new List<RedeSocialModel>();

            string strJson = Tweet.AuthTwitter(userid);
            if (!String.IsNullOrEmpty(strJson))
            {
                var retorno = JsonConvert.DeserializeObject<dynamic>(strJson);
                if (retorno.Count > 0)
                {
                    foreach (var item in retorno)
                    {
                        retornoPosts.Add(
                            new RedeSocialModel()
                            {
                                TipoRedeSocial = RedeSocialTipo.Twitter,
                                PostId = item.id,
                                Mensagem = item.text,
                                Picture = item.entities.media != null ? item.entities.media[0].media_url_https : "",
                                Video = "",
                                Link = item.entities.media != null ? item.entities.media[0].url : "",
                                dataPublicacao = DateTime.ParseExact(item.created_at.ToString(), "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture).ToString()
                            });
                    }
                }
            }

            return retornoPosts.OrderByDescending(x => x.dataPublicacao).ToList();
        }

        public List<RedeSocialModel> getJsonYoutube(string userid)
        {
            //Parkshopping - 141048155937027
            string apiKey = ConfigurationManager.AppSettings["facebookClientId"];
            List<RedeSocialModel> retorno = new List<RedeSocialModel>();


            var preRequest = (HttpWebRequest)WebRequest.Create(@"https://www.googleapis.com/youtube/v3/channels?key=AIzaSyDr8YEwMkzF-zDt8ATOFDOwIbF-JqXCTOg&forUsername=" + userid + "&part=id");

            var preResponse = preRequest.GetResponse();

            var hashCanal = "";

            using (var responseStream = preResponse.GetResponseStream())
            {
                var reader = new StreamReader(responseStream, Encoding.UTF8);
                var retornoFace = reader.ReadToEnd();
                var requests = JsonConvert.DeserializeObject<dynamic>(retornoFace);

                if (requests.items != null)
                {
                    foreach (var item in requests.items)
                    {
                        hashCanal = item.id;
                    }
                }
            }

            var request = (HttpWebRequest)WebRequest.Create(@"https://www.googleapis.com/youtube/v3/search?key=AIzaSyDr8YEwMkzF-zDt8ATOFDOwIbF-JqXCTOg&channelId=" + hashCanal + "&part=snippet&order=date");

            var response = request.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                var reader = new StreamReader(responseStream, Encoding.UTF8);
                var retornoFace = reader.ReadToEnd();
                var requests = JsonConvert.DeserializeObject<dynamic>(retornoFace);

                //requests.AddRange(requestFb);

                if (requests.items.Count > 0)
                {
                    foreach (var item in requests.items)
                    {
                        retorno.Add(new RedeSocialModel()
                        {
                            TipoRedeSocial = RedeSocialTipo.Youtube,
                            PostId = item.id.videoId,
                            Mensagem = item.snippet.description,
                            Picture = item.snippet.thumbnails.medium.url,
                            Video = "",
                            Link = "https://www.youtube.com/watch?v=" + item.id.videoId,
                            dataPublicacao = Convert.ToDateTime(item.snippet.publishedAt).ToString()
                        });
                    }
                }
            }

            return retorno;
        }

        #endregion

    }
}
