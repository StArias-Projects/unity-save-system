/*  
 * This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.  
 * If a copy of the MPL was not distributed with this file, you can obtain one at  
 * https://mozilla.org/MPL/2.0/.  
 *  
 * Copyright (c) 2025 StArias - https://github.com/starias  
 */

using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace StArias.API.SaveLoadSystem
{
    public class SaveLoadManager
    {
        private readonly string SavePath = Path.Combine(Application.persistentDataPath, "save");

        private List<GameData> GameDataSlots = new List<GameData>();

        private static SaveLoadManager Instance;

        public SaveLoadManager()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

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
                    var hash = HashGenerator.Hash(dataToSave);
                    slot.hash = hash;

                    // 2. Transform the Slot + HASH into JSON
                    dataToSave = JsonUtility.ToJson(slot);
                    DebugLogger.Log("Data correctly saved...", color: DebugColor.Green);

                    // 4. Se sobreescribe el archivo 

                    File.WriteAllText(Path.Combine(SavePath, slot.gameDataName + ".json"), dataToSave);
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

            string[] files = Directory.GetFiles(SavePath + "/", "*.json");

            foreach (string file in files)
            {
                Debug.Log("Archivo encontrado: " + Path.GetFileName(file));

                // Leer el contenido del archivo si lo necesitas
                string jsonContent = File.ReadAllText(file);
                var obj = JsonUtility.FromJson<GameData>(jsonContent);
                //GameDataSlots.Add();
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