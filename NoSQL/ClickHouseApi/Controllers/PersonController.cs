using ClickHouseApi.Entities;
using ClickHouseApi.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using static ClickHouseApi.Controllers.Helpers.PredicateBuilder;

namespace ClickHouseApi.Controllers;

[Route("clickhouse/people")]
[ApiController]
public class PersonController (IGenericRepository<Person> repository) : ControllerBase
{
    [HttpPost("table")]
    public async Task<IActionResult> CreateTable()
    {
        await repository.CreateTableAsync();

        return Ok();
    }

    [HttpDelete("table")]
    public async Task<IActionResult> DeleteTable()
    {
        await repository.DeleteTableAsync();

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddPerson(Person person)
    {
        await repository.InsertAsync(person);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePersonById([FromBody] Guid id)
    {
        await repository.DeleteAsync(p => p.Id == id);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? firstName,
        [FromQuery] string? lastName,
        [FromQuery] Sex? sex,
        [FromQuery] int? salary)
    {
        var filters = new Dictionary<string, object?>
        {
            [nameof(Person.FirstName)] = firstName,
            [nameof(Person.LastName)] = lastName,
            [nameof(Person.Sex)] = sex,
            [nameof(Person.Salary)] = salary,
        };

        var results = await repository.WhereAsync(BuildPredicate<Person>(filters));
        return Ok(results);
    }
}
