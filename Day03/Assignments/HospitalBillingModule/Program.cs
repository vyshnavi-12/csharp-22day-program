using System;

namespace HospitalBillingModule
{
    class Bill
    {
        public string PatientName;
        public int Age;
        public decimal BaseAmount;
        public decimal Discount;
        public decimal Tax;
        public decimal NetAmount;
    }

    class BillingSystem
    {
        static void Main(string[] args)
        {
            const decimal CONSULTATION_FEE = 500m;
            const decimal BLOOD_TEST_FEE = 200m;
            const decimal XRAY_FEE = 1000m;
            const decimal ADMISSION_FEE = 2000m;

            Bill bill = new Bill();

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("       HOSPITAL BILLING CALCULATOR");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();

            // Patient Name
            while (true)
            {
                Console.Write("Patient Name: ");
                bill.PatientName = Console.ReadLine();

                if (bill.PatientName != "")
                {
                    break;
                }

                Console.WriteLine("Name cannot be empty.");
            }

            // Age
            while (true)
            {
                try
                {
                    Console.Write("Patient Age: ");
                    bill.Age = Convert.ToInt32(Console.ReadLine());

                    if (bill.Age > 0)
                    {
                        break;
                    }

                    Console.WriteLine("Age must be greater than 0.");
                }
                catch
                {
                    Console.WriteLine("Please enter a valid age.");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Add Services:");

            int choice;

            while (true)
            {
                Console.WriteLine("1. Consultation (500)");
                Console.WriteLine("2. Blood Test (200)");
                Console.WriteLine("3. X-Ray (1000)");
                Console.WriteLine("4. Admission (2000)");
                Console.WriteLine("5. Done");

                try
                {
                    Console.Write("Choice: ");
                    choice = Convert.ToInt32(Console.ReadLine());

                    if (choice == 1)
                    {
                        bill.BaseAmount += CONSULTATION_FEE;
                        Console.WriteLine("[Added Consultation]");
                    }
                    else if (choice == 2)
                    {
                        bill.BaseAmount += BLOOD_TEST_FEE;
                        Console.WriteLine("[Added Blood Test]");
                    }
                    else if (choice == 3)
                    {
                        bill.BaseAmount += XRAY_FEE;
                        Console.WriteLine("[Added X-Ray]");
                    }
                    else if (choice == 4)
                    {
                        bill.BaseAmount += ADMISSION_FEE;
                        Console.WriteLine("[Added Admission]");
                    }
                    else if (choice == 5)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Choice.");
                    }
                }
                catch
                {
                    Console.WriteLine("Please enter a valid number.");
                }

                Console.WriteLine();
            }

            // Discount Calculation

            if (bill.Age > 60)
            {
                bill.Discount = bill.BaseAmount * 20 / 100;
            }
            else if (bill.Age < 10)
            {
                bill.Discount = CONSULTATION_FEE * 50 / 100;

                if (bill.Discount > bill.BaseAmount)
                {
                    bill.Discount = bill.BaseAmount;
                }
            }
            else
            {
                bill.Discount = 0;
            }

            decimal amountAfterDiscount = bill.BaseAmount - bill.Discount;

            bill.Tax = amountAfterDiscount * 5 / 100;

            bill.NetAmount = amountAfterDiscount + bill.Tax;

            Console.WriteLine();
            Console.WriteLine("[Calculating Bill...]");
            Console.WriteLine();

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("              FINAL BILL INVOICE");
            Console.WriteLine("--------------------------------------------------");

            if (bill.Age > 60)
            {
                Console.WriteLine("Patient: " + bill.PatientName + " (Senior Citizen)");
            }
            else if (bill.Age < 10)
            {
                Console.WriteLine("Patient: " + bill.PatientName + " (Child)");
            }
            else
            {
                Console.WriteLine("Patient: " + bill.PatientName);
            }

            Console.WriteLine();

            Console.WriteLine("Base Amount:      " + bill.BaseAmount.ToString("F2"));
            Console.WriteLine("Discount:        -" + bill.Discount.ToString("F2"));
            Console.WriteLine("Tax (5%):        +" + bill.Tax.ToString("F2"));

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("TOTAL PAYABLE:    " + bill.NetAmount.ToString("F2"));
            Console.WriteLine("--------------------------------------------------");

            Console.ReadKey();
        }
    }
}