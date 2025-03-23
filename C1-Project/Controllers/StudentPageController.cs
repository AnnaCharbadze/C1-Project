using Microsoft.AspNetCore.Mvc;
using C1_Project.Models;
using System.Collections.Generic;

namespace C1_Project.Controllers
{
    public class StudentPageController : Controller
    {
        private readonly StudentsAPIController _api;

        public StudentPageController(StudentsAPIController api)
        {
            _api = api;
        }

        // GET: /StudentPage/List?SearchKey=Sarah
        public IActionResult List(string? SearchKey)
        {
            // The API returns a plain List<Student>
            List<Student> students = _api.ListStudents(SearchKey);
            return View(students);
        }

        // GET: /StudentPage/Show/1
        public IActionResult Show(int id)
        {
            // The API returns a single Student
            Student student = _api.FindStudent(id);

            // If StudentId == 0, then it is "not found"
            if (student.StudentId == 0)
            {
                return View("error");
            }
            return View(student);
        }
    }
}