/*  
 * This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.  
 * If a copy of the MPL was not distributed with this file, you can obtain one at  
 * https://mozilla.org/MPL/2.0/.  
 *  
 * Copyright (c) 2025 StArias - https://github.com/starias  
 */

using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Reflection;

namespace StArias.API.SaveLoadSystem
{
    public class SaveLoadManager
    {
        private readonly string SavePath = Path.Combine(Application.persistentDataPath, "save");

        private List<GameData> GameDataSlots = new List<GameData>();

        private static SaveLoadManager Instance;

        private SaveLoadManager() { }

        public static SaveLoadManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new SaveLoadManager();
            }

            return Instance;
        }

        public void Save()
        {
            try
            {
                DebugLogger.Log("Starting to save data...", DebugColor.White);

                if (!Directory.Exists(SavePath))
                {
                    Directory.CreateDirectory(SavePath);
                }

                foreach (var slot in GameDataSlots)
                {
                    // 1. Add the HASH to the slot
                    var dataToSave = JsonUtility.ToJson(slot);

                    // 2. The file is overwritten
                    File.WriteAllText(Path.Combine(SavePath, slot.gameDataName), dataToSave);
                    DebugLogger.Log("Data: " + slot.gameDataName + " correctly saved", color: DebugColor.Green);
                }

            }
            catch (System.Exception e)
            {
                DebugLogger.Log("- Error when saving data -", color: DebugColor.Red);
                DebugLogger.Log(e.Message, color: DebugColor.Red);
                throw new System.Exception(e.Message);
            }
        }

        public void Load()
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            string[] files = Directory.GetFiles(SavePath + "/");

            foreach (string file in files)
            {
                string jsonContent = File.ReadAllText(file);
                GameData slot = ScriptableObject.CreateInstance<GameData>();
                JsonUtility.FromJsonOverwrite(jsonContent, slot);
                string dataType = slot.GetDataType();

                if (string.IsNullOrEmpty(dataType))
                {
                    DebugLogger.Log("Invalid file: Data type is empty or null", DebugColor.Red);
                    return;
                }

                Type type = Type.GetType(dataType);

                if (dataType == null || !typeof(GameData).IsAssignableFrom(type))
                {
                    Debug.LogError("Tipo desconocido o inválido: " + dataType);
                    return;
                }

                slot = ScriptableObject.CreateInstance(dataType) as GameData;
                JsonUtility.FromJsonOverwrite(jsonContent, slot);

                DebugLogger.Log("Data: " + slot.gameDataName + " correctly loaded", color: DebugColor.Green);
            }
        }

        public void AddGameDataToSave(GameData gameData)
        {
            if (GameDataSlots.Contains(gameData))
            {
                DebugLogger.Log("The game data is already in the list", color: DebugColor.Yellow);
                return;
            }

            GameDataSlots.Add(gameData);
        }
    }
}