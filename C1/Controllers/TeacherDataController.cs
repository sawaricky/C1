using C1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;


namespace C1.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database contect class which allows acccess to the MYSQL Database
        private SchoolDbContext school = new SchoolDbContext();
        //This controller will access the teacher table of the school database
        /// <summary>
        /// returns a list of teachers in the system
        /// </summary>
        /// <param name="SearchKey"
        /// <example> GET api/TeacherData/ListTeachers</example>
        /// <returns>A list of teachers (first name and last name) search  for additional information of the teacher once teacher is selected and/or searched </returns>
        [HttpGet]
        //the ? together with SearchKey = null after the SearchKey variable allows the list of teachers to be displayed 
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            //create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL query
            //lower - for case sensitivity
            //preventing MySql injection 
            cmd.CommandText = "select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or hiredate like @key or salary like @key or employeenumber like lower(@key) or lower(concat (teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            //Gather Results Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teacher
            List<Teacher> Teachers = new List<Teacher>();

            //Loop through Each row the Result set
            while (ResultSet.Read())
            {
                //Access column information by the DB column names as an index 
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                Teacher NewTeacher = new Teacher();
                //reference from the teacher.cs file on the left side of the = sign
                //-> right side of the = sign references from the above
                NewTeacher.Teacherid = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.Employeenumber = EmployeeNumber;
                NewTeacher.Hiredate = HireDate;
                NewTeacher.Salary = Salary;
                //add the Teacher name to the List 
                Teachers.Add(NewTeacher);
            }
            //close the connection between the MySql Database and the WebServer
            Conn.Close();
            //Return the final list of teacher names
            return Teachers;
        }
        [HttpGet]


        //to find the teacher information for one current id 
        /// <summary>
        /// returns information of teachers in the system
        /// </summary>
        /// <param name="id"
        /// <example> GET /api/TeacherData/FindTeacher/3 </example>
        /// <returns>information of teachers (TeacherFirstname, TeacherLastname, ClassName,Employeenumber,Hiredate.Salary,Teacherid  )</returns>

        public Teacher FindTeacher(int id)
        {
            
            Teacher NewTeacher = new Teacher();
           
            //create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the conection between the web server and database 
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL query
            //displays teacher information with the classes they taech
            //note: the + used to string concatenation to form a single string
            //note: date format while searching - 2016-08-05 
            cmd.CommandText = "SELECT teachers.teacherid, teachers.teacherfname, teachers.teacherlname, teachers.employeenumber, teachers.hiredate, teachers.salary, classes.classname " +
                  "FROM teachers " +
                  "INNER JOIN classes ON teachers.teacherid = classes.teacherid " +
                  "WHERE teachers.teacherid = @id";

            cmd.Parameters.AddWithValue("@id", id);
            //Gather Results Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Loop through Each row the Result set
            while (ResultSet.Read())
            {
                //Access column information by the DB column names as an index 
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);
                string ClassName = ResultSet["classname"].ToString();

                NewTeacher.Teacherid = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.Employeenumber = EmployeeNumber;
                NewTeacher.Hiredate = HireDate;
                NewTeacher.Salary = Salary;
                NewTeacher.ClassName.Add(ClassName);
            }
            return NewTeacher;
        }
    }
}
