using System.ComponentModel.DataAnnotations;

namespace Zhankui_Wang_ProblemAssignment2.Models
{
    public class ManageCourseViewModel
    {
        public Course Course { get; set; }
        public List<Student> Students { get; set; }

        // Properties for the new student
        [Required]
        [MaxLength(50)]
        public string NewStudentFirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string NewStudentLastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string NewStudentEmail { get; set; }
    }
}
