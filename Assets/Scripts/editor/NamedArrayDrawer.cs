using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NamedArrayAttribute))]public class NamedArrayDrawer : PropertyDrawer
{
   public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
   {
      //base.OnGUI(position, property, label);
      try
      {
         int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
         EditorGUI.PropertyField(position, property, new GUIContent(((NamedArrayAttribute)attribute).names[pos]));
      }
      catch 
      {
         EditorGUI.PropertyField(position, property, label);
      }
   }
}
