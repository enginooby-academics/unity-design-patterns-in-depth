// SC Post Effects
// Staggart Creations
// http://staggart.xyz

#if URP && PPS
#undef URP
#endif

using UnityEditor;
using UnityEngine;

namespace SCPE
{
    public class InstallerWindow : EditorWindow
    {
        //Window properties
        private static readonly int width = 450;
        private static readonly int height = 550;
        private Vector2 scrollPos;

        private static bool hasError = false;

        public enum Tab
        {
            Start,
            Install,
            Finish
        }

        public static Tab INSTALLATION_TAB
        {
            get { return (Tab)SessionState.GetInt("INSTALLATION_PROGRESS", 0); }
            set { SessionState.SetInt("INSTALLATION_PROGRESS", (int)value); }
        }

#if (!URP && !PPS) || SCPE_DEV
        [MenuItem("SC Post Effects Installer/Open", false, 0)]
#endif
        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(InstallerWindow), false, " Installer", true);

            editorWindow.titleContent.image = EditorGUIUtility.IconContent("_Popup").image;
            editorWindow.autoRepaintOnSceneChange = true;
            editorWindow.ShowAuxWindow();

            //Open somewhat in the center of the screen
            editorWindow.position = new Rect(Screen.width / 2, 175f, width, height);

            //Fixed size
            editorWindow.maxSize = new Vector2(width, height);
            editorWindow.minSize = new Vector2(width, height);

            Init();

