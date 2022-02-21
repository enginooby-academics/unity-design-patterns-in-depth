using System.Collections.Generic;
using UnityEngine;

namespace BusinessDelegatePattern.Case2.Base1 {
  public class NetflixService : IVideoStreamingService {
    public IEnumerable<string> SupportedMovies => new[] {"Die Hard 2"};

    public void Process() => Debug.Log("NetflixService is now processing");
  }
}