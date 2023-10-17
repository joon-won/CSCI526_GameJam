using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam
{
    public class GunTowerCard : Card
    {
        #region Fields
        [ClassHeader(typeof(GunTowerCard))]
        [SerializeField] private TowerConfig config;

        private Player player = FindObjectOfType<Player>();
        #endregion

        #region Publics
        public override void PlayLv1()
        {
            player.AddTower(config, 1);
        }

        public override void PlayLv2()
        {
            player.AddTower(config, 3);
        }

        public override void PlayLv3()
        {
            player.AddTower(config, 6);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
