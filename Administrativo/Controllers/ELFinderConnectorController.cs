﻿using Administrativo.Commons;
using ELFinder.Connector.ASPNet.Controllers;
using ELFinder.WebServer.ASPNet.Config;
using System.Web.Http;

namespace ELFinder.WebServer.ASPNet.Controllers
{

    /// <summary>
    /// ELFinder connector controller
    /// IMPORTANT: If you use this controller in your existing application, make sure to register the custom ELFinder model binder (ELFinderModelBinder) with ASP.NET MVC.
    ///            Look at the Application_Start() method in Global.asax.cs on how to do so.
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "Administrador,Esportes,Especiais,Eventos,Colunistas,Editorial Noticias")]
    public class ELFinderConnectorController : ELFinderBaseConnectorController
    {

        #region Constructors

        /// <summary>
        /// Create a new instance
        /// </summary>
        public ELFinderConnectorController() : base(SharedConfig.ELFinder)
        {
        }

        #endregion

    }
}