using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using TransferMateProject.Models;

namespace TransferMateProject.App_Classes
{
    public class Helper
    {

    }

    public class UserLogin
    {
        public bool UserIsAuth(Users user)
        {
            using (var db = new TodoListEntities())
            {
                int control = db.Users.Where(x => x.Username == user.Username && x.Password == user.Password).Count();
                if (control > 0)
                {
                    var isuser = db.Users.Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
                    isuser.LastLoginDate = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }

    public class Todolist
    {
        public List<Task> GetTodolist(TodolistFilter filter, TodolistPaging paging, out int totalrecords)
        {
            using (var db = new TodoListEntities())
            {
                DateTime start = DateTime.Now.AddDays(-15);
                DateTime end = DateTime.Now.AddDays(1);
                var todolist = (from t in db.Task.Include("Status1").Where(x => EntityFunctions.TruncateTime(x.StartDate) >= start && EntityFunctions.TruncateTime(x.EndDate) <= end) select t);
                if (!string.IsNullOrEmpty(filter.Taskname))
                {
                    todolist = todolist.Where(x => x.TaskName.Contains(filter.Taskname));
                }
                if (filter.Status > 0)
                {
                    todolist = todolist.Where(x => x.Status == filter.Status);
                }
                else
                {
                    todolist = todolist.Where(x => x.Status1.StatusName != "Completed");
                }
                if (filter.Startdate.Year > 1 && filter.Enddate.Year > 1)
                {
                    todolist = todolist.Where(x => DbFunctions.TruncateTime(x.StartDate) >= filter.Startdate && DbFunctions.TruncateTime(x.EndDate) <= filter.Enddate);
                }
                totalrecords = todolist.Count();
                todolist = todolist.OrderByDescending(x => x.id).Skip(paging.skip).Take(paging.pagesize);
                return todolist.ToList();
            }
        }

        public List<Task> UnfilterTodolist()
        {
            using (var db = new TodoListEntities())
            {
                var todolist = (from t in db.Task.Include("Status1") select t);
                return todolist.ToList();
            }
        }

        public List<Status> GetStatusDropdownlist()
        {
            using (var db = new TodoListEntities())
            {
                return db.Status.ToList();
            }
        }

        public void CreateTodolist(Task todo)
        {
            using (var db = new TodoListEntities())
            {
                db.Task.Add(todo);
                db.SaveChanges();
            }
        }

        public void UpdateTodolist(Task todo)
        {
            using (var db = new TodoListEntities())
            {
                var todolist = db.Task.Find(todo.id);
                todolist.TaskName = todo.TaskName;
                todolist.StartDate = todo.StartDate;
                todolist.EndDate = todo.EndDate;
                todolist.Status = todo.Status;
                db.SaveChanges();
            }
        }

        public void DeleteTodoList(Task todo)
        {
            using (var db = new TodoListEntities())
            {
                var todolist = db.Task.Find(todo.id);
                db.Task.Remove(todolist);
                db.SaveChanges();
            }
        }
    }

    public class TodolistFilter
    {
        public string Taskname { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public int Status { get; set; }
    }

    public class TodolistPaging
    {
        public int pagesize { get; set; } = 10;
        public int page { get; set; }
        public int skip { get; set; }
        public int take { get; set; }

    }

    #region JsonReturn
    public class JsonReturnClass
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
    #endregion

}