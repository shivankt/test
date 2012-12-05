using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using testing.Sql.Dapper;
using Common.Util.Models;

namespace testing.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        static MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        public ActionResult Index(string id)
        {
            if (object.Equals(id, null))
            {
                return View();
            }
            else
            {
                var result = new Models.Employee();
                using (conn)
                {
                    conn.Open();
                    string sql1 = string.Format("Select * from Employee where id={0}", id);
                    result = conn.Query<Models.Employee>(sql1).FirstOrDefault();
                    conn.Close();
                }

                return View(result);
            }
        }

        [HttpPost]
        public ActionResult Home(Models.Employee Emp,Pager pager)
        {
            var result = new List<Models.Employee>();
            string sql = string.Empty;
            using (conn)
            {
                conn.Open();
                if (Emp.id > 0)
                {
                    sql = string.Format(@"Update Employee set EmpName= '{0}',Email='{1}',Password='{2}',Role='{3}' where id={4}", Emp.EmpName, Emp.Email, Emp.Password, Emp.Role, Emp.id);
                    conn.Execute(sql);
                }
                else
                {
                    sql = "INSERT INTO Employee(EmpName,Email,Password,Role) VALUES (@EmpName,@Email,@Password,@Role)";
                    conn.Execute(sql, new { Emp.EmpName, Emp.Email, Emp.Password, Emp.Role });
                }
                
                string sql1 = string.Format("Select * from Employee ");
                string qrycount = string.Format(@"select count(e.id)'count' from Employee e");
                ViewBag.PagerReceived= pager;
                result = conn.Query<Models.Employee>(sql1).ToList();
                conn.Close();
            }

            return View(result);
        }

   
        public ActionResult Delete(string id)
        {
            var result = new List<Models.Employee>();
            string sql = string.Empty;
            using (conn)
            {
                conn.Open();
                sql = string.Format(@"Delete from Employee where id={0}", id);
                conn.Execute(sql);


                string sql1 = string.Format("Select * from Employee ");
                result = conn.Query<Models.Employee>(sql1).ToList();
                conn.Close();
            }

            return View("Home",result);
        }
    }
}
