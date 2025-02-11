/*  
 * This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.  
 * If a copy of the MPL was not distributed with this file, you can obtain one at  
 * https://mozilla.org/MPL/2.0/.  
 *  
 * Copyright (c) 2025 StArias - https://github.com/starias  
 */

using UnityEngine;

namespace StArias.API.SaveLoadSystem
{
    public enum DebugColor
    {
        Red = 0xFF0000,
        Green = 0x00FF00,
        Blue = 0x0000FF,
        Black = 0x000000,
        White = 0xFFFFFF,
        Yellow = 0xFFFF00,
        UnityDefault = 0xB8B8B8,
    }

    public static class DebugLogger
    {
        public static string SelectLogColor(DebugColor color) 
        {
            string selectedColor = "<color=#B8B8B8>";

            switch (color)
            {
               case DebugColor.Red:
                    selectedColor = "<color=#FF0000>";
                    break;
                case DebugColor.Green:
                    selectedColor = "<color=#00FF00>";
                    break;
                case DebugColor.Blue:
                    selectedColor = "<color=#0000FF>";
                    break;
                case DebugColor.Black:
                    selectedColor = "<color=#000000>";
                    break;
                case DebugColor.White:
                    selectedColor = "<color=#FFFFFF>";
                    break;
                case DebugColor.Yellow:
                    selectedColor = "<color=#FFFF00>";
                    break;
                default:
                    break;
            }

            return selectedColor;
        } 

        public static void Log(string message, DebugColor color = DebugColor.UnityDefault)
        {
            string debugColor = SelectLogColor(color);
            Debug.Log($"{debugColor}{message}</color>");
        }
    }
}