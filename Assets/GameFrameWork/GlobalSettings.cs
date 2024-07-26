using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个全局设置管理的类型
/// 你可以根据键和值设置并读取一个设置的值
/// 默认的设置值为int(float的字符化不稳定), 但你也可以读取其他类型(这将不受保护, 你需要确定你正在做什么)
/// 你可以通过json文件设置设置的默认值
/// </summary>
namespace GameFramework
{
    public static class GlobalSettings
    {
        //public

        public static bool initialized { get { return _initialized; } }

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
                throw new KeyNotFoundException("You are trying to get an unexisting setting:" + name);
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
                throw new KeyNotFoundException("You are trying to get an unexisting setting:" + name);
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
                Debug.LogWarning("You are adding a new setting key, you should use SetNewData() if you really want to.");
            }
            _settings[name] = data;
        }

        public static void SetData<T>(string name, T data)
        {
            if (!initialized)
            {
                Initialize();
            }
            if (!_settings.ContainsKey(name))
            {
                Debug.LogWarning("You are adding a new setting key, you should use SetNewData() if you really want to.");
            }
            _settings[name] = data;
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

        private static Dictionary<string, object> _settings;

        private static void Initialize()
        {
            //读取默认设置



            Debug.Log("GlobalSettings initialized.");
            _initialized = true;
        }
    }

}