using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C1.Models;
namespace C1.Controllers
{
    public class TeacherController : Controller
    {
        //listen for Http request. The TeacherController.cs is to link to the dynamic rendered web pages 
        /// <summary>
        /// display list of teachers and show their information such as salary, hiredate, class
        /// adds and delete teachers with there information such as salary, hiredate, class
        /// </summary>
        // for list
        /// <param name="SearchKey"
        /// <example> GET: /Teacher/Show</example>
        // for Show
        /// <param name="id"
        /// <example> Get: /Teacher/List</example>
        /// for Delete
        /// <param name="id"
        /// <example> Get: /Teacher/DeleteConfirm/{id}</example>
        /// for Delete_ajax
        /// <param name="id">
        /// <example> Get: /Teacher/Ajax_New_Delete/{id}</example>
        /// // for New
        /// <example> Get: /Teacher/New</example>
        ///  for Ajax_New
        /// <example> Get: /Teacher/Ajax_New</example>
        /// <returns>
        /// List => returns list of teachers from the teachers table 
        /// Show => returns information of the selected teacher 
        /// </returns>
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        // Get: /Teacher/List
        public ActionResult List(string SearchKey = null) 
        { 
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }
        // GET: /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();

            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }
        //GET": /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);

            return View(NewTeacher);
        }

        //POST : /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //Get : /Teacher/New
        public ActionResult New()
        {
            return View();
        }
        // Teacher/Ajax_New
        public ActionResult Ajax_New()
        {
            return View();
        }

        public ActionResult Ajax_New_Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);

        }

        [HttpPost]
        //POST : /Teacher/Create
        /// <summary>
        /// method to create a new teacher record.
        /// </summary>
        /// <param name="TeacherFname">
        /// <param name="TeacherLname">
        /// <param name="Employeenumber">
        /// <param name="Hiredate">
        /// <param name="Salary">
        /// <rerturns>RedirectToAction list of teachers once teacher has been added </rerturns> 
        public ActionResult Create(string TeacherFname, string TeacherLname, string Employeenumber, DateTime Hiredate, decimal Salary)
        {
            //Identify that this method is running 
            //identify the inputs provided from the form

            Debug.WriteLine("I have accessed the create Method");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(Employeenumber);
            Debug.WriteLine(Hiredate);
            Debug.WriteLine(Salary);

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.Employeenumber = Employeenumber;
            NewTeacher.Hiredate = Hiredate;
            NewTeacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }

        /// <summary>
        /// ROutes dynamically to Teacher Update page, collectes data from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns> dynamic "update teacher" webapage which gives information of a teacher and asks the user for new information as part of the form</returns>
        /// <example>GET: /Teacher/Update/{id}</example>
        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();

            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }
        /// <summary>
        /// Updates a Teacher on the MySQL Database. 
        /// </summary>
        /// <param name="id"
        /// <param name="TeacherFname"
        /// <param name="TeacherLname"
        /// <param name="Hiredate"
        /// <param name="Employeenumber"
        /// <param name="Salary"
        /// <example>
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// </example>
        //Post : /Teacher/Update/{id}
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string Employeenumber, DateTime Hiredate, decimal Salary)
        {

            Teacher TeacherInfo = new Teacher();
            TeacherInfo.TeacherFname = TeacherFname;
            TeacherInfo.TeacherLname = TeacherLname;
            TeacherInfo.Employeenumber = Employeenumber;
            TeacherInfo.Hiredate = Hiredate;
            TeacherInfo.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);

            return RedirectToAction("Show/" + id);
        }
    }
}