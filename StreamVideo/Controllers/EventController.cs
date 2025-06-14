using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StreamVideo.models;
using System.Text.Json;

namespace StreamVideo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly string jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "events.json");

        [EnableCors("AllowAll")]
        [HttpGet]
        public IActionResult GetEvents()
        {
            if (!System.IO.File.Exists(jsonFile))
                return Ok(new List<EventItem>());
            var json = System.IO.File.ReadAllText(jsonFile);
            var events = JsonSerializer.Deserialize<List<EventItem>>(json) ?? new List<EventItem>();
            return Ok(events);
        }

        [EnableCors("AllowAll")]
        [HttpPost]
        public IActionResult AddEvent([FromBody] EventItem newEvent)
        {
            List<EventItem> events = new();
            if (System.IO.File.Exists(jsonFile))
            {
                var json = System.IO.File.ReadAllText(jsonFile);
                events = JsonSerializer.Deserialize<List<EventItem>>(json) ?? new List<EventItem>();
            }
            // Đặt tất cả top = false
            foreach (var ev in events)
                ev.Top = false;
            // Đặt top cho sự kiện mới
            newEvent.Top = true;
            events.Add(newEvent);
            var updatedJson = JsonSerializer.Serialize(events, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(jsonFile, updatedJson);
            return Ok(newEvent);
        }
    }
} 