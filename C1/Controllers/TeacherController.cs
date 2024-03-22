using System;
using System.Collections.Generic;
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
        /// </summary>
        // for list
        /// <param name="SearchKey"
        /// <example> GET: /Teacher/Show</example>
        // for Show
        /// <param name="id"
        /// <example> Get: /Teacher/List</example>
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

            Teacher NewTeacher = controller.FindTeacher(id);

            return View(NewTeacher);
        }
    }
}