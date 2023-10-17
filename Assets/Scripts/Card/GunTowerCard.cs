using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam
{
    [CreateAssetMenu(menuName = "Config/Card/<Temp> Add Gun Tower")]
    public class GunTowerCard : Card
    {
        #region Fields
        [ClassHeader(typeof(GunTowerCard))]
        [SerializeField] private TowerConfig config;
        [SerializeField] private int numLv1;
        [SerializeField] private int numLv2;
        [SerializeField] private int numLv3;
        private Player player = Player.Instance;
        #endregion

        #region Publics
        public override void PlayLv1()
        {
            player.AddTower(config, numLv1);
        }

        public override void PlayLv2()
        {
            player.AddTower(config, numLv2);
        }

        public override void PlayLv3()
        {
            player.AddTower(config, numLv3);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
