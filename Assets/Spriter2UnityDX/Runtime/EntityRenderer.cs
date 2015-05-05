using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Spriter2UnityDX {
	[DisallowMultipleComponent, ExecuteInEditMode]
	public class EntityRenderer : MonoBehaviour {
		private SpriteRenderer[] _renderers;
		private SpriteRenderer[] renderers {
			get { 
				if (_renderers == null)
					_renderers = new SpriteRenderer[0];
				return _renderers;
			}
			set { _renderers = value; }
		}
		private SpriteRenderer _first;
		private SpriteRenderer first {
			get {
				if (_first == null && renderers.Length > 0)
					_first = renderers [0];
				return _first;
			}
		}
        private Dictionary<String, Material> materialDictionary;
		public Color Color {
			get { return (first != null) ? first.color : default(Color); }
			set { DoForAll (x => x.color = value); }
		}

        public List<Material> MaterialList = new List<Material>();

		public int SortingLayerID {
			get { return (first != null) ? first.sortingLayerID : 0; }
			set { DoForAll (x => x.sortingLayerID = value); }
		}

		public string SortingLayerName {
			get { return (first != null) ? first.sortingLayerName : null; }
			set { DoForAll (x => x.sortingLayerName = value); }
		}

		public int SortingOrder {
			get { return (first != null) ? first.sortingOrder : 0; }
			set { DoForAll (x => x.sortingOrder = value); }
		}

		private void Awake () {
			RefreshRenders ();
		}

		private void OnEnable () {
			DoForAll (x => x.enabled = true);
		}

		private void OnDisable () {
			DoForAll (x => x.enabled = false);
		}
		
		private void DoForAll (Action<SpriteRenderer> action) {
			for (var i = 0; i < renderers.Length; i++) action (renderers [i]);
		}
        public void ApplyMaterials() {
            foreach (Material material in MaterialList)
                materialDictionary.Add(material.name, material);
            foreach (SpriteRenderer r in renderers)
                if (materialDictionary.ContainsKey(r.name))
                    r.sharedMaterial = materialDictionary[r.name];
        }
		public void RefreshRenders () {
			renderers = GetComponentsInChildren<SpriteRenderer> (true);
			_first = null;
		}
	}
}
