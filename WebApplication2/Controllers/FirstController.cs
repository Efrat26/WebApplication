using Logs.ImageService.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
        static String potenialRemovedHandler;
        static PhotosModel photos = new PhotosModel();
        static LogsModel logs = new LogsModel();
        static List<LogsModel> logFiles = new List<LogsModel>()
        {
            logs
        };
        static ConfigModel config = new ConfigModel();
        static List<ConfigModel> conf = new List<ConfigModel>()
        {
            config
        };
        static ImageWebModel imageWeb = new ImageWebModel();
        static List<ImageWebModel> m = new List<ImageWebModel>()
        {
            imageWeb
        };
        static List<Employee> employees = new List<Employee>()
        {
            //
          new Employee  { FirstName = "Moshe", LastName = "Aron", Email = "Stam@stamstam", Salary = 10000, Phone = "08-8888888" },
          new Employee  { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 2000, Phone = "08-8888888" },
          new Employee   { FirstName = "Mor", LastName = "Sinai", Email = "Stam@stam", Salary = 500, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 20, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 700, Phone = "08-8888888" }
        };
        // GET: First
        public ActionResult Index()
        {
            return View(m);
        }

        [HttpGet]
        public ActionResult Logs()
        {
            return View(logFiles);
        }

        [HttpGet]
        public JObject GetEmployee()
        {
            JObject data = new JObject();
            data["Message"] = "Kuky";
            data["Type"] = "Mopy";
            return data;
        }

        [HttpPost]
        public JObject GetRelevantLogs(string selection, int index)
        {
            JObject data;
            if (selection != null && index >= 0 && logs != null && logs.Logs != null)
            {
                LogMessage current = logs.Logs.ElementAt(index);
                string all = "ALL";
                if (selection.Equals(current.Type.ToString()) || selection.Equals(all))
                {
                    data = new JObject();
                    data["Message"] = current.Message;
                    data["Type"] = current.Type.ToString();
                    return data;
                }
            }
            else
            {
                return null;
            }
            return null;
        }
        // GET: First/Details
        [HttpGet]
        public ActionResult Details()
        {
            return View(employees);
        }
        // GET: First/Config
        [HttpGet]
        public ActionResult Config()
        {
            config.ClientAdapter.GetAppConfig();
            return View(conf);
        }

        [HttpGet]
        public ActionResult DeleteHandler(String handlerToRemove)
        {
            if (handlerToRemove != null)
            {
                potenialRemovedHandler = handlerToRemove;
                ViewBag.handler = handlerToRemove;
                return View("DeleteHandler");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Remove(String answer)
        {
            return RedirectToAction("Index");
        }
        // POST: First/Create
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            try
            {
                employees.Add(emp);

                return RedirectToAction("Details");
            }
            catch
            {
                return View();
            }
        }

        // GET: First/Edit/5
        public ActionResult Edit(int id)
        {
            foreach (Employee emp in employees)
            {
                if (emp.ID.Equals(id))
                {
                    return View(emp);
                }
            }
            return View("Error");
        }

        // POST: First/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Employee empT)
        {
            try
            {
                foreach (Employee emp in employees)
                {
                    if (emp.ID.Equals(id))
                    {
                        emp.copy(empT);
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // GET: First/Delete/5
        public ActionResult Delete(int id)
        {
            int i = 0;
            foreach (Employee emp in employees)
            {
                if (emp.ID.Equals(id))
                {
                    employees.RemoveAt(i);
                    return RedirectToAction("Details");
                }
                i++;
            }
            return RedirectToAction("Error");
        }
        [HttpPost]
        public bool RemoveHandler()
        {
            if(config != null && potenialRemovedHandler != null)
            {
                while (!config.ClientAdapter.RemoveHandler(potenialRemovedHandler)) { }
            }
            //potenialRemovedHandler = null;
            bool answer = true;
            return answer;
        }
    }
}
