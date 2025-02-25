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

namespace StArias.API.Demo
{
    public class DemoManager : MonoBehaviour
    {
        #region Private Variables

        /// <summary>
        /// The save load manager instance
        /// </summary>
        private SaveLoadManager _saveLoadManager;

        private const string SAVE_MODE_TEXT = "Toggle Save/LoadMode\nSAVE MODE";
        private const string LOAD_MODE_TEXT = "Toggle Save/LoadMode\nLOAD MODE";

        #endregion

        #region Editor Variables

        [Header("Save Load Mode")]
        [SerializeField]
        private bool _isSaveMode = true;

        [SerializeField]
        private Button _toggleSaveLoadButton;

        [SerializeField]
        private TextMeshProUGUI _toggleSaveLoadText;

        [SerializeField]
        private GameObject _SaveModeParent;

        [SerializeField]
        private GameObject _LoadModeParent;

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
        private RectTransform _loadButtonsParent;

        [SerializeField]
        private RectTransform _loadRectTransformPrefab;

        [SerializeField]
        private TextMeshProUGUI currentDataText;

        #endregion

        private void Awake()
        {
            _saveLoadManager = SaveLoadManager.GetInstance();
            _saveLoadManager.InitGameDataCollection();

            InitSaveLoadModeButtons();
            InitSaveDataButtons();
            InitLoadDataButtons();
        }

        #region Init

        private void InitSaveLoadModeButtons()
        {
            _toggleSaveLoadText.text = _isSaveMode ? SAVE_MODE_TEXT : LOAD_MODE_TEXT;

            _SaveModeParent.SetActive(_isSaveMode);
            _LoadModeParent.SetActive(!_isSaveMode);

            _toggleSaveLoadButton.onClick.AddListener(() =>
            {
                _isSaveMode = !_isSaveMode;

                _SaveModeParent.SetActive(_isSaveMode);
                _LoadModeParent.SetActive(!_isSaveMode);

                _toggleSaveLoadText.text = _isSaveMode ? SAVE_MODE_TEXT : LOAD_MODE_TEXT;
            });
        }

        private void InitSaveDataButtons()
        {
            if (_dataAButton != null)
                _dataAButton.onClick.AddListener(SaveDataA);
            else
                DebugLogger.Log("Data A Button is null", DebugColor.Red);

            if (_dataBButton != null)
                _dataBButton.onClick.AddListener(SaveDataB);
            else
                DebugLogger.Log("Data B Button is null", DebugColor.Red);

            if (_dataCButton != null)
                _dataCButton.onClick.AddListener(SaveDataC);
            else
                DebugLogger.Log("Data C Button is null", DebugColor.Red);
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
                obj.localPosition = -pos;
                obj.GetComponentInChildren<TextMeshProUGUI>().text = collectionData.Key;

                var objButton = obj.GetComponent<Button>();
                objButton.onClick.AddListener(() =>
                {
                    MyGameData data = SaveLoadManager.CastFromGameData<MyGameData>(collectionData.Value);
                    currentDataText.text = $"ID: {data.id}\n" +
                    $"Health: {data.health}\n" +
                    $"Mana: {data.mana}\n" +
                    $"Position: {data.position}";
                });
            }

            _loadButtonsParent.sizeDelta = sizeDelta;
        }

        #endregion

        private void SaveDataA()
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

            string gameDataID = _saveLoadManager.SaveNewData(slotA);
            UpdateGameDataList(gameDataID);
        }

        private void SaveDataB()
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

            string gameDataID = _saveLoadManager.SaveNewData(slotB);
            UpdateGameDataList(gameDataID);
        }

        private void SaveDataC()
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

            string gameDataID = _saveLoadManager.SaveNewData(_slotC);
            UpdateGameDataList(gameDataID);
        }

        private void UpdateGameDataList(string gameDataID)
        {
            // 1. Creation of the object
            RectTransform obj = Instantiate(_loadRectTransformPrefab, _loadButtonsParent);

            // 2. Positioning the object
            Vector2 sizeDelta = _loadButtonsParent.sizeDelta;
            obj.localPosition = new Vector2(0, -sizeDelta.y);

            // 3. Update the size of the parent
            sizeDelta.y += _loadRectTransformPrefab.sizeDelta.y * 1.05f;
            _loadButtonsParent.sizeDelta = sizeDelta;

            // 4. Update the object attributes
            obj.GetComponentInChildren<TextMeshProUGUI>().text = gameDataID;
            var objButton = obj.GetComponent<Button>();
            objButton.onClick.AddListener(() =>
            {
                MyGameData data = SaveLoadManager.CastFromGameData<MyGameData>(_saveLoadManager.GetGameDataByID(gameDataID));
                currentDataText.text = $"ID: {data.id}\n" +
                $"Health: {data.health}\n" +
                $"Mana: {data.mana}\n" +
                $"Position: {data.position}";
            });
        }
    }
}