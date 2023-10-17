using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CSCI526GameJam
{

    public abstract class Skill : ScriptableObject
    {

        #region Fields
        [ClassHeader(typeof(Skill))]

        [SerializeField] private string skillName;
        [SerializeField] private int coolDown;

        [TextArea(5, 10)]
        [SerializeField] private string description;
        [SerializeField] private Sprite image;
        #endregion

        #region Publics
        public string CardName => cardName;
        public int CoolDown => coolDown;

        public string Description => description;
        public Sprite Image => image;

        public abstract void PlayLv1();
        public abstract void PlayLv2();
        public abstract void PlayLv3();

        public abstract void SetCoolDown(int sec);
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
