using System.Collections.Generic;
using System.Linq;

namespace BusinessDelegatePattern.Case2.Base1 {
  public class BusinessLookup : MonoBehaviourSingleton<BusinessLookup> {
    private IEnumerable<IVideoStreamingService> _videoStreamingServices;

    public IEnumerable<IVideoStreamingService> VideoStreamingServices =>
      _videoStreamingServices ??= TypeUtils.CreateInstancesOf<IVideoStreamingService>();

    public IVideoStreamingService GetVideoStreamingServiceService(string movieTile) =>
      VideoStreamingServices.FirstOrDefault(service => service.SupportedMovies.Contains(movieTile));
  }
}