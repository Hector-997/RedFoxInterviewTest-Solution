namespace RedFox.Domain.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string CatchPhrase { get; set; } = string.Empty;

    public string Bs { get; set; } = string.Empty;
    public IEnumerable<User> Users { get; set; } = [];
}