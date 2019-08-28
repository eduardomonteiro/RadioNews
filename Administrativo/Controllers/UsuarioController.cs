using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Administrativo.Filters;
using Administrativo.Models;
using System.Data;
using System.Data.Entity;
using PagedList;
using Administrativo.Commons;
using Administrativo.Helpers;
using Administrativo.Enums;

namespace Administrativo.Controllers
{
    [InitializeSimpleMembership]
    [CustomAuthorize(Roles = "Administrador,Usuario")]
    public class UsuariosController : BaseController
    {
        public int areaADM = 23;
        //
        // GET: /Usuarios/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.UserProfileActive = "active";
            ViewBag.UserProfileSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassUserId = "sorting";
            ViewBag.SortClassUserName = "sorting";
            ViewBag.SortClassUserFullName = "sorting";
            ViewBag.SortClassUserEmail = "sorting";
            ViewBag.SortClassUserDate = "sorting";

        }

        public IPagedList<UserProfile> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var pagedList = _db.UserProfiles.Where(user => !user.Excluido);
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                //int outDate = 0;
                //int title = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(t => t.UserName.ToLower().Contains(searchParam.ToLower()));

            }


            switch (sortField)
            {
                case "UserId":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(t => t.UserId);
                        ViewBag.SortClasscod = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(t => t.UserId);
                        ViewBag.SortClasscod = "sorting_desc";
                    }
                    break;
                case "UserName":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(t => t.UserName);
                        ViewBag.SortClasstitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(t => t.UserName);
                        ViewBag.SortClasstitulo = "sorting_desc";
                    }
                    break;

                default:
                    pagedList = pagedList.OrderByDescending(t => t.UserId);
                    ViewBag.SortClasscod = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<UserProfile>(pageNumber, 10);
        }




        
        public ActionResult Cadastrar()
        {

            List<string> roles = Roles.GetAllRoles().ToList();
            ViewBag.Roles = roles.Where(r => r != "Administrador").ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(RegisterModel user, string[] roles)
        {

            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(user.UserName, user.Password, new { Nome = user.Nome, Email = user.Email, Ativo = user.Ativo });

                if (roles != null)
                {
                    if (roles.Contains("Administrador"))
                    {
                        Roles.AddUserToRole(user.UserName, "Administrador");
                    }
                    else
                    {
                        Roles.AddUserToRoles(user.UserName, roles);
                    }
                }
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, WebSecurity.GetUserId(user.UserName));

                return RedirectToAction("Index");
            }

            ViewBag.Roles = Roles.GetAllRoles().Where(r => r != "Administrador").ToList();
            return View(user);
        }


        public ActionResult Editar(int id)
        {

            UserProfile User = _db.UserProfiles.Find(id);

            //var roles = new List<RolesViewModel>();

            //foreach (string role in Roles.GetAllRoles().Where(r => r != "Administrador").ToList())
            //{
            //   roles.Add(new RolesViewModel { Role = role, Checked = Roles.IsUserInRole(User.UserName, role) ? "checked" : "" });
            //}

            //ViewBag.Roles = roles;

            ViewBag.Roles = Roles.GetRolesForUser(User.UserName);


            return View(User);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(UserProfile user, string[] roles)
        {

            if (ModelState.IsValid)
            {
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                if (Roles.GetRolesForUser(user.UserName).Length > 0)
                {
                    Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                }

                if (roles != null)
                {
                    if (roles.Contains("Administrador"))
                    {
                        Roles.AddUserToRole(user.UserName, "Administrador");
                    }
                    else
                    {
                        Roles.AddUserToRoles(user.UserName, roles);
                    }
                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, WebSecurity.GetUserId(user.UserName));

                return RedirectToAction("Index");
            }

            ViewBag.Roles = Roles.GetRolesForUser(user.UserName);
            ViewBag.AllRoles = Roles.GetAllRoles().Where(r => r != "Administrador");
            return View();
        }

        [HttpPost]
        public JsonResult AlterarSenha(PasswordChangeModel mudarSenha)
        {
            var retorno = false;

            if (ModelState.IsValid && mudarSenha.Senha == mudarSenha.Confirmar)
            {
                var usuario = _db.UserProfiles.Find(mudarSenha.Id);
                WebSecurity.ChangePassword(usuario.UserName, mudarSenha.Antiga, mudarSenha.Senha);
                GerenciaLogs.saveLog(ref db, usuario.UserId, 23, TipoAcesso.Edicao);
                retorno = true;
            }

            return Json(retorno);
        }

        public JsonResult CheckUserName(string username)
        {
            var result = WebSecurity.UserExists(username);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Servicos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserProfile userProfile = _db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        //
        // POST: /Usuarios/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirme(int id)
        {
            try
            {
                UserProfile userProfile = _db.UserProfiles.Find(id);

                // TODO: Add delete logic here
                if (Roles.GetRolesForUser(userProfile.UserName).Count() > 0)
                {
                    Roles.RemoveUserFromRoles(userProfile.UserName, Roles.GetRolesForUser(userProfile.UserName));
                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, WebSecurity.GetUserId(userProfile.UserName));

                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(userProfile.UserName); // deletes record from webpages_Membership table
                
                userProfile.Excluido = true;
                
                _db.SaveChanges();


                //((SimpleMembershipProvider)Membership.Provider).DeleteUser(userProfile.UserName, false); // deletes record from UserProfile table


                return RedirectToAction("index", "Usuarios");
                
            }
            catch (Exception ex)
            {
                return View(id);
            }
        }

    }
}
