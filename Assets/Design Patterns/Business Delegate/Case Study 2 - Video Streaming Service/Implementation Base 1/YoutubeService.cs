using System.Collections.Generic;
using UnityEngine;

namespace BusinessDelegatePattern.Case2.Base1 {
  public class YoutubeService : IVideoStreamingService {
    public IEnumerable<string> SupportedMovies => new[] {"Maradona: The Greatest Ever"};

    public void Process() => Debug.Log("Youtube service is now processing");
  }
}