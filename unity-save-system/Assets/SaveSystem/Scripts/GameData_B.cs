/*  
 * This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.  
 * If a copy of the MPL was not distributed with this file, you can obtain one at  
 * https://mozilla.org/MPL/2.0/.  
 *  
 * Copyright (c) 2025 StArias - https://github.com/starias  
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StArias.API.SaveLoadSystem
{
    [Serializable]
    public class GameData_B : GameData
    {
        [SerializeField]
        private int health = 100;
        public int Health { get => health; }

        [SerializeField]
        private int mana = 100;
        public int Mana { get => mana; }

        [SerializeField]
        private Vector3 position = new Vector3(0, 0, 0);
        public Vector3 Position { get => position; }

        public GameData_B(
            string gameDataName,
            int numHints,
            bool premium,
            int health,
            int mana,
            Vector3 position
            ) : base(gameDataName, numHints, premium)
        {
            this.health = health;
            this.mana = mana;
            this.position = position;
        }
    }
}