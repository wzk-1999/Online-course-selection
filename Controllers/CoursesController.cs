using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Zhankui_Wang_ProblemAssignment2.Data;
using Zhankui_Wang_ProblemAssignment2.Models;

namespace Zhankui_Wang_ProblemAssignment2.Controllers
{
    public class CoursesController : Controller
    {
        private readonly Zhankui_Wang_ProblemAssignment2Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;

        public CoursesController(Zhankui_Wang_ProblemAssignment2Context context, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Course.Include(c => c.Students).ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Code,Title,ClassRoom,Instructor")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Code,Title,ClassRoom,Instructor")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseID == id);
        }

		// GET: Courses/ManageCourse/5
		public async Task<IActionResult> ManageCourse(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

            // Retrieve course details
            var course = await _context.Course
                .FirstOrDefaultAsync(c => c.CourseID == id);

            var students = await _context.Students.Where(m => m.CourseID == id).ToListAsync();
            //if (student == null||student.Count==0)
            //{
            //	return NotFound();
            //}

            // Create a view model to pass both course and students to the view
            var viewModel = new ManageCourseViewModel
            {
                Course = course,
                Students = students
            };

            return View(viewModel);
		}

        [HttpPost]
        public async Task<IActionResult> ManageCourse(ManageCourseViewModel model, int id)
        {

            // Remove Course and Students properties from ModelState validation
            ModelState.Remove(nameof(ManageCourseViewModel.Course));
            ModelState.Remove(nameof(ManageCourseViewModel.Students));

            if (ModelState.IsValid)
            {
                var course = await _context.Course.FindAsync(id);
                if (course == null)
                {
                    return NotFound();
                }

                var newStudent = new Student
                {
                    FirstName = model.NewStudentFirstName,
                    LastName = model.NewStudentLastName,
                    Email = model.NewStudentEmail,
                    CourseID = id,
                    _Status = Student.Status.JustCreated // Default status
                };

                _context.Students.Add(newStudent);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ManageCourse), new { id = id });
            }

            // Log validation errors
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
            }

            // Retrieve existing course and students again if model state is invalid
            model.Course = await _context.Course.FindAsync(id);
            model.Students = await _context.Students.Where(s => s.CourseID == id).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> EmailSender(int id)
        {
            var Students = await _context.Students.FindAsync(id);
            if (Students == null)
            {
                return NotFound();
            }

            // Logic to send email (if any)...
            var emailSender = new EmailSender(_httpContextAccessor, _context);
            await emailSender.Sent(_urlHelperFactory.GetUrlHelper(ControllerContext), Students.FullName, Students.StudentID,Students.CourseID);
            // Update the status to InvitationSent
            Students._Status = Student.Status.InvitationSent;
            await _context.SaveChangesAsync();
            return View(Students);

        }

        public async Task<IActionResult> IfEnroll(int studentID, int courseID)
        {
            var student = await _context.Students.FindAsync(studentID);
            var course = await _context.Course.FindAsync(courseID);

            if (student == null || course == null)
            {
                return NotFound();
            }

            var manageEnrollViewModel = new ManageEnrollViewModel
            {
                Student = student,
                Course = course
            };

            return View(manageEnrollViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> EnrollmentConfirmation(ManageEnrollViewModel model)
        {
            var student = await _context.Students.FindAsync(model.Student.StudentID);
            if (student == null)
            {
                return NotFound();
            }

            if (Enum.TryParse(model.IfEnroll, out Student.Status newStatus))
            {
                student._Status = newStatus;
                await _context.SaveChangesAsync();
                return RedirectToAction("ConfirmationResult");
            }
            else
            {
                ModelState.AddModelError("IfEnroll", $"'{model.IfEnroll}' is not a valid status");
                return View("IfEnroll", model);
            }
        }


        public IActionResult ConfirmationResult()
        {
            return View();
        }

    }
}
