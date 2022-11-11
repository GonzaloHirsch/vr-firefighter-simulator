#define SAMPLE_OUTSIDE_EDITOR

// Unity Framework
using UnityEngine;

namespace Framework
{
	public class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviorSingleton<T>
	{
		private static MonoBehaviorSingleton<T> _instance;

		/// <summary>
		/// Used to select the behavior of the singleton when it's destroyed, if it's "true" then
		/// the instance value will be set to "null" when the MonoBehavior is Destroyed, otherwise 
		/// it will keep the last instance value.
		/// If it's false, then Instance will try to find a new non destroyed instance of the singleton in the scene, if
		/// it can't find one, it will return the last value. Can be useful for components that need to access the singleton
		/// after it has been destroyed to remove references / set some values / etc..
		/// </summary>
		protected static bool setToNullAfterDestroy = true;

		/// <summary>
		/// Used to detect if the singleton is destroyed or not. We can't rely on the value of "instance" because
		/// it's value will be keep if "setToNullAfterDestroy" is false.
		/// </summary>
		private static bool _destroyed = true;

		/// <summary>
		/// Used to detect if the singleton was found at least once, and if it wasn't then show 
		/// an error message informing this when trying to access the Instance
		/// </summary>
		private static bool _initializedAtLeastOnce = false;

		/// <summary>
		/// Used to detect if the singleton's Initialize() function needs to be called
		/// </summary>
		private static bool _needInitialization = true;

		protected bool Destroyed
		{
			get { return _destroyed; }
		}

		public void Awake()
		{
			if (_instance == null || _destroyed)
			{
				_instance = this;
				_destroyed = false;
			}
			else if (_instance != this)
			{
				Debug.LogError("Two instances of the same singleton '" + this + "'");
			}

			if (_needInitialization)
			{
				_needInitialization = false;
				_initializedAtLeastOnce = true;

#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
				UnityEngine.Profiling.Profiler.BeginSample(_instance.name + ".Initialize");
#endif

				Initialize();

#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
				UnityEngine.Profiling.Profiler.EndSample();
#endif
			}
		}

		public void OnDestroy()
		{
#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
			UnityEngine.Profiling.Profiler.BeginSample(name + ".Destroy");
#endif

			Destroy();

#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
			UnityEngine.Profiling.Profiler.EndSample();
#endif
			_destroyed = true;
			_needInitialization = true;

			if (setToNullAfterDestroy)
				_instance = null;
		}

		public static T Instance
		{
			get
			{
				if (_instance == null || _destroyed || _needInitialization)
				{
					if (_instance == null || _destroyed)
					{
						MonoBehaviorSingleton<T> newInstance = MonoBehaviour.FindObjectOfType(typeof(MonoBehaviorSingleton<T>)) as MonoBehaviorSingleton<T>;

						if (newInstance != null)
						{
							_instance = newInstance;
							_destroyed = false;
						}
					}

					if (_instance != null && !_destroyed)
					{
						if (_needInitialization)
						{
							_needInitialization = false;
							_initializedAtLeastOnce = true;

#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
							UnityEngine.Profiling.Profiler.BeginSample(_instance.name + ".Initialize");
#endif
							_instance.Initialize();
#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
							UnityEngine.Profiling.Profiler.EndSample();
#endif
						}
					}
					else
					{
						if (!_initializedAtLeastOnce)
							Debug.LogError("Missing Singleton '" + typeof(T).Name + "'");
					}
				}

				return (T) _instance;
			}
		}

		public static void Dispose()
		{
			if (_instance != null && !_destroyed)
			{
				Destroy(_instance.gameObject);

				_instance = null;
			}
		}

		protected virtual void Initialize()
		{
		}

		protected virtual void Destroy()
		{
		}

		static public bool IsReady()
		{
			return _instance != null && !_destroyed;
		}

		static public bool IsAvailable()
		{
			return IsReady() || MonoBehaviour.FindObjectOfType(typeof(MonoBehaviorSingleton<T>)) != null;
		}
	}
}