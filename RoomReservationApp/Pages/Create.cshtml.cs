using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using RoomReservationApp.Models;
using System.Globalization;

namespace RoomReservationApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [BindProperty]
        public RoomReservationModel RoomReservation { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = @"
                        INSERT INTO room_reservation (RoomNumber, ReservedBy, StartTime, EndTime, Description) 
                        VALUES (@RoomNumber, @ReservedBy, @StartTime, @EndTime, @Description)";
                    tableCmd.Parameters.AddWithValue("@RoomNumber", RoomReservation.RoomNumber);
                    tableCmd.Parameters.AddWithValue("@ReservedBy", RoomReservation.ReservedBy);
                    tableCmd.Parameters.AddWithValue("@StartTime", RoomReservation.StartTime.ToString("yyyy-MM-dd HH:mm"));
                    tableCmd.Parameters.AddWithValue("@EndTime", RoomReservation.EndTime.ToString("yyyy-MM-dd HH:mm"));
                    tableCmd.Parameters.AddWithValue("@Description", RoomReservation.Description);

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex.Message);
                // Optionally, display an error message to the user
                ModelState.AddModelError(string.Empty, "An error occurred while saving the record. Please try again.");
                return Page();
            }
        }
    }
}
