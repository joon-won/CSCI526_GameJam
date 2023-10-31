using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam
{
    [CreateAssetMenu(menuName = "Config/Skill/General")]
    public class SkillConfig : ScriptableObject
    {
        #region Fields

        [SerializeField] private Sprite image;
        [SerializeField] private string description;
        [SerializeField] private int coolDown;
        [SerializeField] private string skillName;

        #endregion

        public Sprite Image => image;

        public int CoolDown => coolDown;
        public string SkillName => skillName;


        public string Description => description;
        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
