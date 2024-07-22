using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Zhankui_Wang_ProblemAssignment2.Models
{
    public class Student
    {
        [Key]
        public int StudentID {  get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "maximum size of FirstName is 50 characters")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "maximum size of LastName is 50 characters")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100, ErrorMessage = "maximum size of email is 100 characters")]
        public string Email { get; set; }
        [Required]
        [EnumDataType(typeof(Status))]
        public Status _Status { get; set; }

        [DisplayName("Name")]
        public string FullName
        {
            get
            {
                return FirstName + "," + LastName;
            }
        }
        // Foreign key for Course
        public int? CourseID { get; set; }

        // Navigation property for the Course
        public virtual Course Course { get; set; }
        public enum Status { InvitationSent, InvitationRejected, Enrolled,JustCreated }

        public List<string> StatusValues()
        {
            List<string> statusStrings = new List<string>();
            foreach (Status status in Enum.GetValues(typeof(Status)))
            {
                statusStrings.Add(status.ToString());
            }
            return statusStrings;
        }

      
    }
}
