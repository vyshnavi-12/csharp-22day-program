using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace HIPAAAccessPortal
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                "Server=localhost;Database=CareBridgeDB;Trusted_Connection=True;TrustServerCertificate=True;";

            while (true)
            {
                Console.WriteLine("\n===== HIPAA COMPLIANT ACCESS PORTAL =====");
                Console.WriteLine("1. Clinical Team");
                Console.WriteLine("2. Billing Team");
                Console.WriteLine("3. Analytics Team");
                Console.WriteLine("4. Exit");
                Console.Write("Enter Choice: ");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        DisplayView(connectionString, "vw_Clinical");
                        break;

                    case 2:
                        DisplayView(connectionString, "vw_Billing");
                        break;

                    case 3:
                        DisplayView(connectionString, "vw_Analytics_DeId");
                        break;

                    case 4:
                        Console.WriteLine("Exiting Application...");
                        return;

                    default:
                        Console.WriteLine("Invalid Choice!");
                        break;
                }
            }
        }

        static void DisplayView(string connectionString, string viewName)
        {
            SqlConnection connection =
                new SqlConnection(connectionString);

            string query = "SELECT * FROM " + viewName;

            SqlCommand command =
                new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            Console.WriteLine();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(reader.GetName(i) + "\t");
            }

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------");

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i] + "\t");
                }

                Console.WriteLine();
            }

            reader.Close();
            connection.Close();

            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
        }
    }
}