// SC Post Effects
// Staggart Creations
// http://staggart.xyz

#if PPS
using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;
#endif

#if URP
using UnityEngine.Rendering.Universal;
using UnityEditor.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;
#endif

using System;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;


namespace SCPE
{
    public class SCPE_GUI : Editor
    {

        public static void DisplayDocumentationButton(string section)
        {
            GUILayout.Space(-18f);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(new GUIContent("Doc", HelpIcon, "Open documentation web page\n\nHover over a parameter\nto read its description."), DocButton))
                {
                    Application.OpenURL(SCPE.DOC_URL + "?section=" + section);
                }
            }

            GUILayout.Space(2f);
        }

#if URP
        public static void ShowDepthTextureWarning(bool required = true)
        {
            if (UniversalRenderPipeline.asset)
            {
                if(UniversalRenderPipeline.asset.supportsCameraDepthTexture == false && required)
                {
                    EditorGUILayout.HelpBox("The render pipeline does not render the depth texture,\n\nwhich is required for the effect's current configuration", MessageType.Error);

                    GUILayout.Space(-32);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button(new GUIContent("Fix", EditorGUIUtility.IconContent("d_tab_next").image), GUILayout.Width(60)))
                        {
                            UniversalRenderPipeline.asset.supportsCameraDepthTexture = true;
                        }
                        GUILayout.Space(8);
                    }
                    GUILayout.Space(11);
                }
            }
        }
#endif

#if PPS
        //Append the chosen mode to the title when overriden

        public static string ModeTitle(SerializedParameterOverride prop)
        {
            return ((prop.overrideState.boolValue) ? " (" + prop.value.enumDisplayNames[prop.value.intValue] + ")" : string.Empty);
        }
#endif

        //Append the chosen mode to the title when overriden
#if URP
        public static string ModeTitle<T>(VolumeParameter prop)
        {
            return string.Empty;
        }
#endif

#if URP && !PPS
        public static void DisplaySetupWarning<T>(ref bool state, bool compatible2D = true)
#else
        public static void DisplaySetupWarning<T>(bool state = true, bool compatible2D = true)
#endif
        {
#if URP && !PPS
            ScriptableRendererData[] rendererDataList = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(UniversalRenderPipeline.asset);
            int defaultRendererIndex = (int)typeof(UniversalRenderPipelineAsset).GetField("m_DefaultRendererIndex", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(UniversalRenderPipeline.asset);
            ScriptableRendererData forwardRenderer = rendererDataList[defaultRendererIndex];
            
            bool is2D = forwardRenderer.GetType() == typeof(Renderer2DData);
            #if !URP_12_0_OR_NEWER
            if (is2D)
            {
                EditorGUILayout.HelpBox("2D renderer is not supported. Requires Unity 2021.2.0 (URP v12)", MessageType.Error);
                return;
            }
            #endif

            if (is2D && !compatible2D)
            {
                EditorGUILayout.HelpBox("This effect has limited or no practical purpose for 2D rendering", MessageType.Error);
            }
            
            if (state) return;

            EditorGUILayout.HelpBox("Effect has not been added to the default renderer's\n\"Renderer Features\" list. Will not render otherwise.", MessageType.Warning);

            GUILayout.Space(-32);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(new GUIContent("Add", EditorGUIUtility.IconContent("d_tab_next").image), GUILayout.Width(60)))
                {
                    AutoSetup.SetupEffect<T>();
                    state = true;
                }
                GUILayout.Space(8);
            }
            GUILayout.Space(11);
#endif
//Post processing feature set was removed in 2020.1, compile out in case the user imports the asset with URP+PPS in 2020.1
#if URP && PPS && !UNITY_2020_1_OR_NEWER
            if (UniversalRenderPipeline.asset && UniversalRenderPipeline.asset.postProcessingFeatureSet == PostProcessingFeatureSet.Integrated)
            {
                EditorGUILayout.HelpBox("Post Processing Stack v2 installed, but URP isn't configured to use it", MessageType.Error);

                GUILayout.Space(-32);
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(new GUIContent("Fix", EditorGUIUtility.IconContent("d_tab_next").image), GUILayout.Width(60)))
                    {
                        UniversalRenderPipeline.asset.postProcessingFeatureSet = PostProcessingFeatureSet.PostProcessingV2;
                    }
                    GUILayout.Space(8);
                }
                GUILayout.Space(11);
            }
