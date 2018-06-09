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
        private List<string> images;
        public List<string> Thumbnails { get { return this.images; } set { this.images = value; } }

        public PhotosModel()
        {
            
            string[] photos = Directory.GetFiles(HttpContext.Current.Server.MapPath("/App_Data/output_service/Thumbnails"),
                "*.*", SearchOption.AllDirectories);
            images = new List<string>(photos);
            CreateRelativePath();
            Console.WriteLine("hello");
        }
        private void CreateRelativePath()
        {
            int indexOfThumbnail, sizeOfString;
            int index;
            string current, newString;
            for(index = 0; index < images.Count; ++index) {
                current = images.ElementAt(index);
                sizeOfString = current.Length;
                indexOfThumbnail = current.IndexOf("\\Thumbnails");
                if (indexOfThumbnail != -1)
                {
                    newString ="\\App_Data\\output_service" +current.Substring(indexOfThumbnail, sizeOfString - indexOfThumbnail);
                    images[index] = newString;
                }
            }
        }
    }
}