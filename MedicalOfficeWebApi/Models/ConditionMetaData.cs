using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MedicalOfficeWebApi.Models
{
    public class ConditionMetaData
    {
        public int ID { get; set; }

        [Display(Name = "Medical Condition")]
        [Required(ErrorMessage = "You cannot leave the name of the condition blank.")]
        [StringLength(50, ErrorMessage = "Condition Name cannot exceed 50 caracters.")]
        public string ConditionName { get; set; }

        
    }
}
