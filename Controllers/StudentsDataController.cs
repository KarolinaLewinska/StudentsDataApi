using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsDataApi.Models;
using System;
using System.Linq;

namespace StudentsDataApi.Controllers
{
    [Route("api/students-data")]
    [ApiController]
    public class StudentsDataController : ControllerBase
    {
        private readonly StudentDataContext _dbContext;

        public StudentsDataController(StudentDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet, Route("all-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentData))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentData> GetAllStudentsData()
        {
            try
            {
                if (_dbContext.Students == null)
                    return NotFound("Data not found");
                
                return Ok(_dbContext.Students);
            }
            catch (Exception exc)
            {
                while (exc.InnerException != null)
                    exc = exc.InnerException;

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
            try
            {
                var student = _dbContext.Students.Find(id);

                if (student == null)
                    return NotFound("Not found any student with ID: " + id);
                
                return Ok(student);
            }
            catch (Exception exc)
            {
                while (exc.InnerException != null)
                    exc = exc.InnerException;

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
                var existingPesel = _dbContext.Students.SingleOrDefault(x => x.pesel == student.pesel);
                var existingIndexNumber = _dbContext.Students.SingleOrDefault(x => x.indexNumber == student.indexNumber);
                var existingEmail = _dbContext.Students.SingleOrDefault(x => x.email == student.email);
                
                if (existingPesel != null || existingIndexNumber != null || existingEmail != null)
                    return StatusCode(StatusCodes.Status409Conflict, "Student's data with the same pesel / index number or email already exists.");
                
                _dbContext.Students.Add(student);
                _dbContext.SaveChanges();

                return CreatedAtAction(nameof(GetStudentData), new { id = student.Id }, student);
            }
            catch (Exception exc)
            {
                while (exc.InnerException != null)
                    exc = exc.InnerException;

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
                var existingPesel = _dbContext.Students.SingleOrDefault(x => x.pesel == student.pesel && x.Id != student.Id);
                var existingIndexNumber = _dbContext.Students.SingleOrDefault(x => x.indexNumber == student.indexNumber && x.Id != student.Id);
                var existingEmail = _dbContext.Students.SingleOrDefault(x => x.email == student.email && x.Id != student.Id);
                
                if (id != student.Id)
                    return NotFound("Not found any student with ID: " + id);

                if (existingPesel != null || existingIndexNumber != null || existingEmail != null)
                    return StatusCode(StatusCodes.Status409Conflict, "Student's data with the same pesel / index number or email already exists.");

                var studentToUpdate = _dbContext.Students
                    .Where(s => s.Id == id).FirstOrDefault<StudentData>();

                if (studentToUpdate != null)
                {
                    studentToUpdate.name = student.name;
                    studentToUpdate.surname = student.surname;
                    studentToUpdate.indexNumber = student.indexNumber;
                    studentToUpdate.pesel = student.pesel;
                    studentToUpdate.email = student.email;
                    studentToUpdate.studiesType = student.studiesType;
                    studentToUpdate.degree = student.degree;
                    studentToUpdate.fieldOfStudy = student.fieldOfStudy;
                    studentToUpdate.specialization = student.specialization;
                    
                    _dbContext.SaveChanges();
                }
                   
                return Ok("Successfully updated student's data with ID: " + id);
            }
            catch (Exception exc)
            {
                while (exc.InnerException != null)
                    exc = exc.InnerException;

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
            try
            {
                var studentToDelete = _dbContext.Students.Find(id);

                if (studentToDelete == null)
                    return NotFound("Not found any student with ID: " + id) ;
                
                _dbContext.Students.Remove(studentToDelete);
                _dbContext.SaveChanges();

                return Ok("Successfully deleted student's data");
            }
            catch (Exception exc)
            {
                while (exc.InnerException != null)
                    exc = exc.InnerException;

                return StatusCode(StatusCodes.Status500InternalServerError, exc.Message);
            }
        }
    }
}
