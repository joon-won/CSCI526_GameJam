using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam
{
    [CreateAssetMenu(menuName = "Config/Card/<Temp> Add Ice Tower")]
    public class IceTowerCard : Card
    {
        #region Fields
        [ClassHeader(typeof(IceTowerCard))]
        [SerializeField] private TowerConfig config;
        [SerializeField] private int numLv3;
        [SerializeField] private int numLv2;
        [SerializeField] private int numLv1;


        #endregion

        #region Publics
        public override void PlayLv3()
        {
            Player.Instance.AddTower(config, numLv3);
        }
        
        public override void PlayLv2()
        {
            Player.Instance.AddTower(config, numLv2);
        }

        public override void PlayLv1()
        {
            Player.Instance.AddTower(config, numLv1);
        }

        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
