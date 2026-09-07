using LOS.Domain.Common;

namespace LOS.Domain.Clients;

public class Employment : BaseEntity
{
    public Guid ClientId { get; private set; }
    public Client Client { get; private set; }
    public string EmployerName { get; private set; }
    public string Position { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public bool IsCurrent { get; private set; }

    private Employment() { }

    public Employment(
        Guid clientId,
        string employerName,
        string position,
        DateTime startDate,
        bool isCurrent = true)
    {
        ClientId = clientId;
        EmployerName = employerName;
        Position = position;
        StartDate = startDate;
        IsCurrent = isCurrent;
    }
}