#endif
#if URP && PPS && UNITY_2020_1_OR_NEWER
			EditorGUILayout.HelpBox("Post Processing package and URP no longer work together since Unity 2020.1+.\n\nUninstall the Post-processing package and re-run the SC Post Effects installer. See documentation", MessageType.Error);
#endif
#if !URP && PPS
            //Nothing
#endif
        }


        public static void DisplayVRWarning()
        {
            if (UnityEditorInternal.VR.VREditor.GetVREnabledOnTargetGroup(BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget)) && PlayerSettings.stereoRenderingPath == StereoRenderingPath.SinglePass)
            {
                EditorGUILayout.HelpBox("Not supported in Single-Pass Stereo Rendering", MessageType.Warning);
            }
        }

        public enum Status
        {
            Ok,
            Warning,
            Error,
            Info
        }

        public static string HEADER_IMG_PATH
        {
            get { return SessionState.GetString(SCPE.ASSET_ABRV + "_HEADER_IMG_PATH", string.Empty); }
            set { SessionState.SetString(SCPE.ASSET_ABRV + "_HEADER_IMG_PATH", value); }
        }

        public static void DrawStatusBox(GUIContent content, string status, SCPE_GUI.Status type, bool boxed = true)
        {

            using (new EditorGUILayout.HorizontalScope(EditorStyles.label))
            {
                if (content != null)
                {
                    content.text = "  " + content.text;
                    EditorGUILayout.LabelField(content, StatusContent);
                }
                DrawStatusString(status, type, boxed);
            }

        }

        public static bool DrawActionBox(string text, Texture image = null)
        {
            if (text != string.Empty) text = " " + text;

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(" ");

                if (GUILayout.Button(new GUIContent(text, image), EditorStyles.miniButton, GUILayout.MaxWidth(200f)))
                {
                    return true;
                }

            }

            return false;
        }

        /*
        public static void DrawSwitchBox(GUIContent content, string textLeft, string textRight, bool inLeft, bool inRight, out bool outLeft, out bool outRight)
        {
            outLeft = inLeft;
            outRight = inRight;

            using (new EditorGUILayout.HorizontalScope(EditorStyles.label))
            {
                using (new EditorGUILayout.HorizontalScope(EditorStyles.label))
                {
                    if (content != null)
                    {
                        content.text = "  " + content.text;
                        EditorGUILayout.LabelField(content, EditorStyles.label);
                    }

                    if (GUILayout.Button(new GUIContent(textLeft), (outLeft) ? ToggleButtonLeftToggled : ToggleButtonLeftNormal))
                    {
                        outLeft = true;
                        outRight = !outLeft;
                    }
                    if (GUILayout.Button(new GUIContent(textRight), (outRight) ? ToggleButtonRightToggled : ToggleButtonRightNormal))
                    {
                        outRight = true;
                        outLeft = !outRight;
                    }
                }
            }
        }
        */

        public static void DrawStatusString(string text, Status status, bool boxed = true)
        {
            GUIStyle guiStyle = EditorStyles.label;
            Color defaultTextColor = GUI.contentColor;

            //Personal skin
            if (EditorGUIUtility.isProSkin == false)
            {
                defaultTextColor = GUI.skin.customStyles[0].normal.textColor;
                guiStyle = new GUIStyle();

                GUI.skin.customStyles[0] = guiStyle;
            }


            //Grab icon and text color for status
            Texture icon = null;
            Color statusTextColor = Color.clear;

            StyleStatus(status, out statusTextColor, out icon);


            if (EditorGUIUtility.isProSkin == false)
            {
                GUI.skin.customStyles[0].normal.textColor = statusTextColor;
            }
            else
            {
                GUI.contentColor = statusTextColor;
            }

            if (boxed)
            {
                using (new EditorGUILayout.HorizontalScope(StatusBox))
                {
                    EditorGUILayout.LabelField(new GUIContent(" " + text, icon), guiStyle);
                }
            }
            else
            {
                EditorGUILayout.LabelField(new GUIContent(" " + text, icon), guiStyle);
            }


            if (EditorGUIUtility.isProSkin == false)
            {
                GUI.skin.customStyles[0].normal.textColor = defaultTextColor;
            }
            else
            {
                GUI.contentColor = defaultTextColor;
            }
        }

        public static void StyleStatus(Status status, out Color color, out Texture icon)
        {
            color = Color.clear;
            icon = null;

            float sin = Mathf.Sin((float)EditorApplication.timeSinceStartup * 3.14159274f * 2f) * 0.5f + 0.5f;

            switch (status)
            {
                case (Status)0: //Ok
                    {
                        color = Color.Lerp(new Color(97f / 255f, 255f / 255f, 66f / 255f), Color.green, sin);

                        icon = CheckMark;
                    }
                    break;
                case (Status)1: //Warning
                    {
                        color = Color.Lerp(new Color(252f / 255f, 174f / 255f, 78f / 255f), Color.yellow, sin);
                        icon = EditorGUIUtility.IconContent("console.warnicon.sml").image;
                    }
                    break;
                case (Status)2: //Error
                    {
                        color = Color.Lerp(new Color(255f / 255f, 112f / 255f, 112f / 255f), new Color(252f / 255f, 174f / 255f, 78f / 255f), sin);

                        icon = EditorGUIUtility.IconContent("CollabError").image;
                    }
                    break;
                case (Status)3: //Info
                    {
                        color = Color.Lerp(new Color(1f, 1f, 1f), new Color(0.9f, 0.9f, 0.9f), sin);
                        icon = EditorGUIUtility.IconContent("curvekeyframe").image;
                    }
                    break;
            }

            //Darken colors on personal skin
            if (EditorGUIUtility.isProSkin == false)
            {
                color = Color.Lerp(color, Color.black, (status != Status.Info) ? 0.5f : 0.1f);
            }
        }

        public static void DrawLogLine(string text)
        {
            EditorGUILayout.LabelField(new GUIContent(text, CheckMark), SCPE_GUI.LogText);
        }

        public static void DrawLogLine(string text, SCPE_GUI.Status status = Status.Ok)
        {
            EditorGUILayout.LabelField(new GUIContent(text), LogText);
        }

        public static void DrawProgressBar(Rect rect, float progress)
        {
            Color defaultColor = UnityEngine.GUI.contentColor;
            Texture fillTex = Texture2D.whiteTexture;

            //Background
            GUILayout.BeginArea(rect, EditorStyles.textArea);
            {
                //Fill
                Color color = new Color(99f / 255f, 138f / 255f, 124f / 255f);
                Rect barRect = new Rect(0, 0, rect.width * progress, rect.height);
                EditorGUI.DrawRect(barRect, color);

                //EditorGUILayout.LabelField(progress * 100 + "%");
            }
            GUILayout.EndArea();

            UnityEngine.GUI.contentColor = defaultColor;
        }

        public static void DrawWindowHeader(float windowWidth, float windowHeight)
        {
            Rect headerRect = new Rect(0, 0, windowWidth, windowHeight / 6.5f);
            if (SCPE_GUI.HeaderImg)
            {
                UnityEngine.GUI.DrawTexture(headerRect, SCPE_GUI.HeaderImg, ScaleMode.ScaleToFit);
                GUILayout.Space(SCPE_GUI.HeaderImg.height / 4 + 65);
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<b><size=24>SC Post Effects</size></b>\n<size=16>For Post Processing Stack</size>", Header);
            }

        }

        private static Texture2D _HeaderImg;
        public static Texture2D HeaderImg
        {
            get
            {
                if (_HeaderImg == null)
                {
                    if (HEADER_IMG_PATH == String.Empty) SCPE.GetRootFolder(); //If called before any initialization was done

                    _HeaderImg = (Texture2D)AssetDatabase.LoadAssetAtPath(HEADER_IMG_PATH, typeof(Texture2D));
                }
                return _HeaderImg;
            }
        }

        public static GUIContent TargetPlatform()
        {
            GUIContent c = new GUIContent();

            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    {
                        c.text = "Android";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.Android.Small").image;
                    }
                    break;
                case BuildTarget.iOS:
                    {
                        c.text = "iOS";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.iPhone.Small").image;
                    }
                    break;
                case BuildTarget.PS4:
                    {
                        c.text = "Playstation 4";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.PS4.Small").image;
                    }
                    break;
#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    {
                        c.text = "MacOS";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.Standalone.Small").image;
                    }
                    break;
