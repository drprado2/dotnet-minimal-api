using System.Diagnostics.Metrics;

namespace MinimalApi.Observability;

public static class Metrics
{
    public static Counter<long> CompanyCreatedCounter = AppObservability.AppMeter.CreateCounter<long>("company_created_counter");
    public static Counter<long> EmployeeCreatedCounter = AppObservability.AppMeter.CreateCounter<long>("employee_created_counter");
}
