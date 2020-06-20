using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw10.DTOs;
using cw10.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace cw10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly s20015Context _context;

        public StudentsController(s20015Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Student.ToList();

            List<StudentResponse> results = new List<StudentResponse>();
            foreach(var s in students)
            {
                var r = new StudentResponse();
                r.IndexNumber = s.IndexNumber;
                r.FirstName = s.FirstName;
                r.LastName = s.LastName;
                r.BirthDate = s.BirthDate;
                results.Add(r);
            }

            return Ok(results);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateStudent([FromRoute]string index, [FromBody]StudentRequest request)
        {
            var student = _context.Student.Where(s => s.IndexNumber == index).FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }

            if (request.IndexNumber != index)
            {
                return BadRequest("indexNumber cannot be changed");
            }

            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.BirthDate = request.BirthDate;

            _context.Entry(student).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent([FromRoute]string index)
        {
            var student = _context.Student.Where(s => s.IndexNumber == index).FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }

            _context.Entry(student).State = EntityState.Deleted;
            _context.SaveChanges();

            return NoContent();
        }
    }
}