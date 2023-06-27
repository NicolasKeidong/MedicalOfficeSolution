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
    public class PatientsController : ControllerBase
    {
        private readonly MedicalOfficeContext _context;

        public PatientsController(MedicalOfficeContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients()
        {
            return await _context.Patients
                .Include(p => p.Doctor)
                .Include(p => p.PatientConditions)
                .Select(p => new PatientDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    OHIP = p.OHIP,
                    DOB = p.DOB,
                    ExpYrVisits = p.ExpYrVisits,
                    RowVersion = p.RowVersion,
                    NumberOfConditions = p.PatientConditions.Count,
                    DoctorID = p.DoctorID,
                    Doctor = new DoctorDTO
                    {
                        ID = p.Doctor.ID,
                        FirstName = p.Doctor.FirstName,
                        MiddleName = p.Doctor.MiddleName,
                        LastName = p.Doctor.LastName
                    }
                })
                .ToListAsync();
        }

        // GET: api/Patients
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatientsHistory()
        {
            return await _context.Patients
                .Include(p => p.Doctor)
                .Include(p => p.PatientConditions).ThenInclude(p => p.Condition)
                .Select(p => new PatientDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    OHIP = p.OHIP,
                    DOB = p.DOB,
                    ExpYrVisits = p.ExpYrVisits,
                    RowVersion = p.RowVersion,
                    Conditions = p.PatientConditions.Select(c => new ConditionDTO { 
                        ID = c.ConditionID,
                        ConditionName = c.Condition.ConditionName
                    }).ToList(),
                    DoctorID = p.DoctorID,
                    Doctor = new DoctorDTO
                    {
                        ID = p.Doctor.ID,
                        FirstName = p.Doctor.FirstName,
                        MiddleName = p.Doctor.MiddleName,
                        LastName = p.Doctor.LastName
                    }
                })
                .ToListAsync();
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatient(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Doctor)
                .Select(p => new PatientDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    OHIP = p.OHIP,
                    DOB = p.DOB,
                    ExpYrVisits = p.ExpYrVisits,
                    RowVersion = p.RowVersion,
                    DoctorID = p.DoctorID,
                    Doctor = new DoctorDTO
                    {
                        ID = p.Doctor.ID,
                        FirstName = p.Doctor.FirstName,
                        MiddleName = p.Doctor.MiddleName,
                        LastName = p.Doctor.LastName
                    }
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (patient == null)
            {
                return NotFound(new { message = "Error: Patient record not found."});
            }

            return patient;
        }

        // GET: api/PatientsByDoctor
        [HttpGet("ByDoctor/{id}")]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatientsByDoctor(int id)
        {
            var patientDTOs = await _context.Patients
                .Include(e => e.Doctor)
                .Select(p => new PatientDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    OHIP = p.OHIP,
                    DOB = p.DOB,
                    ExpYrVisits = p.ExpYrVisits,
                    RowVersion = p.RowVersion,
                    DoctorID = p.DoctorID,
                    Doctor = new DoctorDTO
                    {
                        ID = p.Doctor.ID,
                        FirstName = p.Doctor.FirstName,
                        MiddleName = p.Doctor.MiddleName,
                        LastName = p.Doctor.LastName
                    }
                })
                .Where(e => e.DoctorID == id)
                .ToListAsync();

            if (patientDTOs.Count() > 0)
            {
                return patientDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Patient records for that Doctor." });
            }
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, PatientDTO patientDTO)
        {
            if (id != patientDTO.ID)
            {
                return BadRequest(new {message = "Error: ID does not match Patient."});
            }

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            //Get the record to update
            var patientToUpdate = await _context.Patients.FindAsync(id);
            
            //Check that you got it
            if(patientToUpdate == null) 
            {
                return NotFound(new { message = "Error: Patient record not found." });
            }

            if(patientDTO.RowVersion != null)
            {
                if(!patientToUpdate.RowVersion.SequenceEqual(patientDTO.RowVersion)) 
                {
                    return Conflict(new { message = "Concurrency Error: Patient has been changed by another user. Try again later." });
                }
            }

            //Update the properties for the entity object from the DTO object
            patientToUpdate.ID = patientDTO.ID;
            patientToUpdate.FirstName = patientDTO.FirstName;
            patientToUpdate.MiddleName = patientDTO.MiddleName;
            patientToUpdate.LastName = patientDTO.LastName;
            patientToUpdate.OHIP = patientDTO.OHIP;
            patientToUpdate.DOB = patientDTO.DOB;
            patientToUpdate.ExpYrVisits = patientDTO.ExpYrVisits;
            patientToUpdate.RowVersion = patientDTO.RowVersion;
            patientToUpdate.DoctorID = patientDTO.DoctorID;

            _context.Entry(patientToUpdate).Property("RowVersion").OriginalValue = patientDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!PatientExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Patient has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Patient has been updated by another user." });
                }
            }
            catch(DbUpdateException dex)
            {
                if(dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save : Duplicate OHIP number." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persist, contact to your database administrator." });
                }
            }
        }

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PatientDTO>> PostPatient(PatientDTO patientDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Patient patient = new Patient
            {
                ID = patientDTO.ID,
                FirstName = patientDTO.FirstName,
                MiddleName = patientDTO.MiddleName,
                LastName = patientDTO.LastName,
                OHIP = patientDTO.OHIP,
                DOB = patientDTO.DOB,
                ExpYrVisits = patientDTO.ExpYrVisits,
                RowVersion = patientDTO.RowVersion,
                DoctorID = patientDTO.DoctorID,

            };
            
            try
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                patientDTO.ID = patient.ID;
                patientDTO.RowVersion = patient.RowVersion;

                return CreatedAtAction(nameof(GetPatient), new { id = patient.ID }, patientDTO);
            }
            catch (DbUpdateException dex)
            {
                if(dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = " Unable to save: Duplicate OHIP number." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persist, contact to your database administrator." });
                }
            }
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Patient>> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound(new {message = "Delete Error: Patient has already been removed."});
            }
            try
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Patient." });
            }
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }
    }
}
