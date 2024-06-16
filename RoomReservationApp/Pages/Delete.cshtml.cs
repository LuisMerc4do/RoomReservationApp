using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using RoomReservationApp.Models;
using System.Globalization;

namespace RoomReservationApp.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public RoomReservationModel RoomReservation { get; set; }

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            RoomReservation = GetById(id);
            if (RoomReservation == null)
            {
                return NotFound();
            }
            return Page();
        }

        private RoomReservationModel GetById(int id)
        {
            RoomReservationModel roomReservationRecord = null;
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM room_reservation WHERE Id = @Id";
                tableCmd.Parameters.AddWithValue("@Id", id);

                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        roomReservationRecord = new RoomReservationModel()
                        {
                            Id = reader.GetInt32(0),
                            RoomNumber = reader.GetInt32(1),
                            ReservedBy = reader.GetString(2),
                            StartTime = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            EndTime = DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Description = reader.GetString(5)
                        };
                    }
                }
            }
            return roomReservationRecord;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM room_reservation WHERE Id = @Id";
                tableCmd.Parameters.AddWithValue("@Id", RoomReservation.Id);

                int rowsAffected = tableCmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
