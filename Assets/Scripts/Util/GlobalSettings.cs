using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 一个全局设置管理的类型
/// 你可以根据键和值设置并读取一个设置的值
/// 默认的设置值为int(float的字符化不稳定), 但你也可以读取其他类型(这将不受保护, 你需要确定你正在做什么)
/// 你可以通过json文件设置设置的默认值
/// 
/// 通过GetData获取值
/// 通过SetData设置已存在设置的值
/// 通过SetNewData添加新设置并赋值
/// </summary>
namespace GameFramework
{
    public static class GlobalSettings
    {
        //public

        public static bool initialized { get { return _initialized; } }

        public static void Initialize()
        {
            if (!initialized)
            {
                _Initialize();
            }
        }

        public static int GetData(string name)
        {
            if (!initialized)
            {
                Initialize();
            }
            if (_settings.ContainsKey(name))
            {
                return (int)_settings[name];
            }
            else
            {
                throw new KeyNotFoundException("You are trying to get an unexisting setting: " + name);
            }
        }

        public static T GetData<T>(string name)
        {
            if (!initialized)
            {
                Initialize();
            }
            if (_settings.ContainsKey(name))
            {
                return (T)_settings[name];
            }
            else
            {
                throw new KeyNotFoundException("You are trying to get an unexisting setting: " + name);
            }
        }
        public static void SetData(string name, int data)
        {
            if (!initialized)
            {
                Initialize();
            }
            if (!_settings.ContainsKey(name))
            {
                Debug.LogError("The key is not existing, you should use SetNewData() if you want to add a new setting.");
            }
            else
            {
                _settings[name] = data;
            }
        }

        public static void SetData<T>(string name, T data)
        {
            if (!initialized)
            {
                Initialize();
            }
            if (!_settings.ContainsKey(name))
            {
                Debug.LogError("The key is not existing, you should use SetNewData() if you want to add a new setting.");
            }
            else
            {
                _settings[name] = data;
            }
        }
        public static void SetNewData(string name, int data)
        {
            if (!initialized)
            {
                Initialize();
            }
            if (_settings.ContainsKey(name))
            {
                Debug.LogError("The key is already existing, you should use SetData() if you want to change it.");
            }
            else
            {
                _settings[name] = data;
            }
        }

        public static void SetNewData<T>(string name, T data)
        {
            if (!initialized)
            {
                Initialize();
            }
            if (_settings.ContainsKey(name))
            {
                Debug.LogError("The key is already existing, you should use SetData() if you want to change it.");
            }
            else
            {
                _settings[name] = data;
            }
        }

        //private

        private const string _defaultSettingsLocation = "";

        private static bool _initialized = false;

        private static Dictionary<string, object> _settings=new Dictionary<string, object>();

        private static void _Initialize()
        {
            //读取默认设置
            TextAsset settingFile = Resources.Load<TextAsset>("DefaultSettings");
            if (settingFile != null)
            {
                Debug.Log("Success loaded DefaultSettings.txt.");
                Dictionary<string, int> data= ParseTextToDictionary(settingFile.text);

                foreach (string key in data.Keys)
                {
                    _settings[key] = data[key];
                    Debug.Log(key+": " + data[key]);
                }
            }
            else
            {
                Debug.LogError("Missing DefaultSettings.txt!");
            }

            Debug.Log("GlobalSettings initialized.");
            _initialized = true;
        }

        private static Dictionary<string, int> ParseTextToDictionary(string text)
        {
            // 创建字典
            Dictionary<string, int> result = new Dictionary<string, int>();

            // 按行分割文本
            string[] lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                // 按冒号分割每一行
                line.Replace(" ", "");
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    if (int.TryParse(parts[1].Trim(), out int value))
                    {
                        // 添加到字典
                        result[key] = value;
                    }
                }
            }
            return result;
        }
    }


}