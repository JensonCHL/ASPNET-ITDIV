using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace ASP2.Pages.Account
{
    public class RegisterModel : PageModel
    {

        [BindProperty, Required(ErrorMessage = "Email is required")]

        public string FullName { get; set; }

        [BindProperty, Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [BindProperty, Required(ErrorMessage = "Password is required")]

        public string Password { get; set; }

        [BindProperty, Required(ErrorMessage = "Phone is required")]

        public string Phone_number { get; set; }
        [BindProperty, Required(ErrorMessage = "Address is required")]

        public string Address { get; set; }

        public string driver_license_number { get; set; } = "";


        public string ErrorMessage { get; set; } = "";
        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            if (driver_license_number == null) driver_license_number = "";

            try
            {
                string connectionString = "Server=JOE;Database=rentcars;Trusted_Connection=True;TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string Customer_id = GenerateCustomerId(connection);

                    string sql = "INSERT INTO MsCustomer" +
                        "(Customer_id, FullName, Email, Password,Phone_number, Address,driver_license_number)" +
                        "VALUES (@Customer_id, @FullName, @Email, @Password,@Phone, @Address,@driver_license_number)";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Customer_id", Customer_id);
                        command.Parameters.AddWithValue("@FullName", FullName);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Phone_number", Phone_number);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@driver_license_number", driver_license_number);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Show success message or redirect to a success page
                            Response.Redirect("/Account/Login?message=Registration successful");
                        }
                        else
                        {
                            ErrorMessage = "Failed to register. Please try again.";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                throw;
            }

            Response.Redirect("");

        }
        private string GenerateCustomerId(SqlConnection connection)
        {
            string sql = "SELECT COUNT(*) FROM MsCustomer"; // Query to get the current number of rows
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                int rowCount = (int)command.ExecuteScalar(); // Get the row count
                return $"CU{rowCount + 1:000}"; // Format the Customer ID as "CU0001", "CU0002", etc.
            }
        }
    }


    public class CustomerModel
    {
        public string Customer_id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone_number { get; set; }
        public string Address { get; set; }

        public string driver_license_number { get; set; }
    }
}
