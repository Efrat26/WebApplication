using Logs.ImageService.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Web;
namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
        static String potentialDeletedPhoto;
        static String potentialDeletedPhotoThumbnail;
        int indexOfPhotoToBeRemoved, indexOfHandlerToBeRemoved;
        static String potenialRemovedHandler;
        static PhotosModel photosModel = new PhotosModel();
        static List<PhotosModel> photos = new List<PhotosModel>()
        {
            photosModel
        };
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
        // GET: First/Photos
        [HttpGet]
        public ActionResult Photos()
        {
            return View(photos);
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
            bool answer = false;
            if (config != null && potenialRemovedHandler != null &&imageWeb.IsConnected)
            {
                //find index of handler:
                for (int i = 0; i < config.Handlers.Count; ++i)
                {
                    if (config.Handlers.ElementAt(i).Equals(potenialRemovedHandler))
                    {
                        indexOfHandlerToBeRemoved = i;
                        break;
                    }
                }
                while (!config.ClientAdapter.RemoveHandler(potenialRemovedHandler)) { }
                config.Handlers.RemoveAt(indexOfHandlerToBeRemoved);
                return (answer = true);
            }
            //potenialRemovedHandler = null;
            
            return answer;
        }
      
        [HttpPost]
        public bool RemoveImageFromComputer(bool delete)
        {
            if (delete && potentialDeletedPhotoThumbnail != null && potentialDeletedPhoto != null)
            {
                int index;
                for (index = 0; index < photosModel.Thumbnails.Count; ++index)
                {
                    Photo current = photosModel.Thumbnails.ElementAt(index);
                    if (current.PathToFullSizeImage.Equals(potentialDeletedPhoto))
                    {
                        indexOfPhotoToBeRemoved = index;
                    }
                }

                String pThumb = System.Web.HttpContext.Current.Server.MapPath(potentialDeletedPhotoThumbnail);
                String p = System.Web.HttpContext.Current.Server.MapPath(potentialDeletedPhoto);
                if (System.IO.File.Exists(pThumb))
                {
                    try
                    {
                        System.IO.File.Delete(pThumb);
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error while removing image: " + potentialDeletedPhotoThumbnail + '\n' +
                            "error is: " + e.ToString());
                        return false;
                    }
                }
                if (System.IO.File.Exists(p))
                {
                    try
                    {
                        System.IO.File.Delete(p);
                        photosModel.Thumbnails.RemoveAt(indexOfPhotoToBeRemoved);
                       
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error while removing image: " + potentialDeletedPhoto + '\n' +
                            "error is: " + e.ToString());
                        return false;

                    }
                }
                return true;
                // return RedirectToAction("Photos");
            }
            return true;
            //return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeletePhoto(String photoToRemove)
        {
            if (photosModel != null && photoToRemove != null)
            {

                int index;
                for (index = 0; index < photosModel.Thumbnails.Count; ++index)
                {
                    Photo current = photosModel.Thumbnails.ElementAt(index);
                    if (current.PathToFullSizeImage.Equals(photoToRemove))
                    {
                        ViewBag.photo = photoToRemove;
                        ViewBag.month = current.Month.ToString();
                        ViewBag.year = current.Year.ToString();
                        ViewBag.name = current.NameWithoutExt;
                        potentialDeletedPhoto = photoToRemove;
                        potentialDeletedPhotoThumbnail = current.Path;
                        //indexOfPhotoToBeRemoved = index;
                        return View("DeleteImage");
                    }
                }

                return View("DeleteImage");
                //return View("Index");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        public ActionResult ViewPhoto(String photoToView)
        {
            if (photoToView != null && photos != null)
            {
                int index;
                for (index = 0; index < photosModel.Thumbnails.Count; ++index)
                {
                    Photo current = photosModel.Thumbnails.ElementAt(index);
                    if (current.PathToFullSizeImage.Equals(photoToView))
                    {
                        ViewBag.photo = photoToView;
                        ViewBag.month = current.Month.ToString();
                        ViewBag.year = current.Year.ToString();
                        ViewBag.name = current.NameWithoutExt;
                        potentialDeletedPhoto = photoToView;
                        potentialDeletedPhotoThumbnail = current.Path;
                        //indexOfPhotoToBeRemoved = index;
                        return View("ViewImage");
                    }
                }
                return View("ViewImage");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult DirectToDelete(String photoToRemove)
        {
            if (photosModel != null && photoToRemove != null)
            {
                int index;
                for (index = 0; index < photosModel.Thumbnails.Count; ++index)
                {
                    Photo current = photosModel.Thumbnails.ElementAt(index);
                    if (current.PathToFullSizeImage.Equals(photoToRemove))
                    {
                        ViewBag.photo = photoToRemove;
                        ViewBag.month = current.Month.ToString();
                        ViewBag.year = current.Year.ToString();
                        ViewBag.name = current.NameWithoutExt;
                        potentialDeletedPhoto = photoToRemove;
                        potentialDeletedPhotoThumbnail = current.Path;
                        return View("DeleteImage");
                    }
                }

                return View("DeleteImage");
                //return View("Index");
            }
            else
            {
                return View("Index");
            }
        }
    }
}