#else
                case BuildTarget.StandaloneOSXIntel:
                    {
                        c.text = "MacOS";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.Standalone.Small").image;
                    }
                    break;
                case BuildTarget.StandaloneOSXIntel64:
                    {
                        c.text = "MacOS";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.Standalone.Small").image;
                    }
                    break;
#endif
                case BuildTarget.StandaloneWindows:
                    {
                        c.text = "Windows";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.Metro.Small").image;
                    }
                    break;
                case BuildTarget.StandaloneWindows64:
                    {
                        c.text = "Windows";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.Metro.Small").image;
                    }
                    break;
                case BuildTarget.Switch:
                    {
                        c.text = "Nintendo Switch";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.Switch.Small").image;
                    }
                    break;
                case BuildTarget.XboxOne:
                    {
                        c.text = "Xbox One";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.XboxOne.Small").image;
                    }
                    break;
                case BuildTarget.WebGL:
                    {
                        c.text = "Web";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.Web.Small").image;
                    }
                    break;
                case BuildTarget.WSAPlayer:
                    {
                        c.text = "Universal Windows Platform";
                        c.image = EditorGUIUtility.IconContent("BuildSettings.WP8.Small").image;
                    }
                    break;
                default:
                    {
                        c.text = "Unknown";
                        c.image = EditorGUIUtility.IconContent("console.erroricon.sml").image;
                    }
                    break;
            }

            return c;
        }

        public class Installation
        {

            public static void DrawPackageVersion()
            {
                string versionText;
                SCPE_GUI.Status versionStatus;

                if (PackageVersionCheck.queryStatus == PackageVersionCheck.QueryStatus.Fetching)
                {
                    versionStatus = SCPE_GUI.Status.Warning;
                    versionText = "Checking update server...";
                }
                else
                {
                    versionStatus = (PackageVersionCheck.IS_UPDATED) ? SCPE_GUI.Status.Ok : SCPE_GUI.Status.Warning;
                    versionText = (PackageVersionCheck.IS_UPDATED) ? "Latest version" : "New version available";
                }

                SCPE_GUI.DrawStatusBox(new GUIContent("Package version " + SCPE.INSTALLED_VERSION, EditorGUIUtility.IconContent("cs Script Icon").image), versionText, versionStatus);

                if (!PackageVersionCheck.IS_UPDATED)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (SCPE_GUI.DrawActionBox("Update to " + PackageVersionCheck.fetchedVersionString, EditorGUIUtility.IconContent("BuildSettings.Standalone.Small").image))
                        {
                            SCPE.OpenStorePage();
                        }
                    }
                }
            }

            public static void DrawUnityVersion()
            {
                string versionText = null;
                versionText = (UnityVersionCheck.COMPATIBLE) ? "Compatible" : "Not compatible";
                versionText = (UnityVersionCheck.BETA) ? "Beta/alpha!" : versionText;
                SCPE_GUI.Status versionStatus;
                versionStatus = (UnityVersionCheck.COMPATIBLE) ? SCPE_GUI.Status.Ok : SCPE_GUI.Status.Error;
                versionStatus = (UnityVersionCheck.BETA) ? SCPE_GUI.Status.Warning : versionStatus;

                string tooltip = (UnityVersionCheck.COMPATIBLE) ? "Tested with the current package version" : "This version is not compatible";
                tooltip = (UnityVersionCheck.BETA) ? "Alpha/beta version are not subject to support or fixes until release" : tooltip;

                SCPE_GUI.DrawStatusBox(new GUIContent("Unity " + UnityVersionCheck.UnityVersion, EditorGUIUtility.IconContent("UnityLogo").image, tooltip), versionText, versionStatus);
            }

            public static void DrawPlatform()
            {
                string compatibilityText = SCPE.IsCompatiblePlatform ? "Compatible" : "Unsupported";
                SCPE_GUI.Status compatibilityStatus = SCPE.IsCompatiblePlatform ? SCPE_GUI.Status.Ok : SCPE_GUI.Status.Error;

                SCPE_GUI.DrawStatusBox(TargetPlatform(), compatibilityText, compatibilityStatus);
            }

            public static void DrawColorSpace()
            {
                string colorText = (UnityEditor.PlayerSettings.colorSpace == ColorSpace.Linear) ? "Linear" : "Linear is recommended";
                SCPE_GUI.Status folderStatus = (UnityEditor.PlayerSettings.colorSpace == ColorSpace.Linear) ? SCPE_GUI.Status.Ok : SCPE_GUI.Status.Warning;

                SCPE_GUI.DrawStatusBox(new GUIContent("Color space", EditorGUIUtility.IconContent("d_PreTextureRGB").image), colorText, folderStatus);
            }

            public static void DrawPipeline()
            {
                string pipelineText = string.Empty;
                string pipelineName = "Built-in Render pipeline";
                SCPE_GUI.Status compatibilityStatus = SCPE_GUI.Status.Info;

                switch (RenderPipelineInstallation.CurrentPipeline)
                {
                    case RenderPipelineInstallation.Pipeline.BuiltIn:
                        pipelineName = "Built-in Render pipeline";
                        break;
                    case RenderPipelineInstallation.Pipeline.LWRP:
                        pipelineName = "Lightweight Render Pipeline " + RenderPipelineInstallation.SRP_VERSION;
                        break;
                    case RenderPipelineInstallation.Pipeline.URP:
                        pipelineName = "URP " + RenderPipelineInstallation.SRP_VERSION;
                        break;
                    case RenderPipelineInstallation.Pipeline.HDRP:
                        pipelineName = "HDRP " + RenderPipelineInstallation.SRP_VERSION;
                        break;
                }

                if (RenderPipelineInstallation.VersionStatus == RenderPipelineInstallation.Version.Compatible)
                {
                    pipelineText = "Compatible";
                    compatibilityStatus = SCPE_GUI.Status.Ok;
                }
                if (RenderPipelineInstallation.VersionStatus == RenderPipelineInstallation.Version.Outdated)
                {
                    pipelineText = "Outdated (Requires " + RenderPipelineInstallation.MIN_URP_VERSION + ")";
                    compatibilityStatus = Status.Error;


                }
                if (RenderPipelineInstallation.VersionStatus == RenderPipelineInstallation.Version.Incompatible)
                {
                    pipelineText = "Unsupported";
                    compatibilityStatus = SCPE_GUI.Status.Error;
                }

                SCPE_GUI.DrawStatusBox(new GUIContent(pipelineName, EditorGUIUtility.IconContent("d_Profiler.Rendering").image), pipelineText, compatibilityStatus);

                if (RenderPipelineInstallation.VersionStatus == RenderPipelineInstallation.Version.Outdated)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (SCPE_GUI.DrawActionBox("Update to " + RenderPipelineInstallation.LATEST_COMPATIBLE_VERSION, EditorGUIUtility.IconContent("BuildSettings.Standalone.Small").image))
                        {
                            RenderPipelineInstallation.UpdateToLatest();
                        }
                    }
                }
            }

            public static void DrawPostProcessing()
            {
                //Post Processing Stack
                string ppsLabel = "Post Processing Stack v2";
                string ppsText = (PostProcessingInstallation.IS_INSTALLED) ? "Installed (Package Manager)" : "Not installed";
                ppsText = (PostProcessingInstallation.Config == PostProcessingInstallation.Configuration.Integrated) ? "SRP Volume System" : ppsText;
                SCPE_GUI.Status ppsStatus = (PostProcessingInstallation.IS_INSTALLED) ? SCPE_GUI.Status.Ok : SCPE_GUI.Status.Error;

                ppsLabel = "Post Processing";
                //Append current version
                if (PostProcessingInstallation.IS_INSTALLED) ppsLabel += " (" + PostProcessingInstallation.PPS_VERSION + ")";

                //Outdated version
                ppsText = (PostProcessingInstallation.PPSVersionStatus == PostProcessingInstallation.VersionStatus.Outdated) ? "Outdated" : ppsText;
                ppsStatus = (PostProcessingInstallation.PPSVersionStatus == PostProcessingInstallation.VersionStatus.Outdated) ? SCPE_GUI.Status.Warning : ppsStatus;

                SCPE_GUI.DrawStatusBox(new GUIContent(ppsLabel, EditorGUIUtility.IconContent("Camera Gizmo").image), ppsText, ppsStatus);

                //Warning in 2019.3 and 2019.4
#if UNITY_2019_3_OR_NEWER && !UNITY_2020_1_OR_NEWER
                if (RenderPipelineInstallation.CurrentPipeline == RenderPipelineInstallation.Pipeline.URP && PostProcessingInstallation.Config == PostProcessingInstallation.Configuration.PackageManager)
                {
                    EditorGUILayout.HelpBox("Support for the Post Processing Stack with URP will no longer be possible in Unity 2020.1!", MessageType.Warning);
                }
#endif
                if (RenderPipelineInstallation.CurrentPipeline == RenderPipelineInstallation.Pipeline.URP && (PostProcessingInstallation.Config == PostProcessingInstallation.Configuration.Integrated))
                {
                    EditorGUILayout.HelpBox("URP is installed, effects will use integrated post processing system", MessageType.None);
                }

                //Built-in render pipeline and LWRP require PPS
                if (PostProcessingInstallation.IS_INSTALLED == false && RenderPipelineInstallation.CurrentPipeline != RenderPipelineInstallation.Pipeline.URP)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (SCPE_GUI.DrawActionBox("Install " + PostProcessingInstallation.LATEST_COMPATIBLE_VERSION, EditorGUIUtility.IconContent("BuildSettings.Standalone.Small").image))
                        {
                            PostProcessingInstallation.InstallPackage();
                        }

                    }

                } //End if-installed
                //When installed but outdated
                else
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (PostProcessingInstallation.Config == PostProcessingInstallation.Configuration.PackageManager && PostProcessingInstallation.PPSVersionStatus == PostProcessingInstallation.VersionStatus.Outdated)
                        {
                            if (SCPE_GUI.DrawActionBox("Update to " + PostProcessingInstallation.LATEST_COMPATIBLE_VERSION, EditorGUIUtility.IconContent("BuildSettings.Standalone.Small").image))
                            {
                                PostProcessingInstallation.InstallPackage();
                            }
                        }
                    }
                }
            }
        }


        public class BoolSwitchGUI : Editor
        {
            const float height = 10f;
            const float width = 30f;
            const float tackExtrusion = 1f;

            static Color onColor = new Color(99f / 255f, 138f / 255f, 124f / 255f); //Staggart
            static Color fillColor = onColor * 0.66f;
            static float offBrightness = 0.33f;
            static Color offColor = new Color(offBrightness, offBrightness, offBrightness, 1f);

            static GUIStyle _textStyle;
            static GUIStyle textStyle
            {
                get
                {
                    if (_textStyle == null)
                    {
                        _textStyle = new GUIStyle(EditorStyles.miniLabel);
                        _textStyle.fontSize = 9;
                        _textStyle.alignment = TextAnchor.MiddleLeft;
                    }
                    return _textStyle;
                }
            }

            static GUIStyle _backGroundStyle;
            static GUIStyle backGroundStyle
            {
                get
                {
                    if (_backGroundStyle == null)
                    {
                        _backGroundStyle = new GUIStyle(EditorStyles.miniTextField);
                        _backGroundStyle.fixedWidth = width;
                        _backGroundStyle.fixedHeight = height;
                    }
                    return _backGroundStyle;
                }
            }

            private static bool Draw(bool value)
            {

                Rect rect = GUILayoutUtility.GetLastRect();
                float prefix = EditorGUIUtility.labelWidth;
                rect.x += prefix * 2f;
                rect.y += 2f;

                //Background functions as a button
                value = (GUI.Toggle(rect, value, "", backGroundStyle));

                //Fill with color when enabled
                Rect fillrect = new Rect(rect.x + 1, rect.y + 1, (value ? width : 0), height - 2);
                if (value == true) EditorGUI.DrawRect(fillrect, fillColor);

                //Shift tack from left to right
                Rect tackRect = new Rect(rect.x + (value ? width / 2f + 1f : 0f), rect.y - tackExtrusion, width / 2f, height + tackExtrusion + 1f);
                EditorGUI.DrawRect(tackRect, (value ? onColor : offColor));

                textStyle.padding = new RectOffset((value ? 19 : 4), 0, -2, 0);
                //GUI.Label(new Rect(rect.x, rect.y, width, height), value ? "ON" : "OFF", textStyle);
                //GUI.Label(new Rect(rect.x, rect.y, width, height), "≡", textStyle);

                return value;
            }

            public static bool Draw(bool value, string text)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(text);

                    value = BoolSwitchGUI.Draw(value);
                }

                return value;
            }

            public static bool Draw(bool value, GUIContent content)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(content);

                    value = BoolSwitchGUI.Draw(value);
                }

                return value;
            }


        }

