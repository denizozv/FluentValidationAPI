using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OtsApi.Models;

namespace OtsApi.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IValidator<Employee> _validator;
    private static readonly List<Employee> _employees = new();

    public EmployeeController(IValidator<Employee> validator)
    {
        _validator = validator;
    }

    [HttpGet("all")]
    public IActionResult GetAll()
    {
        return Ok(_employees);
    }

    //QUERY
    [HttpGet("query-by-id")]
    public IActionResult GetByIdFQ([FromQuery] int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        return employee is null ? NotFound(new { message = "Employee not found" }) : Ok(employee);
    }

    //ROUTE
    [HttpGet("route/{id:int}")]
    public IActionResult GetByIdFR([FromRoute] int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        return employee is null ? NotFound(new { message = "Employee not found" }) : Ok(employee);
    }


    //QUERY 
    [HttpPost("query")]
    public IActionResult CreateFQ(
    [FromQuery] string name,
    [FromQuery] string surname,
    [FromQuery] string email,
    [FromQuery] string phone,
    [FromQuery] string address,
    [FromQuery] int age,
    [FromQuery] DateTime dateOfBirth,
    [FromServices] IValidator<Employee> validator)
    {
        var newEmployee = new Employee
        {
            Name = name,
            Surname = surname,
            Email = email,
            Phone = phone,
            Address = address,
            Age = age,
            DateOfBirth = dateOfBirth
        };

        
        var validationResult = validator.Validate(newEmployee);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        
        newEmployee.Id = _employees.Count > 0 ? _employees.Max(e => e.Id) + 1 : 1;
        _employees.Add(newEmployee);

        return CreatedAtAction(nameof(GetByIdFR), new { id = newEmployee.Id }, newEmployee);
    }


    //BODY
    [HttpPost("body")]
    public IActionResult CreateFB([FromBody] Employee employee)
    {
        var validationResult = _validator.Validate(employee);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        employee.Id = _employees.Count > 0 ? _employees.Max(e => e.Id) + 1 : 1;
        _employees.Add(employee);

        return CreatedAtAction(nameof(GetByIdFR), new { id = employee.Id }, employee);
    }


    //QUERY 
    [HttpPut("query")]
    public IActionResult UpdateFQ(
        [FromQuery] int id,
        [FromQuery] string name,
        [FromQuery] string surname,
        [FromQuery] string email,
        [FromQuery] string phone,
        [FromQuery] string address,
        [FromQuery] int age,
        [FromQuery] DateTime dateOfBirth)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee is null) return NotFound(new { message = "Employee not found" });

        employee.Name = name;
        employee.Surname = surname;
        employee.Email = email;
        employee.Phone = phone;
        employee.Address = address;
        employee.Age = age;
        employee.DateOfBirth = dateOfBirth;

        return Ok(employee);
    }

    //BODY
    [HttpPut("body")]
    public IActionResult UpdateFB([FromQuery] int id, [FromBody] Employee updatedEmployee)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee is null) return NotFound(new { message = "Employee not found" });

        var validationResult = _validator.Validate(updatedEmployee);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        employee.Name = updatedEmployee.Name;
        employee.Surname = updatedEmployee.Surname;
        employee.Email = updatedEmployee.Email;
        employee.Phone = updatedEmployee.Phone;
        employee.Address = updatedEmployee.Address;
        employee.Age = updatedEmployee.Age;
        employee.DateOfBirth = updatedEmployee.DateOfBirth;

        return Ok(employee);
    }

    //QUERY
    [HttpPatch("query")]
    public IActionResult PartialUpdateFQ(
        [FromQuery] int id,
        [FromQuery] string? name,
        [FromQuery] string? email,
        [FromQuery] string? phone)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee is null) return NotFound(new { message = "Employee not found" });

        if (!string.IsNullOrEmpty(name)) employee.Name = name;
        if (!string.IsNullOrEmpty(email)) employee.Email = email;
        if (!string.IsNullOrEmpty(phone)) employee.Phone = phone;

        return Ok(employee);
    }

    //BODY
    [HttpPatch("body")]
    public IActionResult PartialUpdateFB([FromQuery] int id, [FromBody] Dictionary<string, object> updates)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee is null) return NotFound(new { message = "Employee not found" });

        if (updates.ContainsKey("Name") && updates["Name"] is string name)
            employee.Name = name;

        if (updates.ContainsKey("Email") && updates["Email"] is string email)
            employee.Email = email;

        if (updates.ContainsKey("Phone") && updates["Phone"] is string phone)
            employee.Phone = phone;

        return Ok(employee);
    }

    //BONUS

    [HttpGet("list-filtered")]
    public IActionResult GetAllWithFilters(
    [FromQuery] string? name,
    [FromQuery] string? sortField,
    [FromQuery] string? sortOrder)
    {
        var filteredEmployees = _employees.AsQueryable();

        //İsim
        if (!string.IsNullOrEmpty(name))
        {
            filteredEmployees = filteredEmployees.Where(e => e.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        //sortField ve sortOrder
        if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
        {
            filteredEmployees = sortField.ToLower() switch
            {
                "name" => sortOrder.ToLower() == "asc" ? filteredEmployees.OrderBy(e => e.Name) : filteredEmployees.OrderByDescending(e => e.Name),
                "age" => sortOrder.ToLower() == "asc" ? filteredEmployees.OrderBy(e => e.Age) : filteredEmployees.OrderByDescending(e => e.Age),
                "email" => sortOrder.ToLower() == "asc" ? filteredEmployees.OrderBy(e => e.Email) : filteredEmployees.OrderByDescending(e => e.Email),
                _ => filteredEmployees
            };
        }

        return Ok(filteredEmployees.ToList());
    }



}
