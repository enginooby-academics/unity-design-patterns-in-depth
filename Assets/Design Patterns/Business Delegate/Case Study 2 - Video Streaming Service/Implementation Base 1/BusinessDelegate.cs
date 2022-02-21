using System.Collections.Generic;
using UnityEngine;

namespace BusinessDelegatePattern.Case2.Base1 {
  public class BusinessDelegate : MonoBehaviour {
    public void PlaybackMovie(string movieTitle) {
      var videoStreamingService = BusinessLookup.Instance.GetVideoStreamingServiceService(movieTitle);
      videoStreamingService?.Process();
    }

    public static IEnumerable<string> GetAllSupportedMovies() {
      var movies = new List<string>();

      foreach (var service in BusinessLookup.Instance.VideoStreamingServices) movies.AddRange(service.SupportedMovies);

      return movies;
    }
  }
}