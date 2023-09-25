using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam
{
    public class BeastEnemy : Enemy {
        #region Fields
        [ClassHeader(typeof(BugEnemy))]

        [MandatoryFields]

        [SerializeField] protected string altname = "Beast";
        #endregion

        #region Public

        #endregion Public
    
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
