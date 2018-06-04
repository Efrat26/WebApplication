using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class StudentDetails
    {
        
        private String name;
        private String lastName;
        private int id;
       // [Display(Name = "ID")]
        public int ID { get {return this.id; } set {this.id = value; } }
       // [Display(Name = "LastName")]
        public string LastName { get {return this.lastName; } set {this.lastName = value; } }
       // [Display(Name = "FirstName")]
        public string FirstName { get {return this.name; } set {this.name = value; } }
        public StudentDetails(String firstName, String lastName, int id)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = id;
        }
    }
}
/*
        public void copy(StudentDetails student)
        {
            FirstName = student.FirstName;
            LastName = student.LastName;
            ID = student.ID;
        }
      
        [Required]
        [Display(Name = "ID")]
       
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
      

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        
          */
