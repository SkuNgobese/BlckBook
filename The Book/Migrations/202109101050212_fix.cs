namespace The_Book.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeactivatedTeachers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        reason = c.String(nullable: false, maxLength: 50),
                        _date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Parents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        title = c.String(nullable: false),
                        fName = c.String(nullable: false),
                        mName = c.String(),
                        lName = c.String(nullable: false),
                        contactNo = c.String(nullable: false, maxLength: 10),
                        workTel = c.String(maxLength: 10),
                        emailAddress = c.String(),
                        School_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.School_Id)
                .Index(t => t.School_Id);
            
            CreateTable(
                "dbo.DeactivatedStudents",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        reason = c.String(nullable: false, maxLength: 50),
                        _date = c.DateTime(nullable: false),
                        Teacher_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .Index(t => t.Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.SchoolAddresses",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        province = c.String(nullable: false),
                        street = c.String(),
                        suburb = c.String(nullable: false),
                        city = c.String(nullable: false),
                        code = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.ClassTasks", "dueTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Teachers", "active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Teachers", "_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Students", "active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Students", "_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Students", "parent_Id", c => c.Long());
            AlterColumn("dbo.Teachers", "mobileNo", c => c.String());
            AlterColumn("dbo.Teachers", "telNo", c => c.String());
            CreateIndex("dbo.Students", "parent_Id");
            AddForeignKey("dbo.Students", "parent_Id", "dbo.Parents", "Id");
            DropColumn("dbo.Schools", "province");
            DropColumn("dbo.Schools", "street");
            DropColumn("dbo.Schools", "suburb");
            DropColumn("dbo.Schools", "city");
            DropColumn("dbo.Schools", "code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schools", "code", c => c.String(nullable: false, maxLength: 4));
            AddColumn("dbo.Schools", "city", c => c.String(nullable: false));
            AddColumn("dbo.Schools", "suburb", c => c.String(nullable: false));
            AddColumn("dbo.Schools", "street", c => c.String());
            AddColumn("dbo.Schools", "province", c => c.String(nullable: false));
            DropForeignKey("dbo.SchoolAddresses", "Id", "dbo.Schools");
            DropForeignKey("dbo.Students", "parent_Id", "dbo.Parents");
            DropForeignKey("dbo.DeactivatedStudents", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.DeactivatedStudents", "Id", "dbo.Students");
            DropForeignKey("dbo.Parents", "School_Id", "dbo.Schools");
            DropForeignKey("dbo.DeactivatedTeachers", "Id", "dbo.Teachers");
            DropIndex("dbo.SchoolAddresses", new[] { "Id" });
            DropIndex("dbo.DeactivatedStudents", new[] { "Teacher_Id" });
            DropIndex("dbo.DeactivatedStudents", new[] { "Id" });
            DropIndex("dbo.Students", new[] { "parent_Id" });
            DropIndex("dbo.Parents", new[] { "School_Id" });
            DropIndex("dbo.DeactivatedTeachers", new[] { "Id" });
            AlterColumn("dbo.Teachers", "telNo", c => c.String(maxLength: 10));
            AlterColumn("dbo.Teachers", "mobileNo", c => c.String(maxLength: 10));
            DropColumn("dbo.Students", "parent_Id");
            DropColumn("dbo.Students", "_date");
            DropColumn("dbo.Students", "active");
            DropColumn("dbo.Teachers", "_date");
            DropColumn("dbo.Teachers", "active");
            DropColumn("dbo.ClassTasks", "dueTime");
            DropTable("dbo.SchoolAddresses");
            DropTable("dbo.DeactivatedStudents");
            DropTable("dbo.Parents");
            DropTable("dbo.DeactivatedTeachers");
        }
    }
}
