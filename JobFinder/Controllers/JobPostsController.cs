using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using JobFinder.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace JobFinder.Controllers
{
    public class JobPostsController : Controller
    {
        private JobFinderContext db = new JobFinderContext();

       
        private List<string> GetNorthMacedoniaTowns()
        {
            return new List<string>
            {
                "Skopje",
                "Bitola",
                "Kumanovo",
                "Prilep",
                "Tetovo",
                "Veles",
                "Ohrid",
                "Gostivar",
                "Štip",
                "Strumica",
                "Kavadarci",
                "Kočani",
                "Kičevo",
                "Struga",
                "Radoviš",
                "Gevgelija",
                "Debar",
                "Kriva Palanka",
                "Sveti Nikole",
                "Delčevo",
                "Vinica",
                "Probištip",
                "Aračinovo",
                "Bogdanci",
                "Demir Kapija",
                "Demir Hisar",
                "Pehčevo",
                "Berovo",
                "Makedonski Brod",
                "Makedonska Kamenica",
                "Valandovo",
                "Resen",
                "Kratovo",
                "Kruševo",
                "Plasnica",
                "Brvenica",
                "Vrapčište",
                "Čaška",
                "Zelenikovo",
                "Ilinden",
                "Petrovec",
                "Sopište",
                "Studeničani",
                "Želino",
                "Jegunovce",
                "Tearce",
                "Lipkovo",
                "Staro Nagoričane",
                "Rankovce",
                "Karbinci",
                "Lozovo",
                "Rosoman",
                "Gradsko",
                "Mogila",
                "Novaci",
                "Krivogaštani",
                "Dolneni",
                "Češinovo-Obleševo",
                "Zrnovci",
                "Konče",
                "Miravci",
                "Dojran",
                "Bogovinje",
                "Mavrovo and Rostuša",
                "Centar Župa",
                "Debarca",
                "Drugovo",
                "Zajas",
                "Oslomej",
                "Vraneštica"
            }.OrderBy(t => t).ToList();
        }

        
        public ActionResult Index(string category = null, string location = null)
        {
            var q = db.JobPosts.Include(j => j.Company).AsQueryable();
            if (!string.IsNullOrEmpty(category))
                q = q.Where(j => j.Category == category);
            if (!string.IsNullOrEmpty(location))
                q = q.Where(j => j.Location.Contains(location));
            
           
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            
           
            ViewBag.Towns = new SelectList(GetNorthMacedoniaTowns(), location);
            
            return View(q.OrderByDescending(j => j.DatePosted).ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var job = db.JobPosts.Include(j => j.Company).FirstOrDefault(j => j.Id == id);
            if (job == null) return HttpNotFound();
            
            
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            
            return View(job);
        }

        
        [Authorize(Roles = "Company")]
        public ActionResult Create()
        {
            
            ViewBag.Towns = new SelectList(GetNorthMacedoniaTowns());
            return View();
        }

        
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Company")]
        public ActionResult Create(JobPost job)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    job.CompanyId = User.Identity.GetUserId();
                    job.DatePosted = DateTime.Now;
                    
                    db.JobPosts.Add(job);
                    db.SaveChanges();
                    
                    TempData["Success"] = "Job posted successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while posting the job. Please try again.");
                    
                    ViewBag.Towns = new SelectList(GetNorthMacedoniaTowns());
                    return View(job);
                }
            }
            
            
            ViewBag.Towns = new SelectList(GetNorthMacedoniaTowns());
            return View(job);
        }

        
        [Authorize(Roles = "Company")]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var job = db.JobPosts.FirstOrDefault(j => j.Id == id);
            if (job == null) return HttpNotFound();
            
            
            if (job.CompanyId != User.Identity.GetUserId())
            {
                TempData["Error"] = "You can only edit your own job posts.";
                return RedirectToAction("Index");
            }
            
            
            ViewBag.Towns = new SelectList(GetNorthMacedoniaTowns());
            
            return View(job);
        }

        
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Company")]
        public ActionResult Edit(int id, JobPost job)
        {
            if (id != job.Id) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var existingJob = db.JobPosts.FirstOrDefault(j => j.Id == id);
            if (existingJob == null) return HttpNotFound();
            
            
            if (existingJob.CompanyId != User.Identity.GetUserId())
            {
                TempData["Error"] = "You can only edit your own job posts.";
                return RedirectToAction("Index");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    
                    existingJob.Title = job.Title;
                    existingJob.Description = job.Description;
                    existingJob.Location = job.Location;
                    existingJob.Category = job.Category;
                    
                    db.SaveChanges();
                    
                    TempData["Success"] = "Job post updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the job. Please try again.");
                    
                    ViewBag.Towns = new SelectList(GetNorthMacedoniaTowns());
                    return View(job);
                }
            }
            
            
            ViewBag.Towns = new SelectList(GetNorthMacedoniaTowns());
            return View(job);
        }

        
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Company")]
        public ActionResult Delete(int id)
        {
            var job = db.JobPosts.Include(j => j.Company).FirstOrDefault(j => j.Id == id);
            
            if (job == null)
            {
                return HttpNotFound();
            }
            
            
            if (job.CompanyId != User.Identity.GetUserId())
            {
                TempData["Error"] = "You can only delete your own job posts.";
                return RedirectToAction("Index");
            }
            
            try
            {
                db.JobPosts.Remove(job);
                db.SaveChanges();
                TempData["Success"] = "Job post deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the job post.";
            }
            
            return RedirectToAction("Index");
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
