namespace BitirmeBackend.Contracts.Responses;

public class EmployeeBasicInfoDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string JobRole { get; set; } = string.Empty;
}
