using Microsoft.AspNetCore.Mvc;
using C1_Project.Models;
using System.Collections.Generic;

namespace C1_Project.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CoursesAPIController _api;

        public CoursePageController(CoursesAPIController api)
        {
            _api = api;
        }

        // GET: /CoursePage/List?SearchKey=Web
        public IActionResult List(string? SearchKey)
        {
            List<Course> courses = _api.ListCourses(SearchKey);
            return View(courses);
        }

        // GET: /CoursePage/Show/1
        public IActionResult Show(int id)
        {
            Course course = _api.FindCourse(id);

            // If CourseId is 0, show an error 
            if (course.CourseId == 0)
            {
                return View("error");
            }
            return View(course);
        }
    }
}
