using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw10.DTOs;
using cw10.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wyklad3.Models;

namespace cw10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly s20015Context _context;

        public EnrollmentsController(s20015Context context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateEnrollment([FromBody]EnrollmentRequest request)
        {
            var study = _context.Studies
                .Where(s => s.Name == request.Studies)
                .FirstOrDefault();
            if (study == null)
            {
                return BadRequest("study not found");
            }

            var enrollent = _context.Enrollment
                .Where(e => e.IdStudy == study.IdStudy)
                .Where(e => e.Semester == 1)
                .FirstOrDefault();

            if (enrollent == null)
            {
                enrollent = new Enrollment();
                Random rnd = new Random();
                // should be set by db sequence
                enrollent.IdEnrollment = rnd.Next(1, 10000);
                enrollent.IdStudy = study.IdStudy;
                enrollent.Semester = 1;
                enrollent.StartDate = DateTime.Today;

                _context.Enrollment.Add(enrollent);
            }

            var student = new Student();
            student.IndexNumber = request.IndexNumber;
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.BirthDate = request.BirthDate;
            student.IdEnrollment = enrollent.IdEnrollment;

            _context.Student.Add(student);

            _context.SaveChanges();

            var response = new EnrollmentResponse();
            response.LastName = student.LastName;
            response.Semester = enrollent.Semester;
            response.StartDate = enrollent.StartDate;
            return Ok(response);
        }


        [HttpPost("promotions")]
        public IActionResult Promote([FromBody] Promotion request)
        {
            var study = _context.Studies
                .Where(s => s.Name == request.Studies)
                .FirstOrDefault();
            if (study == null)
            {
                return BadRequest("study not found");
            }

            var currnetEnrollent = _context.Enrollment
                .Where(e => e.IdStudy == study.IdStudy)
                .Where(e => e.Semester == request.Semester)
                .FirstOrDefault();

            if (currnetEnrollent == null)
            {
                return BadRequest("enrollent not found");
            }

            var nextSemesterEnrollent = _context.Enrollment
                .Where(e => e.IdStudy == study.IdStudy)
                .Where(e => e.Semester == request.Semester + 1)
                .FirstOrDefault();

            if (nextSemesterEnrollent == null)
            {
                var newEnrollent = new Enrollment();
                Random rnd = new Random();
                // should be set by db sequence
                newEnrollent.IdEnrollment = rnd.Next(1, 10000);
                newEnrollent.IdStudy = study.IdStudy;
                newEnrollent.Semester = request.Semester + 1;
                newEnrollent.StartDate = DateTime.Today;

                _context.Enrollment.Add(newEnrollent);

                nextSemesterEnrollent = newEnrollent;
            }

            var students = _context.Student.Where(s => s.IdEnrollment == currnetEnrollent.IdEnrollment).ToList();

            foreach(var s in students)
            {
                s.IdEnrollment = nextSemesterEnrollent.IdEnrollment;
                _context.Entry(s).State = EntityState.Modified;
                _context.SaveChanges();
            }

            request.Semester += 1;
            return StatusCode(StatusCodes.Status201Created, request);
        }

    }
}