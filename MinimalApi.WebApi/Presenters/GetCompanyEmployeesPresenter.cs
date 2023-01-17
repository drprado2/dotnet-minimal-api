using MinimalApi.Domain.UseCases.Queries;

namespace MinimalApi.WebApi.Presenters;

public static class GetCompanyEmployeesPresenter
{
    public static GetCompanyEmployeesQuery ToUseCaseInput(Guid companyId, int pageSize, int? lastPageItem)
    {
        return new GetCompanyEmployeesQuery(companyId, pageSize, lastPageItem, null);
    }

    public static GetCompanyEmployeesOutput ToOutput(GetCompanyEmployeesQuery cmd)
    {
        if (cmd == null || cmd.EmployeesRetrieved == null)
            throw new NullReferenceException();

        var data= cmd.EmployeesRetrieved.Select(e => new GetCompanyEmployeesOutput.Employee()
        {
            EmployeeId = e.Id,
            Name = e.Name,
            Email = e.Email,
            CreatedAt = e.CreatedAt,
            DataVersion = e.DataVersion,
        }).ToList();
        return new GetCompanyEmployeesOutput
        {
            Data = data,
            LastPageId = cmd.LastPageItem
        };
    }
}

public struct GetCompanyEmployeesOutput
{
    public struct Employee
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint DataVersion { get; set; }
    }
    
    public IList<Employee> Data { get; set; }
    public long? LastPageId { get; set; }
}
