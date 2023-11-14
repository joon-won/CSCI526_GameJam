using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam
{
    [CreateAssetMenu(menuName = "Config/Skill/General")]
    public class SkillConfig : ScriptableObject
    {
        #region Fields
        [SerializeField] private string skillName;
        [SerializeField] private int coolDown;

        [SerializeField] private string description;
        [SerializeField] private Sprite image;


        #endregion


        public int CoolDown => coolDown;
        public string SkillName => skillName;
        public Sprite Image => image;


        public string Description => description;
        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
