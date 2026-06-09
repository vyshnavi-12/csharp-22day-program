namespace CareBridge.EFCoreDemo.Models
{
    public class DepartmentLoadDto
    {
        public string DepartmentName { get; set; } = string.Empty;

        public int Inpatient { get; set; }

        public int Outpatient { get; set; }

        public int Ed { get; set; }

        public int Total { get; set; }
    }
}