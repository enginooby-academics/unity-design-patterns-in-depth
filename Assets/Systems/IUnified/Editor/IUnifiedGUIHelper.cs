using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.IUnified.Editor
{
    public class IUnifiedGUIHelper : ScriptableObject
    {
        public static IUnifiedGUIHelper Instance
        {
            get
            {
                if(_instance == null)
                {
                    var instances = Resources.FindObjectsOfTypeAll<IUnifiedGUIHelper>().ToList();
                    _instance = instances.FirstOrDefault();
                    if(_instance != null)
                    {
                        instances.RemoveAt(0);
                        foreach(var instance in instances)
                        {
                            DestroyImmediate(instance);
                        }
                    }
                    else
                    {
                        _instance = CreateInstance<IUnifiedGUIHelper>();
                    }
                }
                _instance.Initialize();
                return _instance;
            }
        }
        private static IUnifiedGUIHelper _instance;

        public static readonly Color FieldBorder = new Color32(96, 96, 96, 255);
        public static readonly Color FieldBG = new Color32(209, 209, 209, 255);

        public static readonly Color SelectionBorder = new Color32(44, 84, 141, 255);
        public static readonly Color SelectionBG = new Color32(61, 128, 223, 255);

        public static readonly Color DropBorder = new Color32(62, 125, 231, 255);
        public static readonly Color DropBG = new Color32(193, 213, 247, 255);

        public static readonly Color DontPanicBorder = new Color32(90, 133, 127, 255);
        public static readonly Color DontPanicBG = new Color32(114, 228, 211, 255);

        public static readonly Color ErrorBorder = new Color32(150, 50, 50, 255);
        public static readonly Color ErrorBG = new Color32(220, 180, 180, 255);

        public static readonly Color PingBorder = new Color32(159, 131, 45, 255);
        public static readonly Color PingBG = new Color32(243, 225, 60, 255);

        public static readonly Color ObjectGroupBackgroundColor = new Color32(180, 180, 180, 255);
        public static readonly Color ObjectGroupSwitchBackgroundColor = new Color32(194, 194, 194, 255);
        public static readonly Color ProObjectGroupBackgroundColor = new Color32(49, 49, 49, 255);
        public static readonly Color ProObjectGroupSwitchBackgroundColor = new Color32(41, 41, 41, 255);
        
        #region Utility Methods
        
        public static Texture2D BackgroundTexture(Color color)
        {
            var texture = new Texture2D(1, 1)
                {
                    name = GUITextureName,
                    hideFlags = HideFlags.DontSave
                };
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        public static Texture2D BuildBoxTexture(Color bgColor, Color borderColor)
        {
            var boxStyle   = GUI.skin.box;
            var boxTexture = boxStyle.normal.background ?? boxStyle.normal.scaledBackgrounds?.FirstOrDefault();
            var texture = new Texture2D(boxTexture?.width ?? 12, boxTexture?.height ?? 12, TextureFormat.ARGB32, false)
                {
                    name = GUITextureName,
                    hideFlags = HideFlags.DontSave,
                    wrapMode = TextureWrapMode.Repeat
                };
            for(var y = 0; y < texture.height; ++y)
            {
                for(var x = 0; x < texture.width; ++x)
                {
                    var border = (y == 0 || y == texture.height - 1) || (x == 0 || x == texture.width - 1);
                    texture.SetPixel(x, y, border ? borderColor : bgColor);
                }
            }
            texture.Apply();
            return texture;
        }

        public static bool IsPingable(Object @object)
        {
            if(@object == null)
            {
                return false;
            }

            if(@object is Component || @object is GameObject)
            {
                return true;
            }

            return !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(@object));
        }

        public static string GetObjectName(Object @object)
        {
            if(@object == null)
            {
                return "[null - you shouldn't be seeing this]";
            }

            if(@object is Component || string.IsNullOrEmpty(@object.name))
            {
                return IUnifiedContainerBase.IUnifiedContainerBase.ConstructResolvedName(@object.GetType());
            }

            return @object.name;
        }

        /// <summary>
        /// Pings the nearest object up the hierarchy that is ping-able. Necessary for prefabs.
        /// </summary>
        public static void PingObject(Object @object)
        {
            if(@object == null)
            {
                return;
            }

            var component = @object as Component;
            if(component != null)
            {
                @object = component.gameObject;
            }

            if(!IsSceneObject(@object))
            {
                var gameObject = @object as GameObject;
                if(gameObject != null)
                {
                    while(gameObject.transform.parent != null && gameObject.transform.parent.parent != null)
                    {
                        gameObject = gameObject.transform.parent.gameObject;
                    }

                    @object = gameObject;
                }
            }

            EditorGUIUtility.PingObject(@object);
        }

        public static bool IsSceneObject(Object @object)
        {
            if(@object == null)
            {
                return false;
            }

            return FindObjectsOfType(@object.GetType()).Contains(@object);
        }

        public static Rect CombineRects(Rect a, Rect b)
        {
            return new Rect(Math.Min(a.x, b.x), Math.Min(a.y, b.y), a.width + b.width, Math.Max(a.height, b.height));
        }

        public static float GetMinWidth(GUIContent content, GUIStyle style = null)
        {
            style = style != null ? style : GUI.skin.label;
            float min, max;
            style.CalcMinMaxWidth(content, out min, out max);
            return min;
        }

        public static float GetScaledTextureWidth(Texture texture, float height, GUIStyle style = null)
        {
            if(texture == null || texture.height == 0)
            {
                return 0.0f;
            }

            return texture.width * ((height - (style == null ? 0 : style.padding.top + style.padding.bottom)) / texture.height);
        }

        public static IEnumerable<Object> EnumerateSavedObjects()
        {
            return AssetDatabase.FindAssets("t:Object").Select(s => AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(s)));
        }

        private const string GUITextureName = "IUnifiedGUITexture";

        #endregion

        #region GUI Block Methods

        public static void ColorBlock(Action block)
        {
            var colorBlock = new GUIColorBlock();
            block();
            colorBlock.RestoreColors();
        }

        public static void EnabledBlock(Action block)
        {
            var enabledBlock = new GUIEnabledBlock();
            block();
            enabledBlock.RestoreEnabled();
        }

        public static void HorizontalBlock(Action block)
        {
            GUILayout.BeginHorizontal();
            block();
            GUILayout.EndHorizontal();
        }

        public static void HorizontalBlock(GUIStyle style, Action block)
        {
            GUILayout.BeginHorizontal(style);
            block();
            GUILayout.EndHorizontal();
        }

        public static void VerticalBlock(Action block)
        {
            GUILayout.BeginVertical();
            block();
            GUILayout.EndVertical();
        }

        public static void VerticalBlock(GUIStyle style, Action block)
        {
            GUILayout.BeginVertical(style);
            block();
            GUILayout.EndVertical();
        }

        public static void IndentBlock(Action block)
        {
            IndentBlock(1, block);
        }

        public static void IndentBlock(int indentLevel, Action block)
        {
            EditorGUI.indentLevel += indentLevel;
            block();
            EditorGUI.indentLevel -= indentLevel;
        }

        public static Vector2 ScrollViewBlock(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, Action block)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical);
            block();
            EditorGUILayout.EndScrollView();
            return scrollPosition;
        }

        #endregion

        #region Private Parts

        private static readonly List<FieldInfo> TextureFields = CachedType<IUnifiedGUIHelper>.Type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(f => f.FieldType == CachedType<Texture2D>.Type)
            .ToList();

        private static GUIStyle InitializedStyle(GUIStyle style, Texture2D normalBackground, Texture2D activeBackground = null)
        {
            style.normal.background = normalBackground;
            style.active.background = activeBackground;
            return style;
        }

        private class GUIColorBlock
        {
            private readonly Color _oldColor = GUI.color;
            private readonly Color _oldBackgroundColor = GUI.backgroundColor;
            private readonly Color _oldContentColor = GUI.contentColor;

            public void RestoreColors()
            {
                GUI.color = _oldColor;
                GUI.backgroundColor = _oldBackgroundColor;
                GUI.contentColor = _oldContentColor;
            }
        }

        private class GUIEnabledBlock
        {
            private readonly bool _enabled = GUI.enabled;

            public void RestoreEnabled()
            {
                GUI.enabled = _enabled;
            }
        }

        #endregion

        #region Instance Members

        public Texture2D FieldBox;
        public Texture2D PingedBackground;
        public Texture2D PingedBoxBackground;
        public Texture2D SelectionBackground;
        public Texture2D SelectionBoxBackground;
        public Texture2D DropBoxBackground;
        public Texture2D DontPanicBackground;
        public Texture2D ErrorBackground;
        public Texture2D ObjectGroupBackground;
        public Texture2D ObjectGroupSwitchBackground;

        public void Initialize()
        {
            hideFlags = HideFlags.DontSave;
            var isProSkin = EditorGUIUtility.isProSkin;
            if(_proSkin == null || _proSkin != isProSkin)
            {
                DestroyTextures();
                _proSkin                    = isProSkin;
                FieldBox                    = BuildBoxTexture(FieldBG, FieldBorder);
                PingedBackground            = BackgroundTexture(PingBG);
                PingedBoxBackground         = BuildBoxTexture(PingBG, PingBorder);
                SelectionBackground         = BackgroundTexture(SelectionBG);
                SelectionBoxBackground      = BuildBoxTexture(SelectionBG, SelectionBorder);
                DropBoxBackground           = BuildBoxTexture(DropBG, DropBorder);
                DontPanicBackground         = BuildBoxTexture(DontPanicBG, DontPanicBorder);
                ErrorBackground             = BuildBoxTexture(ErrorBG, ErrorBorder);
                ObjectGroupBackground       = BackgroundTexture(isProSkin ? ProObjectGroupBackgroundColor : ObjectGroupBackgroundColor);
                ObjectGroupSwitchBackground = BackgroundTexture(isProSkin ? ProObjectGroupSwitchBackgroundColor : ObjectGroupSwitchBackgroundColor);
            }
        }

        private bool? _proSkin;

        private void OnDestroy()
        {
            DestroyTextures();
        }

        private void DestroyTextures()
        {
            foreach(var texture in TextureFields.Select(f => (Texture2D)f.GetValue(this)))
            {
                if(texture != null)
                {
                    DestroyImmediate(texture);
                }
            }
        }

        #endregion

        public static class SelectWindowStyles
        {
            private const int Padding = 4;

            public static readonly GUIStyle SelectedButton = new GUIStyle(GUI.skin.button)
                {
                    normal = GUI.skin.button.active
                };

            public static GUIStyle NullOption
            {
                get { return InitializedStyle(_nullOption, Instance.FieldBox); }
            }
            private static readonly GUIStyle _nullOption = new GUIStyle(GUI.skin.box)
                {
                    alignment = TextAnchor.MiddleCenter,
                    normal = new GUIStyleState { textColor = Color.black }
                };

            public static GUIStyle NullSelected
            {
                get { return InitializedStyle(_nullSelected, Instance.SelectionBoxBackground); }
            }
            private static readonly GUIStyle _nullSelected = new GUIStyle(GUI.skin.box)
                {
                    alignment = TextAnchor.MiddleCenter,
                    normal = new GUIStyleState { textColor = Color.white }
                };

            public static GUIStyle Pinged
            {
                get { return InitializedStyle(_pinged, Instance.PingedBackground); }
            }
            private static readonly GUIStyle _pinged = new GUIStyle(GUI.skin.label)
                {
                    normal = new GUIStyleState { textColor = PingBorder },
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, Padding, Padding)
                };

            public static GUIStyle SelectedObject
            {
                get { return InitializedStyle(_selectedObject, Instance.SelectionBackground); }
            }
            private static readonly GUIStyle _selectedObject = new GUIStyle(GUI.skin.label)
                {
                    normal = new GUIStyleState { textColor = Color.white },
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, Padding, Padding)
                };

            public static GUIStyle ObjectGroup
            {
                get { return InitializedStyle(_objectGroup, Instance.ObjectGroupBackground); }
            }
            private static readonly GUIStyle _objectGroup = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, Padding, Padding)
                };

            public static GUIStyle ObjectGroupSwitch
            {
                get { return InitializedStyle(_objectGroupSwitch, Instance.ObjectGroupSwitchBackground); }
            }
            private static readonly GUIStyle _objectGroupSwitch = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, Padding, Padding)
                };

            public static GUIStyle DontPanic
            {
                get { return InitializedStyle(_dontPanic, Instance.DontPanicBackground); }
            }
            private static readonly GUIStyle _dontPanic = new GUIStyle(GUI.skin.box)
                {
                    normal = new GUIStyleState { textColor = DontPanicBorder }
                };
        }

        public static class InspectorStyles
        {
            public static readonly GUIStyle NullOutButton = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize  = 10,
                    padding   = new RectOffset(3, 0, 0, 0)
                };

            public static readonly GUIStyle SelectFromListButton = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize  = 20,
                    padding   = new RectOffset(0, 0, 2, 0)
                };

            public static GUIStyle Result
            {
                get { return InitializedStyle(_result, GUI.skin.textField.normal.background); }
            }
            private static readonly GUIStyle _result = new GUIStyle(GUI.skin.textField)
                {
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    normal = new GUIStyleState { textColor = GUI.skin.textField.normal.textColor }
                };

            public static GUIStyle ResultContent
            {
                get { return InitializedStyle(_resultContent, null); }
            }
            private static readonly GUIStyle _resultContent = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    normal = new GUIStyleState { textColor = GUI.skin.textField.normal.textColor }
                };

            public static GUIStyle Selecting
            {
                get { return InitializedStyle(_selecting, Instance.SelectionBoxBackground); }
            }
            private static readonly GUIStyle _selecting = new GUIStyle(GUI.skin.textField)
                {
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    normal = new GUIStyleState { textColor = Color.white }
                };

            public static GUIStyle SelectingContent
            {
                get { return InitializedStyle(_selectingContent, null); }
            }
            private static readonly GUIStyle _selectingContent = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    normal = new GUIStyleState { textColor = Color.white }
                };

            public static GUIStyle DropBox
            {
                get { return InitializedStyle(_dropBox, Instance.DropBoxBackground); }
            }
            private static readonly GUIStyle _dropBox = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleLeft,
                imagePosition = ImagePosition.ImageLeft,
                normal = new GUIStyleState { textColor = DropBorder }
            };

            public static GUIStyle DropBoxContent
            {
                get { return InitializedStyle(_dropBoxContent, null); }
            }
            private static readonly GUIStyle _dropBoxContent = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                imagePosition = ImagePosition.ImageLeft,
                normal = new GUIStyleState { textColor = DropBorder }
            };

            public static GUIStyle Pinging
            {
                get { return InitializedStyle(_pinging, Instance.PingedBoxBackground); }
            }
            public static GUIStyle _pinging = new GUIStyle(GUI.skin.textField)
                {
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    normal = new GUIStyleState { textColor = PingBorder }
                };

            public static GUIStyle PingingContent
            {
                get { return InitializedStyle(_pingingContent, null); }
            }
            public static GUIStyle _pingingContent = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    normal = new GUIStyleState { textColor = PingBorder }
                };

            public static GUIStyle Error
            {
                get { return InitializedStyle(_error, Instance.ErrorBackground); }
            }
            private static readonly GUIStyle _error = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 9,
                    normal = new GUIStyleState { textColor = ErrorBorder }
                };
        }
    }

    public static class CachedType<T>
    {
        public static readonly Type Type = typeof(T);
    }

    public struct GUIContentRect
    {
        public GUIContent Content;
        public Rect Rect;

        public GUIContentRect(GUIContent content, Rect rect)
        {
            Content = content;
            Rect = rect;
        }

        public void SetWidth(float width)
        {
            Rect.xMax = Rect.xMin + width;
        }

        public void MoveNextTo(Rect rect, float space = 0.0f)
        {
            Rect.xMin = rect.xMax + space;
        }

        public static implicit operator Rect(GUIContentRect contentRect)
        {
            return contentRect.Rect;
        }

        public static implicit operator GUIContent(GUIContentRect contentRect)
        {
            return contentRect.Content;
        }
    }
}