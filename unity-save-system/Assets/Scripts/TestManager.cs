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
    public GameData slotC;
    private SaveLoadManager saveLoadManager;

    private void Awake()
    {
        saveLoadManager = SaveLoadManager.GetInstance();    
        saveLoadManager.Load();
    }

    public void CreateDataA()
    {
        if (saveLoadManager == null)
        {
            DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
            return;
        }

        GameData slotA = ScriptableObject.CreateInstance<GameData>();
        slotA.name = "DataA";
        slotA.gameDataName = "DataA";

        saveLoadManager.AddGameDataToSave(slotA);
    }

    public void CreateDataB()
    {
        if (saveLoadManager == null)
        {
            DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
            return;
        }

        GameData_B slotB = ScriptableObject.CreateInstance<GameData_B>();
        slotB.name = "Data B";
        slotB.gameDataName = "Data B";
        slotB.numHints = 200;
        slotB.isPremium = false;
        slotB.health = 50;
        slotB.mana = 50;
        slotB.position = new Vector3(1, 2, 3);

        saveLoadManager.AddGameDataToSave(slotB);
    }

    public void CreateDataC()
    {
        if (saveLoadManager == null)
        {
            DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
            return;
        }

        if (slotC == null)
        {
            DebugLogger.Log("Slot C is null", DebugColor.Red);
            return;
        }

        saveLoadManager.AddGameDataToSave(slotC);
    }

    public void Save()
    {
        if(saveLoadManager == null)
        {
            DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
            return;
        }

        saveLoadManager.Save();
    }
}
