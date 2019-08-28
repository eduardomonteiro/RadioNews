using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Administrativo.Models;
using WebMatrix.WebData;
using Administrativo.Commons;
using System.Web.Security;
using Administrativo.Filters;
using Microsoft.Web.WebPages.OAuth;
using System.Transactions;
using Administrativo.Enums;
using Administrativo.Helpers;

namespace Administrativo.Controllers
{
    [InitializeSimpleMembership]
    [CustomAuthorize(Roles = "Administrador,Colunistas")]
    public class ColunistasController : BaseController
    {
        public int areaADM = 5;

        [HttpPost]
        public bool ReOrder(int[] ids)
        {
            if (ids != null)
            {

                foreach (var id in ids.Select((value, i) => new { i, value }))
                {
                    Colunista colunista = db.Colunista.Find(id.value);
                    colunista.Ordem = id.i;
                    db.Entry(colunista).State = EntityState.Modified;

                }
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        //
        // GET: /Colunistas/
        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.ColunistaActive = "active";
            ViewBag.ColunistaSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassid = "sorting";
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

        public IPagedList<Colunista> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {
           var list = db.Colunista.Where(a => !a.excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(c => c.id == id || c.nome.ToLower().Contains(searchParam.ToLower()) || c.chave.ToLower().Contains(searchParam.ToLower()));

            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.nome);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.nome);
                        ViewBag.SortClassnome = "sorting_desc";
                    }
                    break;
                case "dataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderBy(c => c.Ordem);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Colunista>(pageNumber, 10);
        }

        //
        // GET: /Colunistas/Details/5

        public ActionResult Details(int id = 0)
        {
            Colunista colunista = db.Colunista.Find(id);

            if (colunista == null)
            {
                return HttpNotFound();
            }

            UserProfile user = _db.UserProfiles.Where(x => x.ColunistaId == colunista.id).FirstOrDefault();
            if (user != null)
            {
                ViewBag.UserName = user.UserName;
            }
            return View(colunista);
        }

        //
        // GET: /Colunistas/Create
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Colunistas/Create

        [HttpPost]
        public ActionResult Create(Colunista colunista, HttpPostedFileBase foto, HttpPostedFileBase fotoMini, string username = "", string password = "", int cropX = 0, int cropY = 0, int cropWidth = 190, int cropHeight = 132)
        {
            if (ModelState.IsValid)
            {
                string criarUsuario = Request.Form["CriarUsuario"];

                if (criarUsuario == "Sim")
                {
                    username = colunista.nome.GenerateSlug();
                    password = "123mudar";
                }

                if (foto != null)
                {
                    if (cropWidth > 0)
                        colunista.foto = Utils.SaveAndCropColunista(foto, Server.MapPath("~/Conteudo/Colunistas/Foto/"), cropX, cropY, cropWidth, cropHeight);
                    else
                        colunista.foto = Utils.SaveAndCropImage(foto, Server.MapPath("~/Conteudo/Colunistas/Foto/"), 0, 0, 190, 132);

                }
                colunista.dataCadastro = DateTime.Now;

                int suffix = 0;

                do
                {
                    colunista.chave = colunista.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Colunista.Where(o => o.chave == colunista.chave).Count() > 0);

                db.Colunista.Add(colunista);
                db.SaveChanges();

                try
                {
                    if (criarUsuario == "Sim")
                    {
                        WebSecurity.CreateUserAndAccount(username, password, propertyValues: new { ColunistaId = colunista.id, Email = colunista.email, Ativo = colunista.liberado, Nome = colunista.nome }, requireConfirmationToken: false);
                        Roles.AddUserToRole(username, "Colunistas");
                    }

                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("Cadastro", ErrorCodeToString(e.StatusCode));
                    //return View(colunista);
                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, colunista.id);

                return RedirectToAction("Index");
            }

            return View(colunista);
        }

