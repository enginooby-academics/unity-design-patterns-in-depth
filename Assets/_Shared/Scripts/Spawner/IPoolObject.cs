/// <summary>
/// Add additional logic for pool event for specific pool object (vs. uniform PoolObject)
/// </summary>
public interface IPoolObject {
  /// <summary>
  ///  Clean object for reuse.
  /// </summary>
  void OnPoolReuse();
}