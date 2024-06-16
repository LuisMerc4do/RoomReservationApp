using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using RoomReservationApp.Models;
using System.Data;
using System.Globalization;

namespace RoomReservationApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<RoomReservationModel> Records { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Records = GetAllRecords();
            ViewData["Total"] = Records.AsEnumerable().Sum(x => x.RoomNumber);
        }
        private List<RoomReservationModel> GetAllRecords()
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                        $"SELECT * FROM room_reservation";
                var tableData = new List<RoomReservationModel>();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    tableData.Add(new RoomReservationModel()
                    {
                        Id = reader.GetInt32(0),
                        RoomNumber = reader.GetInt32(1),
                        ReservedBy = reader.GetString(2),
                        StartTime = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        EndTime = DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Description = reader.GetString(5)

                    }); ;
                }
                return tableData;
            }

        }
    }
}
