# Unity Save System SDK

Save System  is a personal and free tool-project to save the data of video games on different platforms.

The Save System allows you to save the progress of a video game on the disk of the device using the
[Unity Persistence Data Path](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Application-persistentDataPath.html).

The tool is intended to be open-source to let you modify and custom the scripts as you need for your own project.

<table>
    <tbody style="text-align:center;">
        <tr>
            <td valign="top" style="text-align:left;">
                <p style="max-width:100%;"><b>Overview</b></p>
            </td>
            <td style="text-align:left;">
                <ul>
                    <li>JSON Support</li>
                    <li>Cross Platform</li>
                    <li>Useful for different Video Games</li>
                    <li>Data Serialization</li>
                    <li>File Management</li>
                    <li>Simple and easy to use</li>
                    <li>Custom Debug Logger with different colours</li>
                </ul>
            </td>
            <tr>
                <td valign="top" style="text-align:left;">
                    <p style="max-width:100%;"><b>Unity version</b></p>
                </td>
                <td style="text-align:left;">
                    <ul>
                        <li>UNITY 2020.3.X or HIGHER </li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td valign="top" style="text-align:left;">
                <p style="max-width:100%;"><b>Supported Platforms</b></p>
                </td>
                <td style="text-align:left;">
                    <ul>
                        <li>Android</li>
                        <li>iOS</li>
                        <li>Windows + UWP</li>
                        <li>macOS</li>
                        <li>Editor</li>
                    </ul>
                </td>
            </tr>
        </tr>
    </tbody>
</table>

## Save System API


<table>
    <tbody style="text-align:center;">
            <td valign="top" style="text-align:left;">
                <p style="max-width:100%;"><b>Refer to the following classes</b></p>
            </td>
            <td style = "text-align: left;">
                <ul>
                    <li>SaveLoadManager.cs</li>
                    <li>SaveLoadHelper.cs</li>
                    <li>DebugLogger.cs</li>
                    <li>GameData.cs</li>
                </ul>
            </td>
    </tbody>
</table>

### **SaveLoadManager.cs**:
The main script that controls the save and load functionalities. It includes private and public classes than can be modified as you need.

#### Private API & Fields
- **private readonly string _savePath**:  Contains the information of the path where the game data will be stored.

- **private readonly string _fileExtension**: Determines the file extension of the game data once it is stored in the disk.

- **public static SaveLoadManager GetInstance()**: Returns the singletong

- **private Dictionary<string, GameData> _gameDataCollection**: Collection of the saved game data. Each element is accessed by the ID of the game data. In order to read the saved game data from the disk and store it into this variable, please use [**InitGameDataCollection()**](#init_collection)

#### Public API & Fields
- <a name = "save_data"> **public GameData SaveNewData(GameData dataToSave, bool overwrite = false)** </a>: Adds a new game data to the collection. If a game data with the same ID already exists, a new one will be created with a different ID. Finally, it returns a copy of the final GameData that was stored. The collection of the game data is accessible via [**GetGameDataCollection()**](#get_collection).

    <table>
        <tr>
            <th>Parameter</th>
            <th>Description</th>
        </tr>
        <tr>
            <td>GameData dataToSave</td>
            <td>The game data to be saved</td>
        </tr>
        <tr>
            <td>bool overwrite</td>
            <td>If true, the data will be overwritten if it already exists. Default value is false.</td>
        </tr>
    </table>

- <a name="init_collection">**public void InitGameDataCollection()**</a>: Initialize the game data collection by reading the saved data from the disk. The collection of the game data is accessible via [**GetGameDataCollection()**](#get_collection)

- <a name = "delete_data"> **public void DeleteDataByID(string gameDataID)**</a>: Removes the game data from the disk and from the collection if it exists.

    <table>
        <tr>
            <th>Parameter</th>
            <th>Description</th>
        </tr>
        <tr>
            <td>string gameDataID</td>
            <td>The ID of the game data to be delete</td>
        </tr>
    </table>

- <a name = "get_data"> **public GameData GetGameDataByID(string gameDataID)**</a>: Gets a GameData by its ID.

    <table>
        <tr>
            <th>Parameter</th>
            <th>Description</th>
        </tr>
        <tr>
            <td>string gameDataID</td>
            <td>The ID of the GameData</td>
        </tr>
    </table>

- <a name = "get_data"> **public Dictionary<string, GameData> GetGameDataCollection()**</a>: Gets a copy of the collection of the current saved data.

### **SaveLoadHelper.cs**:
Class dedicated of keeping useful functionalities for the usage of the SDK. 

#### Public API & Fields
- <a name = "get_fields"> **public static FieldInfo[] GetGameDataFields<T>() where T : GameData</a>**: Collects the public fields of a certain type class that inherits GameData class. Finally, it returns an object of type [**FieldInfo[]**](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PropertyDrawer-fieldInfo.html) containing the different fields from the class. 

    <table>
        <tr>
            <th>Parameter</th>
            <th>Description</th>
        </tr>
        <tr>
            <td>T</td>
            <td>They type of the object</td>
        </tr>
    </table>

- <a name = "cast_game_data">**public static T CastFromGameData<T>(GameData dataToCast) where T**: GameData</a>: Allows a cast from GameData to any type that inherits it. 

    <table>
        <tr>
            <th>Parameter</th>
            <th>Description</th>
        </tr>
        <tr>
            <td>T</td>
            <td>They type to be cast</td>
        </tr>
    </table>

### DebugLogger.cs
Utility to display log messages with different colors and custom prefix.

#### Public API & Fields

- **public enum DebugColor**: Debug colors for the log messages
- **public static bool EnableDebugLog**: Determines if the logs will be displayed or not.  

- <a name = "debug_log">**public static void Log(string message, DebugColor color = DebugColor.UnityDefault, string prefix = "SaveLoadManager - ")**</a>: Logs a message with a specific color. All the messages are prefixed with "SaveLoadManager - " by default.

    <table>
        <tr>
            <th>Parameter</th>
            <th>Description</th>
        </tr>
        <tr>
            <td>string message</td>
            <td>The message to be displayed</td>
        </tr>
        <tr>
            <td>DebugColor color</td>
            <td>The color of the message to be displayed</td>
        </tr>
        <tr>
            <td>string prefix</td>
            <td>The string to be displayed at the start of the message</td>
        </tr>
    </table>

### <a name = "game_data"></a>GameData.cs
Base class for the game's system data. It inherits from [ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html), allowing objects of this type to be created in the Editor for easy customization.

A new game data class must inherit from this class in order to save custom data.

#### Protected Fields
- **protected string dataType**:  The type of GameData. It detects the inherited type and is determined automatically within the constructor.

#### Public API & Fields
- **public string id = "GameData"**: The ID of the GameData. 

- **public string GetDataType()**: Returns the type name of the GameData.