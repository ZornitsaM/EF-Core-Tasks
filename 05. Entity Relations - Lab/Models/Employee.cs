

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityRelations_Lab.Models
{
    public class Employee
    {
        [Key]
        public int EID { get; set; }

        public int Egn { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public DateTime? StartWorkDate { get; set; }

        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("Employee")]
        public int AdressId { get; set; }
        public Address Address { get; set; }
    }
}
