using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using RoomReservationApp.Models;
using System.Globalization;

namespace RoomReservationApp.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public RoomReservationModel RoomReservation { get; set; }

        public UpdateModel(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        public IActionResult OnGet(int id)
        {
            RoomReservation = GetById(id);
            return Page();
        }
        private RoomReservationModel GetById(int id)
        {
            var roomReservationRecord = new RoomReservationModel();
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM room_reservation WHERE Id = {id}";

                SqliteDataReader reader = tableCmd.ExecuteReader();
                while (reader.Read())
                {

                    roomReservationRecord.Id = reader.GetInt32(0);
                    roomReservationRecord.RoomNumber = reader.GetInt32(1);
                    roomReservationRecord.ReservedBy = reader.GetString(2);
                    roomReservationRecord.StartTime = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    roomReservationRecord.EndTime = DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    roomReservationRecord.Description = reader.GetString(5);
                }
            }
            return roomReservationRecord;
        }
        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE room_reservation SET RoomNumber = @RoomNumber, ReservedBy = @ReservedBy, StartTime = @StartTime, EndTime = @EndTime, Description = @Description WHERE Id = @Id";

                tableCmd.Parameters.AddWithValue("@RoomNumber", RoomReservation.RoomNumber);
                tableCmd.Parameters.AddWithValue("@ReservedBy", RoomReservation.ReservedBy);
                tableCmd.Parameters.AddWithValue("@StartTime", RoomReservation.StartTime.ToString("yyyy-MM-dd HH:mm"));
                tableCmd.Parameters.AddWithValue("@EndTime", RoomReservation.EndTime.ToString("yyyy-MM-dd HH:mm"));
                tableCmd.Parameters.AddWithValue("@Description", RoomReservation.Description);
                tableCmd.Parameters.AddWithValue("@Id", id);

                tableCmd.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }
    }
}
