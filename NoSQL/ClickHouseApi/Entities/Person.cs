namespace ClickHouseApi.Entities;

public class Person
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public int Salary { get; set; }
    public Sex Sex { get; set; }
}

public enum Sex
{
    Male,
    Female
}
