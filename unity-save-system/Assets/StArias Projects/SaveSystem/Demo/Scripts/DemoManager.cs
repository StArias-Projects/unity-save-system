/*  
 * This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.  
 * If a copy of the MPL was not distributed with this file, you can obtain one at  
 * https://mozilla.org/MPL/2.0/.  
 *  
 * Copyright (c) 2025 StArias - https://github.com/starias  
 */

using UnityEngine;
using StArias.API.SaveLoadSystem;
using UnityEngine.UI;

namespace StArias.API.Demo
{
    public class DemoManager : MonoBehaviour
    {
        private SaveLoadManager _saveLoadManager;

        private void Awake()
        {
            _saveLoadManager = SaveLoadManager.GetInstance();
            _saveLoadManager.ReadDataFromDisk();

            UpdateGameDataList();

            // TMP
            InitSaveLoadModeButtons();
            InitCreateDataButtons();
        }

        #region TMP

        [Header("Save Load Mode")]
        [SerializeField]
        private Button _saveModeButton;

        [SerializeField]
        private Button _loadModeButton;


        [Header("Data Creation")]
        [SerializeField]
        private Button _dataAButton;

        [SerializeField]
        private Button _dataBButton;

        [SerializeField]
        private Button _dataCButton;

        [SerializeField]
        private GameData _slotC;

        [Header("Data Loader")]

        [SerializeField]
        private GameObject _loadButtonsParent;

        private void InitSaveLoadModeButtons()
        {
            _saveModeButton.onClick.AddListener(() =>
            {

            });

            _loadModeButton.onClick.AddListener(() =>
            {

            });
        }

        private void InitCreateDataButtons()
        {
            if (_dataAButton != null)
                _dataAButton.onClick.AddListener(CreateDataA);
            else
                DebugLogger.Log("Data A Button is null", DebugColor.Red);

            if (_dataBButton != null)
                _dataBButton.onClick.AddListener(CreateDataB);
            else
                DebugLogger.Log("Data B Button is null", DebugColor.Red);

            if (_dataCButton != null)
                _dataCButton.onClick.AddListener(CreateDataC);
            else
                DebugLogger.Log("Data C Button is null", DebugColor.Red);
        }

        private void CreateDataA()
        {
            if (_saveLoadManager == null)
            {
                DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
                return;
            }

            MyGameData slotA = ScriptableObject.CreateInstance<MyGameData>();
            slotA.name = slotA.id = "DataA";
            slotA.health = 100;
            slotA.mana = 150;
            slotA.position = new Vector3(100, 50, 35);

            _saveLoadManager.SaveNewData(slotA);
            UpdateGameDataList();
        }

        private void CreateDataB()
        {
            if (_saveLoadManager == null)
            {
                DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
                return;
            }

            MyGameData slotB = ScriptableObject.CreateInstance<MyGameData>();
            slotB.name = slotB.id = "DataB";
            slotB.health = 50;
            slotB.mana = 50;
            slotB.position = new Vector3(1, 2, 3);

            _saveLoadManager.SaveNewData(slotB);
        }

        private void CreateDataC()
        {
            if (_saveLoadManager == null)
            {
                DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
                return;
            }

            if (_slotC == null)
            {
                DebugLogger.Log("Slot C is null", DebugColor.Red);
                return;
            }

            _saveLoadManager.SaveNewData(_slotC);
            UpdateGameDataList();
        }

        private void UpdateGameDataList() 
        {
            // TODO: I don't want this method to return the dictionary and then to be able to 
            // modify values. It is not supposed to work like this
            var gameList = _saveLoadManager.GetGameDataSlots();
            // Casting the data to MyGameData
            MyGameData myGameData = _saveLoadManager.CastFromGameData<MyGameData>(gameList["DataA"]);
            myGameData.id = "AAAAA";
        }

        #endregion
    }
}