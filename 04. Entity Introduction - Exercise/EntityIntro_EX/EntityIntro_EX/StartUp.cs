using System;
using System.Linq;
using System.Text;
using SoftUni.Data;
using SoftUni.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new SoftUniContext();

            using (db)
            {
                Console.WriteLine(DeleteProjectById(db));
            }
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Select(x => new
                {
                    x.EmployeeId,
                    x.FirstName,
                    x.LastName,
                    x.MiddleName,
                    x.JobTitle,
                    x.Salary
                }).OrderBy(x => x.EmployeeId)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .Select(x => new
                {

                    x.FirstName,
                    x.Salary

                })
             .OrderBy(x => x.FirstName)
             .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();
            var employees = context.Employees.Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    DepartmentName = x.Department.Name,
                    x.Salary

                }).OrderBy(x => x.Salary)
              .ThenByDescending(x => x.FirstName)
              .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employee = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
            employee.Address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.SaveChanges();

            var employees = context.Employees.Select(x => new
            {
                x.AddressId,
                AddressText = x.Address.AddressText
            })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToList();

            foreach (var empl in employees)
            {
                sb.AppendLine(empl.AddressText);

            }

            return sb.ToString().TrimEnd();

        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {

            var sb = new StringBuilder();

            var employees = context.Employees.Include(x => x.EmployeesProjects)
                 .ThenInclude(x => x.Project)
                 .Where(x => x.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                 .Select(x => new
                 {
                     x.FirstName,
                     x.LastName,
                     ManagerFirstName = x.Manager.FirstName,
                     ManagerLastName = x.Manager.LastName,
                     Projects = x.EmployeesProjects.Select(p => new
                     {
                         ProjectName = p.Project.Name,
                         ProjectStartDate = p.Project.StartDate,
                         ProjectEndDate = p.Project.EndDate
                     })
                 })
                 .Take(10)
                 .ToList();


            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var empl in employee.Projects)
                {
                    var endDate = empl.ProjectEndDate.HasValue ? empl.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished";

                    sb.AppendLine($"--{empl.ProjectName} - {empl.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();

        }


        public static string GetAddressesByTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var adresses = context.Addresses
                .OrderByDescending(x => x.Employees.Count)
                .ThenBy(t => t.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(ad => new
                {
                    ad.AddressText,
                    TownName = ad.Town.Name,
                    EmployeeCount = ad.Employees.Count()
                })
                .Take(10)
                .ToList();


            foreach (var adress in adresses)
            {
                sb.AppendLine($"{adress.AddressText}, {adress.TownName} - {adress.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();

        }


        public static string GetEmployee147(SoftUniContext context)
        {
            var sb = new StringBuilder();

            //var employee = context.Employees.FirstOrDefault(x => x.EmployeeId == 147);

            //var employeeProjects = employee.EmployeesProjects.Select(x => new
            //{
            //    ProjectName = x.Project.Name
            //})
            // .OrderBy(p => p.ProjectName)
            // .ToList();

            //sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            //foreach (var emplProject in employeeProjects)
            //{
            //    sb.AppendLine($"{emplProject.ProjectName}");
            //}

            //return sb.ToString().TrimEnd();

            //----------------------


            var employeeTwo = context.Employees
                .Select(p => new Employee
                {
                    EmployeeId = p.EmployeeId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    JobTitle = p.JobTitle,
                    EmployeesProjects = p.EmployeesProjects.Select(ep => new EmployeeProject
                    {
                        Project = ep.Project
                    })
                    .OrderBy(x => x.Project.Name)
                    .ToList()
                })
                .FirstOrDefault(x => x.EmployeeId == 147);

            sb.AppendLine($"{employeeTwo.FirstName} {employeeTwo.LastName} - {employeeTwo.JobTitle}");

            foreach (var emplProject in employeeTwo.EmployeesProjects)
            {
                sb.AppendLine($"{emplProject.Project.Name}");
            }
            return sb.ToString().TrimEnd();
        }


        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var departments = context.Departments.Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(y => y.Name)
                .Select(x => new
                {
                    x.Name,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Employees = x.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(x => x.FirstName)
                    .ThenBy(y => y.LastName)
                    .ToList()
                })
                .ToList();


            foreach (var department in departments)
            {
                sb.AppendLine($"{department.Name} - {department.ManagerFirstName}  {department.ManagerLastName}");

                foreach (var employeeInDep in department.Employees)
                {
                    sb.AppendLine($"{employeeInDep.FirstName} {employeeInDep.LastName} - {employeeInDep.JobTitle}.");
                }
            }

            return sb.ToString().TrimEnd();


        }


        public static string GetLatestProjects(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects.OrderByDescending(x => x.StartDate)
                .Take(10)
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    StartDate = x.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .OrderBy(x => x.Name)
                .ToList();

            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate}");

            }
            return sb.ToString().TrimEnd();
        }


        public static string IncreaseSalaries(SoftUniContext context)
        {
            var sb = new StringBuilder();
            var employees = context.Employees.Where(x => x.Department.Name == "Engineering" || x.Department.Name == "Tool Design" || x.Department.Name == "Marketing"
            || x.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var empl in employees)
            {
                empl.Salary += empl.Salary * 0.12M;
            }

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();


        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees.Where(x => x.FirstName.StartsWith("Sa") || x.FirstName.StartsWith("sa"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();



            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();


        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projectToDelete = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            var employeeProjectsToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2)
                .ToList();

            foreach (var employeeProject in employeeProjectsToDelete)
            {
                context.EmployeesProjects.Remove(employeeProject);
            }

            context.Projects.Remove(projectToDelete);

            context.SaveChanges();

            var projects = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToList();

            foreach (var project in projects)
            {
                sb.AppendLine($"{project}");
            }

            return sb.ToString().Trim();
        }

        public static string RemoveTown(SoftUniContext context)
        {

            var town = context.Towns.FirstOrDefault(x => x.Name == "Seattle");
            var removeAdress = context.Addresses.Where(x => x.TownId == town.TownId);
            var count = removeAdress.Count();
            var employeesAddresIdToRemove = context.Employees.Where(x => removeAdress.Any(a => a.AddressId == x.AddressId)).ToList();

            foreach (var employee in employeesAddresIdToRemove)
            {
                employee.AddressId = null;
            }

            foreach (var ra in removeAdress)
            {
                context.Addresses.Remove(ra);

            }

            context.Towns.Remove(town);
            context.SaveChanges();
            return $"{count} addresses in Seattle were deleted";
        }
    }
}
