using System.Runtime.CompilerServices;
using lc.df.api.Models;

namespace lc.df.api.Services;

public interface ISimpleDataService : IDisposable
{
    IEnumerable<SimpleModel> GetList();
    IAsyncEnumerable<SimpleModel> GetListAsync(CancellationToken token);
}

internal class SimpleDataService : ISimpleDataService
{
    private readonly IDataService _propagator;

    public SimpleDataService(IDataService propagator)
    {
        _propagator = propagator;
    }

    public IEnumerable<SimpleModel> GetList()
        => Enumerable.Range(0, 100).Select(e => new SimpleModel());

    public async IAsyncEnumerable<SimpleModel> GetListAsync([EnumeratorCancellation] CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500), token);
            var item = new SimpleModel();
            await _propagator.Push(item);
            yield return item;
        }
    }

    public void Dispose()
    {
    }
}