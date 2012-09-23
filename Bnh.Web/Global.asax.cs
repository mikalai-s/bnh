﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Bnh.Cms;
using Bnh.Cms.Models.Utilities;
using Bnh.Core;
using Bnh.Infrastructure.WebSecurity;

namespace Bnh.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        IContainer container = null;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            MapControllerInvertRoutes(routes, "Community");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        private static void MapControllerInvertRoutes(RouteCollection routes, string controller)
        {
            routes.MapRoute(
                controller + 1,
                "Calgary/" + controller,
                new { controller = controller, action = "Index" }
            );

            routes.MapRoute(
                controller + 2,
                "Calgary/" + controller + "/Create",
                new { controller = controller, action = "Create" }
            );

            routes.MapRoute(
                controller + 3,
                "Calgary/" + controller + "/{id}/{action}",
                new { controller = controller, action = "Details" }
            );
        }

        protected void Application_Start()
        {
            RegisterAutofacModules();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            MsCms.Setup(this, 
                layout: "~/Views/Shared/_Layout.cshtml", 
                tinymce: "~/Scripts/tiny_mce/tiny_mce_src.js",
                googleMaps: "//maps.googleapis.com/maps/api/js?sensor=false");

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.DefaultBinder = new BnhModelBinder();

            Bnh.Web.Controllers.InitData.Init();

            RegisterExternalLoginServices();
        }

        private void RegisterExternalLoginServices()
        {
            var config = this.container.Resolve<Config>();
            if (config.Authentification == null)
            {
                return;
            }

            foreach (var client in config.Authentification)
            {
                OAuthWebSecurity.RegisterClient(client);
            }
        }

        private void RegisterAutofacModules()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<Bnh.Infrastructure.AutofacModule>();
            builder.RegisterModule<Bnh.Web.AutofacModule>();
            builder.RegisterModule<Bnh.Cms.AutofacModule>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(typeof(Bnh.Cms.AutofacModule).Assembly);
            this.container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(this.container));
        }
    }
}