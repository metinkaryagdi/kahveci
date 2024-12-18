using System.ComponentModel.DataAnnotations;

namespace Kahveci.Models
{
    public class Employee
    {
        [Key]

        public int EmployeeId { get; set; }
        public String? EmployeeName { get; set; } = string.Empty;
        public String? EmployeeSurame { get; set; } = string.Empty;
        public String? EmployeeEmail { get; set; } = string.Empty;
        public String? EmployeeAdress { get; set; } = string.Empty;
        public String? EmployeePhone { get; set; } = string.Empty;
        public String? Password { get; set; } = string.Empty;
    }
}