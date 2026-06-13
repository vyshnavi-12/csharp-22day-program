using System;

namespace VitalSignsMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            string patientName;
            double temperature;
            int oxygenLevel;
            int pulseRate;

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("            VITAL SIGNS MONITOR");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();

            // Patient Name
            while (true)
            {
                Console.Write("Enter Patient Name: ");
                patientName = Console.ReadLine();

                if (patientName != "")
                {
                    break;
                }

                Console.WriteLine("Error: Name cannot be empty.");
            }

            // Temperature
            while (true)
            {
                try
                {
                    Console.Write("Enter Temperature (C): ");
                    temperature = Convert.ToDouble(Console.ReadLine());

                    if (temperature > 0 && temperature < 50)
                    {
                        break;
                    }

                    Console.WriteLine("Invalid temperature.");
                }
                catch
                {
                    Console.WriteLine("Please enter a valid temperature.");
                }
            }

            // Oxygen Level
            while (true)
            {
                try
                {
                    Console.Write("Enter Oxygen Level (%): ");
                    oxygenLevel = Convert.ToInt32(Console.ReadLine());

                    if (oxygenLevel >= 0 && oxygenLevel <= 100)
                    {
                        break;
                    }

                    Console.WriteLine("Oxygen level must be between 0 and 100.");
                }
                catch
                {
                    Console.WriteLine("Please enter a valid oxygen level.");
                }
            }

            // Pulse Rate
            while (true)
            {
                try
                {
                    Console.Write("Enter Pulse Rate (BPM): ");
                    pulseRate = Convert.ToInt32(Console.ReadLine());

                    if (pulseRate > 0 && pulseRate < 250)
                    {
                        break;
                    }

                    Console.WriteLine("Invalid pulse rate.");
                }
                catch
                {
                    Console.WriteLine("Please enter a valid pulse rate.");
                }
            }

            Console.WriteLine();
            Console.WriteLine("[Analyzing Data...]");
            Console.WriteLine();

            string status = CheckStatus(temperature, oxygenLevel, pulseRate);

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("         MEDICAL ASSESSMENT REPORT");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine("Patient: " + patientName);
            Console.WriteLine();

            Console.WriteLine("Vitals Recorded:");
            Console.WriteLine("- Temp:   " + temperature + " C");
            Console.WriteLine("- Oxygen: " + oxygenLevel + " %");
            Console.WriteLine("- Pulse:  " + pulseRate + " BPM");
            Console.WriteLine();

            Console.WriteLine("Status Assessment: " + status);
            Console.WriteLine();

            if (status == "CRITICAL / EMERGENCY")
            {
                Console.WriteLine("Action: Immediate medical attention required.");
            }
            else if (status == "OBSERVATION NEEDED")
            {
                Console.WriteLine("Action: Nurse to monitor every hour.");
            }
            else
            {
                Console.WriteLine("Action: Patient is stable.");
            }

            Console.WriteLine("--------------------------------------------------");

            Console.ReadKey();
        }

        static string CheckStatus(double temp, int oxygen, int pulse)
        {
            if (temp > 39.0 || oxygen < 90 || pulse < 50 || pulse > 120)
            {
                return "CRITICAL / EMERGENCY";
            }
            else if (temp > 37.5 || oxygen < 95 || pulse > 100)
            {
                return "OBSERVATION NEEDED";
            }
            else
            {
                return "NORMAL";
            }
        }
    }
}