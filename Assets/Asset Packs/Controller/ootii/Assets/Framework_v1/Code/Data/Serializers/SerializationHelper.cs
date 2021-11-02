using UnityEngine;
using System.Globalization;
using System.Text;

namespace com.ootii.Data.Serializers
{
    /// <summary>
    /// Helper methods for serializing Unity types (numeric types, in particular)
    /// </summary>
    public static class SerializationHelper 
    {
        public static string Serialize(this float lValue)
        {
            return lValue.ToString("G8", CultureInfo.InvariantCulture);
        }

        public static string Serialize(this int lValue)
        {
            return lValue.ToString(CultureInfo.InvariantCulture);
        }

        public static string Serialize(this Vector2 lValue)
        {
            return "(" + lValue.x.Serialize() + "," + lValue.y.Serialize() + ")";
        }

        public static string Serialize(this Vector3 lValue)
        {
            return "(" + lValue.x.Serialize() + ", " + lValue.y.Serialize() + ", " + lValue.z.Serialize() + ")";            
        }

        public static string Serialize(this Vector4 lValue)
        {
            return "(" + lValue.x.Serialize() + ", " + lValue.y.Serialize() + ", " + lValue.z.Serialize() + ", " + lValue.w.Serialize() + ")";            
        }

        public static string Serialize(this Quaternion lValue)
        {
            return "(" + lValue.x.Serialize() + ", " + lValue.y.Serialize() + ", " + lValue.z.Serialize() + ", " + lValue.w.Serialize() + ")";            
        }

        public static string Serialize(this AnimationCurve rCurve)
        {
            if (rCurve == null) { return string.Empty; }

            StringBuilder lStringBuilder = new StringBuilder();

            for (int i = 0; i < rCurve.keys.Length; i++)
            {                
                Keyframe lKey = rCurve.keys[i];                

                lStringBuilder.Append(
                    lKey.time.ToString("f5", CultureInfo.InvariantCulture) + "|" +
                    lKey.value.ToString("f5", CultureInfo.InvariantCulture) + "|" +  
                    GetLeftTangentMode(rCurve, i) + "|" +                    
                    lKey.inTangent.ToString("f5", CultureInfo.InvariantCulture) + "|" +
                    lKey.outTangent.ToString("f5", CultureInfo.InvariantCulture) + "|" +
                    GetRightTangentMode(rCurve, i));

                if (i < rCurve.keys.Length - 1) { lStringBuilder.Append(";"); }
            }

            return lStringBuilder.ToString();
        }

        public static AnimationCurve GetAnimationCurve(string rValue)
        {
            if (rValue.Length == 0) { return null; }

            AnimationCurve lCurve = new AnimationCurve();

            string[] lItems = rValue.Split(';');
            for (int i = 0; i < lItems.Length; i++)
            {
                string[] lElements = lItems[i].Split('|');
                if (lElements.Length == 5 || lElements.Length == 6)
                {
                    float lFloatValue = 0f;

                    Keyframe lKey = new Keyframe();
                    if (float.TryParse(lElements[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lFloatValue))
                    {
                        lKey.time = lFloatValue;
                    }

                    if (float.TryParse(lElements[1], NumberStyles.Float, CultureInfo.InvariantCulture, out lFloatValue))
                    {
                        lKey.value = lFloatValue;
                    }

                    if (float.TryParse(lElements[3], NumberStyles.Float, CultureInfo.InvariantCulture, out lFloatValue))
                    {
                        lKey.inTangent = lFloatValue;
                    }

                    if (float.TryParse(lElements[4], NumberStyles.Float, CultureInfo.InvariantCulture, out lFloatValue))
                    {
                        lKey.outTangent = lFloatValue;
                    }

                    // UnityEngine.Keyframe.tangentMode is obsolete; we will only try to deserialize the left/right tangent
                    // modes if we have 6 elements in the string
                    if (lElements.Length == 6)
                    {
                        lCurve.AddKey(lKey);

                        var lIntValue = 0;
                        int.TryParse(lElements[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out lIntValue);
                        SetLeftTangentMode(lCurve, i, lIntValue);

                        int.TryParse(lElements[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out lIntValue);
                        SetRightTangentMode(lCurve, i, lIntValue);
                    }
                }
            }

            return lCurve;
            
        }

        #region Animation Curve Utilities

        /// <summary>
        /// Wrapper around UnityEditor call to AnimationUtility.SetKeyLeftTangentMode
        /// </summary>
        /// <param name="rCurve"></param>
        /// <param name="rIndex"></param>
        /// <param name="rTangentMode"></param>
        private static void SetLeftTangentMode(AnimationCurve rCurve, int rIndex, int rTangentMode)
        {
#if UNITY_EDITOR
            var lTangentMode = (UnityEditor.AnimationUtility.TangentMode) rTangentMode;
            UnityEditor.AnimationUtility.SetKeyLeftTangentMode(rCurve, rIndex, lTangentMode);
#endif
        }

        /// <summary>
        /// Wrapper around UnityEditor call to AnimationUtility.SetKeyRightTangentMode
        /// </summary>
        /// <param name="rCurve"></param>
        /// <param name="rIndex"></param>
        /// <param name="rTangentMode"></param>
        private static void SetRightTangentMode(AnimationCurve rCurve, int rIndex, int rTangentMode)
        {
#if UNITY_EDITOR
            var lTangentMode = (UnityEditor.AnimationUtility.TangentMode) rTangentMode;
            UnityEditor.AnimationUtility.SetKeyRightTangentMode(rCurve, rIndex, lTangentMode);
#endif
        }

        /// <summary>
        /// Wrapper around UnityEditor call to AnimationUtility.GetKeyLeftTangentMode
        /// </summary>
        /// <param name="rCurve"></param>
        /// <param name="rIndex"></param>
        /// <returns></returns>
        private static int GetLeftTangentMode(AnimationCurve rCurve, int rIndex)
        {
#if UNITY_EDITOR
            return (int) UnityEditor.AnimationUtility.GetKeyLeftTangentMode(rCurve, rIndex);
#else
            return 0;
#endif
        }

        /// <summary>
        /// Wrapper around UnityEditor call to AnimationUtility.SetKeyRightTangentMode
        /// </summary>
        /// <param name="rCurve"></param>
        /// <param name="rIndex"></param>
        /// <returns></returns>
        public static int GetRightTangentMode(AnimationCurve rCurve, int rIndex)
        {
#if UNITY_EDITOR
            return (int) UnityEditor.AnimationUtility.GetKeyRightTangentMode(rCurve, rIndex);
#else
            return 0;
#endif
        }

        #endregion Animation Curve Utilities
    }
}
