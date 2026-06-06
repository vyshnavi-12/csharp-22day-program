//using Microsoft.Data.SqlClient;

//// Connection string to SQL Server

//string connectionString =
//    "Server=localhost;" +
//    "Database=CareBridgeDB;" +
//    "Trusted_Connection=True;" +
//    "TrustServerCertificate=True;";

//// Stored procedure we already created in SQL Server

//string procedure = "usp_ReadmissionAnalytics";

//using SqlConnection conn =
//    new SqlConnection(connectionString);

//using SqlCommand cmd =
//    new SqlCommand(procedure, conn);

//// Tell C# this is a stored procedure

//cmd.CommandType =
//    System.Data.CommandType.StoredProcedure;

//// Parameter value

//cmd.Parameters.AddWithValue(
//    "@WithinDays",
//    30
//);

//conn.Open();

//using SqlDataReader reader =
//    cmd.ExecuteReader();

//Console.WriteLine(
//    "30-Day Readmission Report"
//);

//Console.WriteLine(
//    "--------------------------"
//);

//// Read each returned row

//while (reader.Read())
//{
//    Console.WriteLine(
//        $"Patient: {reader["PatientId"]} | " +
//        $"Encounter: {reader["EncounterId"]} | " +
//        $"Days Since Previous Visit: " +
//        $"{reader["DaysSincePreviousVisit"]}"
//    );
//}

using Microsoft.Data.SqlClient;

string connectionString =
    "Server=localhost;" +
    "Database=CareBridgeDB;" +
    "Trusted_Connection=True;" +
    "TrustServerCertificate=True;";

string query =
@"SELECT TOP 10
    AgeBand,
    Gender,
    EncounterType
FROM vw_Analytics_DeId";

using SqlConnection conn =
    new SqlConnection(connectionString);

using SqlCommand cmd =
    new SqlCommand(query, conn);

conn.Open();

using SqlDataReader reader =
    cmd.ExecuteReader();

Console.WriteLine("CareBridge Analytics");
Console.WriteLine("-------------------");

while (reader.Read())
{
    Console.WriteLine(
        $"{reader["AgeBand"]} | " +
        $"{reader["Gender"]} | " +
        $"{reader["EncounterType"]}"
    );
}
