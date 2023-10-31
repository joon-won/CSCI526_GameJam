using UnityEngine.Serialization;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


namespace CSCI526GameJam
{

    public abstract class Skill : MonoBehaviour
    {

        #region Fields
        [SerializeField] protected SkillConfig config;
        #endregion

        #region Publics
        public SkillConfig Config => config;



        public abstract void PlayLv3();
        public abstract void PlayLv2();
        public abstract void PlayLv1();

        public abstract void SetCoolDown(int sec);
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