#region Styles
        private static Texture _HelpIcon;
        public static Texture HelpIcon
        {
            get
            {
                if (_HelpIcon == null)
                {
                    _HelpIcon = EditorGUIUtility.FindTexture("_Help");
                }
                return _HelpIcon;
            }
        }

        private static Texture _InfoIcon;
        public static Texture InfoIcon
        {
            get
            {
                if (_InfoIcon == null)
                {
                    _InfoIcon = EditorGUIUtility.FindTexture("d_UnityEditor.InspectorWindow");
                }
                return _InfoIcon;
            }
        }

        private static GUIStyle _Footer;
        public static GUIStyle Footer
        {
            get
            {
                if (_Footer == null)
                {
                    _Footer = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
                    {
                        alignment = TextAnchor.LowerCenter,
                        wordWrap = true,
                        fontSize = 12
                    };
                }

                return _Footer;
            }
        }

        private static Texture _CheckMark;
        public static Texture CheckMark
        {
            get
            {
                if (_CheckMark == null)
                {
                    _CheckMark = EditorGUIUtility.IconContent("FilterSelectedOnly").image;
                }
                return _CheckMark;
            }
        }

        private static GUIStyle _StatusContent;
        public static GUIStyle StatusContent
        {
            get
            {
                if (_StatusContent == null)
                {
                    _StatusContent = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        fixedWidth = 200f,
                        imagePosition = ImagePosition.ImageLeft
                    };
                }

                return _StatusContent;
            }
        }

        private static GUIStyle _StatusBox;
        public static GUIStyle StatusBox
        {
            get
            {
                if (_StatusBox == null)
                {
                    _StatusBox = new GUIStyle(EditorStyles.textArea)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        fixedWidth = 200f
                    };
                }

                return _StatusBox;
            }
        }


        private static GUIStyle _LogText;
        public static GUIStyle LogText
        {
            get
            {
                if (_LogText == null)
                {
                    _LogText = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.UpperLeft,
                        richText = true,
                        wordWrap = true,
                        stretchHeight = false,
                        stretchWidth = false,
                        fontStyle = FontStyle.Normal,
                        fontSize = 11
                    };
                }

                return _LogText;
            }
        }

        private static GUIStyle _PathField;
        public static GUIStyle PathField
        {
            get
            {
                if (_PathField == null)
                {
                    _PathField = new GUIStyle(EditorStyles.textField)
                    {
                        alignment = TextAnchor.MiddleRight
                    };
                }

                return _PathField;
            }
        }

