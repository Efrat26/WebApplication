using Logs.ImageService.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    //the controller of the website
    public class FirstController : Controller
    {
        /// <summary>
        /// The potential deleted photo
        /// </summary>
        static String potentialDeletedPhoto;
        /// <summary>
        /// The potential deleted photo thumbnail
        /// </summary>
        static String potentialDeletedPhotoThumbnail;
        /// <summary>
        /// The index of photo/handler to be removed
        /// </summary>
        int indexOfPhotoToBeRemoved, indexOfHandlerToBeRemoved;
        /// <summary>
        /// The potenial removed handler
        /// </summary>
        static String potenialRemovedHandler;
        /// <summary>
        /// The photos model
        /// </summary>
        static PhotosModel photosModel = new PhotosModel();
        /// <summary>
        /// a list with photos model (to send to the view bag)
        /// </summary>
        static List<PhotosModel> photos = new List<PhotosModel>()
        {
            photosModel
        };
        /// <summary>
        /// The logs model
        /// </summary>
        static LogsModel logs = new LogsModel();
        /// <summary>
        ///  a list with logs model (to send to the view bag)
        /// </summary>
        static List<LogsModel> logFiles = new List<LogsModel>()
        {
            logs
        };
        /// <summary>
        /// The configuration model
        /// </summary>
        static ConfigModel config = new ConfigModel();
        /// <summary>
        ///  a list with configuration model (to send to the view bag)
        /// </summary>
        static List<ConfigModel> conf = new List<ConfigModel>()
        {
            config
        };
        /// <summary>
        /// The image web model
        /// </summary>
        static ImageWebModel imageWeb = new ImageWebModel();
        /// <summary>
        ///  a list with image model (to send to the view bag)
        /// </summary>
        static List<ImageWebModel> m = new List<ImageWebModel>()
        {
            imageWeb
        };
        //get the index page (first page)
        // GET: First
        public ActionResult Index()
        {
            return View(m);
        }
        //get the log page
        //Get: Logs
        [HttpGet]
        public ActionResult Logs()
        {
            return View(logFiles);
        }
        //get a json with a log object
        [HttpGet]
        public JObject GetEmployee()
        {
            JObject data = new JObject();
            data["Message"] = "Kuky";
            data["Type"] = "Mopy";
            return data;
        }
        /// <summary>
        /// Gets the relevant logs according to the user's selection.
        /// the function is being called in a loop, each time it gets the index the 
        /// loop is currently is. if the selection is as the type as the message in the 
        /// logs's list index - it creates an object with it and sends it back.
        /// </summary>
        /// <param name="selection">The user's selection.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
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
        /// <summary>
        /// returns the photos page
        /// </summary>
        /// <returns></returns>
        // GET: First/Photos
        [HttpGet]
        public ActionResult Photos()
        {
            //photosModel = new PhotosModel();
            //imageWeb = new ImageWebModel();
            return View(photos);
        }
        /// <summary>
        /// returns the config page (and calls the get config)
        /// </summary>
        /// <returns></returns>
        // GET: First/Config
        [HttpGet]
        public ActionResult Config()
        {
           // config.ClientAdapter.GetAppConfig();
            return View(conf);
        }
        /// <summary>
        /// redrect to teh deletes handler page.
        /// </summary>
        /// <param name="handlerToRemove">The handler to remove.</param>
        /// <returns></returns>
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
        /// <summary>
        /// redirect to index.
        /// </summary>
        /// <param name="answer">The answer.</param>
        /// <returns></returns>
        public ActionResult Remove(String answer)
        {
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Removes the handler specified.
        /// sends a command to the server (if is connected) and waits for answer.
        /// removes the handler also from the config model's list
        /// </summary>
        /// <returns>bool -true if successfully removed handler and false otherwise</returns>
        [HttpPost]
        public bool RemoveHandler()
        {
            bool answer = false;
            if (config != null && potenialRemovedHandler != null && imageWeb.IsConnected)
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
        /// <summary>
        /// Removes the image from computer and delete it from the photos
        /// model list of photos.
        /// </summary>
        /// <param name="delete">if set to <c>true</c> should delete photo.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RemoveImageFromComputer(bool delete)
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
                    while (this.IsFileLocked(new FileInfo(pThumb))) { };
                    try
                    {
                        System.IO.File.Delete(pThumb);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error while removing image: " + potentialDeletedPhotoThumbnail + '\n' +
                            "error is: " + e.ToString());
                       
                    }
                }
                if (System.IO.File.Exists(p))
                {
                    while (this.IsFileLocked(new FileInfo(p))) { };
                    try
                    {
                        System.IO.File.Delete(p);
                        photosModel.Thumbnails.RemoveAt(indexOfPhotoToBeRemoved);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error while removing image: " + potentialDeletedPhoto + '\n' +
                            "error is: " + e.ToString());
                        photosModel.Thumbnails.RemoveAt(indexOfPhotoToBeRemoved);
                        // return RedirectToAction("Photos");

                    }
                }
               // return RedirectToAction("Photos");
                // return RedirectToAction("Photos");
            }
            return RedirectToAction("Photos");
            //return RedirectToAction("Index");
        }
        /// <summary>
        /// redirects to the DeleteImage page
        /// </summary>
        /// <param name="photoToRemove">The photo to be remove.</param>
        /// <returns></returns>
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
        /// <summary>
        /// redirects to the ViewImage page.
        /// </summary>
        /// <param name="photoToView">The photo to view.</param>
        /// <returns></returns>
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
        /// <summary>
        /// redirects to DeleteImage page
        /// </summary>
        /// <param name="photoToRemove">The photo to remove.</param>
        /// <returns></returns>
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
        /// <summary>
        /// redirects to DeleteImage page
        /// </summary>
        /// <param name="photoToRemove">The photo to remove.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DirectToDeleteAfterView()
        {
            if (photosModel != null && potentialDeletedPhoto != null)
            {
                int index;
                for (index = 0; index < photosModel.Thumbnails.Count; ++index)
                {
                    Photo current = photosModel.Thumbnails.ElementAt(index);
                    if (current.PathToFullSizeImage.Equals(potentialDeletedPhoto))
                    {
                        ViewBag.photo = potentialDeletedPhoto;
                        ViewBag.month = current.Month.ToString();
                        ViewBag.year = current.Year.ToString();
                        ViewBag.name = current.NameWithoutExt;
                        //potentialDeletedPhoto = photoToRemove;
                        //potentialDeletedPhotoThumbnail = current.Path;
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
        /// <summary>
        /// Determines whether the file is locked.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        ///   <c>true</c> if is file locked and otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
