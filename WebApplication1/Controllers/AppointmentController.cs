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
            Appointment appointment = new Appointment()
            {
                Name = personName,
                Phone = phoneNumber,
                PetType = petType,
                preferredDateTime = dateAndTime,
                Reason = AReason,
                Email = User.Identity.Name

            };
            AppointmentRepository repository = new AppointmentRepository();
            repository.Add(appointment);
            return RedirectToAction("MyAppointments", "Appointment");
        }
        public IActionResult MyAppointments()
        {
            List<Appointment> list = new List<Appointment>();
            AppointmentRepository appointmentRepository = new AppointmentRepository();
            list = appointmentRepository.GetByEmail(User.Identity.Name);
            return View(list);
        }
        public IActionResult Delete(int id)
        {
            AppointmentRepository repository=new AppointmentRepository();
            repository.DeleteById(id);
            return RedirectToAction("ShowAll", "AdminPanel");
        }
    }
}
        