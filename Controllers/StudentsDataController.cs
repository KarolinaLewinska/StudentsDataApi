using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsDataApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentsDataApi.Controllers
{
    [Route("api/dane-studentow-wzr")]
    [ApiController]
    public class StudentsDataController : ControllerBase
    {
        private readonly StudentDataContext _context;

        public StudentsDataController(StudentDataContext context)
        {
            _context = context;
        }

        [HttpGet, Route("wszyscy")]
        public IEnumerable<StudentData> GetStudents()
        {
            try
            {
                return _context.Students.ToList();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}"), Route("dane-studenta/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentData))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentData> GetStudentsData(int id)
        {
            try
            {
                var Student = _context.Students.Find(id);

                if (Student == null)
                {
                    return NotFound("Nieprawidłowe id studenta!");
                }

                return Ok(Student);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPost, Route("dodaj-dane")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<StudentData> AddStudentData(StudentData student)
        {
            try
            {
                _context.Students.Add(student);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetStudentsData), new { id = student.Id }, student);
            }
            catch (System.Exception)
            {
                throw;
            } 
        }

        [HttpPatch("{id}"), Route("zaktualizuj-dane/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentData> PatchStudentData(int id, StudentData student)
        {
            try
            {
                if (id != student.Id)
                {
                    return BadRequest("Nieprawidłowe id studenta!");
                }

                _context.Students.Update(student);
                _context.SaveChanges();

                return Ok("Zaktualizowano dane studenta!");
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}"), Route("usun-dane/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentData> DeleteStudentData(int id)
        {
            try
            {
                var Student = _context.Students.Find(id);

                if (Student == null)
                {
                    return NotFound("Nieprawidłowe id studenta!");
                }

                _context.Students.Remove(Student);
                _context.SaveChanges();

                return Ok("Usunięto dane studenta!");
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
