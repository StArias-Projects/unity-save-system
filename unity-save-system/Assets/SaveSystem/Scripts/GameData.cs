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
    public class GameData : ScriptableObject
    {
        [SerializeField] private string gameDataName = "Default";
        public string GameDataName { get { return gameDataName; } }


        [SerializeField] private int numHints = 0;
        public int NumHints { get => numHints; }

        
        [SerializeField] private bool isPremium = false;
        public bool IsPremium { get => isPremium; }

        [SerializeField] public string hash = "";

        public GameData(string gameDataName, int numHints, bool isPremium)
        {
            this.gameDataName = gameDataName;
            this.numHints = numHints;
            this.isPremium = isPremium;
        }

        public GameData(GameData gameData)
        {
            if (gameData == null) return;

            gameDataName = gameData.gameDataName;
            numHints = gameData.numHints;
            isPremium = gameData.isPremium;
        }
    }
}