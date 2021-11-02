using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Digger
{
    public class PreprocessBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder => -1;

        public void OnPreprocessBuild(BuildReport report)
        {
            NativeCollectionsPool.Instance.Dispose();

            var streamingAssetsBasePath = Path.Combine(Application.streamingAssetsPath, "DiggerData");
            if (Directory.Exists(streamingAssetsBasePath))
                Directory.Delete(streamingAssetsBasePath, true);
        }
    }
}