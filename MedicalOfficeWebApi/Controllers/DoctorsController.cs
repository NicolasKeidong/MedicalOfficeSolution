using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalOfficeWebApi.Data;
using MedicalOfficeWebApi.Models;

namespace MedicalOfficeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly MedicalOfficeContext _context;

        public DoctorsController(MedicalOfficeContext context)
        {
            _context = context;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetDoctors()
        {
            return await _context.Doctors
                .Select(d => new DoctorDTO
                {
                    ID = d.ID,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    RowVersion = d.RowVersion
                })
                .ToListAsync();
        }

        // GET: api/Doctors/inc - Include Pattients Collections
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctorsInc(int id)
        {
            var doctorDTO = await _context.Doctors
                .Include(d => d.Patients)
                .Select(d => new DoctorDTO
                {
                    ID = d.ID,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    RowVersion = d.RowVersion,
                    Patients = d.Patients.Select(dPatient => new PatientDTO
                    {
                        ID = dPatient.ID,
                        FirstName = dPatient.FirstName,
                        MiddleName = dPatient.MiddleName,
                        LastName = dPatient.LastName,
                        OHIP = dPatient.OHIP,
                        DOB = dPatient.DOB,
                        ExpYrVisits = dPatient.ExpYrVisits,
                        DoctorID = dPatient.DoctorID
                    }).ToList()
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (doctorDTO == null)
            {
                return NotFound(new { message = "Error: Doctor not found." });
            }
            return doctorDTO;
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctor(int id)
        {
            var doctorDTO = await _context.Doctors
                .Select(d => new DoctorDTO
                {
                    ID = d.ID,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    RowVersion = d.RowVersion
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (doctorDTO == null)
            {
                return NotFound(new { message = "Error: Doctor not found." });
            }
            return doctorDTO;
        }

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.ID)
            {
                return BadRequest();
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDoctor", new { id = doctor.ID }, doctor);
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.ID == id);
        }
    }
}
