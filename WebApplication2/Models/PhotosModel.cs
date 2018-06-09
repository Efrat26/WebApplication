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
            
            string[] photos = Directory.GetFiles(HttpContext.Current.Server.MapPath("/App_Data/output service/Thumbnails"),
                "*.*", SearchOption.AllDirectories);
            images = new List<string>(photos);

        }
    }
}