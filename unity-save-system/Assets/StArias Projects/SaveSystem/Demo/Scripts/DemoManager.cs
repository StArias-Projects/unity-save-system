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
using TMPro;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

namespace StArias.API.Demo
{
    public class DemoManager : MonoBehaviour
    {
        #region Private Variables

        /// <summary>
        /// The save load manager instance
        /// </summary>
        private SaveLoadManager _saveLoadManager;

        private List<GameObject> _objectsToDelete = new List<GameObject>();

        private bool _isRemovingObjectsRunning = true;

        #endregion

        #region Editor Variables

        [Header("Save Load Mode")]

        [SerializeField]
        private bool _displayLogs = true;

        [Header("Data Creation")]
        [SerializeField]
        private Button _saveCustomGameDataButton;

        [SerializeField]
        private Button _saveDemoGameDataButton;

        [SerializeField]
        private GameData _demoGameData;

        [Header("Data Loader")]

        [SerializeField]
        private RectTransform _loadButtonsParent;

        [SerializeField]
        private RectTransform _loadRectTransformPrefab;

        [SerializeField]
        private TextMeshProUGUI currentDataText;

        #endregion

        private void Awake()
        {
            DebugLogger.EnableDebugLog = _displayLogs;
            StartCoroutine(RemoveDataFromUICollection());

            _saveLoadManager = SaveLoadManager.GetInstance();
            _saveLoadManager.InitGameDataCollection();

            InitSaveDataButtons();
            InitLoadDataButtons();
        }

        #region Init

        private void InitSaveDataButtons()
        {
            if (_saveCustomGameDataButton != null)
                _saveCustomGameDataButton.onClick.AddListener(SaveCustomGameData);
            else
                DebugLogger.Log("Save Custom Game Data Button is null", DebugColor.Red);

            if (_saveDemoGameDataButton != null)
                _saveDemoGameDataButton.onClick.AddListener(SaveDemoGameData);
            else
                DebugLogger.Log("Demo Editor Game Data Button is null", DebugColor.Red);
        }

        private void InitLoadDataButtons()
        {
            var gameCollection = _saveLoadManager.GetGameDataCollection();
            Vector2 sizeDelta = Vector2.zero;

            foreach (var collectionData in gameCollection)
            {
                Vector2 pos = new Vector2(0, sizeDelta.y);

                sizeDelta.y += _loadRectTransformPrefab.sizeDelta.y * 1.05f;
                RectTransform obj = Instantiate(_loadRectTransformPrefab, _loadButtonsParent);
                var child0 = obj.GetChild(0);
                var child1 = obj.GetChild(1);

                obj.localPosition = -pos;


                child0.GetComponentInChildren<TextMeshProUGUI>().text = collectionData.Key;
                Button objButton0 = child0.GetComponent<Button>();
                objButton0.onClick.AddListener(() =>
                {
                    MyGameData data = SaveLoadHelper.CastFromGameData<MyGameData>(collectionData.Value);

                    currentDataText.text = "";
                    FieldInfo[] fields = SaveLoadHelper.GetGameDataFields<MyGameData>();
                    foreach (FieldInfo field in fields)
                    {
                        currentDataText.text += $"{field.Name}: {field.GetValue(data)}\n";
                    }
                });

                Button objButton1 = child1.GetComponent<Button>();
                objButton1.onClick.AddListener(() =>
                {
                    _saveLoadManager.DeleteDataByID(collectionData.Value.id);
                    _objectsToDelete.Add(obj.gameObject);
                });
            }

            _loadButtonsParent.sizeDelta = sizeDelta;
        }

        #endregion

        /// <summary>
        /// Save the created custom game data
        /// </summary>
        private void SaveCustomGameData()
        {
            if (_saveLoadManager == null)
            {
                DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
                return;
            }

            MyGameData gameData = ScriptableObject.CreateInstance<MyGameData>();
            gameData.name = gameData.id = "Script_Game_Data";
            gameData.health = 100;
            gameData.mana = 150;
            gameData.position = new Vector3(100, 50, 35);

            string gameDataID = _saveLoadManager.SaveNewData(gameData);
            AddGameDataToTheCollection(gameDataID);
        }

        /// <summary>
        /// Saves the game data using the Scriptable Object created on the Editor
        /// </summary>
        private void SaveDemoGameData()
        {
            if (_saveLoadManager == null)
            {
                DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
                return;
            }

            if (_demoGameData == null)
            {
                DebugLogger.Log("Demo Game Data is null", DebugColor.Red);
                return;
            }

            string gameDataID = _saveLoadManager.SaveNewData(_demoGameData);
            AddGameDataToTheCollection(gameDataID);
        }

        private void AddGameDataToTheCollection(string gameDataID)
        {
            // 1. Creation of the object
            RectTransform obj = Instantiate(_loadRectTransformPrefab, _loadButtonsParent);
            var child0 = obj.GetChild(0);
            var child1 = obj.GetChild(1);

            // 2. Positioning the object
            Vector2 sizeDelta = _loadButtonsParent.sizeDelta;
            obj.localPosition = new Vector2(0, -sizeDelta.y);

            // 3. Update the size of the parent
            sizeDelta.y += _loadRectTransformPrefab.sizeDelta.y * 1.05f;
            _loadButtonsParent.sizeDelta = sizeDelta;

            // 4. Update the object attributes
            child0.GetComponentInChildren<TextMeshProUGUI>().text = gameDataID;
            Button objButton0 = child0.GetComponent<Button>();
            objButton0.onClick.AddListener(() =>
            {
                MyGameData data = SaveLoadHelper.CastFromGameData<MyGameData>(_saveLoadManager.GetGameDataByID(gameDataID));

                currentDataText.text = "";
                FieldInfo[] fields = SaveLoadHelper.GetGameDataFields<MyGameData>();
                foreach (FieldInfo field in fields)
                {
                    currentDataText.text += $"{field.Name}: {field.GetValue(data)}\n";
                }
            });

            Button objButton1 = child1.GetComponent<Button>();
            objButton1.onClick.AddListener(() =>
            {
                _saveLoadManager.DeleteDataByID(gameDataID);
                _objectsToDelete.Add(obj.gameObject);
            });
        }

        IEnumerator RemoveDataFromUICollection() 
        {
            while (_isRemovingObjectsRunning)
            {
                yield return new WaitUntil(() => _objectsToDelete.Count > 0);

                for (int i = _objectsToDelete.Count - 1; i >= 0; i--)
                {
                    Destroy(_objectsToDelete[i]);
                }

                UpdateGameDataCollection();
            }
        }

        private void UpdateGameDataCollection()
        {
            Vector2 sizeDelta = Vector2.zero;

            for (int i = 0; i < _loadButtonsParent.childCount; i++)
            {
                _loadButtonsParent.GetChild(i).GetComponent<RectTransform>().localPosition = new Vector2(0, -sizeDelta.y);
                sizeDelta.y += _loadRectTransformPrefab.sizeDelta.y * 1.05f;
            }

            _loadButtonsParent.sizeDelta = sizeDelta;
        }
    }
}