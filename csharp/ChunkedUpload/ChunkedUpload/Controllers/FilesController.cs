using System.Buffers;
using Microsoft.AspNetCore.Mvc;

namespace ChunkedUpload.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
  private readonly ILogger<FilesController> _logger;
  private readonly IUploadService _uploadService;

  public FilesController(IUploadService uploadService, ILogger<FilesController> logger)
  {
    _logger = logger;
    _uploadService = uploadService;
  }

  [HttpGet("{sessionId:string}")]
  public Task<FileSession> GetSession(string sessionId)
      => _uploadService.GetSessionInfo(sessionId);

  [HttpPost]
  public Task<FileSession> StartNewSession([FromBody] SessionRequest request)
    => _uploadService.StartSession(request.fileSize, request.FileName);

  [HttpPost("{sessionId:string}/chunk/{chunkNumber:int}")]
  public async Task<bool> AddChunkSession(string sessionId, int chunkNumber, [FromForm] IFormFile chunk)
  {
    return await _uploadService.AddChunk(sessionId, chunkNumber, chunk.OpenReadStream());
  }
}

public class SessionRequest
{
  public string FileName { get; set; }
  public long fileSize { get; set; }
}