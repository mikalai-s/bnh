using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;

namespace Bnh.Controllers
{
    public class SinglePageAttribute : ActionFilterAttribute
    {
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string Module { get; set; }

        protected string GetModuleName(ActionDescriptor actionDescriptor)
        {
            return Utils.GetSpaModuleName(actionDescriptor.ControllerDescriptor.ControllerName, actionDescriptor.ActionName);
        }

        protected string GetSafePageTitle(ActionDescriptor actionDescriptor)
        {
            return !string.IsNullOrWhiteSpace(this.PageTitle) ? this.PageTitle : actionDescriptor.ActionName;
        }

        protected string GetSafePageDescription()
        {
            return this.PageDescription ?? string.Empty;
        }

        protected bool RequiresScript(string moduleName, HttpContextBase context)
        {
            return File.Exists(context.Server.MapPath(GetModuleScriptFilePath(moduleName)));
        }

        protected string GetModuleScriptFilePath(string module)
        {
            return Path.Combine("~/Scripts", module + ".js");
        }

        protected string GetModuleTemplateFilePath(string module)
        {
            return Path.Combine("~/Scripts", module + ".htm");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                //var page = GetPageObject(context, false);

                //var templateFileName = context.HttpContext.Server.MapPath(GetModuleTemplateFilePath(page.module));
                //// if (!File.Exists(templateFileName))
                //{
                //    // there is need to create template file for given module
                //    context.Controller.ViewData.Model = GetEmptyPageObject(page);
                //    var templateResult = new TemplateViewResult
                //    {
                //        ViewName = context.ActionDescriptor.ActionName,
                //        ViewData = context.Controller.ViewData,
                //        TempData = context.Controller.TempData
                //    };
                //    templateResult.ExecuteResult(context.Controller.ControllerContext);
                //    using (var ts = File.CreateText(templateFileName))
                //    {
                //        ts.Write(templateResult.Result);
                //    }
                //}

                //context.Result = new JsonResult
                //{
                //    Data = page,
                //    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                //};
            }
            else
            {
                context.Controller.ViewBag.Module = string.IsNullOrEmpty(Module) ? GetModuleName(context.ActionDescriptor) : Module;
                base.OnActionExecuting(context);
            }
            //else
            //{
            //    context.Controller.ViewData.Model = GetPageObject(context);
            //    context.Result = new ViewResult
            //    {
            //        ViewName = context.ActionDescriptor.ActionName,
            //        MasterName = "~/Views/Shared/_Layout.cshtml",
            //        ViewData = context.Controller.ViewData,
            //        TempData = context.Controller.TempData
            //    };
            //}
        }

        //private object GetEmptyPageObject(Page page)
        //{
        //    return new Page
        //    {
        //        body = page.body.GetType().CreateEmptyExpandedObject()
        //    };
        //}



        //private Page GetPageObject(ActionExecutingContext context, bool anonymousBody = true)
        //{
        //    var body = context.ActionDescriptor.Execute(context.Controller.ControllerContext, context.ActionParameters);
        //    var page = body as Page;
        //    if (page == null)
        //    {
        //        page = new Kope.Models.Page
        //        {
        //            title = GetSafePageTitle(context.ActionDescriptor),
        //            description = GetSafePageDescription(),
        //            body = body
        //        };
        //    }
        //    else
        //    {
        //        page.title = string.IsNullOrEmpty(page.title) ? GetSafePageTitle(context.ActionDescriptor) : page.title;
        //        page.description = string.IsNullOrEmpty(page.description) ? GetSafePageDescription() : page.description;
        //    }

        //    page.body = (anonymousBody && page.body.GetType().IsAnonymousType()) ? page.body.ToExpando() : page.body;
        //    page.module = GetModuleName(context.ActionDescriptor);

        //    return page;
        //}
    }

    public class TemplateViewResult : PartialViewResult
    {
        public string Result { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(this.ViewName))
            {
                this.ViewName = context.RouteData.GetRequiredString("action");
            }
            ViewEngineResult result = null;
            if (this.View == null)
            {
                result = this.FindView(context);
                this.View = result.View;
            }
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                ViewContext viewContext = new ViewContext(context, this.View, this.ViewData, this.TempData, sw);
                this.View.Render(viewContext, sw);
                if (result != null)
                {
                    result.ViewEngine.ReleaseView(context, this.View);
                }
            }
            this.Result = sb.ToString();
        }
    }


    public static class ObjectExtensions
    {
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
            {
                object value = null;

                var ie = item.Value as IEnumerable;
                if (ie != null && ie.GetType().Name.Contains("AnonymousType"))
                {
                    var list = new List<ExpandoObject>();
                    foreach (var i in ie)
                        list.Add(i.ToExpando());
                    value = list;
                }
                else
                {
                    value = item.Value;
                }

                expando.Add(new KeyValuePair<string, object>(item.Key, value));
            }
            return (ExpandoObject)expando;
        }

        public static bool IsAnonymousType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (((!Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) || !type.IsGenericType) || !type.Name.Contains("AnonymousType")) || (!type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) && !type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            TypeAttributes attributes = type.Attributes;
            return (0 == 0);
        }

        public static object CreateEmptyExpandedObject(this Type type)
        {
            var enumerable = typeof(IEnumerable).IsAssignableFrom(type) && typeof(string) != type;
            var genericType = enumerable ? type.GetElementType() : type;

            if (typeof(string) == genericType)
                return enumerable ? (object)new[] { string.Empty } : string.Empty;
            else if (type.IsPrimitive)
                return enumerable ? (object)new[] { Activator.CreateInstance(genericType) } : Activator.CreateInstance(genericType);

            IDictionary<string, object> obj = new ExpandoObject();

            foreach (var property in genericType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                obj[property.Name] = CreateEmptyExpandedObject(property.PropertyType);
            }

            return enumerable ? (object)new[] { obj } : obj;
        }
    }

    public static class Utils
    {
        public static string GetSpaModuleName(string controller, string action)
        {
            return string.Format("views/{0}-{1}", controller.ToLower(), action.ToLower());
        }
    }
}