#region Toggles
        private static GUIStyle _ToggleButtonLeftNormal;
        public static GUIStyle ToggleButtonLeftNormal
        {
            get
            {
                if (_ToggleButtonLeftNormal == null)
                {
                    _ToggleButtonLeftNormal = new GUIStyle(EditorStyles.miniButtonLeft)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        stretchWidth = true,
                        richText = true,
                        wordWrap = false,
                        fixedHeight = 20f,
                        fixedWidth = 105f
                    };
                }

                return _ToggleButtonLeftNormal;
            }
        }
        private static GUIStyle _ToggleButtonLeftToggled;
        public static GUIStyle ToggleButtonLeftToggled
        {
            get
            {
                if (_ToggleButtonLeftToggled == null)
                {
                    _ToggleButtonLeftToggled = new GUIStyle(ToggleButtonLeftNormal);
                    _ToggleButtonLeftToggled.normal.background = _ToggleButtonLeftToggled.active.background;
                }

                return _ToggleButtonLeftToggled;
            }
        }

        private static GUIStyle _ToggleButtonRightNormal;
        public static GUIStyle ToggleButtonRightNormal
        {
            get
            {
                if (_ToggleButtonRightNormal == null)
                {
                    _ToggleButtonRightNormal = new GUIStyle(EditorStyles.miniButtonRight)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        stretchWidth = true,
                        richText = true,
                        wordWrap = false,
                        fixedHeight = 20f,
                        fixedWidth = 105f

                    };
                }

                return _ToggleButtonRightNormal;
            }
        }

        private static GUIStyle _ToggleButtonRightToggled;
        public static GUIStyle ToggleButtonRightToggled
        {
            get
            {
                if (_ToggleButtonRightToggled == null)
                {
                    _ToggleButtonRightToggled = new GUIStyle(ToggleButtonRightNormal);
                    _ToggleButtonRightToggled.normal.background = _ToggleButtonRightToggled.active.background;
                }

                return _ToggleButtonRightToggled;
            }
        }
