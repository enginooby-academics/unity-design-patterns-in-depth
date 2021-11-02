// SC Post Effects
// Staggart Creations
// http://staggart.xyz

//When PPS is installed and using URP in 2019.3 or LTS
#if URP && PPS && (UNITY_2019_3_OR_NEWER || !UNITY_2020_1_OR_NEWER)
#undef URP //Do not use integrated post processing
#endif

#if !URP && !PPS
#define REQUIRE_INSTALLER //Only open on older version where no URP or PPS is installed
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Rendering;

namespace SCPE
{
    public class Installer : Editor
    {
#if REQUIRE_INSTALLER || (URP && !PPS)
        public class RunOnImport : AssetPostprocessor
        {
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
            {
                foreach (string str in importedAssets)
                {
#if REQUIRE_INSTALLER
                    if (str.Contains("Installer.cs"))
                    {
                        InstallerWindow.ShowWindow();
                    }
#endif
#if URP && !PPS
                    if (str.Contains("SCPE.cs")) //Check for SCPE file, since it has root folder location
                    {
                        //When importing with the URP active, prompt installer if URP package wasn't unpacked
                        if (!URPUnpacked()) InstallerWindow.ShowWindow();
                    }
#endif

                }
            }
        }
#endif
        
#if REQUIRE_INSTALLER
        [InitializeOnLoad]
        sealed class InitializeOnLoad : Editor
        {
            public static bool HAS_APPEARED
            {
                get { return SessionState.GetBool("HAS_APPEARED", false); }
                set { SessionState.SetBool("HAS_APPEARED", value); }
            }

            [InitializeOnLoadMethod]
            public static void Initialize()
            {
                if (EditorApplication.isPlaying) return;

                //Package has been imported, but window may not show due to console errors
                //Force window to open after compilation is complete
                if (HAS_APPEARED == false)
                {
                    InstallerWindow.ShowWindow();
                    HAS_APPEARED = true;
                }
            }
        }
#endif

        private const string URPInstallationMarkerGUID = "751b255a24402084da7237349738181f";
#if SCPE_DEV
        [MenuItem("SCPE/Is URP unpacked?")]
#endif
        private static bool URPUnpacked()
        {
            string path = AssetDatabase.GUIDToAssetPath(URPInstallationMarkerGUID);
            UnityEngine.Object file = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));

            bool markerPresent = file;

            //Fallback in case the file was not yet imported
            if (!markerPresent)
            {
                //Safe to use fixed file path on first import
                string runtimeFolder = "Assets/SC Post Effects/Runtime/";
                var info = new DirectoryInfo(runtimeFolder);
                FileInfo[] fileInfo = info.GetFiles();

                foreach (FileInfo item in fileInfo)
                {
                    if (item.Name.Contains("URP_INSTALLED.txt")) markerPresent = true;
                }
            }

#if SCPE_DEV
            Debug.Log("URP installation marker " + (markerPresent ? "" : "NOT") + " present");
#endif
            return markerPresent;
        }

        private static List<UnityEditor.PackageManager.PackageInfo> packages;

        public static void Initialize()
        {
            IS_INSTALLED = false;
            Log.Clear();

            PackageVersionCheck.CheckForUpdate();
            UnityVersionCheck.CheckCompatibility();
            PackageManager.RetreivePackageList();

            RenderPipelineInstallation.CheckInstallation(); //Check first, in case URP is installed
            PostProcessingInstallation.CheckPackageInstallation();

            Demo.FindPackages();

        }

        public static void Install()
        {
            CURRENTLY_INSTALLING = true;
            IS_INSTALLED = false;

            //Add Layer for project olders than 2018.1
            {
                SetupLayer();
            }

            //Using URP with integrated PPS
            if (RenderPipelineInstallation.CurrentPipeline == RenderPipelineInstallation.Pipeline.URP && PostProcessingInstallation.Config == PostProcessingInstallation.Configuration.Integrated)
            {
                RenderPipelineInstallation.UnpackURPFiles();
            }
            //ShaderConfigurator.ConfigureForCurrentPipeline();

            PostProcessingInstallation.ConfigureURPIfNeeded();

            //If option is chosen, unpack demo content
            {
                if (Settings.installSampleContent)
                {
                    Demo.InstallSamples();
                }
                if (Settings.installDemoContent)
                {
                    Demo.InstallScenes();
                }
            }

            Installer.Log.Write("<b>Installation complete</b>");
            CURRENTLY_INSTALLING = false;
            IS_INSTALLED = true;
        }

        public static void PostInstall()
        {
            if (Settings.deleteDemoContent)
            {
                AssetDatabase.DeleteAsset(Demo.SCENES_PACKAGE_PATH);
            }
            if (Settings.setupCurrentScene)
            {
                AutoSetup.SetupCamera();
                AutoSetup.SetupGlobalVolume();
            }
        }

        public static bool CURRENTLY_INSTALLING
        {
            get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_IS_INSTALLING", false); }
            set { SessionState.SetBool(SCPE.ASSET_ABRV + "_IS_INSTALLING", value); }
        }

        public static bool IS_INSTALLED
        {
            get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_IS_INSTALLED", false); }
            set { SessionState.SetBool(SCPE.ASSET_ABRV + "_IS_INSTALLED", value); }
        }

