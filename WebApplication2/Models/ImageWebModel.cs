using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ImageWebModel
    {
        private List<StudentDetails> students;
        private bool isConnected;
        private int numOfPhotos;
        public List<StudentDetails> Students { get { return this.students; }  set { this.students = value; } }
        public bool IsConnected { get { return this.isConnected; }  set { this.isConnected = value; } }
        public int NumberOfPhotos
        {
            get { return this.numOfPhotos; }
             set { this.numOfPhotos = value; }
        }
        public ImageWebModel()
        {
            this.isConnected = false;
            this.numOfPhotos = 0;
            this.students = new List<StudentDetails>();
            this.readStudentsDetails();
        }
        private void readStudentsDetails()
        {
            string line;
            string[] splittedLine;
            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(HttpContext.Current.Server.MapPath("/App_Data/Details.txt"));
            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                if (line != null)
                {
                    try
                    {
                        splittedLine = line.Split(' ');
                        StudentDetails s = new StudentDetails(splittedLine[0], splittedLine[1],
                            Convert.ToInt32(splittedLine[2]));
                        Students.Add(s);
                    } catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            file.Close();
        }
    }
}