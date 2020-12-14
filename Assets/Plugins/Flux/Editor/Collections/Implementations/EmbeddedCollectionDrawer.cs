﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flux.Editor
{
    public abstract class EmbeddedCollectionDrawer<T> : CollectionDrawer where T : UnityEngine.Object
    {
        protected static Dictionary<string, Queue<T>> toAdd = new Dictionary<string, Queue<T>>();
        
        public EmbeddedCollectionDrawer(SerializedObject serializedObject, SerializedProperty property) : base(serializedObject, property)
        {
            menu = new GenericMenu();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(T).IsAssignableFrom(type)) continue;
                    
                    var attribute = type.GetCustomAttribute<ItemPathAttribute>();
                    if (attribute == null) continue;
                    
                    menu.AddItem(new GUIContent(attribute.Path), false, OnItemSelected, type);
                }
            }
            
            if (!toAdd.ContainsKey(property.propertyPath)) toAdd.Add(property.propertyPath, new Queue<T>());
        }
        
        protected GenericMenu menu;

        private void OnItemSelected(object itemType)
        {
            var type = (Type)itemType;
            toAdd[reorderableList.serializedProperty.propertyPath].Enqueue(CreateItem(type));
        }
        protected abstract T CreateItem(Type itemType);
        
        protected override void AddElement() => menu.ShowAsContext();
        protected override void RemoveElement()
        {
            var elementProperty = reorderableList.serializedProperty.GetArrayElementAtIndex(reorderableList.index);
            var item = elementProperty.objectReferenceValue as T;
            
            base.RemoveElement();
            base.RemoveElement();
            
            DeleteItem(item);
        }
        protected abstract void DeleteItem(T item);

        public override void Draw()
        {
            while (toAdd[reorderableList.serializedProperty.propertyPath].Count > 0)
            {
                var arrayProperty = reorderableList.serializedProperty;
                
                if (arrayProperty.arraySize > 0) arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize - 1);
                else arrayProperty.InsertArrayElementAtIndex(0);

                var lastElementProperty = arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);
                lastElementProperty.objectReferenceValue = toAdd[reorderableList.serializedProperty.propertyPath].Dequeue();
            }

            base.Draw();
        }
        protected override void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += EditorGUIUtility.standardVerticalSpacing;
            
            var arrayProperty = reorderableList.serializedProperty;
            if (!arrayProperty.isExpanded) return;
            
            var elementProperty = arrayProperty.GetArrayElementAtIndex(index);
            var serializedObject = new SerializedObject(elementProperty.objectReferenceValue);
            var headerRect = new Rect(rect.position + 14f.ToX(), new Vector2(rect.width, EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing));

            serializedObject.UpdateIfRequiredOrScript();
            
            DrawEnableButton(headerRect, serializedObject);
            DrawDragButton(headerRect, elementProperty, serializedObject);

            var nameAttribute = serializedObject.targetObject.GetType().GetCustomAttribute<ItemNameAttribute>();
            var foldoutContent = nameAttribute == null ? new GUIContent(serializedObject.targetObject.GetType().Name) : new GUIContent(nameAttribute.Name);
            
            elementProperty.isExpanded = EditorGUI.Foldout(headerRect, elementProperty.isExpanded, foldoutContent);
            if (elementProperty.isExpanded)
            {
                var iterator = serializedObject.GetIterator();
                iterator.NextVisible(true);

                var elementRect = headerRect;
                elementRect.x -= 14f;
                elementRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 3f;

                EditorGUI.indentLevel++;
                while (iterator.NextVisible(false))
                {
                    EditorGUI.PropertyField(elementRect, iterator);
                    elementRect.y += EditorGUI.GetPropertyHeight(iterator) + EditorGUIUtility.standardVerticalSpacing * 3f;
                }
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
        }
        private void DrawEnableButton(Rect headerRect, SerializedObject serializedObject)
        {
            var enableSize = EditorGUIUtility.singleLineHeight;
            var enableRect = new Rect(headerRect.position + (headerRect.width - enableSize * 2f - 16f).ToX(), Vector2.one * enableSize);
            enableRect.y += 2f;

            var enabledProperty = serializedObject.FindProperty("m_Enabled");
            enabledProperty.boolValue = EditorGUI.Toggle(enableRect, enabledProperty.boolValue);
        }
        private void DrawDragButton(Rect headerRect, SerializedProperty elementProperty, SerializedObject serializedObject)
        {
            var buttonSize = EditorGUIUtility.singleLineHeight;
            var buttonRect = new Rect(headerRect.position + (headerRect.width - buttonSize - 16f).ToX(), Vector2.one * buttonSize);
            buttonRect.y += 3f;
            
            GUI.DrawTexture(buttonRect, EditorGUIUtility.IconContent("d_scenepicking_pickable_hover@2x").image);

            var Ev = UnityEngine.Event.current;
            if (Ev.type == EventType.MouseDown && Ev.button == 0 && buttonRect.Contains(Ev.mousePosition))
            {
                DragAndDrop.StartDrag($"Dragging : {elementProperty.propertyPath}");
                DragAndDrop.SetGenericData($"PPtr<${typeof(T).Name}>", serializedObject.targetObject);
                DragAndDrop.objectReferences = new UnityEngine.Object[]{serializedObject.targetObject}; 
                
                Ev.Use();
            }
        }

        protected override float GetElementHeight(int index)
        {
            var arrayProperty = reorderableList.serializedProperty;
            if (!arrayProperty.isExpanded) return 0f;
            
            var elementProperty = arrayProperty.GetArrayElementAtIndex(index);

            var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 5f;
            if (elementProperty.isExpanded)
            {
                var serializedObject = new SerializedObject(elementProperty.objectReferenceValue);
                var iterator = serializedObject.GetIterator();
                iterator.NextVisible(true);

                while (iterator.NextVisible(false)) height += EditorGUI.GetPropertyHeight(iterator) + EditorGUIUtility.standardVerticalSpacing * 3f;
            }
            
            return height;
        }
    }
}