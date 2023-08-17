using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // ... Other actions and methods ...

        private decimal CalculateQuote(Insuree insuree)
        {
            decimal baseQuote = 50;
            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (DateTime.Now.DayOfYear < insuree.DateOfBirth.DayOfYear)
            {
                age--;
            }

            if (age <= 18)
            {
                baseQuote += 100;
            }
            else if (age >= 19 && age <= 25)
            {
                baseQuote += 50;
            }
            else
            {
                baseQuote += 25;
            }

            if (insuree.CarYear < 2000 || insuree.CarYear > 2015)
            {
                baseQuote += 25;
            }

            if (insuree.CarMake == "Porsche")
            {
                baseQuote += 25;
                if (insuree.CarModel == "911 Carrera")
                {
                    baseQuote += 25;
                }
            }

            baseQuote += insuree.SpeedingTickets * 10;

            if (insuree.DUI)
            {
                baseQuote *= 1.25m;
            }

            if (insuree.CoverageType)
            {
                baseQuote *= 1.5m;
            }

            return baseQuote;
        }

        public ActionResult Admin()
        {
            var insurees = db.Insurees.ToList();
            return View(insurees);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                insuree.Quote = CalculateQuote(insuree);
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // ... Other actions and methods ...

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
