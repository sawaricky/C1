using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C1.Models
{
    /// <summary> Represents a teacher with  information such as TeacherID, TeacherFname, TeacherLname, Employeenumber,Hiredate, salary, and a list of class names. 
    /// </summary>
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
        /// <summary>
        /// initialize a new instance of the teacher class
        /// </summary>
        /// <returns> new stance of teacher </returns>
        public Teacher()
        {
            // Initialize the ClassNames list
            ClassName = new List<string>();
        }
    }
}