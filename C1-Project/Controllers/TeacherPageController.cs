using Microsoft.AspNetCore.Mvc;
using C1_Project.Models;
using System.Collections.Generic;


namespace C1_Project.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        // GET: /TeacherPage/List?SearchKey=John&hireDateFrom=2020-01-01&hireDateTo=2021-01-01
        public IActionResult List(string? SearchKey, DateTime? hireDateFrom, DateTime? hireDateTo)
        {
            
            List<Teacher> teachers = _api.ListTeachers(SearchKey, hireDateFrom, hireDateTo);
            return View(teachers);
        }
        // GET: /TeacherPage/Show/1
        public IActionResult Show(int id)
        {
            // Returns a single Teacher by their ID
            Teacher teacher = _api.FindTeacher(id);

            // If the teacher was not found  show an error 
            if (teacher.TeacherId == 0)
            {
                return View("error");
            }

            return View(teacher);
        }
    }
}