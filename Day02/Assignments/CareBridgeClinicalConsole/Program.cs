using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CareBridgeClinicalConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                "Server=localhost;Database=CareBridgeDB;Trusted_Connection=True;TrustServerCertificate=True;";

            while (true)
            {
                Console.WriteLine("\n===== CAREBRIDGE CLINICAL OPERATIONS =====");
                Console.WriteLine("1. 30-Day Readmissions");
                Console.WriteLine("2. High-Risk Patients");
                Console.WriteLine("3. Provider Workload");
                Console.WriteLine("4. Revenue Analysis");
                Console.WriteLine("5. Exit");

                Console.Write("Enter Choice: ");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        ExecuteProcedure(connectionString, "usp_ThirtyDayReadmissions");
                        break;

                    case 2:
                        ExecuteProcedure(connectionString, "usp_HighRiskPatients");
                        break;

                    case 3:
                        ExecuteProcedure(connectionString, "usp_ProviderWorkload");
                        break;

                    case 4:
                        ExecuteProcedure(connectionString, "usp_RevenueAnalysis");
                        break;

                    case 5:
                        return;

                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }
            }
        }

        static void ExecuteProcedure(string connectionString, string procedureName)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

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
        }
    }
}