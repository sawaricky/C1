using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C1.Models
{
    public class Teacher
    {
        //The following fields define a teacher
        public int Teacherid;
        public string TeacherFname;
        public string TeacherLname;
        public string Employeenumber;
        public DateTime Hiredate;
        public decimal Salary;
        public List<string> ClassName;
        public Teacher()
        {
            // Initialize the ClassNames list
            ClassName = new List<string>();
        }
    }
}