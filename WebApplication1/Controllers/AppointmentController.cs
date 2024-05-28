using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string personName, string phoneNumber, string petType, DateTime dateAndTime, string AReason)
        {
            Appointment appointment = new Appointment() { 
            Name =personName,
                Phone = phoneNumber,
                PetType = petType,
                preferredDateTime = dateAndTime,
                Reason = AReason
            };
            AppointmentRepository repository = new AppointmentRepository();
            repository.Add(appointment);
            return RedirectToAction("view","Appointment");
        }
        public IActionResult ShowAll()
        {
            AppointmentRepository repo = new AppointmentRepository();
            return View(repo.GetAll());
        }
    }
}
