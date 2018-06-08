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
        public JObject GetRelevantLogs(string selection)
        {
            JObject data;
            if (logFiles != null && logFiles.ElementAt(0) != null &&
                logFiles.ElementAt(0).Logs != null)
            {

                foreach (var log in logFiles.ElementAt(0).Logs)
                {
                    if (selection!= null && (selection.Equals("INFO") || selection.Equals("FAIL") ||
                        selection.Equals("WARNING")))
                    {
                        if (selection.Equals(log.Type.ToString())) {
                            data = new JObject();
                            data["Message"] = log.Message;
                            data["Type"] = log.Type.ToString();
                            return data;
                        }
                    }
                    else
                    {
                        data = new JObject();
                        data["Message"] = log.Message;
                        data["Type"] = log.Type.ToString();
                        return data;
                    }
                }
            }
            data = new JObject();
            data["Message"] = "in get relevant logs";
            data["Type"] = "INFO";
            return data;
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
            foreach (Employee emp in employees) {
                if (emp.ID.Equals(id)) { 
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


    }
}
