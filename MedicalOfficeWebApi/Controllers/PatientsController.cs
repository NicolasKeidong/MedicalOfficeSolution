﻿using System;
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
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.ID)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.ID }, patient);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }
    }
}
