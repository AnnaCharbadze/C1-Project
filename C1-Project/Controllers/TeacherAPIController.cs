using Microsoft.AspNetCore.Mvc;
using C1_Project.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace C1_Project.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of teachers.
        /// If a SearchKey is provided, filters by first or last name.
        /// Also filters teachers by Hire Date if hireDateFrom and/or hireDateTo are provided.
        /// </summary>
        /// <example>
        /// GET api/Teacher/ListTeachers?SearchKey=John&hireDateFrom=2020-01-01&hireDateTo=2021-01-01
        /// </example>
        /// <returns>A list of Teacher objects</returns>
        [HttpGet("ListTeachers")]
        public List<Teacher> ListTeachers(string? SearchKey = null, DateTime? hireDateFrom = null, DateTime? hireDateTo = null)
        {
            List<Teacher> teachers = new List<Teacher>();

            using (MySqlConnection connection = _context.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

               
                string query = "SELECT * FROM teachers WHERE 1=1";

                if (!string.IsNullOrEmpty(SearchKey))
                {
                    query += " AND (lower(teacherfname) LIKE lower(@key) OR lower(teacherlname) LIKE lower(@key))";
                    command.Parameters.AddWithValue("@key", $"%{SearchKey}%");
                }
                if (hireDateFrom.HasValue)
                {
                    query += " AND hiredate >= @hireDateFrom";
                    command.Parameters.AddWithValue("@hireDateFrom", hireDateFrom.Value);
                }
                if (hireDateTo.HasValue)
                {
                    query += " AND hiredate <= @hireDateTo";
                    command.Parameters.AddWithValue("@hireDateTo", hireDateTo.Value);
                }

                command.CommandText = query;
                command.Prepare();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Teacher teacher = new Teacher()
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFName = reader["teacherfname"].ToString(),
                            TeacherLName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = Convert.ToDateTime(reader["hiredate"]),
                            Salary = Convert.ToDecimal(reader["salary"])
                        };
                        teachers.Add(teacher);
                    }
                }
            }

            return teachers;
        }
        /// <summary>
        /// Returns a single teacher by their ID.
        /// Returns an empty teacher object if not found.
        /// </summary>
        /// <example>
        /// GET api/Teacher/FindTeacher/1
        /// </example>
        /// <returns>A Teacher object </returns>
        [HttpGet("FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
           
            Teacher selectedTeacher = new Teacher();

            using (MySqlConnection connection = _context.AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM teachers WHERE teacherid=@id";
                command.Parameters.AddWithValue("@id", id);
                command.Prepare();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                   
                    while (reader.Read())
                    {
                        selectedTeacher.TeacherId = Convert.ToInt32(reader["teacherid"]);
                        selectedTeacher.TeacherFName = reader["teacherfname"].ToString();
                        selectedTeacher.TeacherLName = reader["teacherlname"].ToString();
                        selectedTeacher.EmployeeNumber = reader["employeenumber"].ToString();
                        selectedTeacher.HireDate = Convert.ToDateTime(reader["hiredate"]);
                        selectedTeacher.Salary = Convert.ToDecimal(reader["salary"]);
                    }
                }
            }

           
            return selectedTeacher;
        }
    }
}