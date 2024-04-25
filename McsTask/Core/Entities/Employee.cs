using McsTask.Core.Enums;

namespace McsTask.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Graduation { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public string Assurance { get; set; }
        public decimal Salary { get; set; }

    }
   

}
