using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class PhotosModel
    {
        private List<Photo> images;
        public List<Photo> Thumbnails { get { return this.images; } set { this.images = value; } }

        public PhotosModel()
        {
            this.CreateListOfPhotos();
            Console.WriteLine("hello");
        }
        public void CreateListOfPhotos()
        {
            string[] photos = Directory.GetFiles(HttpContext.Current.Server.MapPath("/images/Thumbnails"),
                "*.*", SearchOption.AllDirectories);
            images = new List<Photo>();
            CreatePhotoObjects(photos);
        }
        private void CreatePhotoObjects(string[] photos)
        {
            Photo p;
            string path;
            string[] splittedString;
            int indexOfThumbnail, sizeOfString;
            int index;
            var replacements = new[]{
             new{Find="\\",Replace=""},
                new{Find="/",Replace=""}};
            string current, newString;
            string temp;
            for(index = 0; index < photos.Length; ++index) {
                current = photos[index];
                sizeOfString = current.Length;
                indexOfThumbnail = current.IndexOf("\\Thumbnails");
                if (indexOfThumbnail != -1)
                {
                    path ="\\images" +current.Substring(indexOfThumbnail, sizeOfString - indexOfThumbnail);
                    splittedString = path.Split('\\');
                    for(int i=0; i< splittedString.Length; ++i) {
                        temp = splittedString[i];
                        foreach (var set in replacements)
                        {
                            temp = temp.Replace(set.Find, set.Replace);
                        }
                        splittedString[i] = temp;
                    }
                   
                    try
                    {
                         p = new Photo(splittedString[splittedString.Length - 1], path,
                             Int32.Parse(splittedString[splittedString.Length - 2]), 
                             Int32.Parse(splittedString[splittedString.Length - 3]));
                    }
                    catch (Exception) { p = null; Console.WriteLine("Error in creating photo object!");}
                    this.images.Add(p);
                }
            }
        }
    }
}