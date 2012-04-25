using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Razor;
using System.IO;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Dynamic;

namespace Bnh.WebFramework
{
    public class RazorEngine
    {
        public static string GetContent(string template, dynamic model)
        {
            if(string.IsNullOrEmpty(template)) { return string.Empty; }

            const string dynamicallyGeneratedClassName = "DynamicContentTemplate";
            const string namespaceForDynamicClasses = "bnh";
            const string dynamicClassFullName = namespaceForDynamicClasses + "." + dynamicallyGeneratedClassName;

            var language = new CSharpRazorCodeLanguage();
            var host = new RazorEngineHost(language)
            {
                DefaultBaseClass = typeof(DynamicContentGeneratorBase).FullName,
                DefaultClassName = dynamicallyGeneratedClassName,
                DefaultNamespace = namespaceForDynamicClasses,
            };
            host.NamespaceImports.Add("System");
            host.NamespaceImports.Add("System.Dynamic");
            host.NamespaceImports.Add("System.Text");
            var engine = new RazorTemplateEngine(host);

            var tr = new StringReader(template); // here is where the string come in place
            GeneratorResults razorTemplate = engine.GenerateCode(tr);

            var compilerParameters = new CompilerParameters();
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
            compilerParameters.ReferencedAssemblies.Add(typeof(DynamicContentGeneratorBase).Assembly.Location);
            compilerParameters.GenerateInMemory = true;

            CompilerResults compilerResults = new CSharpCodeProvider().CompileAssemblyFromDom(compilerParameters, razorTemplate.GeneratedCode);
            if (compilerResults.Errors.Count > 0)
            {
                return "Error: " + compilerResults.Errors[1].ToString();
            }
            var compiledAssembly = compilerResults.CompiledAssembly;

            var templateInstance = (DynamicContentGeneratorBase)compiledAssembly.CreateInstance(dynamicClassFullName);

            templateInstance.DynModel = model;

            return templateInstance.GetContent();
        }        
    }

    public abstract class DynamicContentGeneratorBase
    {
        private StringBuilder buffer;
        protected DynamicContentGeneratorBase()
        {
            DynModel = new ExpandoObject();
        }

        /// <summary>
        /// This is just a custom property
        /// </summary>
        public dynamic DynModel { get; set; }

        /// <summary>
        /// This method is required and have to be exactly as declared here.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// This method is required and can be public but have to have exactly the same signature
        /// </summary>
        protected void Write(object value)
        {
            WriteLiteral(value);
        }

        /// <summary>
        /// This method is required and can be public but have to have exactly the same signature
        /// </summary>
        protected void WriteLiteral(object value)
        {
            buffer.Append(value);
        }

        /// <summary>
        /// This method is just to have the rendered content without call Execute.
        /// </summary>
        /// <returns>The rendered content.</returns>
        public string GetContent()
        {
            buffer = new StringBuilder(1024);
            try
            {
                Execute();
            }
            catch (Exception e)
            {
                WriteLiteral("Error: " + e.Message);
            }
            
            return buffer.ToString();
        }
    }
}
