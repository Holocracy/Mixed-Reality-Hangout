using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Practical.Analytics;

namespace Practical
{
	public class PracticalConfig : MonoBehaviour
	{
		public string APIKey;
		private PracticalDLL _practicalDLL;

		// Use this for initialization
		void Start()
		{
			_practicalDLL = GetComponent<PracticalDLL>();
			_practicalDLL.SetAPIKey(APIKey);
		}

		void Awake()
		{
			DontDestroyOnLoad(transform.gameObject);
		}
	}
}