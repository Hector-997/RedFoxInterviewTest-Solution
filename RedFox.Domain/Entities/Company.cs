namespace RedFox.Domain.Entities;

public class Company
{
    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string CatchPhrase { get; set; } = string.Empty;

    public string Bs { get; set; } = string.Empty;

    public required User User { get; set; }
}