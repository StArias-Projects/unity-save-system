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
    [CreateAssetMenu(fileName = "NewGameData", menuName = "GameData", order = 0)]

    public class GameData_B : GameData
    {
        [SerializeField]
        public int health = 0;

        [SerializeField]
        public int mana = 0;

        [SerializeField]
        public Vector3 position = new Vector3(0, 0, 0);

        [SerializeField]
        public int numHints = 0;

        [SerializeField]
        public bool isPremium = false;
    }
}