using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace com.ootii.Helpers
{    
    /// <summary>
    /// Helper class for working with Unity asset files
    /// </summary>
    public static class AssetHelper 
    {
#if UNITY_EDITOR
        
        /// <summary>
        /// Get the path to the "ootii" folder relative to the top-level Assets folder. 
        /// This should be "Assets/ootii/" unless the folder has been moved.
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            string lRootPath = GetPathTo("AssetHelper.cs").Replace("Assets/Framework_v1/Code/Helpers/", "");

            Debug.Log("Root Path = " + lRootPath);

            return lRootPath;
        }

        /// <summary>
        /// Get the path to the specified file name, starting with the top-level Assets folder
        /// </summary>
        /// <param name="rFileName"></param>
        /// <returns></returns>
        public static string GetPathTo(string rFileName)
        {
            var files = Directory.GetFiles(Application.dataPath, rFileName, SearchOption.AllDirectories);
            if (files.Length == 0 || string.IsNullOrEmpty(files[0]))
            {
                Debug.LogError("GetRootPath failed");
                return null;
            }

            string lAbsolutePath = files[0].Replace("\\", "/");
            string lRelativePath = "Assets" + lAbsolutePath.Substring(Application.dataPath.Length);
            lRelativePath = lRelativePath.Replace(rFileName, "");            

            Debug.Log(string.Format("Path to {0} = {1}", rFileName, lRelativePath));

            return lRelativePath;
        }

        /// <summary>
        /// Verifies that a file exists at the specified file path
        /// </summary>
        /// <param name="rFilePath"></param>
        /// <returns></returns>
        public static bool VerifyFilePath(string rFilePath)
        {
            try
            {
                return File.Exists(rFilePath);
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Get a custom path if one is stored in EditorPrefs. Returns the
        /// supplied default path if one is not found.
        /// </summary>
        /// <param name="rPrefsKey"></param>
        /// <param name="rDefaultPath"></param>
        /// <returns></returns>
        public static string GetStoredPath(string rPrefsKey, string rDefaultPath)
        {
            string lCustomPath = EditorPrefs.GetString(rPrefsKey, "");
            return !string.IsNullOrEmpty(lCustomPath) ? lCustomPath : rDefaultPath;
        }

        /// <summary>
        /// Create a new ScriptableObject asset at the designated path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rAssetPath"></param>        
        /// <returns></returns>
        public static T GetOrCreateAsset<T>(string rAssetPath) where T : ScriptableObject
        {            
            EnsurePath(rAssetPath);
            
            T lAsset = AssetDatabase.LoadAssetAtPath<T>(rAssetPath);            
            if (lAsset == null && !File.Exists(rAssetPath))
            {                
                lAsset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(lAsset, rAssetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            return lAsset;
        }

        /// <summary>
        /// Load all assets of one type at a specified folder path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rAssetPath"></param>
        /// <returns></returns>
        public static List<T> LoadAssets<T>(string rAssetPath) where T : ScriptableObject
        {                               
            //Debug.Log("[LoadAssets] Loading from " + rAssetPath);
            try
            {
                string[] lFilePaths = Directory.GetFiles(rAssetPath, "*.asset");
                if (lFilePaths == null) { return null; }
                //Debug.Log("Found " + lFilePaths.Length + " files");
                List<T> lAssets = new List<T>();

                foreach (string lFile in lFilePaths)
                {
                    //Debug.Log(lFile);
                    string lFileName = Path.GetFileName(lFile);
                    //Debug.Log(lFileName);
                    if (string.IsNullOrEmpty(lFileName)) continue;

                    T lAsset = AssetDatabase.LoadAssetAtPath<T>(lFile);
                    if (lAsset == null) continue;

                    lAssets.Add(lAsset);
                }

                return lAssets;
            }
            catch (IOException ex)
            {
                Debug.LogException(ex);
                return null;
            }            
        }

        /// <summary>
        /// Ensure that the path is valid; if it does not exist, create the folder(s) in the file system
        /// </summary>
        /// <param name="rPath"></param>
        /// <returns></returns>
        public static bool EnsurePath(string rPath)
        {
            string lDirectory = Path.GetDirectoryName(rPath);
            if (string.IsNullOrEmpty(lDirectory)) { return false; }
            
            try
            {
                if (!Directory.Exists(lDirectory))
                {
                    //Debug.Log("Directory does not exist: " + lDirectory);
                    Directory.CreateDirectory(lDirectory);
                }
            }
            catch (IOException ex)
            {
                Debug.LogException(ex);
                return false;
            }

            return true;
        }

       
        /// <summary>
        /// Ensure that an asset has a unique filename before saving it. This will increment and append an
        /// integer after the file name
        /// </summary>
        /// <param name="rPath"></param>
        /// <returns></returns>
        public static string GetNewAssetPath(string rPath)
        {
            if (string.IsNullOrEmpty(rPath)) { return string.Empty; }
            int lRetries = 0;
            string lWorkingPath = rPath;
            try
            {
                // Ensure that the directory exists
                EnsurePath(lWorkingPath);
                                
                // If the file doesn't already exist, then use the original path
                if (!File.Exists(lWorkingPath)) return lWorkingPath;

                while (File.Exists(lWorkingPath))
                {
                    if (++lRetries > 100) { break; }
                    
                    string lOriginalFileName = Path.GetFileNameWithoutExtension(lWorkingPath);
                    int lSequence = 0;
                    string lBaseFileName = lOriginalFileName;

                    // Check if there is a number at the end of the string
                    string lIntString = Regex.Match(lOriginalFileName, @"\d+$").Value;

                    // If there is, then increment it by one
                    if (!string.IsNullOrEmpty(lIntString))
                    {
                        int.TryParse(lIntString, out lSequence);
                        if (lSequence > 0) { lSequence++; }
                        lBaseFileName = lBaseFileName.Replace(lIntString, string.Empty).Trim();
                    }
                    else
                    {
                        lSequence = 1;
                    }

                    string lNewFileName = string.Format("{0} {1}", lBaseFileName.Trim(), lSequence);
                    // Replace the base file name in the path
                    lWorkingPath = lWorkingPath.Replace(lOriginalFileName, lNewFileName);
                }

                return lWorkingPath;                             
            }
            catch (IOException ex)
            {
                Debug.LogException(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Opens the a script file in Visual Studio
        /// </summary>        
        /// <param name="rPath"></param>
        /// <param name="rType"></param>
        public static void OpenScriptFile(string rPath, Type rType)
        {            
            foreach (var lAssetPath in AssetDatabase.GetAllAssetPaths())
            {
                if (!lAssetPath.EndsWith(rPath)) { continue; }

                var lScript = (MonoScript) AssetDatabase.LoadAssetAtPath(lAssetPath, typeof(MonoScript));
                if (lScript != null && rType == lScript.GetClass())
                {
                    AssetDatabase.OpenAsset(lScript);
                    break;
                }
            }
        }
#endif
    }
}

