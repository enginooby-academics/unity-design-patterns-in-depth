using System.Collections.Generic;
using System.Linq;

namespace BusinessDelegatePattern.Case2.Base1 {
  public interface IVideoStreamingService {
    /// <summary>
    /// Return list of movies that service supports streaming.
    /// </summary>
    IEnumerable<string> SupportedMovies => Enumerable.Empty<string>();

    void Process();
  }
}