using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Photo
    {
        private String path;
        private int month;
        private int year;
        private String nameWithExt;
        private String nameWithoutExt;
        private String pathToFullImage;
        public String Path { get { return this.path; } set { this.path = value; } }
        public String NameWithExt { get { return this.nameWithExt; } set { this.nameWithExt = value; } }
        public String NameWithoutExt { get { return this.nameWithoutExt; } set { this.nameWithoutExt = value; } }
        public int Month { get { return this.month; } set { this.month = value; } }
        public int Year { get { return this.year; } set { this.year = value; } }
        public String PathToFullSizeImage { get { return this.pathToFullImage; } set { this.pathToFullImage = value; } }
        public Photo(String n, String p, int m, int y)
        {
            this.nameWithExt = n;
            this.path = p;
            this.year = y;
            this.month = m;
            String[] temp = nameWithExt.Split('.');
            this.nameWithoutExt = temp[0];
            String tempPath = this.path;
            tempPath = tempPath.Replace("\\Thumbnails", "");
            this.pathToFullImage = tempPath;
        }
    }
}