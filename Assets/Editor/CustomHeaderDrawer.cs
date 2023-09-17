using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CSCI526GameJam {
    [CustomPropertyDrawer(typeof(ClassHeaderAttribute))]
    [CustomPropertyDrawer(typeof(MandatoryFieldsAttribute))]
    [CustomPropertyDrawer(typeof(ComputedFieldsAttribute))]
    [CustomPropertyDrawer(typeof(EditorOnlyFieldsAttribute))]
    public class CustomHeaderDrawer : DecoratorDrawer {
        public override void OnGUI(Rect position) {
            var target = attribute as CustomHeaderAttribute;

            var originColor = GUI.color;
            GUI.color = target.TextColor;

            position.yMin += EditorGUIUtility.singleLineHeight * 0.5f;
            position = EditorGUI.IndentedRect(position);

            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.alignment = target.TextAnchor;

            GUI.Label(position, target.Text, style);

            GUI.color = originColor;
        }

        public override float GetHeight() {
            return EditorGUIUtility.singleLineHeight * 1.5f;
        }
    }
}
