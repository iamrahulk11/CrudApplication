using System.ComponentModel.DataAnnotations;

namespace CrudApplicationTest.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "*Name Cannot be Empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Batch Cannot be Empty")]
        public string Batch { get; set; }

        [Required(ErrorMessage = "*Phone Number Cannot be Empty")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "*State Cannot be Empty")]
        public string State { get; set; }

        [Required(ErrorMessage = "*City Cannot be Empty")]
        public string City { get; set; }

        [Required]
        public int? Pincode { get; set; }
    }

}
