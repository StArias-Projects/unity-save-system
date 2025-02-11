/*  
 * This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.  
 * If a copy of the MPL was not distributed with this file, you can obtain one at  
 * https://mozilla.org/MPL/2.0/.  
 *  
 * Copyright (c) 2025 StArias - https://github.com/starias  
 */

using UnityEngine;
using StArias.API.SaveLoadSystem;

public class TestManager : MonoBehaviour
{
    public void CreateDataA()
    {
        GameData newSlot_A = new GameData(gameDataName: "Data A", 100, true);
        SaveLoadManager.AddGameDataToSave(newSlot_A);
    }

    public void CreateDataB()
    {
        GameData_B newSlot_B = new GameData_B(
            gameDataName: "Data B",
            numHints: 200,
            premium: false,
            health: 50,
            mana: 50,
            position: new Vector3(1, 2, 3)
        );
        SaveLoadManager.AddGameDataToSave(newSlot_B);
    }

    public void Save() 
    {
        SaveLoadManager.Save();
    }
}
