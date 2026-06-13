using System;
using System.Collections.Generic;

namespace HospitalReportModule
{
    class PatientRecord
    {
        public string Name;
        public string Department;
        public decimal BillAmount;
        public string Status;
    }

    class HospitalReport
    {
        static void Main(string[] args)
        {
            List<PatientRecord> patients = new List<PatientRecord>();

            patients.Add(new PatientRecord
            {
                Name = "John Doe",
                Department = "General",
                BillAmount = 500,
                Status = "Discharged"
            });

            patients.Add(new PatientRecord
            {
                Name = "Jane Smith",
                Department = "Dental",
                BillAmount = 1200,
                Status = "Admitted"
            });

            patients.Add(new PatientRecord
            {
                Name = "Bob Brown",
                Department = "General",
                BillAmount = 400,
                Status = "Discharged"
            });

            patients.Add(new PatientRecord
            {
                Name = "Alice Wilson",
                Department = "Ortho",
                BillAmount = 2500,
                Status = "Admitted"
            });

            patients.Add(new PatientRecord
            {
                Name = "Sam Kumar",
                Department = "Dental",
                BillAmount = 800,
                Status = "Discharged"
            });

            decimal totalRevenue = 0;

            int generalCount = 0;
            int dentalCount = 0;
            int orthoCount = 0;

            foreach (PatientRecord patient in patients)
            {
                totalRevenue += patient.BillAmount;

                if (patient.Department == "General")
                {
                    generalCount++;
                }
                else if (patient.Department == "Dental")
                {
                    dentalCount++;
                }
                else if (patient.Department == "Ortho")
                {
                    orthoCount++;
                }
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("      DAILY HOSPITAL ACTIVITY REPORT");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine("Date: " + DateTime.Now.ToShortDateString());

            Console.WriteLine();
            Console.WriteLine("Patient List:");
            Console.WriteLine();

            int serialNumber = 1;

            foreach (PatientRecord patient in patients)
            {
                Console.WriteLine(
                    serialNumber + ". " +
                    patient.Name + " - " +
                    patient.Department + " - Rs." +
                    patient.BillAmount);

                serialNumber++;
            }

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("SUMMARY STATISTICS");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine("Total Patients Visited: " + patients.Count);
            Console.WriteLine("Total Revenue: Rs." + totalRevenue);

            Console.WriteLine();
            Console.WriteLine("Traffic by Department:");

            Console.WriteLine("- General: " + generalCount);
            Console.WriteLine("- Dental : " + dentalCount);
            Console.WriteLine("- Ortho  : " + orthoCount);

            Console.WriteLine();
            Console.WriteLine("End of Report.");
            Console.WriteLine("--------------------------------------------------");

            Console.ReadKey();
        }
    }
}