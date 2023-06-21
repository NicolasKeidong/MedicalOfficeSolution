using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace MedicalOfficeWebApi.Models
{
    [ModelMetadataType(typeof(DoctorMetaData))]
    public class DoctorDTO
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public Byte[] RowVersion { get; set; }

        public ICollection<PatientDTO> Patients { get; set; }
    }
}
