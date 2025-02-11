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
    public static class SaveLoadManager
    {
        private static readonly string PersistentDataPath = Application.persistentDataPath + "/";

        private static List<GameData> GameDataSlots = new List<GameData>();

        public static void Save()
        {
            try
            {
                DebugLogger.Log("Starting to save data...", DebugColor.White);

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
                    File.WriteAllText(PersistentDataPath + slot.GameDataName, dataToSave);
                }
            }
            catch (System.Exception e)
            {
                DebugLogger.Log("- Error when saving data -", color: DebugColor.Red);
                DebugLogger.Log(e.Message, color: DebugColor.Red);
                throw new System.Exception(e.Message);
            }
        }

        public static void Load()
        {
            //if (!File.Exists(Path + FileName))
            //{
            //    CreateDefaultJson();
            //}
            //else
            //{
            //    try
            //    {
            //        LogReset();
            //        DebugLogs("Estamos usando la ruta " + Path);
            //        // 1. Se leen los datos del json y se transforman
            //        var json = File.ReadAllText(Path + FileName);
            //        _currData = JsonUtility.FromJson<DataToSave>(json);
            //    }
            //    catch (System.Exception e)
            //    {
            //        DebugLogs(e.Message);
            //        throw new System.Exception(e.Message);
            //    }

            //    // 2. Se compara el hash con el original
            //    var originalHash = _currData.GetHash();
            //    // Como el hash original se construyó a partir de un hash = null, primero hay que
            //    // construir una copia, asignar su hash como null y generar el hash de la copia.
            //    // Mirar punto 4 del método CreateDefaultJson()
            //    var dataAux = new DataToSave(_currData);
            //    dataAux.SetHash(null);
            //    var hash = SecureManager.Hash(JsonUtility.ToJson(dataAux));

            //    if (originalHash.Equals(hash))
            //    {
            //        DebugLogs("Datos verificados...");
            //    }
            //    else
            //    {
            //        DebugLogs("Datos corruptos, creando unos por defecto...");
            //        CreateDefaultJson();
            //    }
            //}
        }

        public static void AddGameDataToSave(GameData gameData)
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