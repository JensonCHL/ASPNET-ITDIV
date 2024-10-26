using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace ASP2.Pages.Customer
{
    public class indexModel : PageModel
    {
        public List<CarInfo> CarList { get; set; } = [];
        public void OnGet()
        {
            try
            {
                string connectionString = "Server=JOE;Database = rentcars; Trusted_connection=True;TrustServerCertificate=True; ";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT top 10 * FROM MsCar ORDER BY year DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CarInfo carInfo = new CarInfo();
                                carInfo.Car_id = reader.GetString(reader.GetOrdinal("Car_id"));
                                carInfo.name = reader.GetString(reader.GetOrdinal("name"));
                                carInfo.model = reader.GetString(reader.GetOrdinal("model"));
                                carInfo.year = reader.GetInt32(reader.GetOrdinal("year"));
                                carInfo.license_plate = reader.GetString(reader.GetOrdinal("license_plate"));
                                carInfo.number_of_car_seats = reader.GetInt32(reader.GetOrdinal("number_of_car_seats"));
                                carInfo.transmission = reader.GetString(reader.GetOrdinal("transmission"));
                                carInfo.price_per_day = reader.GetDecimal(reader.GetOrdinal("price_per_day"));
                                carInfo.status = reader.GetBoolean(reader.GetOrdinal("status"));

                                // Add the carInfo object to the CarList
                                CarList.Add(carInfo);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("We have and error " + ex.Message);
            }
        }
    }

    public class CarInfo
    {
        public string Car_id { get; set; }
        public string name { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public string license_plate { get; set; }
        public int number_of_car_seats { get; set; }
        public string transmission { get; set; }
        public decimal price_per_day { get; set; }
        public bool status { get; set; }
    }

}
