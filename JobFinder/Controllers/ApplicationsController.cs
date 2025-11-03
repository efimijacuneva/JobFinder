using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobFinder.Models;
using Microsoft.AspNet.Identity;
using System;

namespace JobFinder.Controllers
{
    [Authorize(Roles = "Candidate,Company")]
    public class ApplicationsController : Controller
    {
        private JobFinderContext db = new JobFinderContext();

        
        public ActionResult Create(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var jobPost = db.JobPosts.Include(j => j.Company).FirstOrDefault(j => j.Id == id);
            if (jobPost == null) return HttpNotFound();
            ViewBag.JobPost = jobPost;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(int id, HttpPostedFileBase cv)
        {
            if (cv == null || cv.ContentLength == 0)
            {
                ModelState.AddModelError("cv", "Please upload a CV.");
                var jobPost = db.JobPosts.Include(j => j.Company).FirstOrDefault(j => j.Id == id);
                ViewBag.JobPost = jobPost;
                return View();
            }

            
            if (cv.ContentLength > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("cv", "File size must be less than 5MB.");
                var jobPost = db.JobPosts.Include(j => j.Company).FirstOrDefault(j => j.Id == id);
                ViewBag.JobPost = jobPost;
                return View();
            }

            
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(cv.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("cv", "Only PDF, DOC, and DOCX files are allowed.");
                var jobPost = db.JobPosts.Include(j => j.Company).FirstOrDefault(j => j.Id == id);
                ViewBag.JobPost = jobPost;
                return View();
            }

            
            var uploadPath = Server.MapPath("~/App_Data/CVs");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadPath, fileName);
            cv.SaveAs(filePath);

            var appl = new JobApplication
            {
                CandidateId = User.Identity.GetUserId(),
                JobPostId = id,
                CvFilePath = "/App_Data/CVs/" + fileName
            };
            db.JobApplications.Add(appl);
            db.SaveChanges();

            TempData["Success"] = "Application submitted successfully!";
            return RedirectToAction("MyApplications");
        }

        public ActionResult MyApplications()
        {
            var userId = User.Identity.GetUserId();
            var apps = db.JobApplications
                         .Include(a => a.JobPost)
                         .Include(a => a.JobPost.Company)
                         .Where(a => a.CandidateId == userId)
                         .OrderByDescending(a => a.AppliedOn)
                         .ToList();
            return View(apps);
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var application = db.JobApplications
                               .Include(a => a.JobPost)
                               .Include(a => a.JobPost.Company)
                               .Include(a => a.Candidate)
                               .FirstOrDefault(a => a.Id == id);
            if (application == null) return HttpNotFound();
            return View(application);
        }

        
        public ActionResult DownloadCV(int? id)
        {
            try
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
                var application = db.JobApplications
                                   .Include(a => a.JobPost)
                                   .FirstOrDefault(a => a.Id == id);
                
                if (application == null) return HttpNotFound();
                
                
                if (application.CandidateId != User.Identity.GetUserId() && 
                    application.JobPost.CompanyId != User.Identity.GetUserId())
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                
                if (string.IsNullOrEmpty(application.CvFilePath))
                {
                    return HttpNotFound();
                }
                
                var filePath = Server.MapPath(application.CvFilePath);
                if (!System.IO.File.Exists(filePath))
                {
                    return HttpNotFound();
                }
                
                var fileName = Path.GetFileName(filePath);
                var contentType = GetContentType(Path.GetExtension(fileName));
                
                
                Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                
                return File(filePath, contentType, fileName);
            }
            catch (Exception ex)
            {
                
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        private string GetContentType(string extension)
        {
            switch (extension.ToLower())
            {
                case ".pdf":
                    return "application/pdf";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                default:
                    return "application/octet-stream";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
