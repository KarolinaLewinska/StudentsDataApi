using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsDataApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentsDataApi.Controllers
{
    [Route("api/students-data")]
    [ApiController]
    public class StudentsDataController : ControllerBase
    {
        private readonly StudentDataContext _context;

        public StudentsDataController(StudentDataContext context)
        {
            _context = context;
        }

        [HttpGet, Route("all-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentData))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentData> GetAllStudentsData()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (_context.Students == null)
                    return NotFound("Data not found");
                
                return Ok(_context.Students);
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exc.Message); 
            }
        }

        [HttpGet("{id}"), Route("student/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentData))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentData> GetStudentData(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                var Student = _context.Students.Find(id);

                if (Student == null)
                    return NotFound("Not found any student with ID: " + id);
                
                return Ok(Student);
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exc.Message);
            }
        }

        [HttpPost, Route("add-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentData> AddStudentData(StudentData student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingPesel = _context.Students.SingleOrDefault(x => x.pesel == student.pesel);
                var existingIndexNumber = _context.Students.SingleOrDefault(x => x.indexNumber == student.indexNumber);
                var existingEmail = _context.Students.SingleOrDefault(x => x.email == student.email);
                
                if (existingPesel != null || existingIndexNumber != null || existingEmail != null)
                    return StatusCode(StatusCodes.Status409Conflict, "Student's data with the same pesel / index number or email already exists.");
                
                _context.Students.Add(student);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetStudentData), new { id = student.Id }, student);
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exc.Message);
            } 
        }

        [HttpPut("{id}"), Route("update-data/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentData> UpdateStudentData(int id, StudentData student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (id != student.Id)
                    return NotFound("Not found any student with ID: " + id);
                
                var studentDb = _context.Students
                    .Where(s => s.Id == id).FirstOrDefault<StudentData>();

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
                   
                return Ok("Successfully updated student's data with ID: " + id);
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exc.Message);
            }
        }

        [HttpDelete("{id}"), Route("delete-data/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentData> DeleteStudentData(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var Student = _context.Students.Find(id);

                if (Student == null)
                    return NotFound("Not found any student with ID: " + id) ;
                
                _context.Students.Remove(Student);
                _context.SaveChanges();

                return Ok("Successfully deleted student's data");
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exc.Message);
            }
        }
    }
}