            editorWindow.Show();
        }

        private static void Init()
        {
            Installer.Initialize();
            INSTALLATION_TAB = Tab.Start;
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnDisable()
        {
            //Incase installation fails halfway
            Installer.CURRENTLY_INSTALLING = false;
        }

        private void OnGUI()
        {
            if (INSTALLATION_TAB < 0) INSTALLATION_TAB = 0;

            if (EditorApplication.isCompiling)
            {
                this.ShowNotification(new GUIContent(" Compiling...", EditorGUIUtility.IconContent("cs Script Icon").image));
            }
            else
            {
                this.RemoveNotification();
            }

            //Header
            {
                if (SCPE_GUI.HeaderImg)
                {
                    Rect headerRect = new Rect(0, -5, width, SCPE_GUI.HeaderImg.height);
                    UnityEngine.GUI.DrawTexture(headerRect, SCPE_GUI.HeaderImg, ScaleMode.ScaleToFit);
                    GUILayout.Space(SCPE_GUI.HeaderImg.height - 10);
                }
                else
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("<b><size=24>SC Post Effects</size></b>\n<size=16>For Post Processing Stack</size>", SCPE_GUI.Header);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledGroupScope((int)INSTALLATION_TAB != 0))
                    {
                        GUILayout.Toggle((INSTALLATION_TAB == 0), new GUIContent("Pre-install"), SCPE_GUI.ProgressTab);
                    }
                    using (new EditorGUI.DisabledGroupScope((int)INSTALLATION_TAB != 1))
                    {
                        GUILayout.Toggle(((int)INSTALLATION_TAB == 1), "Installation", SCPE_GUI.ProgressTab);
                    }
                    using (new EditorGUI.DisabledGroupScope((int)INSTALLATION_TAB != 2))
                    {
                        GUILayout.Toggle(((int)INSTALLATION_TAB == 2), "Finish", SCPE_GUI.ProgressTab);
                    }
                }
            }

            GUILayout.Space(5f);

            //Body 
            Rect oRect = EditorGUILayout.GetControlRect();
            Rect bodyRect = new Rect(oRect.x + 10, 115, width - 20, height);

            GUILayout.BeginArea(bodyRect);
            {
                switch (INSTALLATION_TAB)
                {
                    case (Tab)0:
                        StartScreen();
                        break;
                    case (Tab)1:
                        InstallScreen();
                        break;
                    case (Tab)2:
                        FinalScreen();
                        break;
                }
            }
            GUILayout.EndArea();

            //Progress buttons

            Rect errorRect = new Rect(225, height - 95, width * 2.2f, 25);
            GUILayout.BeginArea(errorRect);

            if (hasError)
            {
                SCPE_GUI.DrawStatusString("Correct any errors to continue", SCPE_GUI.Status.Error, false);
            }
            GUILayout.EndArea();

            Rect buttonRect = new Rect(width / 2, height - 70, width / 2.2f, height - 25);
            GUILayout.BeginArea(buttonRect);

            using (new EditorGUILayout.HorizontalScope())
            {
                //EditorGUILayout.PrefixLabel(" ");

                //Disable buttons when installing
                using (new EditorGUI.DisabledGroupScope(Installer.CURRENTLY_INSTALLING))
                {
                    //Disable back button on first screen
                    using (new EditorGUI.DisabledGroupScope(INSTALLATION_TAB == Tab.Start))
                    {
                        if (GUILayout.Button("<size=16>‹</size> Back", SCPE_GUI.ProgressButtonLeft))
                        {
                            INSTALLATION_TAB--;
                        }
                    }
                    using (new EditorGUI.DisabledGroupScope(hasError))
                    {
                        string btnLabel = string.Empty;
                        if (INSTALLATION_TAB == Tab.Start) btnLabel = "Next <size=16>›</size>";
                        if (INSTALLATION_TAB == Tab.Install) btnLabel = "Install <size=16>›</size>";
                        if (INSTALLATION_TAB == Tab.Install && Installer.IS_INSTALLED) btnLabel = "Finish";
                        if (INSTALLATION_TAB == Tab.Finish) btnLabel = "Close";

                        if (GUILayout.Button(btnLabel, SCPE_GUI.ProgressButtonRight))
                        {
                            if (INSTALLATION_TAB == Tab.Start)
                            {
                                INSTALLATION_TAB = Tab.Install;
                                return;
                            }

                            //When pressing install again
                            if (INSTALLATION_TAB == Tab.Install)
                            {
                                if (Installer.IS_INSTALLED == false)
                                {
                                    Installer.Install();
                                }
                                else
                                {
                                    INSTALLATION_TAB = Tab.Finish;
                                }

                                return;
                            }

                            if (INSTALLATION_TAB == Tab.Finish)
                            {
                                Installer.PostInstall();
                                this.Close();
                            }

                        }
                    }
                }
            }
            GUILayout.EndArea();

            //Footer
            buttonRect = new Rect(width / 4, height - 30, width / 2.1f, height - 25);
            GUILayout.BeginArea(buttonRect);
            EditorGUILayout.LabelField("- Staggart Creations -", SCPE_GUI.Footer);
            GUILayout.EndArea();

        }

        private void StartScreen()
        {
            EditorGUILayout.HelpBox("\nThis wizard will guide you through the installation of the SC Post Effects package, and will ensure your project is set up correctly\n\nPress \"Next\" to continue...\n", MessageType.Info);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Pre-install checks", SCPE_GUI.Header);

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(EditorStyles.textArea))
            {
                EditorGUILayout.Space();

                SCPE_GUI.Installation.DrawPackageVersion();
                SCPE_GUI.Installation.DrawUnityVersion();
                SCPE_GUI.Installation.DrawPlatform();
                SCPE_GUI.Installation.DrawColorSpace();
                SCPE_GUI.Installation.DrawPipeline();
                SCPE_GUI.Installation.DrawPostProcessing();

                EditorGUILayout.Space();
            }

            //Validate for errors before allowing to continue
            hasError = !UnityVersionCheck.COMPATIBLE;
            //hasError = !Installer.IS_CORRECT_BASE_FOLDER;
            hasError = (PostProcessingInstallation.IS_INSTALLED == false);
            //hasError = !SCPE.UsingCompatiblePlatform;
            //hasError = RenderPipelineInstallation.LWRPVersionStatus != RenderPipelineInstallation.Version.Compatible;
        }

        private void InstallScreen()
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Options", SCPE_GUI.Header);

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledGroupScope(Installer.Demo.HAS_SCENE_PACKAGE == false))
                    {
#if URP && !PPS
                        using(new EditorGUI.DisabledGroupScope(true))
                        {
#endif
                            Installer.Settings.installDemoContent = SCPE_GUI.BoolSwitchGUI.Draw(Installer.Settings.installDemoContent, "Demo scenes");
#if URP && !PPS
                        }
#endif
                    }

#if PPS
                    //When installed
                    if (Installer.Demo.SCENES_INSTALLED)
                    {
                        SCPE_GUI.DrawStatusBox(null, "Installed", SCPE_GUI.Status.Ok, false);
                    }
                    //Not installed and missing source
                    if (!Installer.Demo.SCENES_INSTALLED)
                    {
                        if (Installer.Demo.HAS_SCENE_PACKAGE == false) SCPE_GUI.DrawStatusBox(null, "Missing", SCPE_GUI.Status.Warning, false);
                    }
