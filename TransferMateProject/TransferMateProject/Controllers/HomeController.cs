using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using TransferMateProject.App_Classes;
using TransferMateProject.Models;
namespace TransferMateProject.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return View();
        }

        [Authorize]
        public ActionResult TodoList()
        {
            Todolist todo = new Todolist();
            var todolist = todo.UnfilterTodolist();
            TempData["Canceled"] = todolist.Where(x => x.Status1.StatusName == "Canceled").Count();
            TempData["Waiting"] = todolist.Where(x => x.Status1.StatusName == "Waiting").Count();
            TempData["Completed"] = todolist.Where(x => x.Status1.StatusName == "Completed").Count();
            ViewBag.Status = todo.GetStatusDropdownlist();
            return View();
        }

        [Authorize]
        public PartialViewResult TodolistList(TodolistFilter filter, TodolistPaging paging)
        {
            Todolist todo = new Todolist();
            int totalrecords = 0;
            if (paging.page < 1)
            {
                paging.page = 1;
            }
            paging.skip = (paging.page * paging.pagesize) - paging.pagesize;
            List<Task> Alltask = todo.GetTodolist(filter, paging, out totalrecords);
            ViewBag.Totalrecords = totalrecords;
            return PartialView(Alltask);
        }

        [AllowAnonymous]
        public JsonResult JsonLogin(Users user)
        {
            if (string.IsNullOrEmpty(user.Username))
            {
                return Json(new JsonReturnClass { IsSuccess = false, Message = "Username Cannot be empty" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                return Json(new JsonReturnClass { IsSuccess = false, Message = "Password Cannot be empty" }, JsonRequestBehavior.AllowGet);
            }
            UserLogin login = new UserLogin();
            user.Password = CreateMD5(user.Password);
            bool IsUserAuth = login.UserIsAuth(user);
            if (IsUserAuth)
            {
                FormsAuthenticationTicket ticket;
                ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now.ToUniversalTime(), DateTime.Now.AddHours(1).ToUniversalTime(), true, "");
                string enticket = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, enticket);
                cookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(cookie);
            }
            else
            {
                return Json(new JsonReturnClass { IsSuccess = false, Message = "Username or Password is incorrect" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonReturnClass { IsSuccess = true, Message = "", Url = "/Home/TodoList" }, JsonRequestBehavior.AllowGet);
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        [Authorize]
        public JsonResult JsonTodoCreate(Task todo)
        {
            if (string.IsNullOrEmpty(todo.TaskName))
            {
                return Json(new JsonReturnClass { IsSuccess = false, Message = "Taskname Cannot be empty" }, JsonRequestBehavior.AllowGet);
            }
            if (todo.StartDate > todo.EndDate)
            {
                return Json(new JsonReturnClass { IsSuccess = false, Message = "The start date cannot be greater than the end date" }, JsonRequestBehavior.AllowGet);
            }
            Todolist todoc = new Todolist();
            todoc.CreateTodolist(todo);
            return Json(new JsonReturnClass { IsSuccess = true,Url="/Home/Todolist" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult JsonTodoUpdate(Task todo) 
        {
            if (string.IsNullOrEmpty(todo.TaskName))
            {
                return Json(new JsonReturnClass { IsSuccess = false, Message = "Taskname Cannot be empty" }, JsonRequestBehavior.AllowGet);
            }
            if (todo.StartDate > todo.EndDate)
            {
                return Json(new JsonReturnClass { IsSuccess = false, Message = "The start date cannot be greater than the end date" }, JsonRequestBehavior.AllowGet);
            }
            Todolist todoc = new Todolist();
            todoc.UpdateTodolist(todo);
            return Json(new JsonReturnClass { IsSuccess = true, Url = "/Home/Todolist" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult JsonTodoDelete(Task todo) 
        {
            Todolist todoc = new Todolist();
            todoc.DeleteTodoList(todo);
            return Json(new JsonReturnClass { IsSuccess = true, Url = "/Home/Todolist" }, JsonRequestBehavior.AllowGet);
        }
    }
}