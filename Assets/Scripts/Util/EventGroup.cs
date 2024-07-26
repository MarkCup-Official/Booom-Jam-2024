using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventGroup
{
         private readonly Dictionary<System.Type, List<Action>> _cachedListener = new Dictionary<System.Type, List<Action>>();

		/// <summary>
		/// 添加一个监听
		/// </summary>
		public void AddListener<TEvent>(System.Action listener) where TEvent : IEventMessage
		{
			System.Type eventType = typeof(TEvent);
			if (_cachedListener.ContainsKey(eventType) == false)
				_cachedListener.Add(eventType, new List<Action>());

			if (_cachedListener[eventType].Contains(listener) == false)
			{
				_cachedListener[eventType].Add(listener);
				EventMgr.AddListener<TEvent>(listener);
			}
			else
			{
				Debug.LogError($"Event listener is exist : {eventType}");
			}
		}

		/// <summary>
		/// 移除所有缓存的监听
		/// </summary>
		public void RemoveAllListener()
		{
			foreach (var pair in _cachedListener)
			{
				System.Type eventType = pair.Key;
				for (int i = 0; i < pair.Value.Count; i++)
				{
					EventMgr.RemoveListener(eventType, pair.Value[i]);
				}
				pair.Value.Clear();
			}
			_cachedListener.Clear();
		}
	
}
