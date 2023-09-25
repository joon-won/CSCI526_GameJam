using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {

    // Example:
    //private Numeric damage;
    //private Numeric valueModifier;
    //private Numeric scalarModifier;
    //private void OnItemAdded() {
    //    // Numeric formula:
    //    // resolvedValue = (baseValue + valueModifiers.Value + bonusValue)
    //    //    * (1f + scalarModifiers.Value + bonusScalar);

    //    damage = new(5f); // Now damage = 5
    //    valueModifier = new(10f); // Value modifier is a Numeric of 10
    //    scalarModifier = new(3f); // Scalar modifier is a Numeric of 3

    //    damage.IncreaseValue(10f); // Add 10 bonus value. Now damage = 15
    //    damage.IncreaseScalar(1f); // Add 1 bonus scalar (double damage). Now damage = 30

    //    // Add value modifier with a ratio of 3f.
    //    // Now damage = (5 + 3 * 10 + 10) * (1 + 1) = 90
    //    damage.AddValueModifier(3f, valueModifier);

    //    // Add scalar modifier with a ratio of 0.5f.
    //    // Now damage = (5 + 3 * 10 + 10) * (1 + 0.5 * 3 + 1) = 157.5
    //    damage.AddScalarModifier(0.5f, scalarModifier);
    //}

    /// <summary>
    /// Shortcut for creating a pair of value and scalar modifier. 
    /// </summary>
    [Serializable]
    public class NumericSet {
        [SerializeField] private Numeric value; // extra value
        [SerializeField] private Numeric scalar; // extra scalar

        public Numeric Value => value;
        public Numeric Scalar => scalar;

        public NumericSet() {
            value = new();
            scalar = new();
        }
    }


    /// <summary>
    /// Dynamically modifiable numeric value. 
    /// </summary>
    [Serializable]
    public class Numeric {

        /// <summary>
        /// Modifier of a numeric. 
        /// </summary>
        [Serializable]
        private class Modifier {

            [SerializeField] public float ratio;
            [SerializeReference] public Numeric numeric;

            public float Value {
                get {
                    if (numeric == null) {
                        Debug.Log("Invalid modifier. ");
                        return 0f;
                    }
                    return ratio * numeric;
                }
            }

            /// <summary>
            /// Add additional modifier in runtime. 
            /// </summary>
            public Modifier(float ratio, Numeric numeric) {
                this.numeric = numeric;
                this.ratio = ratio;
            }
        }

        /// <summary>
        /// Collection of modifiers. 
        /// </summary>
        [Serializable]
        private class ModifierCollection {
            [SerializeField] private List<Modifier> modifiers = new();

            public float Value => modifiers.Sum(modifier => modifier.Value);

            public void Add(Modifier other) {
                for (int i = 0; i < modifiers.Count; i++) {
                    var modifier = modifiers[i];
                    if (modifier.numeric == other.numeric) {
                        modifier.ratio += other.ratio;
                        return;
                    }
                }
                modifiers.Add(other);
            }

            public void Remove(Numeric numeric) {
                for (int i = 0; i < modifiers.Count; i++) {
                    var modifier = modifiers[i];
                    if (modifier.numeric == numeric) {
                        modifiers.RemoveAt(i);
                        return;
                    }
                }
            }

            public void Clear() {
                modifiers.Clear();
            }
        }

        #region Fields
        [SerializeField] private float baseValue;

        [SerializeReference] private ModifierCollection valueModifiers = new();
        [SerializeField] private float bonusValue;

        [SerializeReference] private ModifierCollection scalarModifiers = new();
        [SerializeField] private float bonusScalar;

        [SerializeField] private bool isDirty = true;
        [SerializeField] private float resolvedValue; // Final value
        #endregion

        #region Publics
        public Action OnChanged;

        public virtual float Value {
            get {
                // Update if dirty. 
                if (isDirty) {
                    Update();
                    isDirty = false;
                }
                return resolvedValue;
            }
        }

        public static implicit operator float(Numeric numeric) {
            return numeric.Value;
        }

        public static implicit operator string(Numeric numeric) {
            return numeric.ToString();
        }

        public override string ToString() {
            return Value.ToString();
        }

        public Numeric() {
            baseValue = 0f;

            bonusValue = 0f;
            valueModifiers = new();

            bonusScalar = 0f;
            scalarModifiers = new();

            isDirty = true;
        }

        public Numeric(float baseValue) : base() {
            SetBaseValue(baseValue);
        }

        public void SetBaseValue(float value) {
            baseValue = value;
            SetDirty();
        }

        public void AddValueModifier(float ratio, Numeric numeric) {
            AddModifier(valueModifiers, ratio, numeric);
        }

        public void AddScalarModifier(float ratio, Numeric numeric) {
            AddModifier(scalarModifiers, ratio, numeric);
        }

        public void AddNumericSet(NumericSet set) {
            AddValueModifier(1f, set.Value);
            AddScalarModifier(1f, set.Scalar);
        }

        public void RemoveValueModifier(Numeric numeric) {
            RemoveModifier(valueModifiers, numeric);
        }

        public void RemoveScalarModifier(Numeric numeric) {
            RemoveModifier(scalarModifiers, numeric);
        }

        /// <summary>
        /// Add bonus value. 
        /// </summary>
        /// <param name="amount">Value to increase. </param>
        public void IncreaseValue(float amount) {
            bonusValue += amount;
            SetDirty();
        }

        /// <summary>
        /// Decrease bonus value. 
        /// </summary>
        /// <param name="amount">Value to decrease. </param>
        public void DecreaseValue(float amount) {
            bonusValue -= amount;
            SetDirty();
        }

        /// <summary>
        /// Increase bonus scalar. 
        /// </summary>
        /// <param name="amount">Scalar to increase. </param>
        public void IncreaseScalar(float amount) {
            bonusScalar += amount;
            SetDirty();
        }

        /// <summary>
        /// Decrease bonus scalar. 
        /// </summary>
        /// <param name="amount">Scalar to decrease. </param>
        public void DecreaseScalar(float amount) {
            bonusScalar -= amount;
            SetDirty();
        }
        #endregion

        #region Internals
        private void SetDirty() {
            isDirty = true;
            OnChanged?.Invoke();
        }

        private void Update() {
            resolvedValue = (baseValue + valueModifiers.Value + bonusValue)
                * (1f + scalarModifiers.Value + bonusScalar);
        }

        private void AddModifier(ModifierCollection modifiers, float ratio, Numeric numeric) {
            var modifier = new Modifier(ratio, numeric);
            modifiers.Add(modifier);
            numeric.OnChanged += SetDirty;
            SetDirty();
        }

        private void RemoveModifier(ModifierCollection modifiers, Numeric numeric) {
            modifiers.Remove(numeric);
            numeric.OnChanged -= SetDirty;
            SetDirty();
        }
        #endregion

        #region Unity Methods
        #endregion
    }
}
