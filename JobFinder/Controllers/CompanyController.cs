using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using JobFinder.Models;
using Microsoft.AspNet.Identity;
using System;

namespace JobFinder.Controllers
{
    [Authorize(Roles = "Company")]
    public class CompanyController : Controller
    {
        private JobFinderContext db = new JobFinderContext();

        public ActionResult MyPosts()
        {
            var id = User.Identity.GetUserId();
            var posts = db.JobPosts.Where(j => j.CompanyId == id).ToList();
            
            
            ViewBag.CurrentUserId = id;
            
            return View(posts);
        }

        public ActionResult Applicants(int id)
        {
            var apps = db.JobApplications
                         .Include(a => a.Candidate)
                         .Where(a => a.JobPostId == id)
                         .ToList();
            return View(apps);
        }

        
        public ActionResult ViewApplicationDetails(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(400);
            
            var application = db.JobApplications
                               .Include(a => a.Candidate)
                               .Include(a => a.JobPost)
                               .FirstOrDefault(a => a.Id == id);
            
            if (application == null) return HttpNotFound();
            
           
            if (application.JobPost.CompanyId != User.Identity.GetUserId())
            {
                TempData["Error"] = "You can only view applications for your own job posts.";
                return RedirectToAction("MyPosts");
            }
            
            return View(application);
        }

        
        public ActionResult RespondToApplication(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(400);
            
            var application = db.JobApplications
                               .Include(a => a.Candidate)
                               .Include(a => a.JobPost)
                               .FirstOrDefault(a => a.Id == id);
            
            if (application == null) return HttpNotFound();
            
            
            if (application.JobPost.CompanyId != User.Identity.GetUserId())
            {
                TempData["Error"] = "You can only respond to applications for your own job posts.";
                return RedirectToAction("MyPosts");
            }
            
            return View(application);
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RespondToApplication(int id, string status, string companyResponse)
        {
            var application = db.JobApplications
                               .Include(a => a.JobPost)
                               .FirstOrDefault(a => a.Id == id);
            
            if (application == null) return HttpNotFound();
            
            
            if (application.JobPost.CompanyId != User.Identity.GetUserId())
            {
                TempData["Error"] = "You can only respond to applications for your own job posts.";
                return RedirectToAction("MyPosts");
            }
            
            try
            {
                application.Status = status;
                application.CompanyResponse = companyResponse;
                application.ResponseDate = DateTime.Now;
                application.ResponseBy = User.Identity.GetUserId();
                
                db.SaveChanges();
                
                TempData["Success"] = "Response sent successfully!";
                return RedirectToAction("Applicants", new { id = application.JobPostId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while sending the response.";
                return RedirectToAction("RespondToApplication", new { id = id });
            }
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteResponse(int id)
        {
            var application = db.JobApplications
                               .Include(a => a.JobPost)
                               .FirstOrDefault(a => a.Id == id);
            
            if (application == null) return HttpNotFound();
            
           
            if (application.JobPost.CompanyId != User.Identity.GetUserId())
            {
                TempData["Error"] = "You can only delete responses for your own job posts.";
                return RedirectToAction("MyPosts");
            }
            
            try
            {
                
                application.CompanyResponse = null;
                application.ResponseDate = null;
                application.ResponseBy = null;
                
                db.SaveChanges();
                
                TempData["Success"] = "Response deleted successfully!";
                return RedirectToAction("Applicants", new { id = application.JobPostId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the response.";
                return RedirectToAction("Applicants", new { id = application.JobPostId });
            }
        }
    }
}
