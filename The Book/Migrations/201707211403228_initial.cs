namespace The_Book.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssessmentMarks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        mark = c.Double(nullable: false),
                        Assessment_Id = c.Long(),
                        Student_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assessments", t => t.Assessment_Id)
                .ForeignKey("dbo.Students", t => t.Student_Id)
                .Index(t => t.Assessment_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Assessments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        totalMark = c.Double(nullable: false),
                        cassContribution = c.Double(nullable: false),
                        date = c.DateTime(nullable: false),
                        EnrollmentSubject_Id = c.Long(),
                        Enrollment_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnrollmentSubjects", t => t.EnrollmentSubject_Id)
                .ForeignKey("dbo.Enrollments", t => t.Enrollment_Id)
                .Index(t => t.EnrollmentSubject_Id)
                .Index(t => t.Enrollment_Id);
            
            CreateTable(
                "dbo.Enrollments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        grade = c.Int(nullable: false),
                        group = c.String(nullable: false, maxLength: 1),
                        stream_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Streams", t => t.stream_Id)
                .Index(t => t.stream_Id);
            
            CreateTable(
                "dbo.ClassTasks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        heading = c.String(nullable: false, maxLength: 100),
                        content = c.String(nullable: false),
                        dueDate = c.DateTime(nullable: false),
                        postDate = c.DateTime(nullable: false),
                        submittingOption = c.String(nullable: false),
                        Enrollment_Id = c.Long(),
                        Teacher_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Enrollments", t => t.Enrollment_Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .Index(t => t.Enrollment_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.ClassTaskFiles",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        FileName = c.String(),
                        Extension = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassTasks", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Interactions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        content = c.String(nullable: false),
                        postDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ClassTask_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.ClassTasks", t => t.ClassTask_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ClassTask_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        userPhoto = c.Binary(),
                        firstName = c.String(),
                        middleName = c.String(),
                        lastName = c.String(),
                        Password = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        school_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Schools", t => t.school_Id)
                .Index(t => t.Id)
                .Index(t => t.school_Id);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        schoolPhoto = c.Binary(),
                        name = c.String(nullable: false),
                        TelNo = c.String(maxLength: 10),
                        FaxNo = c.String(maxLength: 10),
                        Email = c.String(),
                        province = c.String(nullable: false),
                        street = c.String(),
                        suburb = c.String(nullable: false),
                        city = c.String(nullable: false),
                        code = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Libraries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        title = c.String(nullable: false),
                        _date = c.DateTime(nullable: false),
                        Enrollment_Id = c.Long(),
                        School_Id = c.Long(),
                        Stream_Id = c.Long(),
                        Teacher_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Enrollments", t => t.Enrollment_Id)
                .ForeignKey("dbo.Schools", t => t.School_Id)
                .ForeignKey("dbo.Streams", t => t.Stream_Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .Index(t => t.Enrollment_Id)
                .Index(t => t.School_Id)
                .Index(t => t.Stream_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.Streams",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        school_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.school_Id)
                .Index(t => t.school_Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        stream_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Streams", t => t.stream_Id)
                .Index(t => t.stream_Id);
            
            CreateTable(
                "dbo.StudyMaterials",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        FileName = c.String(),
                        Extension = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Libraries", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        title = c.String(),
                        empNo = c.String(),
                        idNo = c.String(),
                        gender = c.String(),
                        dob = c.DateTime(nullable: false),
                        mobileNo = c.String(maxLength: 10),
                        telNo = c.String(maxLength: 10),
                        school_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Schools", t => t.school_Id)
                .Index(t => t.Id)
                .Index(t => t.school_Id);
            
            CreateTable(
                "dbo.EnrollmentSubjects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        name = c.String(),
                        Enrollment_Id = c.Long(),
                        Teacher_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Enrollments", t => t.Enrollment_Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .Index(t => t.Enrollment_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.TeacherAddresses",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        street = c.String(nullable: false),
                        suburb = c.String(nullable: false),
                        city = c.String(nullable: false),
                        code = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SchoolEvents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        School_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.School_Id)
                .Index(t => t.School_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        studNo = c.String(),
                        idNo = c.String(),
                        gender = c.String(),
                        dob = c.DateTime(nullable: false),
                        contNo = c.String(maxLength: 10),
                        homeTel = c.String(maxLength: 10),
                        enrollment_Id = c.Long(),
                        school_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Enrollments", t => t.enrollment_Id)
                .ForeignKey("dbo.Schools", t => t.school_Id)
                .Index(t => t.Id)
                .Index(t => t.enrollment_Id)
                .Index(t => t.school_Id);
            
            CreateTable(
                "dbo.StudentAddresses",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        street = c.String(nullable: false),
                        suburb = c.String(nullable: false),
                        city = c.String(nullable: false),
                        code = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.TaskSubmissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        submissionDate = c.DateTime(nullable: false),
                        ClassTask_Id = c.Long(),
                        Student_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassTasks", t => t.ClassTask_Id)
                .ForeignKey("dbo.Students", t => t.Student_Id)
                .Index(t => t.ClassTask_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.SubmittedFiles",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        FileName = c.String(),
                        Extension = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskSubmissions", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.TeacherEnrollments",
                c => new
                    {
                        Teacher_Id = c.String(nullable: false, maxLength: 128),
                        Enrollment_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_Id, t.Enrollment_Id })
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id, cascadeDelete: true)
                .ForeignKey("dbo.Enrollments", t => t.Enrollment_Id, cascadeDelete: true)
                .Index(t => t.Teacher_Id)
                .Index(t => t.Enrollment_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AssessmentMarks", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Assessments", "Enrollment_Id", "dbo.Enrollments");
            DropForeignKey("dbo.Interactions", "ClassTask_Id", "dbo.ClassTasks");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Managers", "school_Id", "dbo.Schools");
            DropForeignKey("dbo.SubmittedFiles", "Id", "dbo.TaskSubmissions");
            DropForeignKey("dbo.TaskSubmissions", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.TaskSubmissions", "ClassTask_Id", "dbo.ClassTasks");
            DropForeignKey("dbo.StudentAddresses", "Id", "dbo.Students");
            DropForeignKey("dbo.Students", "school_Id", "dbo.Schools");
            DropForeignKey("dbo.Students", "enrollment_Id", "dbo.Enrollments");
            DropForeignKey("dbo.Students", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SchoolEvents", "School_Id", "dbo.Schools");
            DropForeignKey("dbo.TeacherAddresses", "Id", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "school_Id", "dbo.Schools");
            DropForeignKey("dbo.Libraries", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.EnrollmentSubjects", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.EnrollmentSubjects", "Enrollment_Id", "dbo.Enrollments");
            DropForeignKey("dbo.Assessments", "EnrollmentSubject_Id", "dbo.EnrollmentSubjects");
            DropForeignKey("dbo.TeacherEnrollments", "Enrollment_Id", "dbo.Enrollments");
            DropForeignKey("dbo.TeacherEnrollments", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.ClassTasks", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudyMaterials", "Id", "dbo.Libraries");
            DropForeignKey("dbo.Subjects", "stream_Id", "dbo.Streams");
            DropForeignKey("dbo.Streams", "school_Id", "dbo.Schools");
            DropForeignKey("dbo.Libraries", "Stream_Id", "dbo.Streams");
            DropForeignKey("dbo.Enrollments", "stream_Id", "dbo.Streams");
            DropForeignKey("dbo.Libraries", "School_Id", "dbo.Schools");
            DropForeignKey("dbo.Libraries", "Enrollment_Id", "dbo.Enrollments");
            DropForeignKey("dbo.Managers", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Interactions", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClassTasks", "Enrollment_Id", "dbo.Enrollments");
            DropForeignKey("dbo.ClassTaskFiles", "Id", "dbo.ClassTasks");
            DropForeignKey("dbo.AssessmentMarks", "Assessment_Id", "dbo.Assessments");
            DropIndex("dbo.TeacherEnrollments", new[] { "Enrollment_Id" });
            DropIndex("dbo.TeacherEnrollments", new[] { "Teacher_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.SubmittedFiles", new[] { "Id" });
            DropIndex("dbo.TaskSubmissions", new[] { "Student_Id" });
            DropIndex("dbo.TaskSubmissions", new[] { "ClassTask_Id" });
            DropIndex("dbo.StudentAddresses", new[] { "Id" });
            DropIndex("dbo.Students", new[] { "school_Id" });
            DropIndex("dbo.Students", new[] { "enrollment_Id" });
            DropIndex("dbo.Students", new[] { "Id" });
            DropIndex("dbo.SchoolEvents", new[] { "School_Id" });
            DropIndex("dbo.TeacherAddresses", new[] { "Id" });
            DropIndex("dbo.EnrollmentSubjects", new[] { "Teacher_Id" });
            DropIndex("dbo.EnrollmentSubjects", new[] { "Enrollment_Id" });
            DropIndex("dbo.Teachers", new[] { "school_Id" });
            DropIndex("dbo.Teachers", new[] { "Id" });
            DropIndex("dbo.StudyMaterials", new[] { "Id" });
            DropIndex("dbo.Subjects", new[] { "stream_Id" });
            DropIndex("dbo.Streams", new[] { "school_Id" });
            DropIndex("dbo.Libraries", new[] { "Teacher_Id" });
            DropIndex("dbo.Libraries", new[] { "Stream_Id" });
            DropIndex("dbo.Libraries", new[] { "School_Id" });
            DropIndex("dbo.Libraries", new[] { "Enrollment_Id" });
            DropIndex("dbo.Managers", new[] { "school_Id" });
            DropIndex("dbo.Managers", new[] { "Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Interactions", new[] { "ClassTask_Id" });
            DropIndex("dbo.Interactions", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ClassTaskFiles", new[] { "Id" });
            DropIndex("dbo.ClassTasks", new[] { "Teacher_Id" });
            DropIndex("dbo.ClassTasks", new[] { "Enrollment_Id" });
            DropIndex("dbo.Enrollments", new[] { "stream_Id" });
            DropIndex("dbo.Assessments", new[] { "Enrollment_Id" });
            DropIndex("dbo.Assessments", new[] { "EnrollmentSubject_Id" });
            DropIndex("dbo.AssessmentMarks", new[] { "Student_Id" });
            DropIndex("dbo.AssessmentMarks", new[] { "Assessment_Id" });
            DropTable("dbo.TeacherEnrollments");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.SubmittedFiles");
            DropTable("dbo.TaskSubmissions");
            DropTable("dbo.StudentAddresses");
            DropTable("dbo.Students");
            DropTable("dbo.SchoolEvents");
            DropTable("dbo.TeacherAddresses");
            DropTable("dbo.EnrollmentSubjects");
            DropTable("dbo.Teachers");
            DropTable("dbo.StudyMaterials");
            DropTable("dbo.Subjects");
            DropTable("dbo.Streams");
            DropTable("dbo.Libraries");
            DropTable("dbo.Schools");
            DropTable("dbo.Managers");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Interactions");
            DropTable("dbo.ClassTaskFiles");
            DropTable("dbo.ClassTasks");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Assessments");
            DropTable("dbo.AssessmentMarks");
        }
    }
}
