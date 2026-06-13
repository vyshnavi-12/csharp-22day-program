using System;

namespace PatientRegistrationSystem
{
    class Patient
    {
        public string PatientID;
        public string Name;
        public int Age;
        public string Gender;
        public string PhoneNumber;
        public string City;
    }

    class RegistrationSystem
    {
        static void Main(string[] args)
        {
            Patient patient = new Patient();

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("       HOSPITAL PATIENT REGISTRATION SYSTEM");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();

            patient.PatientID = "PAT-2026-001";

            while (true)
            {
                Console.Write("Enter Patient Name: ");
                patient.Name = Console.ReadLine();

                if (patient.Name != "")
                {
                    break;
                }

                Console.WriteLine("Error: Name cannot be empty.");
            }

            while (true)
            {
                try
                {
                    Console.Write("Enter Age: ");
                    patient.Age = Convert.ToInt32(Console.ReadLine());

                    if (patient.Age > 0 && patient.Age < 120)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Age must be between 1 and 119.");
                    }
                }
                catch
                {
                    Console.WriteLine("Error: Please enter a valid numeric age.");
                }
            }

            Console.Write("Enter Gender (Male/Female/Other): ");
            patient.Gender = Console.ReadLine();

            while (true)
            {
                Console.Write("Enter Phone Number: ");
                patient.PhoneNumber = Console.ReadLine();

                if (patient.PhoneNumber.Length == 10)
                {
                    bool valid = true;

                    for (int i = 0; i < patient.PhoneNumber.Length; i++)
                    {
                        if (!Char.IsDigit(patient.PhoneNumber[i]))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        break;
                    }
                }

                Console.WriteLine("Error: Phone number must contain exactly 10 digits and only numerical values.");
            }

            Console.Write("Enter City: ");
            patient.City = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("[Registration Complete]");
            Console.WriteLine();

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("            PATIENT REGISTRATION SLIP");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine("Date: " + DateTime.Now.ToShortDateString());
            Console.WriteLine();

            Console.WriteLine("Patient ID : " + patient.PatientID);
            Console.WriteLine("Name       : " + patient.Name);
            Console.WriteLine("Age        : " + patient.Age + " years");
            Console.WriteLine("Gender     : " + patient.Gender);
            Console.WriteLine("Contact    : " + patient.PhoneNumber);
            Console.WriteLine("Location   : " + patient.City);

            Console.WriteLine();
            Console.WriteLine("Instructions:");
            Console.WriteLine("Please proceed to the waiting area.");
            Console.WriteLine("--------------------------------------------------");

            Console.ReadKey();
        }
    }
}