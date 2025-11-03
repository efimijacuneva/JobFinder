namespace JobFinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Solved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobPosts", "CompanyId", "dbo.AspNetUsers");
            DropIndex("dbo.JobPosts", new[] { "CompanyId" });
            AlterColumn("dbo.JobPosts", "CompanyId", c => c.String(maxLength: 128));
            CreateIndex("dbo.JobPosts", "CompanyId");
            AddForeignKey("dbo.JobPosts", "CompanyId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobPosts", "CompanyId", "dbo.AspNetUsers");
            DropIndex("dbo.JobPosts", new[] { "CompanyId" });
            AlterColumn("dbo.JobPosts", "CompanyId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.JobPosts", "CompanyId");
            AddForeignKey("dbo.JobPosts", "CompanyId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
