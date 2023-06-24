using Microsoft.AspNetCore.Mvc;

namespace MedicalOfficeWebApi.Models
{
    [ModelMetadataType(typeof(ConditionMetaData))]
    public class Condition
    {
        public int ID { get; set; }
        public string ConditionName { get; set; }
        public ICollection<PatientCondition> PatientConditions { get; set; } = new HashSet<PatientCondition>();
    }
}
