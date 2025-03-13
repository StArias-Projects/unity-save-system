# Unity Save System Quick Start

Welcome to the tutorial of the SDK! Please, follow the below instructions to quickly understand the usage of the API.

<table>
    <tbody style="text-align:center;">
            <td valign="top" style="text-align:left;">
                <p style="max-width:100%;"><b>SDK Version</b></p>
            </td>
            <td style = "text-align: left;">
                <ul>
                    <li>v1.0</li>
                </ul>
            </td>
    </tbody>
</table>

## 1. Download and import package
Download the Unity package from the release page:

- [Release Packages](https://github.com/StArias-Projects/unity-save-system/releases)

Once you create your own project, please import **unity-save-system.unitypackage**:

- Assets > Import Package > Custom Package

<p align="center">
<img width=70% src="https://github.com/user-attachments/assets/730bda7c-60b2-47ef-a3b4-e2be0718ba43">
</p>

## 2. Create your own GameData
In order to create your own data follow the next steps:

1. Create a script that inherits GameData.cs. In this case, it will be named ```NewGameData.cs```
    - Refer to StArias Projects > Save System > Scripts > API > GameData.cs.
    - Remember to add the directive ```using StArias.API.SaveLoadSystem;```.
2. Add the following line over the class name: ```[CreateAssetMenu(fileName = "NewGameData", menuName = "NewGameData", order = 0)]```.
    - Replace ```"NewGameData"``` by your custom name
    - This action will allow you to create this kind of objects     from the Editor
3. Add the following line over the class name: ```[Serializable]``` 
    - This action will allow the system to save your data
4. Add as many fields as you need.

You should have a script similar to this:

```C#
using StArias.API.SaveLoadSystem;

[CreateAssetMenu(fileName = "NewGameData", menuName = "NewGameData", order = 0)]
public class NewGameData : GameData
{
    [Min(0)]
    public uint health = 0;

    [Min(0)]
    public uint mana = 0;

    public Vector3 position = new Vector3(0, 0, 0);
}
```

### Create custom GameData via Editor
In order to create the custom GameData on the Editor, please, go to **Assets > [Folder to create your object]**, right-click on **Create** and select your desired object:

<p align="center">
<img width=100% src="https://github.com/user-attachments/assets/ff2bdde6-492f-4bac-b0e0-491f78212cf6">
</p>

<p align="center">
<img width=100% src="https://github.com/user-attachments/assets/9bc30c5c-c812-4b1f-a8ac-7e6677ee8abd">
</p>

The created object contains all the relevant information you want to save in your game. 

### Create custom GameData via Script
In order to create data at runtime via Script, please, use the following [Unity API](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/ScriptableObject.CreateInstance.html):

```C#
ScriptableObject.CreateInstance<NewGameData>();
```

Example:

```C#
public NewGameData CreateData(string ID, int health, int mana, Vector3 position){
    NewGameData data = ScriptableObject.CreateInstance<NewGameData>();
    data.id = ID;
    data.health = health;
    data.mana = mana;
    data.position = position;

    return data;
}
```

## 3. Save & Load Data
In order to save and load data, please refer to the following API:

- [public GameData SaveNewData(GameData dataToSave, bool overwrite = false)](./unity-save-system-documentation.md#save_data)

- [public void InitGameDataCollection()](./unity-save-system-documentation.md#init_collection)

- [public GameData GetGameDataByID(string gameDataID)](./unity-save-system-documentation.md#get_data)

- [public Dictionary<string, GameData> GetGameDataCollection()](./unity-save-system-documentation.md#get_data_collection)


Code Example:

```C#
[SerializeField]
[Tooltip("Reference to the Demo Game Data")]
private GameData _newGameData;

// 1. Initialize the Instance and the GameData Collection
private void Awake()
{
    _saveLoadManager = SaveLoadManager.GetInstance();
    _saveLoadManager.InitGameDataCollection();
}

// 2. Wrapper API into a function to be used with buttons or other objects 
public void SaveDataButton()
{
    if (_saveLoadManager == null)
    {
        DebugLogger.Log("SaveLoadManager is null", DebugColor.Red);
        return;
    }

    if (_newGameData == null)
    {
        DebugLogger.Log("New Game Data is null", DebugColor.Red);
        return;
    }

    GameData gameData = _saveLoadManager.SaveNewData(_newGameData);
}

```

## Unity Save System Demo
The demo is intended to demonstrate a simple use case in which the data is listed and displayed on the Scene. The data is being created by using both ways, via Editor, and Script at runtime. 

Please, refer to: 

- StArias Projects > SaveSystem > Demo
    - Scripts > DemoManager.cs
    - GameData > DemoGameData.cs
    - Scenes > SaveSystemDemo.cs

- [Demo Video](https://github.com/StArias-Projects/unity-save-system/raw/refs/heads/development/documentation/demo-video.mp4)
