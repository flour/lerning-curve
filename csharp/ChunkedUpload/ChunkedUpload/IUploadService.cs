using System.Collections.Concurrent;

namespace ChunkedUpload;

public interface IUploadService
{
  Task<FileSession> GetSessionInfo(string sessionId, CancellationToken token = default);
  Task<FileSession> StartSession(long fileSize, string fileName, CancellationToken token = default);
  Task<bool> AddChunk(string session, int chunkNumber, Stream chunk, CancellationToken token = default);
}

internal class UploadFileSystemService : IUploadService
{
  private const int DefaultChunkSize = 1024 * 1024;
  private readonly ConcurrentDictionary<string, FileSession> _sessions = new();

  public Task<FileSession> StartSession(long fileSize, string fileName, CancellationToken token)
  {
    var chunks = (int)Math.Round((double)fileSize / DefaultChunkSize, MidpointRounding.AwayFromZero);
    var session = new FileSession(
        Guid.NewGuid().ToString("N"),
        fileName,
        chunks,
        fileSize,
        Enumerable.Range(0, chunks).ToDictionary(e => e, e => new ChunkInfo(e, $"{fileName}_{e}")));
    _sessions.TryAdd(session.SessionId, session);
    return Task.FromResult(session);
  }

  public Task<FileSession> GetSessionInfo(string sessionId, CancellationToken token = default)
  {
    if (_sessions.TryGetValue(sessionId, out var session))
      return Task.FromResult(session);
    throw new FileNotFoundException(sessionId);
  }

  public async Task<bool> AddChunk(string sessionId, int chunkNumber, Stream chunk, CancellationToken token = default)
  {
    if (!_sessions.TryGetValue(sessionId, out var session) || !session.FileChunks.TryGetValue(chunkNumber, out var chunkInfo))
      return false;
    if (!chunk.CanRead)
      return false;
    using var fs = File.OpenWrite(chunkInfo.ChunkName);
    await chunk.CopyToAsync(fs);
    chunkInfo.Filled = true;
    if (session.FileChunks.Values.All(e => e.Filled))
      await MergeChunks(session, token);

    return true;
  }

  private async Task MergeChunks(FileSession session, CancellationToken token)
  {
    await Task.Yield();
    Task.Run(async () =>
    {
      // merge files
    });
  }
}

public record FileSession(
    string SessionId,
    string FileName,
    int Chunks,
    long Size,
    Dictionary<int, ChunkInfo> FileChunks);

public class ChunkInfo
{
  public ChunkInfo(int chunk, string name, bool filled = false)
  {
    Chunk = chunk;
    ChunkName = name;
    Filled = filled;
  }
  public int Chunk { get; set; }
  public string ChunkName { get; set; }
  public bool Filled { get; set; }
};