﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace Spriter2UnityDX.Editors {
	[CustomEditor (typeof(EntityRenderer)), CanEditMultipleObjects]
	public class ERenderEdit : Editor {
		private EntityRenderer renderer;
        SerializedObject getTarget;
		private string[] layerNames;

		private void OnEnable () {
			renderer = (EntityRenderer)target;
			layerNames = GetSortingLayerNames ();
		}

		// Get the sorting layer names
		private string[] GetSortingLayerNames() {
			var sortingLayers = typeof(InternalEditorUtility).GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
			return (string[])sortingLayers.GetValue(null, new object[0]);
		}

		public override void OnInspectorGUI ()
		{
			var changed = false;
			var color = EditorGUILayout.ColorField ("Color", renderer.Color);
			if (color != renderer.Color) {renderer.Color = color; changed = true;}
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MaterialList"), true);
            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("Assign Materials")) { renderer.ApplyMaterials(); }
			var sortIndex = EditorGUILayout.Popup ("Sorting Layer", GetIndex (renderer.SortingLayerName), layerNames, GUILayout.ExpandWidth (true));
			if (layerNames [sortIndex] != renderer.SortingLayerName) {renderer.SortingLayerName = layerNames[sortIndex]; changed = true;}
			var sortingOrder = EditorGUILayout.IntField ("Order In Layer", renderer.SortingOrder);
			if (sortingOrder != renderer.SortingOrder) {renderer.SortingOrder = sortingOrder; changed = true;}
			if (changed) EditorUtility.SetDirty(renderer);
		}
		private int GetIndex (string layerName) {
			var index = ArrayUtility.IndexOf (layerNames, layerName);
			if (index < 0) index = 0;
			return index;
		}
	}
}
