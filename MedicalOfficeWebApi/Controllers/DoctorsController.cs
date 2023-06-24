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
        public async Task<IActionResult> PutDoctor(int id, DoctorDTO doctorDTO)
        {
            if (id != doctorDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Doctor." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record to update
            var doctorToUpdate = await _context.Doctors.FindAsync(id);

            //Check that you got it
            if (doctorToUpdate == null)
            {
                return NotFound(new { message = "Error: Doctor record not found." });
            }

            if (doctorDTO.RowVersion != null)
            {
                if (!doctorToUpdate.RowVersion.SequenceEqual(doctorDTO.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Doctor has been changed by another user. Try again later." });
                }
            }

            //Update the properties for the entity object from the DTO object
            doctorToUpdate.ID = doctorDTO.ID;
            doctorToUpdate.LastName = doctorDTO.LastName;
            doctorToUpdate.FirstName = doctorDTO.FirstName;
            doctorToUpdate.MiddleName = doctorDTO.MiddleName;
            doctorToUpdate.RowVersion = doctorDTO.RowVersion;

            _context.Entry(doctorToUpdate).Property("RowVersion").OriginalValue = doctorDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Doctor has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Doctor has been updated by another user." });
                }
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persist, contact to your database administrator." });
            }
        }

        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(DoctorDTO doctorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Doctor doctor = new Doctor
            {
                FirstName = doctorDTO.FirstName,
                MiddleName = doctorDTO.MiddleName,
                LastName = doctorDTO.LastName
            };

            try
            {
                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                doctorDTO.ID = doctor.ID;
                doctorDTO.RowVersion = doctor.RowVersion;

                return CreatedAtAction(nameof(GetDoctor), new { id = doctor.ID }, doctorDTO);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persist, contact to your database administrator." });
            }
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound(new { message = "Delete Error: Doctor has already been removed." });
            }
            try
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if(dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed."))
                {
                    return BadRequest(new { message = "Delete Error: You cannot delete a Doctor with Patients." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete Doctor. Try again, and if the problem persist, contact to your database administrator." });
                }
            }
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.ID == id);
        }
    }
}
