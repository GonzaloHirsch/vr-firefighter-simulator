// .NET framework
using System.Collections.Generic;

// Unity Framework
using UnityEngine;

namespace Framework
{

	/// <summary>
	/// Game Event dispatcher
	/// </summary>
	public class GameEventDispatcher : Singleton<GameEventDispatcher>
	{
		// Event dictionary
		Dictionary<GameEvent.Event, List<OnGameEvent>> eventCallbacks = new Dictionary<GameEvent.Event, List<OnGameEvent>>();

		// Prevents list modification when it is dispatching evengts to the listeners
		bool dispatching = false;

		// Purge list
		List<KeyValuePair<GameEvent.Event, OnGameEvent>> purge = new List<KeyValuePair<GameEvent.Event, OnGameEvent>>();

		/// <summary>
		/// Clears the scene listeners. This method is usually called from a Scene manager (some object
		/// that wraps the Application.LoadLevel functionality).
		/// </summary>
		public void ClearSceneListeners()
		{
			// Clear the list of events on each Awake
			eventCallbacks.Clear();
		}

		/// <summary>
		/// Add a listener to the list
		/// </summary>
		public void AddListener(GameEvent.Event evtName, OnGameEvent callback)
		{
			// Add the event if it is not part of the list
			if (!eventCallbacks.ContainsKey(evtName))
			{
				eventCallbacks.Add(evtName, new List<OnGameEvent>());
			}

			// Add the callback itself
			if (!eventCallbacks[evtName].Contains(callback))
			{
				eventCallbacks[evtName].Add(callback);
			}
		}

		/// <summary>
		/// Remove a listener from the list
		/// </summary>
		public void RemoveListener(GameEvent.Event evtName, OnGameEvent callback)
		{
			if (!dispatching)
			{
				if (eventCallbacks.ContainsKey(evtName))
				{
					eventCallbacks[evtName].Remove(callback);
				}
			}
			else
			{
				purge.Add(new KeyValuePair<GameEvent.Event, OnGameEvent>(evtName, callback));
			}
		}

		/// <summary>
		/// Dispatch a event
		/// </summary>
		public void Dispatch(System.Object sender, GameEvent evt)
		{
			dispatching = true;

			if (eventCallbacks.ContainsKey(evt.EventName))
			{
				for (int i = 0; i < eventCallbacks[evt.EventName].Count; i++)
				{
					eventCallbacks[evt.EventName][i](sender, evt);
				}
			}

			if (purge.Count > 0)
			{
				foreach (KeyValuePair<GameEvent.Event, OnGameEvent> pair in purge)
				{
					if (eventCallbacks.ContainsKey(pair.Key))
						eventCallbacks[pair.Key].Remove(pair.Value);
				}

				purge.Clear();
			}
			dispatching = false;
		}
	}
}