        //
        // GET: /Colunistas/Edit/5
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id = 0)
        {
            Colunista colunista = db.Colunista.Find(id);
            if (colunista == null)
            {
                return HttpNotFound();
            }

            UserProfile user = _db.UserProfiles.Where(x => x.ColunistaId == colunista.id).FirstOrDefault();
            if (user != null)
            {
                ViewBag.UserName = user.UserName;
            }
            return View(colunista);
        }

        //
        // POST: /Colunistas/Edit/5

        [HttpPost]
        public ActionResult Edit(Colunista colunista, HttpPostedFileBase fotoUpload, HttpPostedFileBase fotoMiniUpload, string oldPassword = "", string username = "", string password = "", int cropX = 0, int cropY = 0, int cropWidth = 190, int cropHeight = 132)
        {
            if (ModelState.IsValid)
            {
                if (fotoUpload != null)
                {
                    if (cropWidth > 0 )
                        colunista.foto = Utils.SaveAndCropColunista(fotoUpload, Server.MapPath("~/Conteudo/Colunistas/Foto/"), cropX, cropY, cropWidth, cropHeight);
                    else
                        colunista.foto = Utils.SaveAndCropImage(fotoUpload, Server.MapPath("~/Conteudo/Colunistas/Foto/"), 0, 0, 190, 132);
                };

                //if (fotoMiniUpload != null)
                //{
                //    colunista.fotoMini = Utils.SaveAndCropImage(fotoMiniUpload, Server.MapPath("~/Conteudo/Colunistas/90x90/"), 0, 0, 90, 90);
                //}

                if (password != "" && oldPassword != "" && username != "")
                {
                    var OldUserName = _db.UserProfiles.Where(x => x.ColunistaId == colunista.id).Select(x => x.UserName).First();
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(OldUserName));
                    ViewBag.HasLocalPassword = hasLocalAccount;
                    ViewBag.ReturnUrl = Url.Action("Manage");
                    if (hasLocalAccount)
                    {
                        if (ModelState.IsValid)
                        {
                            // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                            bool changePasswordSucceeded;
                            try
                            {
                                changePasswordSucceeded = WebSecurity.ChangePassword(OldUserName, oldPassword, password);
                            }
                            catch (Exception)
                            {
                                changePasswordSucceeded = false;
                            }

                            if (!changePasswordSucceeded)
                            {
                                ModelState.AddModelError("Senha", "Seu password está incorreto ou o novo password é inválido.");

                                return View(colunista);
                            }
                        }
                    }

                }

                int suffix = 0;

                do
                {
                    colunista.chave = colunista.nome.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                    suffix++;
                } while (db.Noticias.Where(o => o.url == colunista.chave && o.id != colunista.id).Count() > 0);

                db.Entry(colunista).State = EntityState.Modified;
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, colunista.id);
                return RedirectToAction("Index");
            }
            return View(colunista);
        }

        //
        // GET: /Colunistas/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Colunista colunista = db.Colunista.Find(id);
            if (colunista == null)
            {
                return HttpNotFound();
            }
            return View(colunista);
        }

        //
        // POST: /Colunistas/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Colunista colunista = db.Colunista.Find(id);

            try
            {
                UserProfile userProfile = _db.UserProfiles.Where(x => x.ColunistaId == colunista.id).FirstOrDefault();

                if (userProfile != null)
                {

                    if (Roles.GetRolesForUser(userProfile.UserName).Count() > 0)
                    {
                        Roles.RemoveUserFromRoles(userProfile.UserName, Roles.GetRolesForUser(userProfile.UserName));
                    }

                    ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(userProfile.UserName); // deletes record from webpages_Membership table
                    ((SimpleMembershipProvider)Membership.Provider).DeleteUser(userProfile.UserName, true); // deletes record from UserProfile table
                    
                }

            }
            catch
            {
                return View(colunista);
            }

            colunista.excluido = true;
            db.Entry(colunista).State = EntityState.Modified;
            db.SaveChanges();

            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, colunista.id);
            return RedirectToAction("Index", "Colunistas");
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Nome de usuário já existe. Por favor entre com um nome diferente.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "O password não é válido. Por favor entre com um valor de password válido.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "O nome de usuário não é valido. Por favor entre com um nome diferente.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}




