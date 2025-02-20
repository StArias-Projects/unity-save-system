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

namespace StArias.API.SaveLoadSystem
{
    /// <summary>
    /// Manages the saving and loading of the game data
    /// <para></para>
    /// See <see cref="GetInstance"/> to get the singleton instance of the SaveLoadManager
    /// </summary>
    public class SaveLoadManager
    {
        /// <summary>
        /// The path where the game data is stored
        /// </summary>
        private readonly string _savePath = Path.Combine(Application.persistentDataPath, "save");

        /// <summary>
        /// List of the saved game data. Each element is called "slot" 
        /// </summary>
        private Dictionary<string, GameData> _gameDataSlots = new Dictionary<string, GameData>();

        /// <summary>
        /// Singleton instance of the SaveLoadManager
        /// </summary>
        private static SaveLoadManager Instance;

        private SaveLoadManager() { }

        /// <summary>
        /// Returns the singleton instance of the SaveLoadManager
        /// </summary>
        public static SaveLoadManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new SaveLoadManager();
            }

            return Instance;
        }

        /// <summary>
        /// Creates the directory where the game data will be saved
        /// </summary>
        private void CreateSavePathDir()
        {
            if (Directory.Exists(_savePath))
                return;

            Directory.CreateDirectory(_savePath);
        }

        /// <summary>
        /// Adds a new game data to the list.
        /// <para></para>
        /// If a game data with the same ID already exists, a new one will be created with a different ID
        /// <para></para>
        /// The list of the game data is accessible via <see cref="GetGameDataSlots"/>.
        /// </summary>
        /// <param name="dataToSave">The game data to be saved</param>
        public void SaveNewData(GameData dataToSave)
        {
            try
            {
                string originalID = dataToSave.id;
                CreateSavePathDir();
                DebugLogger.Log($"Saving {originalID}...", DebugColor.White);

                // 1. Check if the data already exists
                bool dataExist = _gameDataSlots.ContainsKey(originalID);
                int i = 0;

                while (dataExist)
                {
                    dataToSave.id = originalID + "_" + i;
                    dataExist = _gameDataSlots.ContainsKey(dataToSave.id);
                    if (!dataExist)
                        DebugLogger.Log($"The game data {originalID} already exists. " +
                            $"The game data {dataToSave.id} will be saved instead", DebugColor.Yellow);
                }

                _gameDataSlots[dataToSave.id] = dataToSave;

                // 2. Save the game data to a file
                string sDataToSave = JsonUtility.ToJson(dataToSave);
                File.WriteAllText(Path.Combine(_savePath, dataToSave.id), sDataToSave);

                DebugLogger.Log("Data: " + dataToSave.id + " correctly saved", color: DebugColor.Green);
            }
            catch (System.Exception e)
            {
                DebugLogger.Log("- Error when saving data -", color: DebugColor.Red);
                DebugLogger.Log(e.Message, color: DebugColor.Red);
            }
        }

        public void SaveExistingData(GameData dataToSave)
        {

        }

        /// <summary>
        /// Reads the saved data from the disk and load it into the game data list
        /// <para></para>
        /// The list of the game data is accessible via <see cref="GetGameDataSlots"/>.
        /// </summary>
        public void ReadDataFromDisk()
        {
            CreateSavePathDir();

            // 1. Get all the files in the save path
            string[] files = Directory.GetFiles(_savePath + "/");
            foreach (string file in files)
            {
                // 2. Read the content of the file
                string dataContent = File.ReadAllText(file);

                // 3. Create a new GameData object and load the content of the file
                GameData loadedData = ScriptableObject.CreateInstance<GameData>();
                JsonUtility.FromJsonOverwrite(dataContent, loadedData);

                // 4. Get the type of the data
                string savedType = loadedData.GetDataType();
                if (string.IsNullOrEmpty(savedType))
                {
                    DebugLogger.Log("Invalid file: Data type is empty or null", DebugColor.Red);
                    continue;
                }

                Type dataType = Type.GetType(savedType);

                if (savedType == null || !typeof(GameData).IsAssignableFrom(dataType))
                {
                    Debug.LogError("The type of the data is invalid or unknown: " + savedType);
                    continue;
                }

                // 5. Create a new GameData object of the correct type
                loadedData = ScriptableObject.CreateInstance(savedType) as GameData;
                JsonUtility.FromJsonOverwrite(dataContent, loadedData);

                DebugLogger.Log("Data: " + loadedData.id + " correctly loaded", color: DebugColor.Green);
                _gameDataSlots[loadedData.id] = loadedData;
            }
        }

        /// <summary>
        /// Gets the game data by its key. The key is the id attached to the <see cref="GameData"/>
        /// <para></para>
        /// Sees also <see cref="GetGameDataSlots"/>
        /// </summary>
        /// <param name="dataIndex">Position of the game data in the list of slots</param>
        public GameData GetGameDataByKey(string gameDataID)
        {
            if (!_gameDataSlots.ContainsKey(gameDataID))
            {
                DebugLogger.Log($"The game data with the ID {gameDataID} does not exist", color: DebugColor.Red);
                return null;
            }

            //if (dataIndex < 0 || dataIndex >= _gameDataSlots.Count)
            //{
            //    DebugLogger.Log($"The index {dataIndex} is out of range", color: DebugColor.Red);
            //    return null;
            //}

            return _gameDataSlots[gameDataID];
        }

        /// <summary>
        /// Gets the list of the current saved data
        /// </summary>
        public Dictionary<string, GameData> GetGameDataSlots()
        {
            return _gameDataSlots;
        }

        /// <summary>
        /// Allows a cast from <see cref="GameData"/> to any type that inherits it
        /// </summary>
        /// <typeparam name="T">The type to cast to</typeparam>
        /// <param name="dataToCast">The data to be cast</param>
        public T CastFromGameData<T>(GameData dataToCast) where T : GameData
        {
            if (dataToCast == null)
            {
                DebugLogger.Log("CastFromGameData - The provided GameData object is null", DebugColor.Red);
                return null;
            }

            if (dataToCast is T castedData)
                return castedData;
         
            Debug.LogError("CastFromGameData - The provided GameData object cannot be cast to the specified type.");
            return null;  // or throw an exception if you prefer
        }
    }
}