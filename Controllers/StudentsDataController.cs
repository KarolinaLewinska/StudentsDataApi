using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsDataApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentsDataApi.Controllers
{
    [Route("api/dane-studentow")]
    [ApiController]
    public class StudentsDataController : ControllerBase
    {
        private readonly StudentDataContext _context;

        public StudentsDataController(StudentDataContext context)
        {
            _context = context;
        }

        [HttpGet, Route("wszyscy")]
        public IEnumerable<StudentData> GetAllStudentsData()
        {
            try
            {
                if (_context.Students == null)
                {
                  throw new Exception("Nie znaleziono danych w bazie");
                }
                return _context.Students.ToList();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        [HttpGet("{id}"), Route("student/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentData))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentData> GetStudentData(int id)
        {
            try
            {
                var Student = _context.Students.Find(id);

                if (Student == null)
                {
                    return NotFound("Nie znaleziono danych studenta o id: " + id);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Nieprawidłowe dane");
                }

                return Ok(Student);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        [HttpPost, Route("dodaj-dane")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentData> AddStudentData(StudentData student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Nieprawidłowe dane");
                }

                _context.Students.Add(student);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetStudentData), new { id = student.Id }, student);
            }
            catch (Exception exc)
            {
                throw exc;
            } 
        }

        [HttpPut("{id}"), Route("zaktualizuj-dane/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentData))]
        public ActionResult<StudentData> UpdateStudentData(int id, StudentData student)
        {
            try
            {
                if (id != student.Id)
                {
                    return NotFound("Nie znaleziono danych studenta o id: " + id);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Nieprawidłowe dane");
                }

                var studentDb = _context.Students.Where(s => s.Id == id).FirstOrDefault<StudentData>();

                if (studentDb != null)
                {
                    studentDb.name = student.name;
                    studentDb.surname = student.surname;
                    studentDb.indexNumber = student.indexNumber;
                    studentDb.pesel = student.pesel;
                    studentDb.email = student.email;
                    studentDb.studiesType = student.studiesType;
                    studentDb.degree = student.degree;
                    studentDb.fieldOfStudy = student.fieldOfStudy;
                    studentDb.specialization = student.specialization;
                    
                    _context.SaveChanges();
                }
                   
                return Ok("Zaktualizowano dane studenta o id: " + id);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        [HttpDelete("{id}"), Route("usun-dane/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentData))]
        public ActionResult<StudentData> DeleteStudentData(int id)
        {
            try
            {
                var Student = _context.Students.Find(id);

                if (Student == null)
                {
                    return NotFound("Nie znaleziono danych studenta o id: " + id) ;
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Nieprawidłowe dane");
                }

                _context.Students.Remove(Student);
                _context.SaveChanges();

                return Ok("Usunięto dane studenta");
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
