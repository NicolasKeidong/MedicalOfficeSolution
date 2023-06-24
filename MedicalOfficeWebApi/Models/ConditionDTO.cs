using Microsoft.AspNetCore.Mvc;

namespace MedicalOfficeWebApi.Models
{
    [ModelMetadataType(typeof(ConditionMetaData))]
    public class ConditionDTO
    {
        public int ID { get; set; }

        public string ConditionName { get; set; }

        public ICollection<PatientDTO> Patients { get; set; }
    }
}
