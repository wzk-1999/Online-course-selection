using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using Zhankui_Wang_ProblemAssignment2.Data;

public class EmailSender
{
    private string _host;
    private int _port;
    private bool _enableSSL;
    private string _userName;
    private string _password; // Generated App Password
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Zhankui_Wang_ProblemAssignment2Context _context;
    public EmailSender(IHttpContextAccessor httpContextAccessor, Zhankui_Wang_ProblemAssignment2Context zhankui_Wang_ProblemAssignment2Context)
    {
        _httpContextAccessor = httpContextAccessor;
        _host = "smtp.gmail.com";
        _port = 587; // Use 465 if required by your SMTP provider
        _enableSSL = true;
        _userName = "your email";
        _password = "generated app password"; // Generated App Password
        _context = zhankui_Wang_ProblemAssignment2Context;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string htmlMessage)
    {
        try
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.Credentials = new NetworkCredential(_userName, _password);
                client.EnableSsl = _enableSSL;

                var mailMessage = new MailMessage(_userName, recipientEmail, subject, htmlMessage) { IsBodyHtml = true };
                await client.SendMailAsync(mailMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while sending email: {ex.Message}");
            throw;
        }
    }

    public async Task Sent(IUrlHelper urlHelper, string toUser, int studentId,int? courseId)
    {
        // Fetch the student's email from the database
        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }
        // Construct the URL for the IfEnroll action
        string enrollUrl = urlHelper.Action("IfEnroll", "Courses", new { studentID = studentId, courseID= courseId }, protocol: "https");

        // Construct the HTML body of the email
        string htmlBody = $"Dear {toUser},<br /><br />";
        htmlBody += "Thank you for showing interest. Please accept the invitation to enroll in the course by clicking the link below:<br /><br />";
        htmlBody += $"<a href='{enrollUrl}'>here</a><br /><br />";
        htmlBody += "Thank you.";

        await SendEmailAsync(student.Email, "Accept offer", htmlBody);
        Console.WriteLine("Email sent successfully.");
    }
}
