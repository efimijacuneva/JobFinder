namespace JobFinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CompanyId = c.String(nullable: false, maxLength: 128),
                    CandidateId = c.String(nullable: false, maxLength: 128),
                    Rating = c.Int(nullable: false),
                    Comment = c.String(),
                    DatePosted = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                // disable cascade delete here
                .ForeignKey("dbo.AspNetUsers", t => t.CandidateId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.CompanyId, cascadeDelete: false)
                .Index(t => t.CompanyId)
                .Index(t => t.CandidateId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Role = c.String(),
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
                "dbo.JobApplications",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CandidateId = c.String(nullable: false, maxLength: 128),
                    JobPostId = c.Int(nullable: false),
                    CvFilePath = c.String(),
                    AppliedOn = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CandidateId, cascadeDelete: true)
                .ForeignKey("dbo.JobPosts", t => t.JobPostId, cascadeDelete: true)
                .Index(t => t.CandidateId)
                .Index(t => t.JobPostId);

            CreateTable(
                "dbo.JobPosts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(nullable: false),
                    Description = c.String(),
                    Location = c.String(nullable: false),
                    DatePosted = c.DateTime(nullable: false),
                    CompanyId = c.String(nullable: false, maxLength: 128),
                    Category = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                // disable cascade delete here as well
                .ForeignKey("dbo.AspNetUsers", t => t.CompanyId, cascadeDelete: false)
                .Index(t => t.CompanyId);

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
                "dbo.SavedJobs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CandidateId = c.String(maxLength: 128),
                    JobPostId = c.Int(nullable: false),
                    SavedOn = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CandidateId)
                .ForeignKey("dbo.JobPosts", t => t.JobPostId, cascadeDelete: true)
                .Index(t => t.CandidateId)
                .Index(t => t.JobPostId);
        }


        public override void Down()
        {
            DropForeignKey("dbo.SavedJobs", "JobPostId", "dbo.JobPosts");
            DropForeignKey("dbo.SavedJobs", "CandidateId", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobPosts", "CompanyId", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobApplications", "JobPostId", "dbo.JobPosts");
            DropForeignKey("dbo.JobApplications", "CandidateId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feedbacks", "CompanyId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feedbacks", "CandidateId", "dbo.AspNetUsers");
            DropIndex("dbo.SavedJobs", new[] { "JobPostId" });
            DropIndex("dbo.SavedJobs", new[] { "CandidateId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.JobPosts", new[] { "CompanyId" });
            DropIndex("dbo.JobApplications", new[] { "JobPostId" });
            DropIndex("dbo.JobApplications", new[] { "CandidateId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Feedbacks", new[] { "CandidateId" });
            DropIndex("dbo.Feedbacks", new[] { "CompanyId" });
            DropTable("dbo.SavedJobs");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.JobPosts");
            DropTable("dbo.JobApplications");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Feedbacks");
        }
    }
}
