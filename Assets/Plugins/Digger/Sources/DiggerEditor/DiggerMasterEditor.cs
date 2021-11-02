using System;
using System.Collections.Generic;
using System.Linq;
using Digger.TerrainCutters;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Digger
{
    [CustomEditor(typeof(DiggerMaster))]
    public class DiggerMasterEditor : Editor
    {
        private DiggerMaster master;
        private DiggerSystem[] diggerSystems;

        private bool clicking;
        private bool keepingHeight;
        private float keptHeight;
        private bool warnedAboutPlayMode;

        private GameObject reticleSphere;
        private GameObject reticleHalfSphere;
        private GameObject reticleCube;
        private GameObject reticleCone;
        private GameObject reticleCylinder;

        private BrushType brush {
            get => (BrushType) EditorPrefs.GetInt("diggerMaster_brush", (int) BrushType.Sphere);
            set => EditorPrefs.SetInt("diggerMaster_brush", (int) value);
        }

        private ActionType action {
            get => (ActionType) EditorPrefs.GetInt("diggerMaster_action", (int) ActionType.Dig);
            set => EditorPrefs.SetInt("diggerMaster_action", (int) value);
        }

        private float opacity {
            get => EditorPrefs.GetFloat("diggerMaster_opacity", 0.3f);
            set => EditorPrefs.SetFloat("diggerMaster_opacity", Mathf.Clamp01(value));
        }

        private bool opacityIsTarget {
            get => EditorPrefs.GetBool("diggerMaster_opacityIsTarget", false);
            set => EditorPrefs.SetBool("diggerMaster_opacityIsTarget", value);
        }

        private float size {
            get => EditorPrefs.GetFloat("diggerMaster_size", 3f);
            set => EditorPrefs.SetFloat("diggerMaster_size", value);
        }

        private float depth {
            get => EditorPrefs.GetFloat("diggerMaster_depth", 0f);
            set => EditorPrefs.SetFloat("diggerMaster_depth", value);
        }

        private float coneHeight {
            get => EditorPrefs.GetFloat("diggerMaster_coneHeight", 6f);
            set => EditorPrefs.SetFloat("diggerMaster_coneHeight", value);
        }

        private bool upsideDown {
            get => EditorPrefs.GetBool("diggerMaster_upsideDown", false);
            set => EditorPrefs.SetBool("diggerMaster_upsideDown", value);
        }

        private int textureIndex {
            get => EditorPrefs.GetInt("diggerMaster_textureIndex", 0);
            set => EditorPrefs.SetInt("diggerMaster_textureIndex", value);
        }

        private MicroSplatPaintType paintType {
            get => (MicroSplatPaintType) EditorPrefs.GetInt("diggerMaster_microSplatPaintType",
                                                            (int) MicroSplatPaintType.Texture);
            set => EditorPrefs.SetInt("diggerMaster_microSplatPaintType", (int) value);
        }

        private bool cutDetails {
            get => EditorPrefs.GetBool("diggerMaster_cutDetails", true);
            set => EditorPrefs.SetBool("diggerMaster_cutDetails", value);
        }

        private int activeTab {
            get => EditorPrefs.GetInt("diggerMaster_activeTab", 0);
            set => EditorPrefs.SetInt("diggerMaster_activeTab", value);
        }

        private bool shortcutsEnabled {
            get => EditorPrefs.GetBool("diggerMaster_shortcutsEnabled", true);
            set => EditorPrefs.SetBool("diggerMaster_shortcutsEnabled", value);
        }

        private static int raycastLength {
            get => EditorPrefs.GetInt("diggerMaster_raycastLength", 2000);
            set => EditorPrefs.SetInt("diggerMaster_raycastLength", value);
        }

        private static string GetReticleLabel(string label)
        {
            if (GraphicsSettings.renderPipelineAsset == null) {
                return label;
            }

            if (GraphicsSettings.renderPipelineAsset.name.Contains("HDRenderPipeline") || GraphicsSettings.renderPipelineAsset.name.Contains("HDRP")) {
                return label + "HDRP";
            }

            return label + "SRP";
        }

        private GameObject ReticleSphere {
            get {
                if (!reticleSphere) {
                    var prefab = LoadAssetWithLabel(GetReticleLabel("Digger_SphereReticle"));
                    reticleSphere = Instantiate(prefab);
                    reticleSphere.hideFlags = HideFlags.HideAndDontSave;
                }

                return reticleSphere;
            }
        }

        private GameObject ReticleCube {
            get {
                if (!reticleCube) {
                    var prefab = LoadAssetWithLabel(GetReticleLabel("Digger_CubeReticle"));
                    reticleCube = Instantiate(prefab);
                    reticleCube.hideFlags = HideFlags.HideAndDontSave;
                }

                return reticleCube;
            }
        }

        private GameObject ReticleHalfSphere {
            get {
                if (!reticleHalfSphere) {
                    var prefab = LoadAssetWithLabel(GetReticleLabel("Digger_HalfSphereReticle"));
                    reticleHalfSphere = Instantiate(prefab);
                    reticleHalfSphere.hideFlags = HideFlags.HideAndDontSave;
                }

                return reticleHalfSphere;
            }
        }

        private GameObject ReticleCone {
            get {
                if (!reticleCone) {
                    var prefab = LoadAssetWithLabel(GetReticleLabel("Digger_ConeReticle"));
                    reticleCone = Instantiate(prefab);
                    reticleCone.hideFlags = HideFlags.HideAndDontSave;
                }

                return reticleCone;
            }
        }

        private GameObject ReticleCylinder {
            get {
                if (!reticleCylinder) {
                    var prefab = LoadAssetWithLabel(GetReticleLabel("Digger_CylinderReticle"));
                    reticleCylinder = Instantiate(prefab);
                    reticleCylinder.hideFlags = HideFlags.HideAndDontSave;
                }

                return reticleCylinder;
            }
        }

        private GameObject Reticle {
            get {
                if (action == ActionType.Reset) {
                    if (reticleSphere)
                        DestroyImmediate(reticleSphere);
                    if (reticleCube)
                        DestroyImmediate(reticleCube);
                    if (reticleHalfSphere)
                        DestroyImmediate(reticleHalfSphere);
                    if (reticleCone)
                        DestroyImmediate(reticleCone);
                    return ReticleCylinder;
                }

                switch (brush) {
                    case BrushType.HalfSphere:
                        if (reticleSphere)
                            DestroyImmediate(reticleSphere);
                        if (reticleCube)
                            DestroyImmediate(reticleCube);
                        if (reticleCylinder)
                            DestroyImmediate(reticleCylinder);
                        if (reticleCone)
                            DestroyImmediate(reticleCone);
                        return ReticleHalfSphere;
                    case BrushType.RoundedCube:
                        if (reticleSphere)
                            DestroyImmediate(reticleSphere);
                        if (reticleHalfSphere)
                            DestroyImmediate(reticleHalfSphere);
                        if (reticleCylinder)
                            DestroyImmediate(reticleCylinder);
                        if (reticleCone)
                            DestroyImmediate(reticleCone);
                        return ReticleCube;
                    case BrushType.Stalagmite:
                        if (reticleSphere)
                            DestroyImmediate(reticleSphere);
                        if (reticleHalfSphere)
                            DestroyImmediate(reticleHalfSphere);
                        if (reticleCylinder)
                            DestroyImmediate(reticleCylinder);
                        if (reticleCube)
                            DestroyImmediate(reticleCube);
                        return ReticleCone;
                    case BrushType.Sphere:
                    default:
                        if (reticleHalfSphere)
                            DestroyImmediate(reticleHalfSphere);
                        if (reticleCube)
                            DestroyImmediate(reticleCube);
                        if (reticleCylinder)
                            DestroyImmediate(reticleCylinder);
                        if (reticleCone)
                            DestroyImmediate(reticleCone);
                        return ReticleSphere;
                }
            }
        }

        private TerrainMaterialType MaterialType {
            get { return diggerSystems.Select(digger => digger.MaterialType).FirstOrDefault(); }
        }

        public void OnEnable()
        {
            master = (DiggerMaster) target;
            CheckDiggerVersion();
            diggerSystems = FindObjectsOfType<DiggerSystem>();
            foreach (var diggerSystem in diggerSystems) {
                DiggerSystemEditor.Init(diggerSystem, false);
            }

            SceneView.duringSceneGui -= OnScene;
            SceneView.duringSceneGui += OnScene;
            Undo.undoRedoPerformed -= UndoCallback;
            Undo.undoRedoPerformed += UndoCallback;

            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        public void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoCallback;
            SceneView.duringSceneGui -= OnScene;

            if (reticleSphere)
                DestroyImmediate(reticleSphere);
            if (reticleHalfSphere)
                DestroyImmediate(reticleHalfSphere);
            if (reticleCube)
                DestroyImmediate(reticleCube);
            if (reticleCone)
                DestroyImmediate(reticleCone);
            if (reticleCylinder)
                DestroyImmediate(reticleCylinder);
        }

        private static void UndoCallback()
        {
            var diggers = FindObjectsOfType<DiggerSystem>();
            foreach (var digger in diggers) {
                digger.DoUndo();
            }
        }

        public override void OnInspectorGUI()
        {
            activeTab = GUILayout.Toolbar(activeTab, new[]
            {
                EditorGUIUtility.TrTextContentWithIcon("Edit", "d_TerrainInspector.TerrainToolSplat"),
                EditorGUIUtility.TrTextContentWithIcon("Settings", "d_TerrainInspector.TerrainToolSettings"),
                EditorGUIUtility.TrTextContentWithIcon("Help", "_Help")
            });
            switch (activeTab) {
                case 0:
                    OnInspectorGUIEditTab();
                    break;
                case 1:
                    OnInspectorGUISettingsTab();
                    break;
                case 2:
                    OnInspectorGUIHelpTab();
                    break;
                default:
                    activeTab = 0;
                    break;
            }

            HandleShortcuts();
        }

        private void HandleShortcuts()
        {
            if (!shortcutsEnabled)
                return;

            var current = Event.current;
            if (current.type != EventType.KeyDown)
                return;

            switch (current.keyCode) {
                case KeyCode.KeypadMinus:
                    this.size -= 0.5f;
                    current.Use();
                    Repaint();
                    break;
                case KeyCode.KeypadPlus:
                    this.size += 0.5f;
                    current.Use();
                    Repaint();
                    break;
                case KeyCode.KeypadDivide:
                    this.opacity -= 0.1f;
                    current.Use();
                    Repaint();
                    break;
                case KeyCode.KeypadMultiply:
                    this.opacity += 0.1f;
                    current.Use();
                    Repaint();
                    break;
                case KeyCode.N:
                    switch (this.action) {
                        case ActionType.Dig:
                            this.action = ActionType.Add;
                            break;
                        case ActionType.Add:
                            this.action = ActionType.Paint;
                            break;
                        case ActionType.Paint:
                            this.action = ActionType.PaintHoles;
                            break;
                        case ActionType.PaintHoles:
                            this.action = ActionType.Smooth;
                            break;
                        case ActionType.Smooth:
                            this.action = ActionType.BETA_Sharpen;
                            break;
                        case ActionType.BETA_Sharpen:
                            this.action = ActionType.Reset;
                            break;
                        case ActionType.Reset:
                            this.action = ActionType.Dig;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    current.Use();
                    Repaint();
                    break;
                case KeyCode.B:
                    switch (this.brush) {
                        case BrushType.Sphere:
                            this.brush = BrushType.RoundedCube;
                            break;
                        case BrushType.RoundedCube:
                            this.brush = BrushType.HalfSphere;
                            break;
                        case BrushType.HalfSphere:
                            this.brush = BrushType.Stalagmite;
                            break;
                        case BrushType.Stalagmite:
                            this.brush = BrushType.Sphere;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    current.Use();
                    Repaint();
                    break;
            }
        }

        public void OnInspectorGUIHelpTab()
        {
            EditorGUILayout.HelpBox("Thanks for using Digger!\n\n" +
                                    "Need help? Checkout the documentation and join us on Discord to get support!\n\n" +
                                    "Want to help the developer and support the project? Please write a review on the Asset Store!",
                                    MessageType.Info);


            if (GUILayout.Button("Open documentation")) {
                Application.OpenURL("https://ofux.github.io/Digger-Documentation/");
            }

            #region DiggerPRO

            if (GUILayout.Button("Write a review")) {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/terrain/digger-pro-149753");
                //Application.OpenURL("https://assetstore.unity.com/packages/tools/terrain/digger-terrain-caves-overhangs-135178");
            }

            #endregion

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Support is on Discord:", EditorStyles.boldLabel, GUILayout.Width(140));
            var style = new GUIStyle(EditorStyles.textField);
            EditorGUILayout.SelectableLabel("https://discord.gg/C2X6C6s", style, GUILayout.Height(18));
            EditorGUILayout.EndHorizontal();
        }

        public void OnInspectorGUISettingsTab()
        {
            EditorGUILayout.LabelField("Global Settings", EditorStyles.boldLabel);
            master.SceneDataFolder = EditorGUILayout.TextField("Scene data folder", master.SceneDataFolder);
            EditorGUILayout.HelpBox($"Digger data for this scene can be found in {master.SceneDataPath}",
                                    MessageType.Info);
            EditorGUILayout.HelpBox(
                "Don\'t forget to backup this folder (including the \".internal\" folder) as well when you backup your project.",
                MessageType.Warning);
            EditorGUILayout.Space();

            shortcutsEnabled = EditorGUILayout.Toggle("Enable shortcuts", shortcutsEnabled);
            if (shortcutsEnabled) {
                EditorGUILayout.HelpBox("Change Brush: B\n" +
                                        "Change Action: N\n" +
                                        "Brush size: keypad - or +\n" +
                                        "Opacity: keypad / or *",
                                        MessageType.Info);
            }

            EditorGUILayout.Space();

            var showUnderlyingObjects = EditorGUILayout.Toggle("Show underlying objects", master.ShowUnderlyingObjects);
            if (showUnderlyingObjects != master.ShowUnderlyingObjects) {
                master.ShowUnderlyingObjects = showUnderlyingObjects;
                var diggers = FindObjectsOfType<DiggerSystem>();
                foreach (var digger in diggers) {
                    digger.ShowDebug = true;
                    foreach (Transform child in digger.transform) {
                        child.gameObject.hideFlags = showUnderlyingObjects
                            ? HideFlags.None
                            : HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                    }
                }

                EditorApplication.DirtyHierarchyWindowSorting();
                EditorApplication.RepaintHierarchyWindow();
                if (showUnderlyingObjects) {
                    EditorUtility.DisplayDialog("Please reload the scene",
                                                "You need to reload the scene (or restart Unity) in order to see chunk objects under LOD Groups.",
                                                "Ok");
                }
            }

            EditorGUILayout.HelpBox(
                "Enable this to reveal all objects created by Digger in the hierarchy. Digger creates objects as children of your terrain(s).",
                MessageType.Info);
            EditorGUILayout.Space();

            var newLayer = EditorGUILayout.LayerField("Layer", master.Layer);
            EditorGUILayout.HelpBox("You can change the layer of meshes/objects generated by Digger.",
                                    MessageType.Info);
            if (newLayer != master.Layer && EditorUtility.DisplayDialog(
                $"Set new layer: {LayerMask.LayerToName(newLayer)}",
                "Digger must recompute internal chunks for the new layer setting to take effect.\n\n" +
                "This operation is not destructive, but can be long.\n\n" +
                "Do you want to proceed?", "Yes", "Cancel")) {
                master.Layer = newLayer;
                DoReload();
            }

            var newTag = EditorGUILayout.TagField("Tag", master.ChunksTag);
            EditorGUILayout.HelpBox("You can change the tag of objects generated by Digger.",
                                    MessageType.Info);
            if (newTag != master.ChunksTag && EditorUtility.DisplayDialog(
                $"Set new tag: {newTag}",
                "Digger must recompute internal chunks for the new tag setting to take effect.\n\n" +
                "This operation is not destructive, but can be long.\n\n" +
                "Do you want to proceed?", "Yes", "Cancel")) {
                master.ChunksTag = newTag;
                DoReload();
            }

            EditorGUILayout.Space();
            var newChunkSize = EditorGUILayout.IntPopup("Chunk size", master.ChunkSize, new[] {"16", "32", "64"},
                                                        new[] {17, 33, 65});
            EditorGUILayout.HelpBox(
                "Lowering the size of chunks improves real-time editing performance, but also creates more meshes.",
                MessageType.Info);
            if (newChunkSize != master.ChunkSize && EditorUtility.DisplayDialog("Change chunk size & clear everything",
                                                                                "All modifications must be cleared for new chunk size to take effect.\n\n" +
                                                                                "THIS WILL CLEAR ALL MODIFICATIONS MADE WITH DIGGER.\n" +
                                                                                "This operation CANNOT BE UNDONE.\n\n" +
                                                                                "Are you sure you want to proceed?", "Yes, clear it", "Cancel")) {
                master.ChunkSize = newChunkSize;
                DoClear();
            }

            EditorGUILayout.Space();
            var newResolutionMult = EditorGUILayout.IntPopup("Resolution", master.ResolutionMult,
                                                             new[] {"x1", "x2", "x4", "x8"}, new[] {1, 2, 4, 8});
            if (newResolutionMult != master.ResolutionMult && EditorUtility.DisplayDialog(
                "Change resolution & clear everything",
                "All modifications must be cleared for new resolution to take effect.\n\n" +
                "THIS WILL CLEAR ALL MODIFICATIONS MADE WITH DIGGER.\n" +
                "This operation CANNOT BE UNDONE.\n\n" +
                "Are you sure you want to proceed?", "Yes, clear it", "Cancel")) {
                master.ResolutionMult = newResolutionMult;
                DoClear();
            }

            EditorGUILayout.HelpBox(
                "If your heightmaps have a low resolution, you might want to set this to x2, x4 or x8 to generate " +
                "meshes with higher resolution and finer details. " +
                "However, keep in mind that the higher the resolution is, the more performance will be impacted.",
                MessageType.Info);

            EditorGUILayout.Space();
            var newEnableOcclusionCulling =
                EditorGUILayout.Toggle("Enable Occlusion Culling", master.EnableOcclusionCulling);
            if (newEnableOcclusionCulling != master.EnableOcclusionCulling && EditorUtility.DisplayDialog(
                $"{(newEnableOcclusionCulling ? "Enable" : "Disable")} Occlusion Culling",
                "Digger must recompute internal chunks for the new Occlusion Culling setting to take effect.\n\n" +
                "This operation is not destructive, but can be long.\n\n" +
                "Do you want to proceed?", "Yes", "Cancel")) {
                master.EnableOcclusionCulling = newEnableOcclusionCulling;
                DoReload();
            }

            EditorGUILayout.HelpBox(
                "Occlusion Culling is not very stable on some versions of Unity, and Unity throws some errors (while it shouldn't) " +
                "when baking Occlusion Culling if some meshes have multiple materials for a single submesh. This is why it is disabled by default on Digger.\n\n" +
                "Note: this setting has no effect if DiggerMasterRuntime (Digger PRO only) is present in the scene as chunks are not static anymore in such case.",
                MessageType.Info);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("LOD Settings", EditorStyles.boldLabel);
            var newCreateLODs = EditorGUILayout.Toggle("Enable LODs generation", master.CreateLODs);
            if (newCreateLODs != master.CreateLODs && EditorUtility.DisplayDialog(
                $"{(newCreateLODs ? "Enable" : "Disable")} LODs generation",
                "Digger must recompute internal chunks for the new LODs generation setting to take effect.\n\n" +
                "This operation is not destructive, but can be long.\n\n" +
                "Do you want to proceed?", "Yes", "Cancel")) {
                master.CreateLODs = newCreateLODs;
                DoReload();
            }

            if (master.CreateLODs) {
                #region DiggerPRO

                if (FindObjectOfType<DiggerMasterRuntime>()) {
                    EditorGUILayout.HelpBox(
                        "It is recommended to disable LODs generation when using Digger at runtime to improve generation speed.",
                        MessageType.Warning);
                }

                #endregion

                EditorGUILayout.LabelField("Screen Relative Transition Height of LODs:");
                master.ScreenRelativeTransitionHeightLod0 = EditorGUILayout.Slider("    LOD 0",
                                                                                   master.ScreenRelativeTransitionHeightLod0, 0.002f, 0.9f);
                master.ScreenRelativeTransitionHeightLod1 = EditorGUILayout.Slider("    LOD 1",
                                                                                   master.ScreenRelativeTransitionHeightLod1, 0.001f,
                                                                                   master.ScreenRelativeTransitionHeightLod0 - 0.001f);
                master.ColliderLodIndex = EditorGUILayout.IntSlider(
                    new GUIContent("Collider LOD",
                                   "LOD that will hold the collider. Increasing it will produce mesh colliders with fewer vertices but also less accuracy."),
                    master.ColliderLodIndex, 0, 2);
            }

            EditorGUILayout.Space();
            OnInspectorGUIClearButtons();
        }

        public void OnInspectorGUIEditTab()
        {
            var diggerSystem = FindObjectOfType<DiggerSystem>();
            if (diggerSystem) {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Editing", EditorStyles.boldLabel);

                action = (ActionType) EditorGUILayout.EnumPopup(new GUIContent("Action", shortcutsEnabled ? "Shortcut: N" : ""), action);

                if (MaterialType == TerrainMaterialType.MicroSplat && action == ActionType.Paint) {
                    paintType = (MicroSplatPaintType) EditorGUILayout.EnumPopup("Type", paintType);
                    opacityIsTarget = EditorGUILayout.Toggle("Opacity is target", opacityIsTarget);
                } else {
                    paintType = MicroSplatPaintType.Texture;
                    opacityIsTarget = false;
                }

                if (action != ActionType.Reset && action != ActionType.Smooth && action != ActionType.BETA_Sharpen) {
                    brush = (BrushType) EditorGUILayout.EnumPopup(new GUIContent("Brush", shortcutsEnabled ? "Shortcut: B" : ""), brush);
                } else if (action == ActionType.Smooth || action == ActionType.BETA_Sharpen) {
                    brush = BrushType.Sphere;
                }

                size = EditorGUILayout.Slider(new GUIContent("Brush Size", shortcutsEnabled ? "Shortcut: keypad - or +" : ""), size, 0.5f, 20f);

                if (action != ActionType.Reset && action != ActionType.Smooth && action != ActionType.BETA_Sharpen &&
                    brush == BrushType.Stalagmite) {
                    coneHeight = EditorGUILayout.Slider("Stalagmite Height", coneHeight, 1f, 10f);
                    upsideDown = EditorGUILayout.Toggle("Upside Down", upsideDown);
                }

                if (action != ActionType.Reset) {
                    opacity = EditorGUILayout.Slider(new GUIContent("Opacity", shortcutsEnabled ? "Shortcut: keypad / or *" : ""), opacity, 0f, 1f);
                    depth = EditorGUILayout.Slider("Depth", depth, -size, size);
                }

                if (action != ActionType.Reset && action != ActionType.Smooth && action != ActionType.BETA_Sharpen && action != ActionType.PaintHoles) {
                    if (paintType == MicroSplatPaintType.Texture) {
                        GUIStyle gridList = "GridList";
                        var errorMessage = new GUIContent("No texture to display.\n\n" +
                                                          "You have to add at least one layer to the terrain with " +
                                                          "both a texture and a normal map. Then, click on 'Sync & Refresh'.");
                        if (MaterialType == TerrainMaterialType.CTS) {
                            errorMessage =
                                new GUIContent(
                                    "CTS does not support vertex control. You can't choose which texture to paint. Texture will be picked-up from the terrain above.");
                        }

                        textureIndex = EditorUtils.AspectSelectionGrid(textureIndex, diggerSystem.TerrainTextures, 64,
                                                                       gridList, errorMessage);
                    }
                }

                if (action != ActionType.Reset && action != ActionType.PaintHoles) {
                    cutDetails = false; // handled by terrain holes feature
                }

                if (action == ActionType.Paint && !opacityIsTarget) {
                    EditorGUILayout.HelpBox(
                        "Hold Ctrl to remove the texture instead of adding it.",
                        MessageType.Info);
                }

                if (action == ActionType.PaintHoles) {
                    EditorGUILayout.HelpBox(
                        "Hold Ctrl to undo hole painting.",
                        MessageType.Info);
                }

                EditorGUILayout.Space();
                keepingHeight = EditorGUILayout.ToggleLeft("Constrain reticle to given altitude", keepingHeight);
                keptHeight = EditorGUILayout.FloatField("Reticle constrained altitude", keptHeight);
                EditorGUILayout.HelpBox(
                    "Press Shift to pick current reticle height.",
                    MessageType.Info);

                EditorGUILayout.Space();
                raycastLength = EditorGUILayout.IntSlider("Reticle raycast length", raycastLength, 1, 2000);
            }

            EditorGUILayout.Space();
            OnInspectorGUIClearButtons();
        }

        private void OnInspectorGUIClearButtons()
        {
            EditorGUILayout.LabelField("Utils", EditorStyles.boldLabel);
            var col = GUI.backgroundColor;

            if (GUILayout.Button(new GUIContent("Save Meshes as Assets", "Save meshes as assets so they are not serialized within the scene file. " +
                                                                         "This will make the scene file a lot lighter and can reduce loading time."))) {
                SaveMeshesAsAssets();
            }

            GUI.backgroundColor = new Color(0.47f, 1f, 0.46f);
            var doReload = GUILayout.Button(new GUIContent("Sync with terrain(s) & Refresh", "Synchronize Digger with terrain data and recompute " +
                                                                                             "all modifications made with Digger. Press this button if you made any " +
                                                                                             "change to your terrain(s), like raising or lowering height, changing textures, etc.")) &&
                           EditorUtility.DisplayDialog("Sync with terrain(s) & Refresh",
                                                       "This will sync Digger with terrain(s) data and recompute all modifications made with Digger. " +
                                                       "This operation is not destructive, but can be long.\n\n" +
                                                       "Important: if you used the 'Save Meshes as Assets' feature, you will have to execute it again after the refresh.\n\n" +
                                                       "Are you sure you want to proceed?",
                                                       "Yes, go ahead", "Cancel");
            if (doReload) {
                DoReload();
            }

            GUI.backgroundColor = new Color(1f, 0.55f, 0.57f);
            var doClear = GUILayout.Button("Clear") && EditorUtility.DisplayDialog("Clear",
                                                                                   "This will clear all modifications made with Digger.\n" +
                                                                                   "This operation CANNOT BE UNDONE.\n\n" +
                                                                                   "Are you sure you want to proceed?", "Yes, clear it", "Cancel");
            if (doClear) {
                DoClear();
            }

            GUI.backgroundColor = col;
        }

        private static void DoClear()
        {
            var diggers = FindObjectsOfType<DiggerSystem>();

            try {
                AssetDatabase.StartAssetEditing();
                foreach (var digger in diggers) {
                    digger.Clear();
                }
            }
            finally {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.Refresh();

            try {
                AssetDatabase.StartAssetEditing();
                foreach (var digger in diggers) {
                    DiggerSystemEditor.Init(digger, true);
                    Undo.ClearUndo(digger);
                }
            }
            finally {
                AssetDatabase.StopAssetEditing();
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            GUIUtility.ExitGUI();
        }

        private static void DoReload()
        {
            var diggers = FindObjectsOfType<DiggerSystem>();
            try {
                AssetDatabase.StartAssetEditing();
                foreach (var digger in diggers) {
                    DiggerSystemEditor.Init(digger, true);
                    Undo.ClearUndo(digger);
                }
            }
            finally {
                AssetDatabase.StopAssetEditing();
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            GUIUtility.ExitGUI();
        }

        private void OnScene(SceneView sceneview)
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var e = Event.current;
            if (e.type == EventType.Layout || e.type == EventType.Repaint) {
                HandleUtility.AddDefaultControl(controlId);
                return;
            }

            if (!clicking && !e.alt && e.type == EventType.MouseDown && e.button == 0) {
                clicking = true;
            } else if (clicking && (e.type == EventType.MouseUp || e.type == EventType.MouseLeaveWindow ||
                                    (e.isKey && !e.control && !e.shift) ||
                                    e.alt || EditorWindow.mouseOverWindow == null ||
                                    EditorWindow.mouseOverWindow.GetType() != typeof(SceneView))) {
                clicking = false;
                if (!Application.isPlaying) {
                    foreach (var diggerSystem in diggerSystems) {
                        diggerSystem.PersistAndRecordUndo(false, action == ActionType.Reset);
                    }
                }
            }

            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            var hit = GetIntersectionWithTerrainOrDigger(ray);

            if (hit.HasValue) {
                var p = hit.Value.point + depth * ray.direction.normalized;

                if (e.shift) {
                    keptHeight = p.y;
                    Repaint();
                }

                if (keepingHeight) {
                    p.y = keptHeight;
                }

                UpdateReticlePosition(p);
                var hitTerrain = GetIntersectionWithTerrain(ray);

                if (clicking && IsActionAllowedHere(hit.Value, hitTerrain)) {
                    if (Application.isPlaying) {
                        if (!warnedAboutPlayMode) {
                            warnedAboutPlayMode = true;
                            EditorUtility.DisplayDialog("Edit in play mode not allowed",
                                                        "Terrain cannot be edited by Digger while playing.\n\n" +
                                                        "Note for Digger PRO: you *can* use DiggerMasterRuntime to edit the terrain while playing (so you can " +
                                                        "test your gameplay), but modifications made in play mode won't be persisted.", "Ok");
                        }
                    } else {
                        warnedAboutPlayMode = false;
                        foreach (var diggerSystem in diggerSystems) {
                            diggerSystem.Modify(brush, action,
                                                (action == ActionType.Paint || action == ActionType.PaintHoles) && e.control ? -opacity : opacity, p, size, coneHeight,
                                                upsideDown,
                                                GetFixedTextureIndex(),
                                                cutDetails, opacityIsTarget);
                        }
                    }
                }

                HandleUtility.Repaint();
            }

            HandleShortcuts();
        }

        private int GetFixedTextureIndex()
        {
            if (paintType == MicroSplatPaintType.Wetness) {
                return 28;
            } else if (paintType == MicroSplatPaintType.Puddles) {
                return 29;
            } else if (paintType == MicroSplatPaintType.Stream) {
                return 30;
            } else if (paintType == MicroSplatPaintType.Lava) {
                return 31;
            } else {
                return textureIndex;
            }
        }

        private bool IsActionAllowedHere(RaycastHit hit, RaycastHit? hitTerrain)
        {
            // Smooth action and Sharpen action can be done only on Digger meshes (not on the terrain)
            return action != ActionType.Smooth && action != ActionType.BETA_Sharpen ||
                   !(hit.collider is TerrainCollider) &&
                   (!hitTerrain.HasValue || Vector3.Distance(hitTerrain.Value.point, hit.point) > 0.2f);
        }

        private void UpdateReticlePosition(Vector3 position)
        {
            var reticle = Reticle.transform;
            reticle.position = position;
            reticle.localScale = 1.9f * size * Vector3.one;
            reticle.rotation = Quaternion.identity;
            if (action == ActionType.Reset) {
                reticle.localScale += 1000f * Vector3.up;
            } else if (brush == BrushType.Stalagmite) {
                reticle.localScale = new Vector3(2f * size, 1f * coneHeight, 2f * size);
                if (upsideDown) {
                    reticle.rotation = Quaternion.AngleAxis(180f, Vector3.right);
                }
            }
        }

        private static RaycastHit? GetIntersectionWithTerrainOrDigger(Ray ray)
        {
            if (Physics.Raycast(ray, out var hit, raycastLength, Physics.DefaultRaycastLayers,
                                      QueryTriggerInteraction.Ignore)) {
                return hit;
            }

            return null;
        }

        private static RaycastHit? GetIntersectionWithTerrain(Ray ray)
        {
            var hits = Physics.RaycastAll(ray, raycastLength, Physics.DefaultRaycastLayers,
                                          QueryTriggerInteraction.Ignore);
            foreach (var hit in hits) {
                if (hit.transform.GetComponent<Terrain>() != null) {
                    return hit;
                }
            }

            return null;
        }

        [MenuItem("Tools/Digger/Setup terrains", false, 1)]
        public static void SetupTerrains()
        {
            if (!FindObjectOfType<DiggerMaster>()) {
                var goMaster = new GameObject("Digger Master");
                goMaster.transform.localPosition = Vector3.zero;
                goMaster.transform.localRotation = Quaternion.identity;
                goMaster.transform.localScale = Vector3.one;
                var master = goMaster.AddComponent<DiggerMaster>();
                master.CreateDirs();
            }

            var isCTS = false;
            var lightmapStaticWarn = false;
            var terrains = FindObjectsOfType<Terrain>();
            try {
                AssetDatabase.StartAssetEditing();

                foreach (var terrain in terrains) {
                    var existingDiggers = terrain.gameObject.GetComponentsInChildren<DiggerSystem>();
                    if (existingDiggers.Count(system => system.Terrain.GetInstanceID() == terrain.GetInstanceID()) ==
                        0) {
                        var go = new GameObject("Digger");
                        go.transform.parent = terrain.transform;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localRotation = Quaternion.identity;
                        go.transform.localScale = Vector3.one;
                        var digger = go.AddComponent<DiggerSystem>();
                        DiggerSystemEditor.Init(digger, true);
                        isCTS = isCTS || digger.MaterialType == TerrainMaterialType.CTS;
                        lightmapStaticWarn =
                            lightmapStaticWarn || GameObjectUtility.GetStaticEditorFlags(terrain.gameObject).HasFlag(StaticEditorFlags.ContributeGI);
                    }
                }
            }
            finally {
                AssetDatabase.StopAssetEditing();
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            if (lightmapStaticWarn) {
                const string title = "Warning: Contribute GI is enabled";
                const string text = "It is recommended to disable 'Contribute GI' on terrains " +
                           "when using Digger. Otherwise there might be a visual difference between " +
                           "Digger meshes and the terrains.\n\n" +
                           "To disable Global Illumination (GI) on a terrain, go to terrain settings and uncheck " +
                           "'Contribute GI'.";
                if (!EditorUtility.DisplayDialog(title, text, "Ok", "Terrain settings?")) {
                    Application.OpenURL("https://docs.unity3d.com/Manual/terrain-OtherSettings.html");
                }
            }

            if (isCTS) {
                EditorUtility.DisplayDialog("Warning - CTS",
                                            "Digger has detected CTS on your terrain(s) and has been setup accordingly.\n\n" +
                                            "You may have to close the scene and open it again (or restart Unity) to " +
                                            "force it to refresh before using Digger.", "Ok");
            }
        }

        #region DiggerPRO

        [MenuItem("Tools/Digger/Setup for runtime", false, 2)]
        public static void SetupRuntimeScripts()
        {
            SetupTerrains();

            if (!FindObjectOfType<DiggerMasterRuntime>()) {
                var goMaster = new GameObject("Digger Master Runtime");
                goMaster.transform.localPosition = Vector3.zero;
                goMaster.transform.localRotation = Quaternion.identity;
                goMaster.transform.localScale = Vector3.one;
                goMaster.AddComponent<DiggerMasterRuntime>();

                var diggers = FindObjectsOfType<DiggerSystem>();
                foreach (var digger in diggers) {
                    digger.UpdateStaticEditorFlags();
                }
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        [MenuItem("Tools/Digger/Setup NavMeshComponents", false, 3)]
        public static void SetupNavMeshRuntimeScripts()
        {
            SetupRuntimeScripts();

            if (!FindObjectOfType<DiggerNavMeshRuntime>()) {
                var goMaster = FindObjectOfType<DiggerMasterRuntime>();
                goMaster.gameObject.AddComponent<DiggerNavMeshRuntime>();
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        #endregion

        [MenuItem("Tools/Digger/Remove Digger from the scene", false, 4)]
        public static void RemoveDiggerFromTerrains()
        {
            var confirm = EditorUtility.DisplayDialog("Remove Digger from the scene",
                                                      "You are about to completely remove Digger from the scene and clear all related Digger data.\n\n" +
                                                      "This operation CANNOT BE UNDONE.\n\n" +
                                                      "Are you sure you want to proceed?", "Yes, remove Digger", "Cancel");
            if (!confirm)
                return;

            var terrains = FindObjectsOfType<Terrain>();
            foreach (var terrain in terrains) {
                var digger = terrain.gameObject.GetComponentInChildren<DiggerSystem>();
                if (digger) {
                    digger.Clear();
                    DestroyImmediate(digger.gameObject);
                }
            }

            var diggerMaster = FindObjectOfType<DiggerMaster>();
            if (diggerMaster) {
                DestroyImmediate(diggerMaster.gameObject);
            }

            #region DiggerPRO

            var diggerMasterRuntime = FindObjectOfType<DiggerMasterRuntime>();
            if (diggerMasterRuntime) {
                DestroyImmediate(diggerMasterRuntime.gameObject);
            }

            #endregion

            AssetDatabase.Refresh();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        public static void LoadAllChunks(Scene scene)
        {
            var diggers = FindObjectsOfTypeInScene<DiggerSystem>(scene);
            foreach (var diggerSystem in diggers) {
                DiggerSystemEditor.Init(diggerSystem, false);
                Undo.ClearUndo(diggerSystem);
            }
        }

        public static void OnEnterPlayMode(Scene scene)
        {
            var diggers = FindObjectsOfTypeInScene<DiggerSystem>(scene);
            foreach (var digger in diggers) {
                Undo.ClearUndo(digger);
            }

            var cutters = FindObjectsOfTypeInScene<TerrainCutter>(scene);
            foreach (var cutter in cutters) {
                cutter.OnEnterPlayMode();
            }
        }

        private static List<T> FindObjectsOfTypeInScene<T>(Scene scene) where T : MonoBehaviour
        {
            var list = new List<T>();
            var rootObjects = scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects) {
                var obj = rootObject.GetComponentInChildren<T>();
                if (obj) {
                    list.Add(obj);
                }
            }

            return list;
        }

        [MenuItem("Tools/Digger/Save Meshes as Assets", false, 30)]
        public static void SaveMeshesAsAssets()
        {
            try {
                AssetDatabase.StartAssetEditing();
                var diggers = FindObjectsOfType<DiggerSystem>();
                foreach (var digger in diggers) {
                    digger.SaveMeshesAsAssets();
                }
            }
            finally {
                AssetDatabase.StopAssetEditing();
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        [MenuItem("Tools/Digger/Check Digger data", false, 31)]
        public static void CheckDiggerVersion()
        {
            var warned = false;
            var warnedAboutLegacyFiles = false;
            var diggers = FindObjectsOfType<DiggerSystem>();
            foreach (var digger in diggers) {
                if (Upgrading.HasLegacyFiles(digger)) {
                    if (!warnedAboutLegacyFiles) {
                        warnedAboutLegacyFiles = true;
                        EditorUtility.DisplayDialog("Legacy Digger files detected",
                                                    "Digger found some files in DiggerData folder that need to be upgraded. " +
                                                    "Digger will attempt to upgrade them automatically. Current files will be backuped in {projectDir}/DiggerBackup\n\n" +
                                                    "This may take a while.\n\nDon't forget to save your scene once this is done.",
                                                    "Ok");
                    }

                    Upgrading.UpgradeDiggerData(digger);
                    DiggerSystemEditor.Init(digger, true);
                    Undo.ClearUndo(digger);
                }

                if (Upgrading2.HasLegacyFiles(digger)) {
                    if (!warnedAboutLegacyFiles) {
                        warnedAboutLegacyFiles = true;
                        EditorUtility.DisplayDialog("Legacy Digger files detected",
                                                    "Digger found some files in DiggerData folder that need to be upgraded. " +
                                                    "Digger will attempt to upgrade them automatically. Current files will be backuped in {projectDir}/DiggerBackup\n\n" +
                                                    "This may take a while.\n\nDon't forget to save your scene once this is done.",
                                                    "Ok");
                    }

                    Upgrading2.UpgradeDiggerData(digger);
                    DiggerSystemEditor.Init(digger, true);
                    Undo.ClearUndo(digger);
                }

                if (digger.GetDiggerVersion() != DiggerSystem.DiggerVersion) {
                    if (!warned) {
                        warned = true;
                        EditorUtility.DisplayDialog("New Digger version",
                                                    "Looks like Digger was updated. Digger is going to synchronize and reload all its data " +
                                                    "to ensure compatibility. This may take a while.\n\nDon't forget to save your scene once this is done.",
                                                    "Ok");
                    }

                    DiggerSystemEditor.Init(digger, true);
                    Undo.ClearUndo(digger);
                }
            }

            if (warned || warnedAboutLegacyFiles) {
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                Undo.ClearAll();
            }
        }

        private static GameObject LoadAssetWithLabel(string label)
        {
            var guids = AssetDatabase.FindAssets($"l:{label}");
            if (guids == null || guids.Length == 0) {
                return null;
            }

            // we loop but there should be only one item in the list
            foreach (var guid in guids) {
                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                var labels = AssetDatabase.GetLabels(asset);
                if (labels != null && labels.Contains(label)) {
                    return asset;
                }
            }

            return null;
        }

        private void OnBeforeAssemblyReload()
        {
            NativeCollectionsPool.Instance.Dispose();
        }

        private void OnAfterAssemblyReload()
        {
            CheckDiggerVersion();
        }
    }
}