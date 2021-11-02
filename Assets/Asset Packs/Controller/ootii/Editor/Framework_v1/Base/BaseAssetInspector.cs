using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEditor;
using com.ootii.Helpers;
using System.Reflection;

namespace com.ootii.Base
{
    /// <summary>
    /// Base class for custom ScriptableObject inspectors that allows serialized properties to be accessed
    /// without hardcoding the field name using a lambda expression.
    /// </summary>
    /// <example>var myProperty = FindProperty(x => x.myProperty);</example>
    /// <typeparam name="T"></typeparam>
    public class BaseAssetInspector<T>  : Editor where T : ScriptableObject
    {
        // The target serialized object as its concrete type
        protected T mTarget;
        
        protected virtual void OnEnable()
        {
            mTarget = (T)target;
        }
      
        /// <summary>
        /// Get the tooltip text for the specified field
        /// </summary>
        /// <param name="rFieldName"></param>
        /// <returns></returns>
        protected string GetTooltip(string rFieldName)
        {
            FieldInfo lFieldInfo = typeof(T).GetField(rFieldName);
            if (lFieldInfo == null) return string.Empty;

            TooltipAttribute[] lAttributes = lFieldInfo.GetCustomAttributes(
                typeof(TooltipAttribute), true) as TooltipAttribute[];
            if (lAttributes == null) { return string.Empty; }

            TooltipAttribute lToolTip = lAttributes.Length > 0 ? lAttributes[0] : null;           
            return (lToolTip != null) ? lToolTip.tooltip : string.Empty;       
        }
        
        /// <summary>
        /// Find a serialized property on the target object using a lambda expression.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="rExpression"></param>
        /// <returns></returns>
        protected SerializedProperty FindProperty<TValue>(Expression<Func<T, TValue>> rExpression)
        {
            return serializedObject.FindProperty(ReflectionHelper.GetFieldPath(rExpression));
        }       
    }
}