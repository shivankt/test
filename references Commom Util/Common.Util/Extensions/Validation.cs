using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace Common.Util.Extensions
{
    public static class Validation
    {
        //public static bool Validate<T>(this T obj, string keySufix, Dictionary<string, string> result)
        //{
        //    try
        //    {
        //        ValidationResults summary = Validation.Validate<T>(obj);
        //        if (summary != null && summary.Count > 0)
        //        {
        //            foreach (var item in summary)
        //            {
        //                string key = string.Empty;
        //                if (!string.IsNullOrEmpty(item.Tag))
        //                    key = keySufix + item.Tag;
        //                else
        //                    key = keySufix + item.Key;

        //                if (!result.Keys.Contains(key))
        //                    result.Add(key, item.Message);
        //            }
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Add("Exception", ex.Message);
        //        return false;
        //    }
        //}

        /// <summary>Check the added data annotations and Validate model</summary>
        public static bool ValidateModel(this object obj, ref Dictionary<string, string> listErrors)
        {
            // get the name of the buddy class for obj
            MetadataTypeAttribute metadataAttrib = obj.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault() as MetadataTypeAttribute;

            // if metadataAttrib is null, then obj doesn't have a buddy class, and in such a case, we'll work with the model class
            Type buddyClassOrModelClass = metadataAttrib != null ? metadataAttrib.MetadataClassType : obj.GetType();

            var buddyClassProperties = TypeDescriptor.GetProperties(buddyClassOrModelClass).Cast<PropertyDescriptor>();
            var modelClassProperties = TypeDescriptor.GetProperties(obj.GetType()).Cast<PropertyDescriptor>();

            var errors = from modelProp in modelClassProperties
                         from attribute in modelProp.Attributes.OfType<ValidationAttribute>() // get only the attributes of type ValidationAttribute
                         where !attribute.IsValid(modelProp.GetValue(obj))
                         select new KeyValuePair<string, string>(modelProp.Name, attribute.FormatErrorMessage(string.Empty));


            if (errors != null && errors.Count() > 0)
            {
                foreach (var item in errors)
                {
                    if (listErrors.ContainsKey(item.Key))
                        listErrors[item.Key] = string.Format("{0}. {1}", listErrors[item.Key], item.Value);
                    else
                        listErrors.Add(item.Key, item.Value);
                }
                return false;
            }

            return true;
        }

        public static string ToErrorString(this Dictionary<string, string> listErrors, string seprator = "<br/>")
        {
            return string.Join(seprator, listErrors.Select(p => p.Value).ToArray());
        }
    }
}
