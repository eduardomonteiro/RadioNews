using Administrativo.Models;
using Omu.Drawing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Administrativo.Controllers
{

    public class FotosBastidoresController : BaseController
    {
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var foto = db.BastidoresMidias.Find(id);

            //db.FotosGaleria.Remove(foto);

            foto.excluido = true;
            db.SaveChanges();

            return Json(new ViewDataUploadFile
            {
                url = "/admin/Conteudo/bastidores/" + foto.idGaleria + "/" + foto.midia
            });
        }

        [HttpDelete]
        public ActionResult DeleteAll(int id)
        {
            var fotos = db.BastidoresMidias.Where(x => x.idGaleria.Value == id).ToList();

            if (fotos != null && fotos.Count > 0)
            {
                foreach (var item in fotos)
                {
                    item.excluido = true;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json(new ViewDataUploadFile
            {
                url = "/admin/Conteudo/bastidores/" + id + "/"
            });
        }


        [HttpPost]
        public ActionResult UploadFiles()
        {
            var r = new List<ViewDataUploadFile>();

            foreach (string file in Request.Files)
            {
                var statuses = new List<ViewDataUploadFile>();
                var headers = Request.Headers;

                UploadWholeFile(Request, statuses);

                var result = Json(new ViewDataUploadFilesResult { files = statuses });
                result.ContentType = "text/plain";

                return result;
            }

            return Json(r);
        }

        [HttpPost]
        public ActionResult SalvaFotos(string[] codVid, string[] legenda, string galeria_id)
        {
            Bastidores galeria = db.Bastidores.Find(int.Parse(galeria_id));
            for (var i = 0; i < codVid.Length; i++)
            {
                BastidoresMidias midia = new BastidoresMidias
                {
                    excluido = false,
                    idGaleria = int.Parse(galeria_id),
                    legenda = legenda[i],
                    midia = codVid[i],
                    video = false,
                    ativo = true,
                    dataCadastro = DateTime.Now
                };

                midia.Bastidores = galeria;
                db.BastidoresMidias.Add(midia);
                db.SaveChanges();
            }

            return Redirect("../Bastidor/Edit/" + galeria_id);
        }


        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFile> statuses)
        {
            for (var i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var idGaleria = request.Form["galeria_id"];
                var mimeType = MimeMapping.GetMimeMapping(file.FileName);
                var ext = Path.GetExtension(file.FileName);

                var foto = new BastidoresMidias
                {
                    legenda = request.Form["legenda"],
                    midia = fileName + ext,
                    excluido = false,
                    video = false,
                    idGaleria = int.Parse(request.Form["galeria_id"]),
                    ativo = true,
                    dataCadastro = DateTime.Now
                    
                };

                db.BastidoresMidias.Add(foto);

                db.SaveChanges();

                var path = Path.Combine(Server.MapPath("/admin/Conteudo/bastidores/"), idGaleria);

                //var j = 0;
                //while(System.IO.File.Exists(Path.Combine(path, fileName))) {
                //    j++;
                //    fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + j + ".jpg";
                //}

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(path + "/min");
                }

                using (var origem = Image.FromStream(file.InputStream, true, true))
                {
                    var imageCodecInfo = Imager.GetEncoderInfo(mimeType);

                    // Imager.SaveJpeg(path + fileName, media != null ? media : origem);
                    // Imager.Save(path + fileName, media != null ? media : origem, imageCodecInfo);

                    Imager.Save(path + @"\" + fileName + ext, origem, imageCodecInfo);

                    var media = Imager.Resize(origem, 117, 73, true);

                    // Imager.SaveJpeg(path + @"\min\" + fileName, media);
                    Imager.Save(path + @"\min\" + fileName + ext, media, imageCodecInfo);

                }

                statuses.Add(new ViewDataUploadFile
                {
                    url = "/Admin//Conteudo/bastidores/" + idGaleria + "/" + fileName + ext,
                    thumbnail_url = "/Admin/Conteudo/bastidores/" + idGaleria + "/min/" + fileName + ext,
                    name = fileName + ext,
                    type = file.ContentType,
                    size = file.ContentLength,
                    delete_url = "/Admin/FotosBastidores/Delete/" + foto.id,
                    delete_type = "DELETE"
                });
            }
        }

        public class ViewDataUploadFile
        {
            public string name { get; set; }
            public int size { get; set; }
            public string type { get; set; }
            public string url { get; set; }
            public string delete_url { get; set; }
            public string thumbnail_url { get; set; }
            public string delete_type { get; set; }
        }

        public class ViewDataUploadFilesResult
        {
            public List<ViewDataUploadFile> files { get; set; }
        }

    }
}
