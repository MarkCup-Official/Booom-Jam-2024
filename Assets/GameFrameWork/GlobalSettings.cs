using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// һ��ȫ�����ù��������
/// ����Ը��ݼ���ֵ���ò���ȡһ�����õ�ֵ
/// Ĭ�ϵ�����ֵΪint(float���ַ������ȶ�), ����Ҳ���Զ�ȡ��������(�⽫���ܱ���, ����Ҫȷ����������ʲô)
/// �����ͨ��json�ļ��������õ�Ĭ��ֵ
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
            //��ȡĬ������



            Debug.Log("GlobalSettings initialized.");
            _initialized = true;
        }
    }

}