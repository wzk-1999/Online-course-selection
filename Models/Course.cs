using System.ComponentModel.DataAnnotations;

namespace Zhankui_Wang_ProblemAssignment2.Models
{
    public class Course
    {
        [Key]
        public int CourseID {  get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z]{3}[0-9]{3,4}$", ErrorMessage = "Code must be in format 'AAA111' or 'AAA1111'")]
        public string Code { get; set; }


        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Location")]
        [RegularExpression(@"^[A-Za-z]{2}[0-9]{2,4}$", ErrorMessage = "Location must be in format 'AA11', 'AA111' or 'AA1111'")]
        public string? ClassRoom { get; set; }

        [Required(ErrorMessage = "Instructor name is required")]
        public string Instructor { get; set; }

        [Display(Name = "Roster Size")]
        public int NumStudents
        {
            get
            {
				return Students?.Count(s => s._Status == Student.Status.Enrolled) ?? 0;
			}
        }

        // Navigation property for Students enrolled in this course
        public virtual ICollection<Student> Students { get; set; }

   
    }
}