#endregion

#region Buttons
        private static GUIStyle _Button;
        public static GUIStyle Button
        {
            get
            {
                if (_Button == null)
                {
                    _Button = new GUIStyle(UnityEngine.GUI.skin.button)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        stretchWidth = true,
                        richText = true,
                        wordWrap = true,
                        padding = new RectOffset()
                        {
                            left = 14,
                            right = 14,
                            top = 8,
                            bottom = 8
                        }
                    };
                }

                return _Button;
            }
        }

        private static GUIStyle _DocButton;
        public static GUIStyle DocButton
        {
            get
            {
                if (_DocButton == null)
                {
                    _DocButton = new GUIStyle(EditorStyles.miniButton)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        stretchWidth = false,
                        richText = true,
                        wordWrap = true,
                        fixedHeight = 16f,
                        fixedWidth = 55f,
                        margin = new RectOffset()
                        {
                            left = 0,
#if SRP_2019
                            right = 0,
#else
                            right = 73,
#endif
                            top = 0,
                            bottom = 0
                        }

                    };
                }

                return _DocButton;
            }
        }

        private static GUIStyle _ProgressButtonLeft;
        public static GUIStyle ProgressButtonLeft
        {
            get
            {
                if (_ProgressButtonLeft == null)
                {
                    _ProgressButtonLeft = new GUIStyle(EditorStyles.miniButtonLeft)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fixedHeight = 30f,
                        stretchWidth = true,
                        stretchHeight = true,
                        richText = true,
                        wordWrap = true,
                        fontSize = 12,
                        fontStyle = FontStyle.Normal
                    };
                }

                return _ProgressButtonLeft;
            }
        }

        private static GUIStyle _ProgressButtonRight;
        public static GUIStyle ProgressButtonRight
        {
            get
            {
                if (_ProgressButtonRight == null)
                {
                    _ProgressButtonRight = new GUIStyle(EditorStyles.miniButtonRight)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fixedHeight = 30f,
                        stretchWidth = true,
                        stretchHeight = true,
                        richText = true,
                        wordWrap = true,
                        fontSize = 12,
                        fontStyle = FontStyle.Bold
                    };
                }

                return _ProgressButtonRight;
            }
        }
