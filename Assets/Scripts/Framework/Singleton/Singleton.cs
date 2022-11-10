using UnityEngine;
using System.Collections;

namespace Framework
{
	public class Singleton<T> where T : Singleton<T>, new()
	{
		private static object handler = new object();

		private static T _instance;

		public static T Instance
		{
			get
			{
				lock (handler)
				{
					if (_instance == null)
					{
						_instance = new T();
						_instance.Initialize();
					}

					return _instance;
				}
			}
		}

		public static void Release()
		{
			lock (handler)
			{
				if (_instance != null)
					_instance.OnRelease();
				_instance = null;
			}
		}

		protected virtual void Initialize()
		{

		}

		protected virtual void OnRelease()
		{

		}
	}
}
