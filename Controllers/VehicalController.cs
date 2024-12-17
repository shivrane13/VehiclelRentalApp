using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using VehiclelRentalApp.Areas.Identity.Data;
using VehiclelRentalApp.Models;

namespace VehiclelRentalApp.Controllers
{
    public class VehicalController : Controller
    {
        private readonly ApplicationDbContext _context;
        public VehicalController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var vehicals = await _context.Vehicles.ToListAsync();
            return View(vehicals);
        }
        [Authorize]
        public async Task<IActionResult> Detail(int id)
        {
            var vehical = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
            var vehicalForBooking = new VehicalBookingViewModel
            {
                Id = vehical.Id,
                Make = vehical.Make,
                Model = vehical.Model,
                RegistrationNumber = vehical.RegistrationNumber,
                Capacity = vehical.Capacity,
                FuelType = vehical.FuelType,
                DailyRate = vehical.DailyRate,
                ImageUrl  = vehical.ImageUrl,
                HourlyRate = vehical.HourlyRate,
            };
            return View(vehicalForBooking);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Vehicle vehical)
        {
            if (ModelState.IsValid)
            {
                if(vehical.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(vehical.ImageFile.FileName);
                    string extension = Path.GetExtension(vehical.ImageFile.FileName);
                    string uniqueFileName = fileName + "_" + Guid.NewGuid().ToString() + "_" + extension;

                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await vehical.ImageFile.CopyToAsync(fileStream);
                    }
                    vehical.ImageUrl = "/images/" + uniqueFileName;

                }
                await _context.AddAsync(vehical);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vehical);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var vehical = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
            return View(vehical);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Vehicle vehical) {
            if (ModelState.IsValid)
            {
                _context.Vehicles.Update(vehical);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vehical);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var vehical = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
            if(vehical != null)
            {
                _context.Remove(vehical);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
