using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehiclelRentalApp.Areas.Identity.Data;
using VehiclelRentalApp.Models;

namespace VehiclelRentalApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager ) { 
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var booking = await _context.RentalBookings.Include(v => v.Vehicle).Where(u => u.userId == userId).ToListAsync();
            return View(booking);
        }
        [Authorize]
        public async Task<IActionResult> CreateBooking(VehicalBookingViewModel bookingData)
        {
            string userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                TimeSpan t = bookingData.EndDate.Date - bookingData.StartDate.Date;
                double NumberOfDays = t.TotalDays;
                decimal totalCost = (decimal)NumberOfDays * bookingData.DailyRate;
                var Booking = new RentalBooking
                {
                    VehicleId = bookingData.Id,
                    userId = userId,
                    StartDate = bookingData.StartDate,
                    EndDate = bookingData.EndDate,
                    TotalCost = totalCost,
                    Status = "UNCONFORM"
                };
                _context.RentalBookings.Add(Booking);
                await _context.SaveChangesAsync();
            }  
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.RentalBookings.FirstOrDefaultAsync(b => b.Id == id);
            _context.RentalBookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin") ]
        public async Task<IActionResult> AllBookingsForAdmin()
        {
            var bookings = await _context.RentalBookings.Include(v => v.Vehicle).ToListAsync();
            return View(bookings);
        }
        public async Task<IActionResult> UpdateStatus(UpdateStatusViewModel model)
        {
            var booking = await _context.RentalBookings.FirstOrDefaultAsync(i => i.Id == model.bookingId);
            booking.Status = model.newStatus;
            _context.RentalBookings.Update(booking);
            await _context.SaveChangesAsync();
            return Ok(booking);
        }
    }
}
