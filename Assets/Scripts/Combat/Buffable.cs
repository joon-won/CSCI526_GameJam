using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CSCI526GameJam.Buffs;

namespace CSCI526GameJam {
    public class Buffable : MonoBehaviour {

        #region Fields
        [ClassHeader(typeof(Buffable))]

        [ComputedFields]
        [ShowInInspector] protected Dictionary<BuffConfig, Buff> buffs = new();
        #endregion

        #region Publics
        public List<Buff> Buffs => buffs.Values.ToList();

        public void AddBuff(Buff buff) {
            if (buffs.TryGetValue(buff.Config, out var existingBuff)) {
                existingBuff.Apply();
            }
            else {
                buffs[buff.Config] = buff;
                buff.Apply();
            }
        }
        #endregion

        #region Internals
        protected void UpdateBuffs() {
            foreach (var buff in buffs.Values.ToList()) {
                buff.Update();
                if (!buff.IsExpired) continue;

                RemoveBuff(buff);
            }
        }

        protected void RemoveBuff(Buff buff) {
            buff.End();
            buffs.Remove(buff.Config);
        }
        #endregion

        #region Unity Methods
        protected virtual void Update() {
            UpdateBuffs();
        }
        #endregion
    }
}
