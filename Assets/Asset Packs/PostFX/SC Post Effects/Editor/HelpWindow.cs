// SC Post Effects
// Staggart Creations
// http://staggart.xyz

using UnityEngine;
using UnityEditor;

namespace SCPE
{
    public class HelpWindow : EditorWindow
    {

        [MenuItem("Help/SC Post Effects", false, 0)]
        public static void ExecuteMenuItem()
        {
            HelpWindow.ShowWindow();
        }

        //Window properties
        private static int width = 440;
        private static int height = 550;

        //Tabs
        private bool isTabSetup = true;
        private bool isTabInstallation = false;
        private bool isTabGettingStarted = false;
        private bool isTabSupport = false;

        //Check if latest version has been pulled from backend and package manager
        private static bool installationVerified;

        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow<HelpWindow>(true, "Help", true);
            //editorWindow.titleContent = new GUIContent(SCPE.ASSET_NAME);
            editorWindow.autoRepaintOnSceneChange = true;

            //Open somewhat in the center of the screen
            editorWindow.position = new Rect((Screen.width) / 2f, 175, width, height);

            //Fixed size
            editorWindow.maxSize = new Vector2(width, height);
            editorWindow.minSize = new Vector2(width, 200);

            editorWindow.Show();

        }

        private void SetWindowHeight(float height)
        {
            this.maxSize = new Vector2(width, height);
            this.minSize = new Vector2(width, height);
        }

        //Store values in the volatile SessionState
        static void InitInstallation()
        {
            //Installer.CheckRootFolder();
            PackageVersionCheck.CheckForUpdate();
            RenderPipelineInstallation.CheckInstallation();
            PostProcessingInstallation.CheckPackageInstallation();

            installationVerified = true;
        }

        void OnGUI()
        {
            DrawHeader();

            GUILayout.Space(5);
            DrawTabs();

            if (isTabSetup) DrawQuickSetup();

            if (isTabInstallation) DrawInstallation();

            if (isTabGettingStarted) DrawGettingStarted();

            if (isTabSupport) DrawSupport();

            //DrawActionButtons();

            DrawFooter();
        }

        void DrawHeader()
        {
            SCPE_GUI.DrawWindowHeader(width, height);

            GUILayout.Label("Version: " + SCPE.INSTALLED_VERSION, SCPE_GUI.Footer);
        }

        void DrawTabs()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Toggle(isTabSetup, "Quick Setup", SCPE_GUI.Tab))
            {
                isTabSetup = true;
                isTabInstallation = false;
                isTabGettingStarted = false;
                isTabSupport = false;
            }
            if (GUILayout.Toggle(isTabInstallation, "Installation", SCPE_GUI.Tab))
            {
                isTabSetup = false;
                isTabInstallation = true;
                isTabGettingStarted = false;
                isTabSupport = false;
            }

            if (GUILayout.Toggle(isTabGettingStarted, "Documentation", SCPE_GUI.Tab))
            {
                isTabSetup = false;
                isTabInstallation = false;
                isTabGettingStarted = true;
                isTabSupport = false;
            }

            if (GUILayout.Toggle(isTabSupport, "Support", SCPE_GUI.Tab))
            {
                isTabSetup = false;
                isTabInstallation = false;
                isTabGettingStarted = false;
                isTabSupport = true;
            }

            EditorGUILayout.EndHorizontal();
        }

        void DrawQuickSetup()
        {
            SetWindowHeight(375f);

            EditorGUILayout.HelpBox("\nThese actions will automatically configure your scene for use with the Post Processing Stack.\n", MessageType.Info);

            EditorGUILayout.Space();

            //Camera setup
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("Setup component on active camera");
                if (GUILayout.Button("Execute")) AutoSetup.SetupCamera();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            //Volume setup
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("Create a new global Post Processing volume");
                if (GUILayout.Button("Execute")) AutoSetup.SetupGlobalVolume();
            }
            EditorGUILayout.EndHorizontal();
        }

        void DrawInstallation()
        {
            if (!installationVerified) InitInstallation();

            SetWindowHeight(420f);

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {

                SCPE_GUI.Installation.DrawPackageVersion();
                SCPE_GUI.Installation.DrawUnityVersion();
                SCPE_GUI.Installation.DrawPlatform();
                SCPE_GUI.Installation.DrawColorSpace();
                SCPE_GUI.Installation.DrawPipeline();
                SCPE_GUI.Installation.DrawPostProcessing();
            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(" ");

                if (GUILayout.Button("Open installer"))
                {
                    InstallerWindow.ShowWindow();
                }
            }

        }

        void DrawGettingStarted()
        {
            SetWindowHeight(335);

            EditorGUILayout.HelpBox("Please view the documentation for further details about this package and its workings.", MessageType.Info);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("<b><size=12>Documentation</size></b>\n<i>Usage instructions</i>", SCPE_GUI.Button))
                {
                    Application.OpenURL(SCPE.DOC_URL);
                }
                if (GUILayout.Button("<b><size=12>Effect details</size></b>\n<i>View effect examples</i>", SCPE_GUI.Button))
                {
                    Application.OpenURL(SCPE.DOC_URL + "?section=effects");
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        void DrawSupport()
        {
            SetWindowHeight(375f);

            EditorGUILayout.HelpBox("\nIf you have any questions, or ran into issues, please get in touch.\n", MessageType.Info);

            EditorGUILayout.Space();

            //Buttons box
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("<b><size=12>Email</size></b>\n<i>Contact</i>", SCPE_GUI.Button))
                {
                    Application.OpenURL("mailto:contact@staggart.xyz");
                }
                if (GUILayout.Button("<b><size=12>Twitter</size></b>\n<i>Follow developments</i>", SCPE_GUI.Button))
                {
                    Application.OpenURL("https://twitter.com/search?q=staggart%20creations");
                }
                if (GUILayout.Button("<b><size=12>Forum</size></b>\n<i>Join the discussion</i>", SCPE_GUI.Button))
                {
                    Application.OpenURL(SCPE.FORUM_URL);
                }
            }
            EditorGUILayout.EndHorizontal();

            DrawActionButtons();
        }

        //TODO: Implement after Beta
        private void DrawActionButtons()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent("<size=12> Rate</size>", EditorGUIUtility.IconContent("d_Favorite").image), SCPE_GUI.Button)) SCPE.OpenStorePage();

            if (GUILayout.Button(new GUIContent("<size=12> Review</size>", EditorGUIUtility.IconContent("d_FilterByLabel").image), SCPE_GUI.Button)) SCPE.OpenStorePage();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void DrawFooter()
        {
            EditorGUILayout.LabelField("", UnityEngine.GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
            GUILayout.Label("- Staggart Creations -", SCPE_GUI.Footer);
        }

#region Styles
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
                        alignment = TextAnchor.MiddleCenter,
                        wordWrap = true,
                        fontSize = 18,
                        fontStyle = FontStyle.Bold
                    };
                }

                return _Header;
            }
        }
#endregion //Stylies

    }//SCPE_Window Class
}
