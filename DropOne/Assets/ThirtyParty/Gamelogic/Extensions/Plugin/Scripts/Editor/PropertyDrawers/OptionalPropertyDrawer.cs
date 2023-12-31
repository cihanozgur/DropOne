﻿using UnityEngine;
using UnityEditor;

namespace Gamelogic.Extensions.Editor
{
	[CustomPropertyDrawer(typeof(Optional), true)]
	public class OptionalPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var valueProperty = property.FindPropertyRelative("value");
			return EditorGUI.GetPropertyHeight(valueProperty, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var useValueProperty = property.FindPropertyRelative("useValue");
			var valueProperty = property.FindPropertyRelative("value");
		
			var toggleRect = new Rect(position.position.x, position.y, 40, position.size.y);
			var valueRect = new Rect(40, position.y, position.size.x - 40 +14, position.size.y);

			EditorGUI.BeginProperty(toggleRect, label, property);
			useValueProperty.boolValue = EditorGUI.Toggle(toggleRect, useValueProperty.boolValue);

			bool oldEnabled = GUI.enabled;
			GUI.enabled = useValueProperty.boolValue;
		
			EditorGUIUtility.labelWidth = EditorGUIUtility.labelWidth - 26;
		
			EditorGUI.PropertyField(valueRect, valueProperty, new GUIContent() {text = property.displayName});
			
			GUI.enabled = oldEnabled;

			EditorGUI.EndProperty();
		}
	}
}