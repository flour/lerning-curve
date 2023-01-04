using lc.df.api.Services.Models;

namespace lc.df.api.Models;

public class SimpleModel : IFeed
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = $"Name {DateTime.UtcNow}";
}