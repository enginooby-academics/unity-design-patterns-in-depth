using System.Collections.Generic;
using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class allows you to change the painting texture of the attached component (e.g. P3dPaintDecal) based on the paint pressure.</summary>
	[System.Serializable]
	public class P3dModifyTexturePressure : P3dModifier
	{
		public static string Group = "Texture";

		public static string Title = "Pressure";

		/// <summary>The painting texture will be changed to this.</summary>
		public Texture Texture { set { texture = value; } get { return texture; } } [System.NonSerialized] private Texture texture;

		/// <summary>The paint pressure must be at least this value.</summary>
		public float PressureMin { set { pressureMin = value; } get { return pressureMin; } } [SerializeField] private float pressureMin = 0.5f;

		/// <summary>The paint pressure must be at most this value.</summary>
		public float PressureMax { set { pressureMax = value; } get { return pressureMax; } } [SerializeField] private float pressureMax = 1.0f;

		public override void SetObjects(Object[] objects)
		{
			if (objects != null && objects.Length > 0)
			{
				texture = objects[0] as Texture;
			}
		}

		public override List<Object> GetObjects()
		{
			tempObjects.Clear();

			tempObjects.Add(texture);

			return tempObjects;
		}

		protected override void OnModifyTexture(ref Texture currentTexture, float pressure)
		{
			if (pressure >= pressureMin && pressure <= pressureMax)
			{
				currentTexture = texture;
			}
		}

#if UNITY_EDITOR
		public override void DrawEditorLayout()
		{
			texture = (Texture)UnityEditor.EditorGUI.ObjectField(P3dEditor.Reserve(18), new GUIContent("Texture", "The painting texture will be changed to this."), texture, typeof(Texture), true);
			pressureMin = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Pressure Min", "The paint pressure must be at least this value."), pressureMin);
			pressureMax = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Pressure Max", "The paint pressure must be at most this value."), pressureMax);
		}
#endif
	}
}