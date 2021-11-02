/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.IO;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro
{
    public static class ResourcesPRO
    {
        private static string _assetFolder;

        public static string assetFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_assetFolder))
                {
                    string[] assets = AssetDatabase.FindAssets("uContextProMenu");
                    FileInfo info = new FileInfo(AssetDatabase.GUIDToAssetPath(assets[0]));
                    _assetFolder = info.Directory.Parent.Parent.Parent.FullName.Substring(Application.dataPath.Length - 6) + "/";
                }

                return _assetFolder;
            }
        }

        public static T Load<T>(string path) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetFolder + path);
        }
    }
}