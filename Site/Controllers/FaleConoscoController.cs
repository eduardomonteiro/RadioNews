using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Site.Helpers;
using System.IO;

namespace Site.Controllers
{
    public class FaleConoscoController : BaseController
    {
        // GET: FaleConosco
        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> Cadastrar(FaleConosco fale)
        {
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (string.IsNullOrEmpty(recaptchaHelper.Response))
            {
                return Json(new { result = "captcha_branco" });
            }

            RecaptchaVerificationResult recaptchaResult = await recaptchaHelper.VerifyRecaptchaResponseTaskAsync();
            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
                return Json(new { result = "captcha_error" });
            }

            if (ModelState.IsValid)
            {
                fale.mensagem = Utils.StripTags(fale.mensagem);
                fale.dataCadastro = DateTime.Now;
                db.FaleConosco.Add(fale);

                try
                {
                    await db.SaveChangesAsync();
                    return Json(new { result = "success" });
                }
                catch (Exception ex)
                {
                    return Json(new { result = "error-" + ex.Message });
                }


            }
            else
            {
                return Json(new { result = "invalid" });
            }

        }


        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Denuncia()
        {
            //ViewBag.regiaoId = new SelectList(db.Regioes, "id", "titulo");
            ViewBag.cidade_id = new SelectList(db.cidade.Where(e=>e.estado==11).ToList(), "id", "nome");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Denuncia(Ouvintes ouvinte)
        {
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (string.IsNullOrEmpty(recaptchaHelper.Response))
            {
                //return Json(new { result = "captcha_branco" });
                ModelState.AddModelError("", "O Captcha não pode ser deixado em branco");
            }

            RecaptchaVerificationResult recaptchaResult = await recaptchaHelper.VerifyRecaptchaResponseTaskAsync();
            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
//                return Json(new { result = "captcha_error" });
                ModelState.AddModelError("", "O captcha não foi validado! Por favor verifique!");
            }

            if (ModelState.IsValid)
            {
                ouvinte.comentario = Utils.StripTags(ouvinte.comentario);
                ouvinte.DataCadastro = DateTime.Now;
                ouvinte.excluido = false;
                db.Ouvintes.Add(ouvinte);

                try
                {
                    await db.SaveChangesAsync();

                    #region upload
                    if (Request.Files.Count > 0)
                    {
                        string diretorio = "/admin/conteudo/ouvintes/arquivos/";

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            var arquivo = Request.Files[i];

                            var fileName = Utils.RemoveAccent(Path.GetFileNameWithoutExtension(arquivo.FileName));

                            if (fileName != "")
                            {
                                var extension = Path.GetExtension(arquivo.FileName);

                                string[]permitidos = { ".jpg", ".jpeg", ".gif", ".js", ".zip", ".png", ".rar", ".doc", ".docx", ".txt", ".mp3 ", ".pdf" , ".mpeg" , ".mp4" , ".ogg" };

                                if (!permitidos.Contains(extension.ToLower()))
                                {
                                    ModelState.AddModelError("", "Extensão não permitida!");
                                }
                                else
                                {
                                    var path = Server.MapPath(diretorio);

                                    if (!Directory.Exists(path))
                                    {
                                        Directory.CreateDirectory(path);
                                    }

                                    var j = 0;
                                    while (System.IO.File.Exists(Path.Combine(path, fileName + extension)))
                                    {
                                        j++;
                                        fileName = Path.GetFileNameWithoutExtension(arquivo.FileName) + "_" + j;
                                    }

                                    arquivo.SaveAs(path + fileName + extension);

                                    var arq = new OuvintesArquivos();
                                    arq.Arquivo = fileName + extension;
                                    arq.DataCadastro = DateTime.Now;
                                    arq.idOuvinteDenuncia = ouvinte.id;

                                    db.OuvintesArquivos.Add(arq);
                                    await db.SaveChangesAsync();

                                }


                            }

                        }



                    }
                    #endregion

                    return RedirectToAction("Enviado");
                    //return Json(new { result = "success" });
                }
                catch (Exception e)
                {
                    //return Json(new { result = "error-" + ex.Message });
                    return View(ouvinte);

                }


            }
            else
            {
                //ViewBag.regiaoId = new SelectList(db.Regioes, "id", "titulo");
                ViewBag.cidade_id = new SelectList(db.cidade.Where(e => e.estado == 11).ToList(), "id", "nome");
                return View(ouvinte);
            }

        }

        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Enviado()
        {
            return View();
        }

        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "id;slug")]
        public ActionResult DetalhesOuvintes(int id, string slug)
        {
            var denuncia = db.Ouvintes.FirstOrDefault(x => x.id == id && !x.excluido);

            if (denuncia == null)
            {
                return RedirectToAction("Ouvintes", "Editorias");
            }
            
            return View(denuncia);
        }




        //#region Notificar Usuario
        //public void NotificarUsuario(string emailDestino, List<string> vouchers, string premio, int? quantidade = null)
        //{
        //    Task<string> sCode = Task.Run(async () =>
        //    {
        //        string msg = await EnviarEmailAsync(emailDestino, vouchers, premio, quantidade);
        //        return msg;
        //    });
        //}

        //private Task<string> EnviarEmailAsync(string emailDestino, List<string> vouchers, string premio, int? quantidade = null)
        //{
        //    return Task.Run<string>(() => EnviaEmailUsuarioAsync(emailDestino, vouchers, premio, quantidade));
        //}

        //private string EnviaEmailUsuarioAsync(string emailDestino, List<string> vouchers, string premio, int? quantidade = null)
        //{
        //    EnviaMailUsuario(emailDestino, vouchers, premio, quantidade);
        //    return "ok";
        //}

        //public void EnviaMailUsuario(string emailDestino, List<string> vouchers, string premio, int? quantidade = null)
        //{
        //    string html = "";

        //    if (quantidade.HasValue)
        //    {
        //        html = System.IO.File.ReadAllText(Server.MapPath("/modelo-mail/resgateMultiplus.cshtml"));
        //        html = html.Replace("###voucher###", vouchers.FirstOrDefault());
        //        html = html.Replace("###quantidade###", quantidade.Value.ToString());
        //    }
        //    else
        //    {
        //        string template = System.IO.File.ReadAllText(string.Format("{0}/resgate.cshtml", Server.MapPath("/modelo-mail/")));
        //        html = Engine.Razor.RunCompile(template, "registerView", null, vouchers);
        //    }


        //    var mail = new MailSender();

        //    mail.To.Add(emailDestino);
        //    mail.Subject = "Resgate do premio: " + premio;
        //    mail.Body = html;
        //    try
        //    {
        //        mail.SendEmail();
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        //#endregion



    }
}