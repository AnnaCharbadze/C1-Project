using Microsoft.AspNetCore.Mvc;
using C1_Project.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace C1_Project.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentsAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public StudentsAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of students.
        /// If a SearchKey is provided, filters by first or last name.
        /// </summary>
        /// <example>
        /// GET api/Student/ListStudents?SearchKey=Sarah
        /// </example>
        /// <returns>A list of Student objects</returns>
        [HttpGet("ListStudents")]
        public List<Student> ListStudents(string? SearchKey = null)
        {
            List<Student> students = new List<Student>();

            using (MySqlConnection connection = _context.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                string query = "SELECT * FROM students";
                if (!string.IsNullOrEmpty(SearchKey))
                {
                    query += " WHERE lower(studentfname) LIKE lower(@key) " +
                             "OR lower(studentlname) LIKE lower(@key)";
                    command.Parameters.AddWithValue("@key", $"%{SearchKey}%");
                }

                command.CommandText = query;
                command.Prepare();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Student student = new Student()
                        {
                            StudentId = Convert.ToInt32(reader["studentid"]),
                            StudentFName = reader["studentfname"].ToString(),
                            StudentLName = reader["studentlname"].ToString(),
                            StudentNumber = reader["studentnumber"].ToString(),

                            
                            EnrollDate = Convert.ToDateTime(reader["enroldate"])
                        };
                        students.Add(student);
                    }
                }
            }

            return students;
        }


        /// <summary>
        /// Returns a specific student by their ID.
        /// Returns an empty Student (StudentId = 0) if not found.
        /// </summary>
        /// <example>
        /// GET api/Student/FindStudent/1
        /// </example>
        /// <returns>A Student object (may be empty if not found)</returns>
        [HttpGet("FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            Student selectedStudent = new Student();

            using (MySqlConnection connection = _context.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM students WHERE studentid=@id";
                command.Parameters.AddWithValue("@id", id);
                command.Prepare();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selectedStudent.StudentId = Convert.ToInt32(reader["studentid"]);
                        selectedStudent.StudentFName = reader["studentfname"].ToString();
                        selectedStudent.StudentLName = reader["studentlname"].ToString();
                        selectedStudent.StudentNumber = reader["studentnumber"].ToString();

                        // IMPORTANT: again, use "enroldate" with one L
                        selectedStudent.EnrollDate = Convert.ToDateTime(reader["enroldate"]);
                    }
                }
            }

            return selectedStudent;
        }
    }
}