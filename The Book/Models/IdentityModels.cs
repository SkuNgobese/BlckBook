using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace The_Book.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Interactions = new List<Interaction>();
        }
        public byte[] userPhoto { get; set; }               
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string fullName
        {
            get
            {
                return firstName + " " + middleName + " " + lastName;
            }
        }
        public string Password { get; internal set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Manager Manager { get; set; }
        public virtual ICollection<Interaction> Interactions { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<The_Book.Models.Manager> Managers { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.School> Schools { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.SchoolAddress> SchoolAddresses { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.Teacher> Teachers { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.Student> Students { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.TeacherAddress> TeacherAddresses { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.Enrollment> Enrollments { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.Stream> Streams { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.Subject> Subjects { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.StudentAddress> StudentAddresses { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.EnrollmentSubject> EnrollmentSubjects { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.Library> Libraries { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.StudyMaterial> StudyMaterials { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.ClassTaskFile> ClassTaskFiles { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.ClassTask> ClassTasks { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.TaskSubmission> TaskSubmissions { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.Interaction> Interactions { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.SchoolEvent> SchoolEvents { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.SubmittedFile> SubmittedFiles { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.Assessment> Assessments { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.AssessmentMark> AssessmentMarks { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.Parent> Parents { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.DeactivatedTeacher> DeactivatedTeachers { get; set; }
        public System.Data.Entity.DbSet<The_Book.Models.DeactivatedStudent> DeactivatedStudents { get; set; }
    }
}