#endif

                }
            }

#if URP && !PPS
            SCPE_GUI.DrawStatusBox(null, "Demo scenes are only compatible with the Post Processing Stack", SCPE_GUI.Status.Warning, false);
            Installer.Settings.installDemoContent = false;
#endif
#if PPS
            using (new EditorGUILayout.HorizontalScope())
            {
                if (Installer.Demo.HAS_SCENE_PACKAGE == true)
                {
                    EditorGUILayout.LabelField("Examples showing volume blending", EditorStyles.miniLabel);
                }
                if (Installer.Demo.HAS_SCENE_PACKAGE == false && Installer.Demo.SCENES_INSTALLED == false)
                {
                    EditorGUILayout.HelpBox("Also import the \"_DemoContents.unitypackage\" file to install.", MessageType.None);

                }
            }
#endif

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledGroupScope(Installer.Demo.HAS_SAMPLES_PACKAGE == false))
                    {
                        //EditorGUILayout.LabelField("Install demo content");

                        Installer.Settings.installSampleContent = SCPE_GUI.BoolSwitchGUI.Draw(Installer.Settings.installSampleContent, "Sample content");

                        if (Installer.Settings.installDemoContent) Installer.Settings.installSampleContent = true;
                        //Installer.Settings.installDemoContent = EditorGUILayout.Toggle(Installer.Settings.installDemoContent);
                    }

                    //When installed
                    if (Installer.Demo.SAMPLES_INSTALLED)
                    {
                        SCPE_GUI.DrawStatusBox(null, "Installed", SCPE_GUI.Status.Ok, false);
                    }
                    //Not installed and missing source
                    if (!Installer.Demo.SAMPLES_INSTALLED)
                    {
                        if (Installer.Demo.HAS_SAMPLES_PACKAGE == false) SCPE_GUI.DrawStatusBox(null, "Missing", SCPE_GUI.Status.Warning, false);
                    }

                }
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                if (Installer.Demo.HAS_SCENE_PACKAGE == true)
                {
                    EditorGUILayout.LabelField("Profiles and sample textures", EditorStyles.miniLabel);
                }
                if (Installer.Demo.HAS_SCENE_PACKAGE == false && Installer.Demo.SCENES_INSTALLED == false)
                {
                    EditorGUILayout.HelpBox("Also import the \"_Samples.unitypackage\" file to install.", MessageType.None);

                }
            }


            if (Installer.CURRENTLY_INSTALLING || Installer.IS_INSTALLED)
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Log", SCPE_GUI.Header);

                EditorGUILayout.Space();
                using (new EditorGUILayout.VerticalScope(EditorStyles.textArea, UnityEngine.GUILayout.MaxHeight(150f)))
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                    for (int i = 0; i < Installer.Log.NumItems; i++)
                    {
                        SCPE_GUI.DrawLogLine(Installer.Log.Read(i));
                    }

                    if (Installer.CURRENTLY_INSTALLING) scrollPos.y += 10f;

                    EditorGUILayout.EndScrollView();
                }
            }

        }

        private void FinalScreen()
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Installation complete", SCPE_GUI.Header);

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.Space();

                //Demo contents not installed, display option to delete package
                if (Installer.Settings.installDemoContent == false && Installer.Demo.HAS_SCENE_PACKAGE == true)
                {
                    using (new EditorGUILayout.HorizontalScope(EditorStyles.label))
                    {
                        Installer.Settings.deleteDemoContent = SCPE_GUI.BoolSwitchGUI.Draw(Installer.Settings.deleteDemoContent, "Delete demo package");
                    }
                }
                using (new EditorGUILayout.HorizontalScope(EditorStyles.label))
                {
                    Installer.Settings.setupCurrentScene = SCPE_GUI.BoolSwitchGUI.Draw(Installer.Settings.setupCurrentScene, "Add post processing to current scene");
                }

                EditorGUILayout.Space();
            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope())
            {
                //Support box
                EditorGUILayout.HelpBox("\nThe help window can be accessed through Help -> SC Post Effects\n\nYou can use this to quickly add post processing to a scene\n", MessageType.Info);
            }
        }
    }

}
