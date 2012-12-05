using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Common.Util.Helpers
{
    /// <summary>
    /// Functionality to Ready a menu xml and render it to ul Li skelton to displaye the menu
    /// Displayed Menu can we of types
    /// </summary>
    /// <include file='Menu.js' path='[@name="~/Scripts/Menu.js"]'/>
    /// <example>
    /// Sample file in use
    ///<menu>
    ///     <menuitem text="A" title="" role=""  link="" />
    ///     <menuitem text="B" title="" role=""  link="" >
    ///         <menuitem text="B1" title="" role=""  link="" />
    ///         <menuitem text="B2" title="" role=""  link="" />
    ///     </menuitem>
    ///</menu>
    /// </example>
    public class UtilMenu
    {
        #region Fields

        public enum MenuTypes
        {
            Horizontal,
            Vertical,
        }

        XDocument _doc;
        IEnumerable<XElement> _eles;
        string _loginUserRole;
        Dictionary<string, bool> _permissions;
        MenuTypes _menuType;
        string _title;
        System.Web.UI.Page _page;

        List<UtilMenuItem> _items;

        string _menuUlID;

        #endregion Fields

        /// <param name="filePath">XML file path</param>
        /// <param name="loginUserRole">Role as string</param>
        /// <param name="menuType">Menu drop down orientation</param>
        /// <param name="title">Title in case of Verticale orientation</param>
        public UtilMenu(string menuUlID, System.Web.UI.Page page, string filePath, string loginUserRole, MenuTypes orientation = MenuTypes.Horizontal, string title = "", Dictionary<string, bool> permissions = null)
        {
            _menuUlID = menuUlID;
            _page = page;
            _loginUserRole = loginUserRole;
            _permissions = permissions;
            _doc = XDocument.Load(filePath);
            _eles = from e in _doc.Elements("menu").Elements("menuitem")
                    select e;
            _menuType = orientation;
            _title = title;
        }

        //Methods
        ///<summary>Get the formatted String for menu</summary>
        public string ParseMenu()
        {
            this._items = new List<UtilMenuItem>();
            foreach (var el in _eles)
                this._items.Add(GetMenuItemFromXElement(el, this._items));

            StringBuilder sb = new StringBuilder();
            sb.Append(GetToUlLi());
            //sb.Append(ResourceCommonUtil.Menu);
            return sb.ToString();
        }

        public string ResourceJS()
        {
            return string.Format("<script src='{0}' type='text/javascript'></script>"
                                 , _page.ClientScript.GetWebResourceUrl(typeof(Common.Util.Helpers.UtilMenu), "Common.Util.Resources.Menu.js")
                                );
        }

        string GetToUlLi()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<ul id=\"" + _menuUlID + "\" class=\"MenuItems" + this._menuType.ToString() + "\">");

            if (!string.IsNullOrEmpty(this._title))
            {
                sb.Append("<li class=\"Title\" >");
                sb.Append(this._title);
                sb.Append("</li>");
            }
            if (_items != null)
                foreach (var item in _items)
                {
                    UtilMenuItem mi = GetMenuItems(item, sb, string.Empty);
                }
            sb.Append("</ul>");
            return sb.ToString();
        }

        UtilMenuItem GetMenuItems(UtilMenuItem item, StringBuilder sb, string idParentA)
        {
            if (item.SubMenuItems == null)
            {
                if (HasAccess(item))
                {
                    sb.Append("<li title=\"" + item.Title + "\" class=\"" + item.CssClass + "\" >");
                    sb.Append("<a class=\"fnClsMenuLink Ajax\" href=\"" + item.Link + "\" " + (string.IsNullOrEmpty(idParentA) ? "" : "parent='" + idParentA + "'") + ">" + item.Text + "</a>");
                    sb.Append("</li>");
                }
                return item;
            }
            else
            {
                string id = Guid.NewGuid().ToString("n");

                if (HasAccess(item))
                {
                    idParentA = Guid.NewGuid().ToString("n");
                    sb.Append("<li class=\"LiSubMenu " + item.CssClass + "\"  title=\"" + item.Title + "\" rel=\"" + id + "\"  >");

                    if (!string.IsNullOrEmpty(item.IsParentClickable) && Convert.ToBoolean(item.IsParentClickable))
                        sb.Append("<a id='" + idParentA + "' href=\"" + item.Link + "\" " + (string.IsNullOrEmpty(idParentA) ? "" : "parent='" + idParentA + "'") + ">" + item.Text + "</a>");
                    else
                        sb.Append("<a id='" + idParentA + "' href=\"#\" >" + item.Text + "</a>");

                    sb.Append("<ul id=\"" + id + "\" style=\"display:none\" >");
                    foreach (var itm in item.SubMenuItems)
                    {
                        UtilMenuItem mi = GetMenuItems(itm, sb, idParentA);
                    }
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
                return item;
            }
        }

        bool HasAccess(UtilMenuItem item)
        {
            if (string.IsNullOrEmpty(item.Role))
                return true;

            string[] splitChar = new string[1] { "," };
            string[] roles = item.Role.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            string[] permission = item.Permission.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            if (roles.Length > 0)
            {
                if (!roles.Contains(_loginUserRole))
                    return false;
            }
            if (permission.Length > 0)
            {
                if (_permissions != null)
                {
                    //Check permissions for other users.
                    foreach (var perm in permission)
                    {
                        try
                        {
                            if (_permissions.Where(x => x.Key == perm && x.Value == true).Count() == 0)
                                return false;
                        }
                        catch
                        {
                            //empty on purpose
                        }
                    }
                }
            }
            return true;
        }

        //XML, read
        UtilMenuItem GetMenuItemFromXElement(XElement element, List<UtilMenuItem> items)
        {
            UtilMenuItem obj = new UtilMenuItem();
            obj.Link = ReadAttribute(element, "link");
            obj.Role = ReadAttribute(element, "role");
            obj.Permission = ReadAttribute(element, "permission");
            obj.Text = ReadAttribute(element, "text");
            obj.Title = ReadAttribute(element, "title");
            obj.CssClass = ReadAttribute(element, "cssclass");
            obj.IsParentClickable = ReadAttribute(element, "isParentClickable");
            if (element.IsEmpty)
            {
                return obj;
            }
            else
            {
                obj.SubMenuItems = new List<UtilMenuItem>();
                IEnumerable<XElement> eles = from e in element.Elements("menuitem")
                                             select e;
                foreach (var el in eles)
                {
                    obj.SubMenuItems.Add(GetMenuItemFromXElement(el, obj.SubMenuItems));
                }
                return obj;
            }
        }

        string ReadAttribute(XElement element, string attribute)
        {
            try
            {
                return element.Attribute(attribute).Value;
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public class UtilMenuItem
    {
        /// <summary>Displaye Text</summary>
        public string Text { get; set; }

        /// <summary>Href values</summary>
        public string Link { get; set; }

        /// <summary>Value for "Title" attribute</summary>
        public string Title { get; set; }

        /// <summary>Role Name, defines the access for menu item</summary>
        public string Role { get; set; }

        public string Permission { get; set; }

        public string CssClass { get; set; }

        public string IsParentClickable { get; set; }

        public List<UtilMenuItem> SubMenuItems { get; set; }
    }
}