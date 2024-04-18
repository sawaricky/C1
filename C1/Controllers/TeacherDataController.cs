using C1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Http.Cors;
using System.Diagnostics;

namespace C1.Controllers
{
    //Wireframe for future extra features with a low fidelity prototype
    //https://www.figma.com/file/JJdfgyH9shmwjQ93DJ8rrb/C%23-C3?type=design&node-id=0-1&mode=design&t=EaNeLFCQMLCGRRef-0
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
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
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
                  "LEFT OUTER JOIN classes ON teachers.teacherid = classes.teacherid " +
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
        /// <summary>
        /// Delete teacher from database
        /// </summary>
        /// <param name="id"></param>
        /// <example> POST : /api/TeacherData/DeleteTeacher/3</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteTeacher(int id)
        {
            Debug.WriteLine(id);
            //create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the conection between the web server and database 
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            /* MySql query*/
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Add teacher to database
        /// </summary>
        /// <param name="NewTeacher"></param>
        /// <exception cref="ArgumentException"></exception>
        /// /// <example> POST : /api/TeacherData/AddTeacher</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            Debug.WriteLine(NewTeacher.TeacherFname);
            // Checking if any required field is missing
            if (string.IsNullOrEmpty(NewTeacher.TeacherFname) || string.IsNullOrEmpty(NewTeacher.TeacherLname) || string.IsNullOrEmpty(NewTeacher.Employeenumber))
            {
                throw new ArgumentException("Please fill in required information");
            }
            //create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the conection between the web server and database 
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            /* MySql query*/
            cmd.CommandText = "insert into teachers (teachers.teacherfname, teachers.teacherlname, teachers.employeenumber, teachers.hiredate, teachers.salary)values (@TeacherFname,@TeacherLname,@Employeenumber,@Hiredate,@Salary)";

            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@Employeenumber", NewTeacher.Employeenumber);
            cmd.Parameters.AddWithValue("@Hiredate", NewTeacher.Hiredate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

        }
        //update Teacher
        /// <summary>
        /// recieve update data, and teahcer id and update teacher data in the database corresponding to the ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TeacherInfo"></param>
        /// <returns> Updated values of the teacher </returns>
        /// curl -d @updatingTeacher.json -H "Content-Type: application/json" http://localhost:44393/api/TeacherData/UpdateTeacher/5
        /// POST : /TeacherData/UpdateTeacher/3
        ///         /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"Employeenumber":"T765",
        ///	"Hiredate":"2024-04-11",
        ///	"Salary":20,
        /// }
        /// <example>POST : /api/TeacherData/UpdateTeacher</example>
        /// C:\Users\Akash\source\repos\C1\C1
        [HttpPost]
        [Route("api/TeacherData/UpdateTeacher/{id}")]
        public void UpdateTeacher(int id, [FromBody]Teacher TeacherInfo)
        {
            Debug.WriteLine(id);
            Debug.WriteLine(TeacherInfo.TeacherFname);
            // Checking if any required field is missing
            if (string.IsNullOrEmpty(TeacherInfo.TeacherFname) || string.IsNullOrEmpty(TeacherInfo.TeacherLname) || string.IsNullOrEmpty(TeacherInfo.Employeenumber))
            {
                throw new ArgumentException("Please fill in required information");
            }
            //create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the conection between the web server and database 
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            /* MySql query*/
            cmd.CommandText = "update teachers set teachers.teacherfname=@TeacherFname, teachers.teacherlname=@TeacherLname, teachers.employeenumber=@Employeenumber, teachers.hiredate=@Hiredate, teachers.salary=@Salary  where teacherid=@Teacherid";

            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@Employeenumber", TeacherInfo.Employeenumber);
            cmd.Parameters.AddWithValue("@Hiredate", TeacherInfo.Hiredate);
            cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@Teacherid", id);
            cmd.Prepare();
            
            cmd.ExecuteNonQuery();

            Conn.Close();
        }
        [HttpPost]
        [Route("api/TeacherData/Test")]
        public string Test()
        {
            return "post test";
        }
    }
}
