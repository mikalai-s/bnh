using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections;
using Ms.Cms.Models;
using System.Data.Objects.DataClasses;

using TypeMap = System.Collections.Generic.Dictionary<string, System.Type>;

namespace Ms.Cms.Web
{
    public class BnhModelBinder : DefaultModelBinder
    {
        // type mapping
        public static readonly Dictionary<Type, TypeMap> HierarchyTypeMap = new Dictionary<Type, TypeMap>
        {
            {
                typeof(Brick), new TypeMap
                {
                    {"0", typeof(EmptyBrick)},
                    {"1", typeof(HtmlBrick)},
                    {"2", typeof(GalleryBrick)},
                    {"3", typeof(MapBrick)},
                    {"4", typeof(RazorBrick)},
                    {"5", typeof(SharedBrick)},
                    {"6", typeof(TocBrick)}
                }
            }
        };

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            bool hasPrefix = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
            string prefix = ((hasPrefix) && (bindingContext.ModelName != "")) ? bindingContext.ModelName + "." : "";

            if (!HierarchyTypeMap.ContainsKey(modelType))
            {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }

            // get the parameter species
            var typeMap = HierarchyTypeMap[modelType];
            var result = bindingContext.ValueProvider.GetValue(prefix + "Type");
            if (result == null || !typeMap.ContainsKey(result.AttemptedValue))
            {
                throw new Exception(string.Format("Unknown type \"{0}\"", result.AttemptedValue));
            }

            var type = typeMap[result.AttemptedValue];
            var model = Activator.CreateInstance(type);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);
            return model;
        }        

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            ModelMetadata propertyMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
            propertyMetadata.Model = value;
            string modelStateKey = CreateSubPropertyName(bindingContext.ModelName, propertyMetadata.PropertyName);

            // Try to set a value into the property unless we know it will fail (read-only 
            // properties and null values with non-nullable types)
            if (!propertyDescriptor.IsReadOnly)
            {
                try
                {
                    if (value == null)
                    {
                        propertyDescriptor.SetValue(bindingContext.Model, value);
                    }
                    else
                    {
                        Type valueType = value.GetType();

                        if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(EntityCollection<>))
                        {
                            IListSource ls = (IListSource)propertyDescriptor.GetValue(bindingContext.Model);
                            IList list = ls.GetList();

                            foreach (var item in (IEnumerable)value)
                            {
                                list.Add(item);
                            }
                        }
                        else
                        {
                            propertyDescriptor.SetValue(bindingContext.Model, value);
                        }
                    }

                }
                catch (Exception ex)
                {
                    // Only add if we're not already invalid
                    if (bindingContext.ModelState.IsValidField(modelStateKey))
                    {
                        bindingContext.ModelState.AddModelError(modelStateKey, ex);
                    }
                }
            }
        }
    }
}
