namespace lc.df.api.Services.Models;

public class Subscription
{
    private readonly IDisposable _disposable;

    public Subscription(IDisposable disposable)
    {
        _disposable = disposable;
    }
    
    public void UnSubscribe()
    {
        _disposable?.Dispose();
    }
}