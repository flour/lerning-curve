using System.Threading.Tasks.Dataflow;
using lc.df.api.Services.Models;

namespace lc.df.api.Services;

public interface IDataService
{
    Subscription Subscribe<T>(Action<T> action) where T : IFeed;
    Task Push<T>(T feedData) where T : IFeed;
}


internal class DataService : IDataService
{
    private readonly BroadcastBlock<IFeed> _broadcast;

    public DataService()
    {
        _broadcast = new BroadcastBlock<IFeed>(e => e);
    }

    public Subscription Subscribe<T>(Action<T> action) where T : IFeed
    {
        void ActionWrapper(IFeed feed) => action((T) feed);

        var actionBlock = new ActionBlock<IFeed>(ActionWrapper);
        var disposable = _broadcast.LinkTo(actionBlock, predicate: e => e is T);
        return new Subscription(disposable);
    }

    public async Task Push<T>(T feedData) where T : IFeed
    {
        await _broadcast.SendAsync(feedData);
    }
}