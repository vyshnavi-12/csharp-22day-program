using System;

namespace AppointmentBookingSystem
{
    class Appointment
    {
        public string PatientName;
        public string Department;
        public string Doctor;
        public string TimeSlot;
    }

    class BookingSystem
    {
        static void Main(string[] args)
        {
            Appointment appointment = new Appointment();

            string[] departments =
            {
                "General Medicine",
                "Dental",
                "Orthopedics"
            };

            string[] generalDoctors =
            {
                "Dr. A. Kumar",
                "Dr. B. Singh"
            };

            string[] dentalDoctors =
            {
                "Dr. C. Roy",
                "Dr. D. Gupta"
            };

            string[] orthopedicDoctors =
            {
                "Dr. E. Sharma",
                "Dr. F. Verma"
            };

            string[] timeSlots =
            {
                "10:00 AM",
                "11:00 AM",
                "12:00 PM"
            };

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("          APPOINTMENT BOOKING SYSTEM");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Enter Patient Name: ");
                appointment.PatientName = Console.ReadLine();

                if (appointment.PatientName != "")
                {
                    break;
                }

                Console.WriteLine("Error: Name cannot be empty.");
            }

            int departChoice = 0;

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Select Department:");
                    Console.WriteLine("1. General Medicine");
                    Console.WriteLine("2. Dental");
                    Console.WriteLine("3. Orthopedics");

                    Console.Write("Enter Choice: ");
                    departChoice = Convert.ToInt32(Console.ReadLine());

                    if (departChoice >= 1 && departChoice <= 3)
                    {
                        break;
                    }

                    Console.WriteLine("Invalid Department Choice.");
                }
                catch
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }

            string[] selectedDoctors = null;

            switch (departChoice)
            {
                case 1:
                    appointment.Department = departments[0];
                    selectedDoctors = generalDoctors;
                    break;

                case 2:
                    appointment.Department = departments[1];
                    selectedDoctors = dentalDoctors;
                    break;

                case 3:
                    appointment.Department = departments[2];
                    selectedDoctors = orthopedicDoctors;
                    break;
            }

            int doctorChoice = 0;

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Select Doctor:");

                    for (int i = 0; i < selectedDoctors.Length; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + selectedDoctors[i]);
                    }

                    Console.Write("Enter Choice: ");
                    doctorChoice = Convert.ToInt32(Console.ReadLine());

                    if (doctorChoice >= 1 && doctorChoice <= selectedDoctors.Length)
                    {
                        appointment.Doctor = selectedDoctors[doctorChoice - 1];
                        break;
                    }

                    Console.WriteLine("Invalid Doctor Choice.");
                }
                catch
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }

            int timeChoice = 0;

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Select Time Slot:");

                    for (int i = 0; i < timeSlots.Length; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + timeSlots[i]);
                    }

                    Console.Write("Enter Choice: ");
                    timeChoice = Convert.ToInt32(Console.ReadLine());

                    if (timeChoice >= 1 && timeChoice <= timeSlots.Length)
                    {
                        appointment.TimeSlot = timeSlots[timeChoice - 1];
                        break;
                    }

                    Console.WriteLine("Invalid Time Slot Choice.");
                }
                catch
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }

            Console.WriteLine();
            Console.WriteLine("[Booking Confirmed]");
            Console.WriteLine();

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("               APPOINTMENT TICKET");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine("Patient:    " + appointment.PatientName);
            Console.WriteLine("Department: " + appointment.Department);
            Console.WriteLine("Doctor:     " + appointment.Doctor);
            Console.WriteLine("Time:       " + appointment.TimeSlot);
            Console.WriteLine("Status:     Confirmed");

            Console.WriteLine();
            Console.WriteLine("Please arrive 15 mins before your slot.");
            Console.WriteLine("--------------------------------------------------");

            Console.ReadKey();
        }
    }
}