#if SCPE_DEV
        [MenuItem("SCPE/Add layer")]
#endif
        public static void SetupLayer()
        {
            if (PostProcessingInstallation.Config == PostProcessingInstallation.Configuration.Integrated) return;

            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            SerializedProperty layers = tagManager.FindProperty("layers");

            bool hasLayer = false;

            //Skip default layers
            for (int i = 8; i < layers.arraySize; i++)
            {
                SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);

                if (layerSP.stringValue == SCPE.PP_LAYER_NAME)
                {
#if SCPE_DEV
                    Debug.Log("<b>SetupLayer</b> " + SCPE.PP_LAYER_NAME + " layer already present");
#endif
                    hasLayer = true;
                    return;
                }

                if (layerSP.stringValue == String.Empty)
                {
                    layerSP.stringValue = SCPE.PP_LAYER_NAME;
                    tagManager.ApplyModifiedProperties();
                    hasLayer = true;
                    Installer.Log.Write("Added \"" + SCPE.PP_LAYER_NAME + "\" layer to project");
#if SCPE_DEV
                    Debug.Log("<b>SetupLayer</b> " + SCPE.PP_LAYER_NAME + " layer added");
#endif
                    return;
                }
            }

            if (!hasLayer)
            {
                Debug.LogError("The layer \"" + SCPE.PP_LAYER_NAME + "\" could not be added, the maximum number of layers (32) has been exceeded");
#if UNITY_2018_3_OR_NEWER
                SettingsService.OpenProjectSettings("Project/Tags and Layers");
#else
                EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
#endif
            }

        }

        public class Demo
        {
            public static string SCENES_PACKAGE_PATH
            {
                get { return SessionState.GetString(SCPE.ASSET_ABRV + "_DEMO_PACKAGE_PATH", string.Empty); }
                set { SessionState.SetString(SCPE.ASSET_ABRV + "_DEMO_PACKAGE_PATH", value); }
            }
            public static string SAMPLES_PACKAGE_PATH
            {
                get { return SessionState.GetString(SCPE.ASSET_ABRV + "_SAMPLES_PACKAGE_PATH", string.Empty); }
                set { SessionState.SetString(SCPE.ASSET_ABRV + "_SAMPLES_PACKAGE_PATH", value); }
            }

            public static bool HAS_SCENE_PACKAGE
            {
                get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_HAS_DEMO_PACKAGE", false); }
                set { SessionState.SetBool(SCPE.ASSET_ABRV + "_HAS_DEMO_PACKAGE", value); }
            }
            public static bool HAS_SAMPLES_PACKAGE
            {
                get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_HAS_SAMPLES_PACKAGE", false); }
                set { SessionState.SetBool(SCPE.ASSET_ABRV + "_HAS_SAMPLES_PACKAGE", value); }
            }

            public static bool SCENES_INSTALLED
            {
                get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_DEMO_INSTALLED", false); }
                set { SessionState.SetBool(SCPE.ASSET_ABRV + "_DEMO_INSTALLED", value); }
            }
            public static bool SAMPLES_INSTALLED
            {
                get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_SAMPLES_INSTALLED", false); }
                set { SessionState.SetBool(SCPE.ASSET_ABRV + "_SAMPLES_INSTALLED", value); }
            }

            public static void FindPackages()
            {
                string packageDir = SCPE.GetRootFolder();

                CheckInstallation();

                string[] assets = AssetDatabase.FindAssets("_DemoContent", new[] { packageDir });

                if (assets.Length > 0)
                {
                    SCENES_PACKAGE_PATH = AssetDatabase.GUIDToAssetPath(assets[0]);
                    HAS_SCENE_PACKAGE = true;
                }
                else
                {
                    Settings.installDemoContent = false;
                    HAS_SCENE_PACKAGE = false;
                }

                assets = null;
                assets = AssetDatabase.FindAssets("_Samples", new[] { packageDir });

                if (assets.Length > 0)
                {
                    SAMPLES_PACKAGE_PATH = AssetDatabase.GUIDToAssetPath(assets[0]);
                    HAS_SAMPLES_PACKAGE = true;
                }
                else
                {
                    Settings.installSampleContent = false;
                    HAS_SAMPLES_PACKAGE = false;
                }
            }

            public static void CheckInstallation()
            {
                SCENES_INSTALLED = AssetDatabase.IsValidFolder(SCPE.PACKAGE_ROOT_FOLDER + "_Demo/");
                SCENES_INSTALLED = AssetDatabase.IsValidFolder(SCPE.PACKAGE_ROOT_FOLDER + "_Samples/");

#if SCPE_DEV
                Debug.Log("<b>Demo</b> Scenes installed: " + SCENES_INSTALLED);
                Debug.Log("<b>Demo</b> Samples installed: " + SCENES_INSTALLED);
#endif
            }

            public static void InstallScenes()
            {
                if (!string.IsNullOrEmpty(SCENES_PACKAGE_PATH))
                {
                    AssetDatabase.ImportPackage(SCENES_PACKAGE_PATH, false);

                    Installer.Log.Write("Unpacked demo scenes");

                    AssetDatabase.Refresh();
                    AssetDatabase.DeleteAsset(SCENES_PACKAGE_PATH);
                    SCENES_INSTALLED = true;
                }
                else
                {
                    Debug.LogError("The \"_DemoContent\" package could not be found, please ensure all the package contents were imported from the Asset Store.");
                    SCENES_INSTALLED = false;
                }
            }

            public static void InstallSamples()
            {
                if (!string.IsNullOrEmpty(SAMPLES_PACKAGE_PATH))
                {
                    AssetDatabase.ImportPackage(SAMPLES_PACKAGE_PATH, false);

                    Installer.Log.Write("Unpacked sample textures");

                    AssetDatabase.Refresh();
                    AssetDatabase.DeleteAsset(SAMPLES_PACKAGE_PATH);
                    SAMPLES_INSTALLED = true;
                }
                else
                {
                    Debug.LogError("The \"_Samples\" package could not be found, please ensure all the package contents were imported from the Asset Store.");
                    SAMPLES_INSTALLED = false;
                }
            }
        }

        public class Settings
        {
            public static bool upgradeShaders = false;
            public static bool installDemoContent
            {
                get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_INSTALL_DEMO", false); }
                set { SessionState.SetBool(SCPE.ASSET_ABRV + "_INSTALL_DEMO", value); }
            }
            public static bool installSampleContent
            {
                get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_INSTALL_SAMPLES", false); }
                set { SessionState.SetBool(SCPE.ASSET_ABRV + "_INSTALL_SAMPLES", value); }
            }
            public static bool deleteDemoContent = false;
            public static bool deleteSampleContent = false;
            public static bool setupCurrentScene = false;
        }

        public static class Log
        {
            public static string Read(int index)
            {
                return SessionState.GetString(SCPE.ASSET_ABRV + "_LOG_ITEM_" + index, string.Empty);
            }

            public static string ReadNext()
            {
                return SessionState.GetString(SCPE.ASSET_ABRV + "_LOG_ITEM_" + NumItems, string.Empty);
            }

            public static int NumItems
            {
                get { return SessionState.GetInt(SCPE.ASSET_ABRV + "_LOG_INDEX", 0); }
                set { SessionState.SetInt(SCPE.ASSET_ABRV + "_LOG_INDEX", value); }
            }

            public static void Write(string text)
            {
                SessionState.SetString(SCPE.ASSET_ABRV + "_LOG_ITEM_" + NumItems, text);
                NumItems++;
            }

            internal static void Clear()
            {
                for (int i = 0; i < NumItems; i++)
                {
                    SessionState.EraseString(SCPE.ASSET_ABRV + "_LOG_ITEM_" + i);
                }
                NumItems = 0;
            }

#if SCPE_DEV
            [MenuItem("SCPE/Test install log")]
            public static void Test()
            {
                Installer.CURRENTLY_INSTALLING = true;

                Installer.Log.Write("Installed Post Processing Stack v2");
                Installer.Log.Write("Upgraded shader library paths");
                Installer.Log.Write("Enabled SCPE scripts");
                Installer.Log.Write("Adding \"PostProcessing\" layer to next available slot");
                Installer.Log.Write("Unpacked demo scenes and samples");
                Installer.Log.Write("<b>Installation completed</b>");

                Installer.CURRENTLY_INSTALLING = false;
            }
#endif
        }
    }

    public class PackageManager
    {
        public static List<UnityEditor.PackageManager.PackageInfo> packages;

        public static void RetreivePackageList()
        {
            UnityEditor.PackageManager.Requests.ListRequest listRequest = Client.List(true);

            while (listRequest.Status == StatusCode.InProgress)
            {
                //Waiting...
            }

            if (listRequest.Status == StatusCode.Failure) Debug.LogError("Failed to retreived packages from Package Manager...");

            PackageCollection packageInfos = listRequest.Result;
            packages = listRequest.Result.ToList();
        }
    }

    public class PostProcessingInstallation
    {
        public enum Configuration
        {
            Auto, //Unknown
            PackageManager,
            Integrated //URP
        }
        public static Configuration Config
        {
            get { return (Configuration)SessionState.GetInt("PPS_CONFIG", 0); }

            set { SessionState.SetInt("PPS_CONFIG", (int)value); }
        }

        public static string PACKAGE_ID = "com.unity.postprocessing";

        public static bool IS_INSTALLED
        {
            get { return SessionState.GetBool("PPS_INSTALLED", false); }
            set { SessionState.SetBool("PPS_INSTALLED", value); }
        }

        public static string MIN_PPS_VERSION = "2.3.0";
        public static string MAX_PPS_VERSION = "9.9.9.9";
        public static string LATEST_COMPATIBLE_VERSION
        {
            get { return SessionState.GetString("LATEST_PPS_VERSION", string.Empty); }
            set { SessionState.SetString("LATEST_PPS_VERSION", value); }
        }

        public enum VersionStatus
        {
            Outdated,
            Compatible,
            InCompatible
        }
        public static VersionStatus PPSVersionStatus
        {
            get { return (VersionStatus)SessionState.GetInt("PPS_VERSION_STATUS", 2); }
            set { SessionState.SetInt("PPS_VERSION_STATUS", (int)value); }
        }
        public static string PPS_VERSION
        {
            get { return SessionState.GetString("PPS_VERSION", string.Empty); }
            set { SessionState.SetString("PPS_VERSION", value); }
        }

        public static Configuration CheckPackageInstallation()
        {
            IS_INSTALLED = false;
            Config = Configuration.Auto;

            if (PackageManager.packages == null) PackageManager.RetreivePackageList();

            foreach (UnityEditor.PackageManager.PackageInfo p in PackageManager.packages)
            {
                if (p.name == PACKAGE_ID)
                {
                    PPS_VERSION = p.version.Replace("-preview", string.Empty);
                    LATEST_COMPATIBLE_VERSION = p.versions.latestCompatible;

                    //Validate installed version against compatible range
                    System.Version curVersion = new System.Version(PPS_VERSION);
                    System.Version minVersion = new System.Version(MIN_PPS_VERSION);
                    System.Version maxVersion = new System.Version(MAX_PPS_VERSION);
                    System.Version latestVersion = new System.Version(LATEST_COMPATIBLE_VERSION);

                    //Clamp to maximum compatible version
                    if (latestVersion > maxVersion) latestVersion = maxVersion;

                    if (curVersion >= minVersion && curVersion <= maxVersion) PPSVersionStatus = VersionStatus.Compatible;
                    if (curVersion < minVersion || curVersion < latestVersion) PPSVersionStatus = VersionStatus.Outdated;
                    if (curVersion < minVersion || curVersion > maxVersion) PPSVersionStatus = VersionStatus.InCompatible;
#if SCPE_DEV
                    Debug.Log("<b>CheckPPSInstallation</b> PPS version " + p.version + " Installed. Required: " + MIN_PPS_VERSION);
#endif

                    IS_INSTALLED = true;
                    Config = Configuration.PackageManager;
                }
            }

            //PPS not installed
            if (IS_INSTALLED == false)
            {
#if UNITY_2019_3_OR_NEWER //URP is available and has integrated PP
                if (RenderPipelineInstallation.CurrentPipeline == RenderPipelineInstallation.Pipeline.URP && RenderPipelineInstallation.VersionStatus == RenderPipelineInstallation.Version.Compatible)
                {
                    IS_INSTALLED = true;
                    Config = Configuration.Integrated;

                    PPSVersionStatus = VersionStatus.Compatible;

#if SCPE_DEV
                    Debug.Log("<b>CheckPPSInstallation</b> URP installed, integrated PP available");
#endif
                }

#else //On older versions, fetch latest compatible PSS package
                UnityEditor.PackageManager.Requests.SearchRequest r = Client.Search(PACKAGE_ID);
                while (r.Status == StatusCode.InProgress)
                {
                    //Waiting
                }
                if (r.IsCompleted)
                {
                    LATEST_COMPATIBLE_VERSION = r.Result[0].versions.latestCompatible;

                    //Clamp to maximum compatible version
                    System.Version maxVersion = new System.Version(MAX_PPS_VERSION);
                    System.Version latestVersion = new System.Version(LATEST_COMPATIBLE_VERSION);
                    if (latestVersion > maxVersion) LATEST_COMPATIBLE_VERSION = MAX_PPS_VERSION;
                }
#endif
            }

#if SCPE_DEV
            if (IS_INSTALLED)
            {
                Debug.Log("<b>PostProcessingInstallation</b> " + Config + " version is installed");
            }
            else
            {
                Debug.Log("<b>PostProcessingInstallation</b> Post Processing Stack is not installed");
            }
#endif
            return Config;
        }

        public static void InstallPackage()
        {

            AddRequest addRequest = UnityEditor.PackageManager.Client.Add(PACKAGE_ID + "@" + LATEST_COMPATIBLE_VERSION);

            Installer.Log.Write("Installed Post Processing " + LATEST_COMPATIBLE_VERSION + " from Package Manager");
#if SCPE_DEV
            Debug.Log("<b>PostProcessingInstallation</b> Installed from Package Manager");
#endif

            //In case of updating an already installed version
            IS_INSTALLED = true;
            PPSVersionStatus = VersionStatus.Compatible;
            PPS_VERSION = LATEST_COMPATIBLE_VERSION;

            while(!addRequest.IsCompleted || addRequest.Status == StatusCode.InProgress) { }

            Debug.Log("Client add request completed");
            RefreshShaders();
        }

#if SCPE_DEV
        [MenuItem("SCPE/RefreshShaders")]
#endif
        private static void RefreshShaders()
        {
            if (SCPE.PACKAGE_ROOT_FOLDER == string.Empty) SCPE.GetRootFolder();
            
            //Very much required, despite the warning
            AssetDatabase.Refresh();
            
            //Re-import shaders so libraries references nudge into place
            string[] guids = AssetDatabase.FindAssets("t: Shader", new string[] { SCPE.PACKAGE_ROOT_FOLDER});

#if SCPE_DEV
            Debug.Log("<b>RefreshShaders</b> refreshing " + guids.Length + " shaders.");
#endif
            EditorUtility.DisplayProgressBar("SC Post Effects", "Reimporting shaders...", 1f);
            for (int i = 0; i < guids.Length; i++)
            {
                AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(guids[i]));
            }
            EditorUtility.ClearProgressBar();
        }

        //If the PPS is installed using URP in 2019.3 or LTS, the pipeline does need to be configured
        public static void ConfigureURPIfNeeded()
        {
#if URP && PPS && (UNITY_2019_3_OR_NEWER && !UNITY_2020_1_OR_NEWER)
            UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset pipeline = UnityEngine.Rendering.Universal.UniversalRenderPipeline.asset;

            if (pipeline.postProcessingFeatureSet != UnityEngine.Rendering.Universal.PostProcessingFeatureSet.PostProcessingV2)
            {
                pipeline.postProcessingFeatureSet = UnityEngine.Rendering.Universal.PostProcessingFeatureSet.PostProcessingV2;

                Installer.Log.Write("URP Post processing feature set was switched to PostProcessingV2");
            }
#endif
        }
    }

    public class RenderPipelineInstallation
    {
        public const string LWRP_PACKAGE_ID = "com.unity.render-pipelines.lightweight";
        public const string MIN_LWRP_VERSION = "5.7.2"; //Min 2019.1 version
        public const string MAX_LWRP_VERSION = "99.9.9"; //Not limited 

        public const string URP_PACKAGE_ID = "com.unity.render-pipelines.universal";
        public const string MIN_URP_VERSION = "7.2.1"; //Supports PPS and camera stacking
        public const string MAX_URP_VERSION = "999.999.999"; //Not limited 

        public const string HDRP_PACKAGE_ID = "com.unity.render-pipelines.high-definition";
        public const string MIN_HDRP_VERSION = "7.2.0";
        public const string MAX_HDRP_VERSION = "9.99.99"; //Currently no limit

        public enum Pipeline
        {
            BuiltIn,
            LWRP,
            URP,
            HDRP
        }
        public static Pipeline CurrentPipeline
        {
            get { return (Pipeline)SessionState.GetInt("SCPE_PIPELINE", 0); }
            set { SessionState.SetInt("SCPE_PIPELINE", (int)value); }
        }

        public enum Version
        {
            Compatible,
            Outdated,
            Incompatible
        }
        public static Version VersionStatus
        {
            get { return (Version)SessionState.GetInt("SRP_VERSION_STATUS", 0); }
            set { SessionState.SetInt("SRP_VERSION_STATUS", (int)value); }
        }

        //Applies to current SRP
        public static string SRP_VERSION
        {
            get { return SessionState.GetString("SRP_VERSION", string.Empty); }
            set { SessionState.SetString("SRP_VERSION", value); }
        }
        public static string MIN_SRP_VERSION;

        public static string LATEST_COMPATIBLE_VERSION
        {
            get { return SessionState.GetString("LATEST_SRP_VERSION", string.Empty); }
            set { SessionState.SetString("LATEST_SRP_VERSION", value); }
        }

        private static System.Version curVersion = new System.Version();
        private static System.Version minVersion = new System.Version();
        private static System.Version maxVersion = new System.Version();
        private static System.Version latestVersion = new System.Version();

#if SCPE_DEV
        [MenuItem("SCPE/Check SRP installation")]
#endif
        public static void CheckInstallation()
        {
            //Default
            CurrentPipeline = Pipeline.BuiltIn;

			//Packages installed, but not active (though this will cause major conflicts!)
            if (GraphicsSettings.renderPipelineAsset == null) return;

            if (PackageManager.packages == null) PackageManager.RetreivePackageList();

            foreach (UnityEditor.PackageManager.PackageInfo p in PackageManager.packages)
            {
                if (p.name == LWRP_PACKAGE_ID)
                {
                    CurrentPipeline = Pipeline.LWRP;

                    minVersion = new System.Version(MIN_LWRP_VERSION);
                    maxVersion = new System.Version(MAX_LWRP_VERSION);

                    CheckVersion(p);

                    return;
                }
                if (p.name == URP_PACKAGE_ID)
                {
                    CurrentPipeline = Pipeline.URP;

                    minVersion = new System.Version(MIN_URP_VERSION);
                    maxVersion = new System.Version(MAX_URP_VERSION);

                    CheckVersion(p);

                    return;
                }
                if (p.name == HDRP_PACKAGE_ID)
                {
                    CurrentPipeline = Pipeline.HDRP;

                    minVersion = new System.Version(MIN_HDRP_VERSION);
                    maxVersion = new System.Version(MAX_HDRP_VERSION);

                    CheckVersion(p);

                    return;
                }
            }
        }

        private static void CheckVersion(UnityEditor.PackageManager.PackageInfo p)
        {
            //Remove any characters after - (-preview.99 suffix)
            SRP_VERSION = p.version.Split('-')[0];
            LATEST_COMPATIBLE_VERSION = p.versions.latestCompatible.Split('-')[0];

            curVersion = new System.Version(SRP_VERSION);
            latestVersion = new System.Version(LATEST_COMPATIBLE_VERSION);

            MIN_SRP_VERSION = minVersion.ToString();

            //Within range of minimum and maximum versions
            if (curVersion >= minVersion && curVersion <= maxVersion)
            {
                VersionStatus = Version.Compatible;
            }
            //Outside range of compatible versions
            if (curVersion < minVersion || curVersion > maxVersion)
            {
                VersionStatus = Version.Incompatible;
            }
            if (curVersion < minVersion)
            {
                VersionStatus = Version.Outdated;
            }

            //HDRP isn't supported from 2018.3+ because it uses integrated post processing
            if (p.name == HDRP_PACKAGE_ID) VersionStatus = Version.Incompatible;
            if (p.name == LWRP_PACKAGE_ID) VersionStatus = Version.Incompatible;

#if SCPE_DEV
            Debug.Log("<b>SRP Installation</b> " + p.name + " " + SRP_VERSION + " Installed (" + VersionStatus + ")");
#endif
        }

        public static void UpdateToLatest()
        {
            string packageID = null;

            switch (CurrentPipeline)
            {
                case Pipeline.LWRP:
                    packageID = LWRP_PACKAGE_ID;
                    break;
                case Pipeline.URP:
                    packageID = URP_PACKAGE_ID;
                    break;
            }

            UnityEditor.PackageManager.Client.Add(packageID + "@" + LATEST_COMPATIBLE_VERSION);

            Installer.Log.Write("Installed " + CurrentPipeline + " " + LATEST_COMPATIBLE_VERSION + " from Package Manager");
#if SCPE_DEV
            Debug.Log("<b>RenderPipelineInstallation</b> Updated " + CurrentPipeline + " to " + LATEST_COMPATIBLE_VERSION);
#endif

            //In case of updating an already installed version
            SRP_VERSION = LATEST_COMPATIBLE_VERSION;
            VersionStatus = Version.Compatible;

            if (EditorUtility.DisplayDialog("SRP Update", CurrentPipeline + " will start updating in a moment, please let it finish first", "OK")) { }
        }

        private const string UniversalPackageGUID = "91f16b1b54b30554b8d0074f9d4bab1b";

        public static void UnpackURPFiles()
        {
            string guid = UniversalPackageGUID;
            string packagePath = AssetDatabase.GUIDToAssetPath(guid);

            if (packagePath == string.Empty)
            {
                Debug.LogError("URP script package with the GUID: <b>" + guid + "</b>. Could not be found in the project, was it changed or not imported? It should be located in <i>" + SCPE.PACKAGE_ROOT_FOLDER + "</i>");
                return;
            }

            AssetDatabase.ImportPackage(packagePath, false);
            AssetDatabase.importPackageCompleted += new AssetDatabase.ImportPackageCallback(ImportURPCallback);

            Installer.Log.Write("Unpacked and overwritten script files for URP");
        }

        static void ImportURPCallback(string packageName)
        {
            AssetDatabase.Refresh();

            AssetDatabase.importPackageCompleted -= ImportURPCallback;
        }
    }

    public class UnityVersionCheck
    {
        public static string GetUnityVersion()
        {
            string version = UnityEditorInternal.InternalEditorUtility.GetFullUnityVersion();
                
            //Remove GUID in parenthesis 
            return version.Substring(0, version.LastIndexOf(" ("));
        }
        
        public static string UnityVersion
        {
            get { return Application.unityVersion; }
        }

        public static bool COMPATIBLE
        {
            get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_COMPATIBLE_VERSION", true); }
            set { SessionState.SetBool(SCPE.ASSET_ABRV + "_COMPATIBLE_VERSION", value); }
        }
        public static bool BETA
        {
            get { return SessionState.GetBool(SCPE.ASSET_ABRV + "_BETA_VERSION", false); }
            set { SessionState.SetBool(SCPE.ASSET_ABRV + "_BETA_VERSION", value); }
        }

        public static void CheckCompatibility()
        {
            //Defaults
            COMPATIBLE = false;
            BETA = false;

            //Positives
#if UNITY_2019_1_OR_NEWER
            COMPATIBLE = true;
#endif
            
            BETA = GetUnityVersion().Contains("f") == false;

#if SCPE_DEV
            Debug.Log("<b>UnityVersionCheck</b> [Compatible: " + COMPATIBLE + "] - [Beta/Alpha: " + BETA + "]");
#endif
        }
    }
}