#endregion

        private static GUIStyle _ProgressTab;
        public static GUIStyle ProgressTab
        {
            get
            {
                if (_ProgressTab == null)
                {
                    _ProgressTab = new GUIStyle(EditorStyles.toolbarButton)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fixedHeight = 25f,
                        stretchWidth = true,
                        stretchHeight = true,
                        richText = true,
                        wordWrap = true,
                        fontSize = 12,
                        fontStyle = FontStyle.Bold
                    };
                }

                return _ProgressTab;
            }
        }

        private static GUIStyle _Tab;
        public static GUIStyle Tab
        {
            get
            {
                if (_Tab == null)
                {
                    _Tab = new GUIStyle(EditorStyles.miniButtonMid)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        stretchWidth = true,
                        richText = true,
                        wordWrap = true,
                        fontSize = 12,
                        fontStyle = FontStyle.Bold,
                        fixedHeight = 30f,
                        padding = new RectOffset()
                        {
                            left = 14,
                            right = 14,
                            top = 8,
                            bottom = 8
                        }
                    };
                }

                return _Tab;
            }
        }

        private static GUIStyle _Header;
        public static GUIStyle Header
        {
            get
            {
                if (_Header == null)
                {
                    _Header = new GUIStyle(UnityEngine.GUI.skin.label)
                    {
                        richText = true,
                        alignment = TextAnchor.MiddleLeft,
                        wordWrap = true,
                        fontSize = 18,
                        fontStyle = FontStyle.Bold,
                        padding = new RectOffset()
                        {
                            left = 5,
                            right = 0,
                            top = 0,
                            bottom = 0
                        }
                    };
                }

                return _Header;
            }
        }
#endregion
    }
}