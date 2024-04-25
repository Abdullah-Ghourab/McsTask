using McsTask.Core.Enums;
using McsTask.Models.Entities;

namespace McsTask.Data
{
    public class DataProvider
    {
        public  List<Employee> employees = new List<Employee>();
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DataProvider(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            PopulateEmployeesFromFile("Employees.txt");
        }
        private void PopulateEmployeesFromFile(string filePath)
        {
            filePath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);


            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split("\t");
                        if (data.Length >= 5)
                        {
                            Employee employee = new Employee
                            {
                                Id = employees.Count + 1,
                                Name = data[0].Trim(),
                                Address = data[1].Trim(),
                                BirthDate = DateOnly.Parse(data[2].Trim()),
                                Graduation = data[3].Trim(),
                                EmploymentType = data[4].Trim().ToLower() switch
                                {
                                    "hourly payroll" => EmploymentType.Hourly,
                                    "free lancer" => EmploymentType.Freelancer,
                                    "monthly payroll" => EmploymentType.Monthly,
                                    _ => EmploymentType.Monthly // Default value for invalid employment type string
                                }
                            };

                            if (data.Length > 5 && employee.EmploymentType != EmploymentType.Freelancer)
                            {
                                employee.Assurance = data[5].Trim();
                            }

                            employees.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
        }


    }
}
