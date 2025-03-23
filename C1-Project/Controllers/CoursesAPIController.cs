using Microsoft.AspNetCore.Mvc;
using C1_Project.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace C1_Project.Controllers
{
    [Route("api/Course")]
    [ApiController]
    public class CoursesAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public CoursesAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of courses.
        /// If a SearchKey is provided, filters by course code or course name.
        /// </summary>
        /// <example>
        /// GET api/Course/ListCourses?SearchKey=Web
        /// </example>
        /// <returns>A list of Course objects</returns>
        [HttpGet("ListCourses")]
        public List<Course> ListCourses(string? SearchKey = null)
        {
            List<Course> courses = new List<Course>();

            using (MySqlConnection connection = _context.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

               
                string query = "SELECT * FROM courses";
                if (!string.IsNullOrEmpty(SearchKey))
                {
                    query += " WHERE lower(coursecode) LIKE lower(@key) " +
                             "OR lower(coursename) LIKE lower(@key)";
                    command.Parameters.AddWithValue("@key", $"%{SearchKey}%");
                }

                command.CommandText = query;
                command.Prepare();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Course course = new Course()
                        {
                            CourseId = Convert.ToInt32(reader["courseid"]),
                            CourseCode = reader["coursecode"].ToString(),
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            StartDate = Convert.ToDateTime(reader["startdate"]),
                            FinishDate = Convert.ToDateTime(reader["finishdate"]),
                            CourseName = reader["coursename"].ToString()
                        };
                        courses.Add(course);
                    }
                }
            }

            return courses;
        }

        /// <summary>
        /// Returns a specific course by its ID.
        /// If no course is found, returns an empty Course object.
        /// </summary>
        /// <example>
        /// GET api/Course/FindCourse/1
        /// </example>
        /// <returns>A Course object (empty if the course is not found)</returns>
        [HttpGet("FindCourse/{id}")]
        public Course FindCourse(int id)
        {
           
            Course selectedCourse = new Course();

            using (MySqlConnection connection = _context.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM courses WHERE courseid=@id";
                command.Parameters.AddWithValue("@id", id);
                command.Prepare();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selectedCourse.CourseId = Convert.ToInt32(reader["courseid"]);
                        selectedCourse.CourseCode = reader["coursecode"].ToString();
                        selectedCourse.TeacherId = Convert.ToInt32(reader["teacherid"]);
                        selectedCourse.StartDate = Convert.ToDateTime(reader["startdate"]);
                        selectedCourse.FinishDate = Convert.ToDateTime(reader["finishdate"]);
                        selectedCourse.CourseName = reader["coursename"].ToString();
                    }
                }
            }

            return selectedCourse;
        }
    }
}