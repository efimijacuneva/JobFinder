namespace JobFinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationResponse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "Status", c => c.String());
            AddColumn("dbo.JobApplications", "CompanyResponse", c => c.String());
            AddColumn("dbo.JobApplications", "ResponseDate", c => c.DateTime());
            AddColumn("dbo.JobApplications", "ResponseBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplications", "ResponseBy");
            DropColumn("dbo.JobApplications", "ResponseDate");
            DropColumn("dbo.JobApplications", "CompanyResponse");
            DropColumn("dbo.JobApplications", "Status");
        }
    }
}
