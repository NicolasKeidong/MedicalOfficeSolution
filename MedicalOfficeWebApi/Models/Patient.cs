﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace MedicalOfficeWebApi.Models
{
    [ModelMetadataType(typeof(PatientMetaData))]
    public class Patient : Auditable,IValidatableObject
    {
        public int ID { get; set; }

        public string FullName
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }

        public string FirstName { get; set; }


        public string MiddleName { get; set; }

        public string LastName { get; set; }


        public string OHIP { get; set; }

        public DateTime DOB { get; set; }

        public byte ExpYrVisits { get; set; }

        [Timestamp]
        public Byte[] RowVersion { get; set; }

        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public ICollection<PatientCondition> PatientConditions { get; set; } = new HashSet<PatientCondition>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB > DateTime.Today.AddDays(1))
            {
                yield return new ValidationResult("Date of Birth cannot be in the future.", new[] { "DOB" });
            }
        }
    }
}
