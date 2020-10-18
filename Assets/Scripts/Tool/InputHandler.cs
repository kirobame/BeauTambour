using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Tooling
{
    public class InputHandler : SerializedMonoBehaviour
    {
        #region Encapsuled Types

        [HideReferenceObjectPicker]
        private class Map
        {
            #if UNITY_EDITOR

            private ActionLink AddNewLink() => new ActionLink(this);
            public void Remove(ActionLink link)
            {
                links = links.Where(item => item != link).ToList();
            }
            #endif
            
            public Map(InputHandler handler, Guid id)
            {
                this.handler = handler;
                this.id = id;
            }

            public InputHandler Handler => handler;
            public Guid Id => id;

            public InputActionMap Value => handler.Asset.FindActionMap(id);
            [ShowInInspector, HideLabel, PropertyOrder(-1)] public string Name => Value.name;
            
            [SerializeField, HideInInspector] private InputHandler handler;
            [SerializeField, HideInInspector] private Guid id;

            [SerializeField] private bool isEnabled = true;
            
            [ListDrawerSettings(HideRemoveButton = true, DraggableItems = false, CustomAddFunction = "AddNewLink")]
            [SerializeField] private List<ActionLink> links = new List<ActionLink>();

            public void Activate()
            {
                if (Value == null) return;
                foreach (var link in links) link.Activate();
            }
            public void Deactivate()
            {
                if (Value == null) return;
                foreach (var link in links) link.Deactivate();
            }
            
            public void Enable()
            {
                if (isEnabled) Value.Enable();
            }
            public void Disable() => Value.Disable();
        }

        [HideReferenceObjectPicker]
        private class ActionLink
        {
            #if UNITY_EDITOR

            private void RemoveSelf() => map.Remove(this);
            private bool IsValid
            {
                get
                {
                    if (reference == null) return false;
                    return map.Value == Action.actionMap;
                }
            }
            #endif
            
            public ActionLink(Map map) => this.map = map;
            
            public InputAction Action => reference.action;
            
            [SerializeField, HideInInspector] private Map map;
            
            [SerializeField, InlineButton("RemoveSelf", "✕")] private InputActionReference reference;
            
            [ListDrawerSettings(DraggableItems = false, AlwaysAddDefaultValue = true)]
            [SerializeField, EnableIf("IsValid")] private List<IBindable> bindables = new List<IBindable>();
            
            public void Activate()
            {
                if (Action == null) return;
                foreach (var bindable in bindables) bindable.Bind(Action);
            }
            public void Deactivate()
            {
                if (Action == null) return;
                foreach (var bindable in bindables) bindable.Unbind(Action);
            }
        }
        #endregion

        #if UNITY_EDITOR
        
        private void OnAssetChanged()
        {
            if (asset == null) return;
            
            maps.Clear();
            foreach (var actionMap in asset.actionMaps)
            {
                var map = new Map(this, actionMap.id);
                maps.Add(map);
            }
        }
        #endif

        public bool IsActive => asset != null;
        public InputActionAsset Asset => asset;

        [SerializeField, OnValueChanged("OnAssetChanged")] private InputActionAsset asset;
        
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
        [SerializeField] private List<Map> maps = new List<Map>();

        void Start()
        {
            if (!IsActive) return;
            foreach (var map in maps) map.Activate();
        }
        
        void OnEnable()
        {
            if (!IsActive) return;
            
            asset.Enable();
            foreach (var map in maps) map.Enable();
        }
        void OnDisable()
        {
            if (!IsActive) return;
            
            asset.Disable();
            foreach (var map in maps) map.Disable();
        }

        void OnDestroy()
        {
            if (!IsActive) return;
            foreach (var map in maps) map.Deactivate();
        }
    }
}