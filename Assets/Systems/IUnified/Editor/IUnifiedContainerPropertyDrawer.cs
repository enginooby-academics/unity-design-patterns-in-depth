using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Assets.IUnified.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(IUnifiedContainerBase.IUnifiedContainerBase), true)]
public class IUnifiedContainerPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var containerType = GetContainerType(property);
        if(containerType == null)
        {
            position.width  -= 4;
            position.x      += 2;
            position.height -= 2;
            position.y      += 1;
            GUI.Label(position, $"Cannot draw '{property.name}'.", IUnifiedGUIHelper.InspectorStyles.Error);
            return;
        }

        var resultType = containerType.GetProperty("Result").PropertyType;
        var drawContainerMethod = GetDrawMethod(resultType);
        
        if(SerializedContainer.Create(property, out var serializedContainer))
        {
            label = EditorGUI.BeginProperty(position, label, serializedContainer.ContainerProperty);
            drawContainerMethod(position, label, serializedContainer);
            EditorGUI.EndProperty();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + ButtonSpace + ButtonSpace;
    }

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        return false;
    }

    #region Private Parts

    private static IUnifiedContainerSelectWindow _selectWindow;
    private static Event                         _currentEvent;

    private const float ButtonSpace = 1.0f;
    private const float ButtonWidth = 20.0f;

    private static void DrawIUnifiedContainer<TResult>(Rect position, GUIContent label, SerializedContainer serializedContainer)
        where TResult : class
    {
        _currentEvent = Event.current;
        var resultTypeName = IUnifiedContainerBase.IUnifiedContainerBase.ConstructResolvedName(CachedType<TResult>.Type);

        if(string.IsNullOrEmpty(label.tooltip))
        {
            label.tooltip = resultTypeName;
        }
        var labelRect = new GUIContentRect(label, position);
        labelRect.SetWidth(EditorGUIUtility.labelWidth);

        var resultRect = new GUIContentRect(null, position);
        resultRect.MoveNextTo(labelRect);
        resultRect.Rect.xMax -= (ButtonSpace + ButtonWidth) * 2;

        var nullButton = new GUIContentRect(null, position);
        nullButton.MoveNextTo(resultRect, ButtonSpace);
        nullButton.SetWidth(ButtonWidth);

        var listButtonRect = new GUIContentRect(null, position);
        listButtonRect.MoveNextTo(nullButton, ButtonSpace);
        listButtonRect.SetWidth(ButtonWidth);
        
        var isProjectAsset = serializedContainer.IsProjectAsset;
        var pingable       = !serializedContainer.ObjectFieldProperty.hasMultipleDifferentValues && IUnifiedGUIHelper.IsPingable(serializedContainer.ObjectField);
        var dragDropResult = GetDragAndDropResult<TResult>(resultRect, isProjectAsset, serializedContainer);

        EditorGUI.LabelField(labelRect, label);
        IUnifiedGUIHelper.EnabledBlock(() =>
        {
            GUI.enabled = pingable;

            IUnifiedGUIHelper.ColorBlock(() =>
            {
                if(serializedContainer.Selecting || serializedContainer.Dropping)
                {
                    GUI.color = new Color(1, 1, 1, 2);
                }
                else
                {
                    GUI.color = pingable ? new Color(1, 1, 1, 2) : Color.white;
                }

                DrawField(serializedContainer, resultRect, pingable);
            });
        });
        
        if(dragDropResult != null)
        {
            serializedContainer.ObjectField = dragDropResult as Object;
            serializedContainer.ApplyModifiedProperties();
            GUI.changed = true;
        }

        if(GUI.Button(nullButton, new GUIContent("○", "Set to null"), IUnifiedGUIHelper.InspectorStyles.NullOutButton))
        {
            serializedContainer.ObjectField = null;
            serializedContainer.ApplyModifiedProperties();
            GUI.changed = true;
        }

        IUnifiedGUIHelper.EnabledBlock(() =>
        {
            if(GUI.Button(listButtonRect, new GUIContent("◉", "Select from list"), IUnifiedGUIHelper.InspectorStyles.SelectFromListButton))
            {
                _selectWindow = IUnifiedContainerSelectWindow.ShowSelectWindow(resultTypeName, isProjectAsset, serializedContainer, GetSelectable<TResult>(isProjectAsset));
            }
        });
    }

    private static void DrawField(SerializedContainer serializedContainer, Rect rect, bool pingable)
    {
        var buttonId = GUIUtility.GetControlID(FocusType.Passive) + 1;
        if(GUI.Button(rect, GUIContent.none, GUIStyle.none))
        {
            if(pingable)
            {
                IUnifiedGUIHelper.PingObject(serializedContainer.ObjectField);
            }
        }
        var pinging = GUIUtility.hotControl == buttonId && pingable;

        GUIStyle fieldStyle;
        GUIStyle contentStyle;
        if(serializedContainer.Dropping)
        {
            fieldStyle   = IUnifiedGUIHelper.InspectorStyles.DropBox;
            contentStyle = IUnifiedGUIHelper.InspectorStyles.DropBoxContent;
        }
        else if(pinging)
        {
            fieldStyle   = IUnifiedGUIHelper.InspectorStyles.Pinging;
            contentStyle = IUnifiedGUIHelper.InspectorStyles.PingingContent;
        }
        else if(serializedContainer.Selecting)
        {
            fieldStyle   = IUnifiedGUIHelper.InspectorStyles.Selecting;
            contentStyle = IUnifiedGUIHelper.InspectorStyles.SelectingContent;
        }
        else
        {
            fieldStyle   = IUnifiedGUIHelper.InspectorStyles.Result;
            contentStyle = IUnifiedGUIHelper.InspectorStyles.ResultContent;
        }

        if(serializedContainer.ObjectFieldProperty.hasMultipleDifferentValues || serializedContainer.ObjectField == null)
        {
            contentStyle.alignment = TextAnchor.MiddleCenter;
            contentStyle.imagePosition = ImagePosition.TextOnly;
        }
        else
        {
            contentStyle.alignment = TextAnchor.MiddleLeft;
            contentStyle.imagePosition = ImagePosition.ImageLeft;
        }

        serializedContainer.GetContainerContents(rect, serializedContainer.Dropping, out var icon, out var label);

        GUI.Label(rect, GUIContent.none, fieldStyle);
        if(icon.Content != null)
        {
            icon.SetWidth(IUnifiedGUIHelper.GetScaledTextureWidth(icon.Content.image, rect.height -4.0f));
            label.MoveNextTo(icon, -3.0f);
            GUI.Label(icon, icon, contentStyle ?? fieldStyle);
        }
        GUI.Label(label, label, contentStyle ?? fieldStyle);

        if(!SerializedContainer.AnyDropping && pingable)
        {
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Zoom);
        }
    }

    private static TResult GetDragAndDropResult<TResult>(Rect dropArea, bool selectingForProjectAsset, SerializedContainer serializedContainer)
        where TResult : class
    {
        TResult result    = null;
        bool?   dropping  = null;
        var     controlId = GUIUtility.GetControlID(FocusType.Passive, dropArea);

        switch(_currentEvent.rawType)
        {
            case EventType.DragExited:
                dropping = false;
                break;

            case EventType.DragUpdated:
            case EventType.DragPerform:
                dropping = false;
                if(!serializedContainer.MouseInRects(dropArea, _currentEvent.mousePosition))
                {
                    break;
                }

                GUIUtility.hotControl = controlId;
                var implementation = DragAndDrop.objectReferences.SelectMany(GetObjectImplementationsOf<TResult>).FirstOrDefault();
                if(implementation != null)
                {
                    var implementationObject = implementation as Object;
                    if(implementationObject != null && (!selectingForProjectAsset || !IUnifiedGUIHelper.IsSceneObject(implementationObject)))
                    {
                        dropping = true;
                    }
                }
                DragAndDrop.visualMode = SerializedContainer.AnyDropping ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;

                if(_currentEvent.type == EventType.DragPerform)
                {
                    _currentEvent.Use();
                    DragAndDrop.AcceptDrag();
                    GUIUtility.hotControl = 0;

                    dropping = false;
                    result   = implementation;
                }

                break;
        }

        if(dropping != null)
        {
            serializedContainer.Dropping = dropping.Value;
        }

        return result;
    }

    private static List<TResult> GetObjectImplementationsOf<TResult>(Object @object)
        where TResult : class
    {
        var implementations = new List<TResult>();
        if(@object is TResult implementation)
        {
            implementations.Add(implementation);
        }

        var gameObject = @object as GameObject;
        if(gameObject != null)
        {
            implementations.AddRange(gameObject.GetComponents<Component>().OfType<TResult>());
        }

        var transform = @object as Transform;
        if(transform != null)
        {
            implementations.AddRange(transform.GetComponents<Component>().OfType<TResult>());
        }

        return implementations.Distinct().ToList();
    }

    private static string BuildEditorResultString(string resultType, Object @object)
    {
        if(@object != null)
        {
            var component = @object as Component;
            if(component != null)
            {
                return $"{component.gameObject.name} ( {IUnifiedContainerBase.IUnifiedContainerBase.ConstructResolvedName(@object.GetType())} )";
            }

            return IUnifiedGUIHelper.GetObjectName(@object);
        }

        if(!string.IsNullOrEmpty(resultType))
        {
            return resultType;
        }

        return null;
    }

    private static IEnumerable<SelectableObject> GetSelectable<TResult>(bool projectAssetsOnly)
        where TResult : class
    {
        var objects         = IUnifiedGUIHelper.EnumerateSavedObjects().Concat(projectAssetsOnly ? new Object[0] : Object.FindObjectsOfType<Object>());
        var implementations = new HashSet<TResult>();
        foreach(var implementation in objects.SelectMany(GetObjectImplementationsOf<TResult>))
        {
            implementations.Add(implementation);
        }
        return implementations.Select(SelectableObject.GetSelectableObject);
    }

    private const BindingFlags FieldBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

    /// <summary>
    /// Given a SerializedProperty of a field, List, or array of IUnifiedContainer&lt;T&gt; derivative(s), will initialize null references and return the container derivative type.
    /// </summary>
    /// <returns>The type deriving from IUnifiedContainer&lt;T&gt;, if any - null otherwise.</returns>
    private static readonly Dictionary<Type, Dictionary<string, Type>> ContainerTypesByPaths = new Dictionary<Type, Dictionary<string, Type>>();
    private static readonly Regex                                      ArrayMatch            = new Regex(@"\.Array\.data\[\d+\]", RegexOptions.Compiled);

    private static Type GetContainerType(SerializedProperty property)
    {
        var type = property.serializedObject.targetObject.GetType();
        if(!ContainerTypesByPaths.TryGetValue(type, out var containerTypes))
        {
            ContainerTypesByPaths.Add(type, (containerTypes = new Dictionary<string, Type>()));
        }

        var cleanPath = ArrayMatch.Replace(property.propertyPath, "");
        if(containerTypes.TryGetValue(cleanPath, out var containerType))
        {
            return containerType;
        }

        foreach(var field in cleanPath.Split('.'))
        {
            var fieldInfo = GetFieldInChain(type, field);
            if(fieldInfo == null)
            {
                return null;
            }

            type = fieldInfo.FieldType;
            if(type.IsArray)
            {
                type = type.GetElementType();
            }
            else
            {
                if(IsSubclassOfRawGeneric(type, typeof(List<>), out var genericArguments))
                {
                    type = genericArguments[0];
                }
            }
        }

        if(IsSubclassOfRawGeneric(type, typeof(IUnifiedContainer<>), out _))
        {
            containerTypes.Add(cleanPath, type);
            return type;
        }

        return null;
    }

    private static FieldInfo GetFieldInChain(Type type, string fieldName)
    {
        if(type == null)
        {
            return null;
        }

        var field = type.GetField(fieldName, FieldBindingFlags);
        if(field == null)
        {
            return GetFieldInChain(type.BaseType, fieldName);
        }

        return field;
    }

    /// <summary>
    /// Credit to JaredPar: http://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
    /// </summary>
    public static bool IsSubclassOfRawGeneric(Type toCheck, Type generic, out List<Type> genericTypeArguments)
    {
        genericTypeArguments = null;

        while(toCheck != null && toCheck != typeof(object))
        {
            var checkGeneric = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if(generic == checkGeneric)
            {
                genericTypeArguments = toCheck.GetGenericArguments().ToList();
                return true;
            }

            toCheck = toCheck.BaseType;
        }

        return false;
    }

    private static readonly MethodInfo DrawMethodInfo = CachedType<IUnifiedContainerPropertyDrawer>.Type.GetMethod("DrawIUnifiedContainer", BindingFlags.Static | BindingFlags.NonPublic);
    private static readonly Dictionary<Type, DrawMethod> DrawMethods = new Dictionary<Type, DrawMethod>();

    private delegate void DrawMethod(Rect position, GUIContent label, SerializedContainer serializedContainer);
    private static DrawMethod GetDrawMethod(Type type)
    {
        if(!DrawMethods.TryGetValue(type, out var drawMethod))
        {
            DrawMethods.Add(type, drawMethod = (DrawMethod)Delegate.CreateDelegate(CachedType<DrawMethod>.Type, DrawMethodInfo.MakeGenericMethod(type)));
        }
        return drawMethod;
    }

    #endregion

    /// <summary>
    /// Handy class wrapping around an IUnifiedContainer-derived serialized property.
    /// </summary>
    public class SerializedContainer
    {
        public static bool AnyDropping { get { return _droppingHashcode != null; } }

        public static bool Create(SerializedProperty property, out SerializedContainer serializedContainer)
        {
            serializedContainer = null;
            if(property != null)
            {
                var serializedObject  = new SerializedObject(property.serializedObject.targetObjects);
                var containerProperty = serializedObject.FindProperty(property.propertyPath);
                if(containerProperty != null)
                {
                    serializedContainer = new SerializedContainer(serializedObject, containerProperty);
                    return true;
                }
            }

            return false;
        }



        public readonly SerializedProperty ContainerProperty;

        public SerializedProperty ObjectFieldProperty { get { return _objectFieldProperty = _objectFieldProperty ?? ContainerProperty.FindPropertyRelative("ObjectField"); } }
        public SerializedProperty ResultTypeProperty  { get { return _resultTypeProperty = _resultTypeProperty ?? ContainerProperty.FindPropertyRelative("ResultType"); } }
        public bool               IsProjectAsset      { get { return !IUnifiedGUIHelper.IsSceneObject(ContainerProperty.serializedObject.targetObject); } }

        public Object ObjectField
        {
            get { return ObjectFieldProperty.objectReferenceValue; }
            set
            {
                ResultType = "";
                ObjectFieldProperty.objectReferenceValue = value;
            }
        }

        public string ResultType
        {
            get { return ResultTypeProperty.stringValue; }
            set { ResultTypeProperty.stringValue = value; }
        }

        public bool Selecting
        {
            get { return _selectWindow?.IsValid == true && _selectingHashcode == _containerHashcode; }
            set
            {
                if(Selecting != value)
                {
                    _selectingHashcode = value ? (int?)_containerHashcode : null;
                }
            }
        }

        public bool Dropping
        {
            get { return _droppingHashcode == _containerHashcode; }
            set
            {
                if(Dropping != value)
                {
                    _droppingHashcode = value ? (int?)_containerHashcode : null;
                }
            }
        }
        
        public void ApplyModifiedProperties()
        {
            ContainerProperty.serializedObject.ApplyModifiedProperties();
        }

        public void GetContainerContents(Rect rect, bool droppingFor, out GUIContentRect iconContent, out GUIContentRect labelContent)
        {
            Texture icon;
            string resultString, resultTip;
            if(ObjectFieldProperty.hasMultipleDifferentValues)
            {
                icon         = null;
                resultString = "-";
                resultTip    = null;
            }
            else
            {
                icon         = EditorGUIUtility.ObjectContent(ObjectField, null).image;
                resultString = (resultTip = BuildEditorResultString(ResultType, ObjectField)) ?? "null";
            }

            iconContent  = icon == null ? new GUIContentRect(null, rect) : new GUIContentRect(new GUIContent(icon, resultTip), rect);
            labelContent = new GUIContentRect(new GUIContent(resultString, !droppingFor ? resultTip : null), rect);
        }

        public bool MouseInRects(Rect rect, Vector2 mousePosition)
        {
            return TimedRect.MouseInRects(_containerHashcode, rect, mousePosition);
        }

        private static int? _droppingHashcode;
        private static int? _selectingHashcode;

        private SerializedProperty _objectFieldProperty;
        private SerializedProperty _resultTypeProperty;
        private readonly int       _containerHashcode;

        private SerializedContainer(SerializedObject serializedObject, SerializedProperty containerProperty)
        {
            ContainerProperty = containerProperty;

            unchecked
            {
                var propertyPath = ObjectFieldProperty?.propertyPath;
                _containerHashcode = ((serializedObject.targetObject?.GetHashCode() ?? 0) * 397) ^ (propertyPath?.GetHashCode() ?? 0);
            }
        }

        ~SerializedContainer()
        {
            ContainerProperty.serializedObject.Dispose();
        }

        /// <summary>
        /// Created to address a very minor and super-rare graphical inconsistency.
        /// Definitely overkill, but... well there it is. And hey, it works!
        /// </summary>
        private class TimedRect
        {
            private Rect _rect;
            private readonly Stopwatch _stopwatch;

            private TimedRect(Rect rect)
            {
                _rect = rect;
                _stopwatch = Stopwatch.StartNew();
            }

            public static bool MouseInRects(int containerHash, Rect rect, Vector2 mousePosition)
            {
                _lastAccess = Stopwatch.StartNew();

                if(!TimestampedRects.TryGetValue(containerHash, out var rects))
                {
                    TimestampedRects.Add(containerHash, (rects = new List<TimedRect>()));
                }
                rects.Add(new TimedRect(rect));

                var mouseInRect = false;
                rects.RemoveAll(r =>
                {
                    if(r._stopwatch.ElapsedMilliseconds > 100)
                    {
                        return true;
                    }

                    if(r._rect.Contains(mousePosition))
                    {
                        mouseInRect = true;
                    }

                    return false;
                });

                return mouseInRect;
            }

            private static Stopwatch _lastAccess;
            private static readonly Dictionary<int, List<TimedRect>> TimestampedRects = new Dictionary<int, List<TimedRect>>();

            static TimedRect()
            {
                EditorApplication.update += MaintainRects;
            }

            private static void MaintainRects()
            {
                if(_lastAccess != null && _lastAccess.ElapsedMilliseconds > 5000)
                {
                    TimestampedRects.Clear();
                    _lastAccess.Stop();
                    _lastAccess = null;
                }
            }
        }
    }

    public class SelectableObject
    {
        public readonly Object Object;
        public readonly bool IsProjectAsset;
        public readonly bool IsComponent;

        private SelectableObject(Object @object)
        {
            Object         = @object;
            IsProjectAsset = !IUnifiedGUIHelper.IsSceneObject(Object);
            IsComponent    = Object is Component;
        }

        public static SelectableObject GetSelectableObject<TResult>(TResult result)
        {
            var @object = result as Object;
            return @object == null ? null : new SelectableObject(@object);
        }
    }
}