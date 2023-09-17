using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

using Sirenix.OdinInspector;

namespace CSCI526GameJam {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public abstract class CustomHeaderAttribute : PropertyAttribute {

        protected string text;
        protected Color textColor = Color.white;
        protected TextAnchor textAnchor;

        public string Text => text;
        public Color TextColor => textColor;
        public TextAnchor TextAnchor => textAnchor;
    }

    public class ClassHeaderAttribute : CustomHeaderAttribute {
        public ClassHeaderAttribute(Type type) {
            var words = Regex.Split(type.Name, @"(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])");
            var name = string.Join(" ", words);
            name = name.Trim();
            name = "<---" + name + "--->";

            text = name;
            textColor = Color.cyan;
            textAnchor = TextAnchor.LowerCenter;
        }
    }

    public class MandatoryFieldsAttribute : CustomHeaderAttribute {
        public MandatoryFieldsAttribute() {
            text = "<Mandatory>";
            textColor = Color.red;
        }
    }
    public class ComputedFieldsAttribute : CustomHeaderAttribute {
        public ComputedFieldsAttribute() {
            text = "<Computed>";
            textColor = Color.green;
        }
    }
    public class EditorOnlyFieldsAttribute : CustomHeaderAttribute {
        public EditorOnlyFieldsAttribute() {
            text = "<Editor Only>";
            textColor = Color.yellow;
        }
    }
}
