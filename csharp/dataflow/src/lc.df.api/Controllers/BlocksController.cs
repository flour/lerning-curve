using System.Runtime.CompilerServices;
using lc.df.api.Models;
using lc.df.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace lc.df.api.Controllers;

[ApiController]
[Route("[controller]")]
public class BlocksController : ControllerBase
{
    private readonly ISimpleDataService _dataService;
    private readonly ILogger<BlocksController> _logger;

    public BlocksController(ISimpleDataService dataService, ILogger<BlocksController> logger)
    {
        _dataService = dataService;
        _logger = logger;
    }

    [HttpGet("simples", Name = "GetSimples")]
    public IEnumerable<SimpleModel> Get() => _dataService.GetList();

    [HttpGet("simples/async", Name = "GetSimplesAsync")]
    public async IAsyncEnumerable<SimpleModel> GetAsync([EnumeratorCancellation] CancellationToken token)
    {
        await foreach (var item in _dataService.GetListAsync(token))
            yield return item;
    }
}