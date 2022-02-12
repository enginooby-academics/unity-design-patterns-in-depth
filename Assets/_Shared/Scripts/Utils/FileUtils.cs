using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class FileUtils {
  /// <summary>
  ///   Return name (without extension) list of assets in the given directory path (ingore meta files).
  /// </summary>
  public static List<string> GetFileNamesAtPath(string directoryPath) {
    var fileNames = new List<string>();
    var dir = new DirectoryInfo(directoryPath);
    var info = dir.GetAllFilesIgnoreMeta();

    foreach (var f in info) fileNames.Add(f.GetNameWithoutExtension());

    return fileNames;
  }

  /// <summary>
  ///   Return name (with extension) list of assets in the given directory path (ingore meta files).
  /// </summary>
  public static List<string> GetFullFileNamesAtPath(string directoryPath) {
    var fileNames = new List<string>();
    var dir = new DirectoryInfo(directoryPath);
    var info = dir.GetAllFilesIgnoreMeta();

    foreach (var f in info) fileNames.Add(f.Name);

    return fileNames;
  }

  public static IEnumerable<FileInfo> GetAllFilesIgnoreMeta(this DirectoryInfo directoryInfo) {
    return directoryInfo.GetFiles("*.*").Where(name => !name.Extension.EqualIgnoreCase(".meta"));
  }

  public static string GetNameWithoutExtension(this FileInfo fileInfo) =>
    Path.GetFileNameWithoutExtension(fileInfo.FullName);
}