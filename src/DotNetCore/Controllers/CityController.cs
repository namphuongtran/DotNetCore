using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCore.Controllers
{
    public class CityController : Controller
    {
        public CityController()
        {

        }

        public IActionResult Index()
        {
            DotNetCoreContext context = HttpContext.RequestServices.GetService(typeof(DotNetCoreContext)) as DotNetCoreContext;
            var allCities = context.GetAllCities();
            return View(allCities);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DotNetCoreContext context = HttpContext.RequestServices.GetService(typeof(DotNetCoreContext)) as DotNetCoreContext;
            var city = context.GetCity(id.Value);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DotNetCoreContext context = HttpContext.RequestServices.GetService(typeof(DotNetCoreContext)) as DotNetCoreContext;
            var city = context.GetCity(id.Value);
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CityInfo cityInfo)
        {
            if (cityInfo == null || cityInfo.ID <= 0)
            {
                return NotFound();
            }

            try
            {
                DotNetCoreContext context = HttpContext.RequestServices.GetService(typeof(DotNetCoreContext)) as DotNetCoreContext;
                context.UpdateCity(cityInfo);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
            }

            return View(cityInfo);
        }

        [HttpPost()]
        public IActionResult EditCity([FromBody] CityInfo cityInfo)
        {
            bool isSuccess = false;
            string message = "An Error Has Occured";
            DotNetCoreContext context = HttpContext.RequestServices.GetService(typeof(DotNetCoreContext)) as DotNetCoreContext;
            isSuccess = context.UpdateCity(cityInfo) > 0;
            if (isSuccess)
            {
                message = "Updated City information successfully.";
            }
            return Json(new { IsSuccess = isSuccess, Message = message });
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
