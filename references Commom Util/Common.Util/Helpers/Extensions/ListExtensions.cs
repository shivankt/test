using System;
using System.Collections.Generic;

namespace Common.Util
{
    public static class ListExtensions
    {
        const string Format_DDL_Option_Select = "<option value=\"\" >--Select--</option>";
        const string Format_DDL_Option = "<option value=\"{0}\" >{1}</option>";
        const string Format_DDL_Selected_Option = "<option value=\"{0}\" selected=\"selected\">{1}</option>";

        /// <summary>To get the Drop Down List Options</summary>
        /// <param name="list">Collection of objects</param>
        /// <param name="valueProperty">Object Property to bind with DDL option value</param>
        /// <param name="txtProperty">Object Property to bind with DDL option Text</param>
        /// <param name="selectedValue">Optional:to set the selected value</param>
        /// <param name="isAddSelect">Set true to include --Select--  as first option</param>
        /// <returns>Concatenated string options</returns>
        public static string UtilGetDDLOptions<T>(this List<T> list, string valueProperty, string txtProperty, string selectedValue, bool isAddSelect)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (isAddSelect)
            {
                sb.Append(Format_DDL_Option_Select);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var obj in list)
                {
                    string txt = obj.UtilGetPropetyValue<object>(txtProperty).ToString();

                    string value = obj.UtilGetPropetyValue<object>(valueProperty).ToString();

                    if (!string.IsNullOrEmpty(selectedValue) && value.Equals(selectedValue))
                    {
                        sb.AppendFormat(Format_DDL_Selected_Option, value, txt);
                    }
                    else
                    {
                        sb.AppendFormat(Format_DDL_Option, value, txt);
                    }
                }
            }
            return sb.ToString();
        }

        public static string UtilGetDDLOptions(this Dictionary<string, string> obj, string selectedValue, bool isAddSelect)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (isAddSelect)
            {
                sb.Append(Format_DDL_Option_Select);
            }
            foreach (var item in obj)
            {
                string value = item.Key;
                string txt = item.Value;

                if (!string.IsNullOrEmpty(selectedValue) && selectedValue.Contains(value))
                {
                    sb.AppendFormat(Format_DDL_Selected_Option, value, txt);
                }
                else
                {
                    sb.AppendFormat(Format_DDL_Option, value, txt);
                }
            }
            return sb.ToString();
        }

        public static string UtilGetDDLOptionsForEnum(this Enum obj, string selectedValue, bool isCastValuesToInt, bool isAddSelect = false)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            foreach (var item in Enum.GetValues(obj.GetType()))
            {
                list.Add((isCastValuesToInt ? ((int)item).ToString() : item.ToString()), ((Enum)item).UtilGetEnumAsString());
            }

            return list.UtilGetDDLOptions(selectedValue, isAddSelect);
        }

        public static string UtilGetDDLOptionsForInitalEnum(this Enum obj, string selectedValue, bool isCastValuesToInt, bool isAddSelect = false)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            foreach (var item in Enum.GetValues(obj.GetType()))
            {
                if (item.ToString() != "None")
                    list.Add((isCastValuesToInt ? ((int)item).ToString() : item.ToString()), ((Enum)item).UtilGetEnumAsString());
            }

            return list.UtilGetDDLOptions(selectedValue, isAddSelect);
        }

        public static string UtilGetDDLOptionsForEmailEnum(this Enum obj, string selectedValue, bool isCastValuesToInt, bool isAddSelect = false)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            foreach (var item in Enum.GetValues(obj.GetType()))
            {
                if (item.ToString() != "Patient")
                    list.Add((isCastValuesToInt ? ((int)item).ToString() : item.ToString()), ((Enum)item).UtilGetEnumAsString());
            }

            return list.UtilGetDDLOptions(selectedValue, isAddSelect);
        }
        public static string UtilGetDDLOptionsForEnum(this Enum obj, string selectedValue, bool isValueSameAsText, bool isCastValuesToIntIfNotSameAsText, bool isAddSelect = false)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            foreach (var item in Enum.GetValues(obj.GetType()))
            {
                string val = string.Empty;
                string txt = ((Enum)item).UtilGetEnumAsString();
                if (isValueSameAsText)
                {
                    val = txt;
                }
                else
                {
                    val = (isCastValuesToIntIfNotSameAsText ? ((int)item).ToString() : item.ToString());
                }

                list.Add(val, txt);
            }

            return list.UtilGetDDLOptions(selectedValue, isAddSelect);
        }

        public static string UtilGetDDLOptionsForEnumByKey(this Enum obj, string selectedValue, string key, bool isCastValuesToInt, bool isAddSelect = false)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            foreach (var item in Enum.GetValues(obj.GetType()))
            {
                if (((Enum)item).UtilGetEnumAsString().StartsWith(key))
                    list.Add((isCastValuesToInt ? ((int)item).ToString() : item.ToString()), item.ToString());
            }

            return list.UtilGetDDLOptionsForEnumByKey(selectedValue, isAddSelect);
        }

        public static string UtilGetDDLOptionsForEnumByKey(this Dictionary<string, string> obj, string selectedValue, bool isAddSelect)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string[] ids = selectedValue.Split(",".ToCharArray(), StringSplitOptions.None);
            if (isAddSelect)
            {
                sb.Append(Format_DDL_Option_Select);
            }

            foreach (var item in obj)
            {
                string value = item.Key;
                string txt = item.Value;
                int counter = 0;
                foreach (var i in ids)
                {
                    if (i == value)
                    {
                        counter++;
                    }
                }
                if (counter > 0)
                {
                    sb.AppendFormat(Format_DDL_Selected_Option, value, txt);
                }
                else
                {
                    sb.AppendFormat(Format_DDL_Option, value, txt);
                }
                counter = 0;
            }

            return sb.ToString();
        }

    }
}