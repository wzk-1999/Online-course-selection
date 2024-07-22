using System.ComponentModel.DataAnnotations;
namespace Zhankui_Wang_ProblemAssignment2.Models
{
    public class ManageEnrollViewModel
    {
        public Course Course { get; set; }
        public Student Student { get; set; } // Single student for this view
        [Required(ErrorMessage = "you need to choose if you are going to enroll before submit")]
        public string IfEnroll { get; set; }
    }
}
