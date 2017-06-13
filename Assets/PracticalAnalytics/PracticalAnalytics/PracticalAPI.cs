using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Runtime.InteropServices;
using HoloToolkit.Unity.InputModule;
using Practical;

namespace Practical.Analytics
{

	public class PracticalAPI : PracticalSingleton<PracticalAPI>
	{
		/**
		* @api {C# - Class} PracticalAPI Overview
		* @apiGroup Basics
		* @apiDescription PracticalAPI contains methods that enable you to track/record events in your application. A helper script is available for gaze input.
		* @apiParam (Methods) {Gaze} RecordGazeOn Tracks what objects are being gazed on.
		* @apiParam (Methods) {Gaze} RecordGazeOff Ends object tracking and reports seconds gazed.
		* @apiParam (Methods) {Gesture} RecordGesture Records when gestures are being used.
		* @apiParam (Methods) {Gesture} RecordHoldStarted Records when hold gesture starts.
		* @apiParam (Methods) {Gesture} RecordHoldCompleted Records when hold gesture completes and reports seconds held.
		* @apiParam (Methods) {Mapping} IncludeMapping Adds gaze tracking to mapped objects.
		* @apiParam (Methods) {Keyword} RecordKeyword Records what keywords are being said.
		* @apiParam (Methods) {Gain} RecordGain Records when user experiences a gain in application.
		* @apiParam (Methods) {Loss} RecordLoss Records when user experiences a loss in application.
		* @apiParam (Methods) {CustomStat} RecordCustomStat Records a customized metric/stat.
		* @apiParam (Methods) {GetErrorMessage} GetErrorMessage Returns specific error message.
		* @apiParam (Methods) {GetLastErrorMessage} GetLastErrorMessage Returns last error message.
		* 
		* 
		* @apiParam (Enums) {Formula} Formula Identifies if user is requesting count or average data analytics.
		* @apiParam (Enums) {Measurment} Measurment Identifies how measurment will be taken.
		* @apiParam (Enums) {GestureType} GestureType Identifies what type of gesture is being used.
		* 
		* @apiParam (Helpers) {Script} PracticalGazeTracker Provides GameObject gaze tracking with an optional custom identifier that allows an additional name for GameObjects and capabilities to group GameObject gaze stats together based on a shared custom identifier.
		* 
		* 
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public bool logSummary;
		private GazeManager gazeManager;
		private Stopwatch gazeStopWatch;
		private Stopwatch gestureStopWatch;
		private string viewedObject;

		/**
		* @api {C# - Enum} Measurement Measurement
		* @apiName Measurement
		* @apiDescription Measurement identifies how measurment will be taken.
		* 
		* @apiParam (DataField) {int} Count Measures by adding 1 (default).
		* @apiParam (DataField) {int} Second Measures in seconds.
		* @apiParam (DataField) {int} Feet Measures in feet.
		* @apiParam (DataField) {int} Meter Measures in meters.
		* 
		* @apiGroup Enums
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public enum Measurement
		{
			Count,
			Second,
			Feet,
			Meter
		};

		/**
		* @api {C# - Enum} Formula Formula
		* @apiName Formula
		* @apiDescription Formula identifies if user is requesting total or average data analytics.
		* 
		* @apiParam (DataField) {int} Total Adds 1 (default).
		* @apiParam (DataField) {int} Average Finds Average.
		* 
		* @apiGroup Enums
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public enum Formula
		{
			Total,
			Average
		};

		/**
		* @api {C# - Enum} GestureType GestureType
		* @apiName GestureType
		* @apiDescription GestureType identifies if user is requesting total or average data analytics.
		* 
		* @apiParam (DataField) {int} Tap Identifies as a tap gesture.
		* @apiParam (DataField) {int} Hold Identifies as a hold gesture.
		* 
		* @apiGroup Enums
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public enum GestureType
		{
			Tap,
			DoubleTap,
			Hold
		};


		void Start()
		{
			gazeStopWatch = new Stopwatch();
			gestureStopWatch = new Stopwatch();
			gazeManager = GazeManager.Instance;
		}

		/**
		* @api {C# - Method} GetErrorMessage() GetErrorMessage()
		* @apiName GetErrorMessage()
		* @apiDescription GetErrorMessage() returns specific error message from the DLL.
		* 
		* @apiParam (Parameters) {int} errorCode References specific error code.
		* @apiParamExample {json} Description:
		* Call this method with the provided error code to retrieve error message.
		* 
		* Error Code:
		* 1) "Authentication did not suceed yet."
		* 2) "No Internet Connection."
		* 3) "API key is invalid."
		* 4) "Message Encoding is invalid."
		* 5) "Must call startup function first."
		* 6) "Hold length parameter must contain float value."
		* 7) "Data buffer is empty."
		* 8) "Value parameter must contain float value."
		* 9) "Measurment parameter must have the enum Measurment Type."
		* 10) "Formula parameter must have the enum Formula Type."
		* 11) "GestureType parameter must have the enum GestureType Type."
		* 
		* @apiParamExample {json} Example:
		* "// Retrieves error message from the DLL: API key is invalid."
		* GetErrorMessage(3);
		* 
		* @apiParamExample {json} Structure:
		* GetErrorMessage(int errorCode);
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

#if WINDOWS_UWP
	public string GetErrorMessage(int errorCode)
	{
		return Marshal.PtrToStringAnsi(PracticalDLL.GetErrorMessage(errorCode));
	}
#endif

		/**
		* @api {C# - Method} GetLastErrorMessage() GetLastErrorMessage()
		* @apiName GetLastErrorMessage()
		* @apiDescription GetLastErrorMessage() returns last error message from the DLL.
		* 
		* @apiParamExample {json} Description:
		* Returns last error message from DLL.
		* 
		* See GetErrorMessage() for specific error codes.
		* @apiParamExample {json} Example:
		* "// Returns last error message from DLL."
		* GetLastErrorMessage();
		* 
		* @apiParamExample {json} Structure:
		* GetLastErrorMessage();
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

#if WINDOWS_UWP
	public string GetLastErrorMessage()
	{
		return Marshal.PtrToStringAnsi(PracticalDLL.GetLastErrorMessage());
	}
#endif


		/**
		* @api {C# - Method} RecordCustomStat() RecordCustomStat()
		* @apiName RecordCustomStat()
		* @apiDescription RecordCustomStat() records a custom metric/stat. Custom stats can be used in variety of ways.
		* 
		* 
		* @apiParam (Parameters) {string} uniqueIdentifier References identification of customized metric/stat.
		* @apiParam (Parameters) {float} value References value of customized metric/stat.
		* @apiParam (Parameters) {Measurment} measurement References measurement of customized metric/stat. Count, Second, Feet, or Meter. (optional)
		* @apiParam (Parameters) {Formula} formula References formula of customized metric/stat. Total or Average. (optional)
		* @apiParam (Parameters) {string} gazedObject References the GameObject being gazed while stat is being recorded. (AutoGenerated)
		* @apiParamExample {json} Description:
		* This method provides the utility to:
		* - Create custom defined stats.
		* - Control how the stats will be measured.
		* - Calculate the total or average of stats collected.
		* 
		* Measurment and Formula are optional parameters.
		* 
		* If they are not provided:
		* - Measurement defaults to count.
		* - Formula defaults to total.
		* 
		* @apiParamExample {json} Examples:
		* "// This example shows how to record a player completing a wave."
		* PracticalAPI.Instance.RecordCustomStat("WavesCompleted", 1);
		* 
		* "// This example shows how to record a user success streak."
		* PracticalAPI.Instance.RecordCustomStat("Combo", 1, Measurement.Count, Formula.Total);
		* 
		* "// This example shows how to record the average of seconds remaining of a timed event."
		* PracticalAPI.Instance.RecordCustomStat("SecondsRemaining", 32, Measurement.Count, Formula.Average);
		* @apiParamExample {json} Structure:
		* PracticalAPI.Instance.RecordCustomStat(string uniqueIdentifier, float value);
		* PracticalAPI.Instance.RecordCustomStat(string uniqueIdentifier, float value, Measurement measurement, Formula formula);
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public void RecordCustomStat(string uniqueIdentifier, float value, Measurement measurement = Measurement.Count,
			Formula formula = Formula.Total, string gazedObject = "")
		{
			if (logSummary)
			{
				Debug.Log("Custom stat recorded: " + uniqueIdentifier + "\n");
				Debug.Log("Value: " + value + "\n");

				if (gazeManager.HitObject != null)
				{
					var objName = gazeManager.HitObject.gameObject.name;
					Debug.Log("Object name: " + objName + "\n");
				}

				Debug.Log("Measurement: " + measurement);
				Debug.Log("Formula: " + formula);
			}

#if WINDOWS_UWP
		if (gazeManager.HitObject != null)
		{
			var objName = gazeManager.HitObject.gameObject.name;

			int ret = PracticalDLL.RecordCustomStat(uniqueIdentifier, value.ToString(), ((int)measurement).ToString(), ((int)formula).ToString(), objName);

			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}
		}
		else
		{
			int ret = PracticalDLL.RecordCustomStat(uniqueIdentifier, value.ToString(), ((int)measurement).ToString(), ((int)formula).ToString(), gazedObject);

			if (ret > 0)
			{
				Debug.Log(GetErrorMessage(ret));
			}
		}
#endif
		}


		/**
		* @api {C# - Method} RecordGain() RecordGain()
		* @apiName RecordGain()
		* @apiDescription RecordGain() records when user experiences a gain behavior.
		* 
		* @apiParam (Parameters) {string} uniqueIdentifier Reference identification of stat.
		* @apiParam (Parameters) {float} value References the stat value.
		* @apiParamExample {json} Description:
		* RecordGain can be used to track when a user gains:
		* - Health
		* - Points
		* - Lives
		* - Currency
		* 
		* RecordGain can also be used to track achievments:
		* - WavesComplete
		* - GoldTierAchieved
		* - EnemiesEliminated
		* - KeyItemsAquired
		* 
		* @apiParamExample {json} Example:
		* "// This example shows how to record a player gaining health."
		* PracticalAPI.Instance.RecordGain("HealthGained", 25);
		* 
		* "// This example shows how to record a player completing a wave."
		* PracticalAPI.Instance.RecordGain("WaveComplete", 1);
		* 
		* "// This example shows how to record a player obtaining lives."
		* PracticalAPI.Instance.RecordGain("LivesGained", 5);
		* @apiParamExample {json} Structure:
		* PracticalAPI.Instance.RecordGain(string uniqueIdentifier, float value);
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public void RecordGain(string uniqueIdentifier, float value)
		{
			if (logSummary)
			{
				Debug.Log("Gain recorded \n");
				Debug.Log("Gain Unique Identifier: " + uniqueIdentifier + "\n");
				Debug.Log("Value: " + value + "\n");
			}
#if WINDOWS_UWP
		int ret = PracticalDLL.RecordGain(uniqueIdentifier, value.ToString());
		
		if(ret > 0) {
			Debug.Log(GetErrorMessage(ret));
		}
#endif
		}

		/**
		* @api {C# - Method} RecordLoss() RecordLoss()
		* @apiName RecordLoss()
		* @apiDescription RecordLoss() records when user experiences a loss behavior.
		* 
		* @apiParam (Parameters) {string} uniqueIdentifier Reference identification of stat.
		* @apiParam (Parameters) {float} value Referencese the stat value.
		* 
		* @apiParamExample {json} Description:
		* RecordLoss can be used to track when a user loses:
		* - Health
		* - Points
		* - Lives
		* - Currency
		* 
		* RecordLoss can also be used to track when the user fails:
		* - MissionFailed
		* - TimeUp
		* - Wipeout
		* - GameOver
		* 
		* @apiParamExample {json} Example:
		* "// This example shows how to record a player losing health."
		* PracticalAPI.Instance.RecordLoss("HealthLoss", 25);
		* 
		* "// This example shows how to record a player failing a wave."
		* PracticalAPI.Instance.RecordLoss("WaiveFailed", 1);
		* 
		* "// This example shows how to record a player losing a life."
		* PracticalAPI.Instance.RecordLoss("LifeLost", 1);
		* @apiParamExample {json} Structure:
		* PracticalAPI.Instance.RecordLoss(string uniqueIdentifier, float value)
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public void RecordLoss(string uniqueIdentifier, float value)
		{
			if (logSummary)
			{
				Debug.Log("Loss recorded \n");
				Debug.Log("Loss Unique Identifier: " + uniqueIdentifier + "\n");
				Debug.Log("Value: " + value + "\n");
			}
#if WINDOWS_UWP
		int ret = PracticalDLL.RecordLoss(uniqueIdentifier, value.ToString());
		
		if(ret > 0) {
			Debug.Log(GetErrorMessage(ret));
		}
#endif
		}

		/**
		* @api {C# - Method} RecordKeyword() RecordKeyword()
		* @apiName RecordKeyword()
		* @apiDescription RecordKeyword() records what keywords are being used in the application.
		* 
		* @apiParam (Parameters) {string} keyword Records keyword use in application.
		* @apiParam (Parameters) {string} gazedObject References the GameObject being gazed while stat is being recorded. (AutoGenerated)
		* @apiParamExample {json} Description:
		* Records what keywords are being used in the application.
		* 
		* Can be used to track:
		* - Command keywords used in your application.
		* - Requests for help from user.
		* - Interaction keywords used with UI or other GameObjects.
		* @apiParamExample {json} Example:
		* "// This example shows how to record when a user speaks the keyword Help."
		* PracticalAPI.Instance.RecordKeyword(“Help”);
		* @apiParamExample {json} Structure:
		* PracticalAPI.Instance.RecordKeyword(string keyword);
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public void RecordKeyword(string keyword, string interactObject = "")
		{
			if (logSummary)
			{
				Debug.Log("Keyword recorded: " + keyword + "\n");

				if (gazeManager.HitObject != null)
				{
					var objName = gazeManager.HitObject.gameObject.name;
					Debug.Log("Target: " + objName + "\n");
				}
			}

			if (gazeManager.HitObject != null)
			{
				var objName = gazeManager.HitObject.gameObject.name;
#if WINDOWS_UWP
				int ret = PracticalDLL.RecordKeyword(keyword, "1.0", objName);
				
				if (ret > 0)
				{
					Debug.Log(GetErrorMessage(ret));
				}
			}
			else
			{
				int ret = PracticalDLL.RecordKeyword(keyword, "1.0", interactObject);

				if (ret > 0)
				{
					Debug.Log(GetErrorMessage(ret));
				}
#endif
			}
		}

		/**
		* @api {C# - Method} RecordGesture() RecordGesture()
		* @apiName RecordGesture()
		* @apiDescription RecordGesture() tracks gestures and the GameObjects being interacted with. GestureType defaults to Tap.
		* 
		* @apiParam (Parameters) {string} uniqueIdentifier Records name of object being gesture to. (Example: HitObject.gameObject.name)
		* @apiParam (Parameters) {GestureType} gestureType Identifies which type of gesture is being used.
		* @apiParam (Parameters) {string} gazedObject Reference the GameObject being gazed while stat is being recorded. (AutoGenerated)
		* @apiParamExample {json} Description:
		* Tracks gestures and the GameObjects being interacted with. 
		* 
		* Can be used for tracking:
		* - Firing projectile
		* - Basic selection
		* - User interaction
		* 
		* @apiParamExample {json} Example:
		* "// This example is from the HoloToolkit script GestureInputs.cs with our API implemented."
		* protected void OnTappedEvent(InteractionSourceKing source, int tapCount, Ray headRay)
		*{
		*   "// Get the current Gaze's hit object"
		*   var hitObj = GazeManager.Instance.HitObject;
		*		
		*   "// Send an OnSelect message to the focused object and it's ancestors."
		*   if (hitObj != null)
		*   {
		*      if (hitObj.layer == PracticalAPI.Instance.MappingPhysicsLayer)
		*      {
		*         PracticalAPI.Instance.RecordGesture("Mapping");
		*      }
		*      else
		*      {
		*         PracticalAPI.Instance.RecordGesture(hitObj.gameObject.name);
		*      }
		*   }
		*   else
		*   {
	    *      PracticalAPI.Instance.RecordGesture("No Hit");
	    *   }
		*}
		* @apiParamExample {json} Structure:
		* PracticalAPI.Instance.RecordGesture(string uniqueIdentifier);
		* 
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public void RecordGesture(string uniqueIdentifier, float HoldLength = 0.0f, GestureType gestureType = GestureType.Tap,
			string gazedObject = "")
		{
			if (logSummary)
			{
				Debug.Log("Tap event recorded \n");
				Debug.Log("GestureType: " + gestureType + "\n");
				if (gazeManager.HitObject != null)
				{
					var customCheck = gazeManager.HitObject.gameObject.GetComponent<PracticalGazeTracker>();

					if (customCheck != null && customCheck.customName)
					{
						Debug.Log("Object name: " + customCheck.customIdentifier + "\n");
					}
					else
					{
						Debug.Log("Object name: " + uniqueIdentifier + "\n");
					}
				}
				if (HoldLength > 0)
				{
					Debug.Log("Tap Hold Length : " + HoldLength + " seconds\n");
				}
			}

#if WINDOWS_UWP
			if (gazeManager.HitObject != null)
			{
				// Will pass objName from HitObject & HoldLength default values if not provided. 
				var customCheck = gazeManager.HitObject.gameObject.GetComponent<PracticalGazeTracker>();

				if (customCheck != null && customCheck.customName)
				{
					int res = PracticalDLL.RecordGesture(customCheck.customIdentifier, "1.0", ((int)gestureType).ToString(), customCheck.customIdentifier, HoldLength.ToString());

					if (res > 0)
					{
						Debug.Log(GetErrorMessage(res));
					}
				}
				else
				{
					var objName = gazeManager.HitObject.gameObject.name;
					int ret = PracticalDLL.RecordGesture(uniqueIdentifier, "1.0", ((int)gestureType).ToString(), objName, HoldLength.ToString());

					if (ret > 0)
					{
						Debug.Log(GetErrorMessage(ret));
					}
				}
			}
			else
			{
				// Will pass just the HoldLength value, and interacted object with default values if not provided. 
				int ret = PracticalDLL.RecordGesture(uniqueIdentifier, "1.0", ((int)gestureType).ToString(), gazedObject, HoldLength.ToString());

				if (ret > 0)
				{
					Debug.Log(GetErrorMessage(ret));
				}

			}
#endif
		}

		/**
		* @api {C# - Method} RecordHoldStarted() RecordHoldStarted()
		* @apiName RecordHoldStarted()
		* @apiDescription RecordHoldStarted() tracks when the Hold gesture is initialized and starts stopwatch.
		* 
		* @apiParamExample {json} Description:
		* Used to track when the Hold gesture is initialized and starts stopwatch.
		* 
		* Can be used to track:
		* - How many seconds is the user spending dragging objects.
		* - What GameObject did the user apply the Hold gesture on.
		* - How many Hold gestures total are being used.
		* 
		* A Stopwatch is used to track how many seconds the user is applying the Hold gesture.
		* @apiParamExample {json} Example:
		* "// This example is from the HoloToolkit script GestureInputs.cs with our API implemented."
		* protected void OnHoldStartedEvent(InteractionSourceKind source, Ray headray)
		* {
		*    PracticalAPI.Instance.RecordHoldStarted();
		*    
		*    inputManager.RaiseHoldStarted(this, 0);
		* }
		* @apiParamExample {json} Structure:
		* PracticalAPI.Instance.RecordHoldStarted();
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public void RecordHoldStarted()
		{
			gestureStopWatch.Start();
		}

		/**
		* @api {C# - Method} RecordHoldComplete() RecordHoldComplete()
		* @apiName RecordHoldComplete()
		* @apiDescription RecordHoldComplete() reports the Hold gesture usage and how many seconds the Hold gesture was applied.
		* @apiParam (Parameters) {string} uniqueIdentifier Records gesture event name.
		* 
		* @apiParamExample {json} Description:
		* Reports the Hold gesture usage and how many seconds the Hold gesture was applied.
		* 
		* Can be used to track:
		* - How many seconds is the user spending dragging objects.
		* - What GameObject did the user apply the Hold gesture on.
		* - How many Hold gestures total are being used.
		* 
		* @apiParamExample {json} Example:
		* "// This example is from the HoloToolkit script GestureInputs.cs with our API implemented."
		* protected void OnHoldCompletedEvent(InteractionSourceKind source, Ray headray)
		* {
		*    "// Identifies if there is a HitObject"
		*    var hitObj = GazeManager.Instance.HitObject;
		*    
		*    "// Send an OnSelect message to the focused object and it's ancestors."
		*    if (hitObj != null)
		*    {
		*       if (hitObj.layer == PracticalAPI.Instance.MappingPhysicsLayer)
		*       {
		*          PracticalAPI.Instance.RecordHoldComplete("Mapping");
		*       }
		*       else
		*       {
		*          PracticalAPI.Instance.RecordHoldComplete(hitObj.name);
		*       }
		*    }
		*    else
		*    {
		*       PracticalAPI.Instance.RecordHoldComplete("No Hit");
		*    }
		*    
		*    inputManager.RaiseHoldComplete(this, 0);
		* }
		* @apiParamExample {json} Structure:
		* PracticalAPI.Instance.RecordHoldComplete(string uniqueIdentifier);
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public void RecordHoldComplete(string uniqueIdentifier)
		{
			gestureStopWatch.Stop();

			double milliseconds = gazeStopWatch.Elapsed.TotalSeconds;

			RecordGesture(uniqueIdentifier, (float) milliseconds, GestureType.Hold);

			gazeStopWatch.Reset();
		}

		/**
		* @api {C# - Method} RecordGazeOn() RecordGazeOn()
		* @apiName RecordGazeOn()
		* @apiDescription RecordGazeOn() records the gazed GameObject name and seconds spent gazing at the GameObject.
		* 
		* @apiParam (Parameters) {string} gazedObject Tracks what objects are being gazed on.
		* @apiParamExample {json} Description:
		* Tracks:
		* - What GameObjects are being gazed on.
		* - Starts stopwatch to count the seconds user spends gazing at GameObject.
		* 
		* See "PracticalGazeTracker.cs" for additional gaze tracking implementations.
		* @apiParamExample {json} Example:
		* "// This example shows how to record when a player gazes on a GameObject."
		* public void OnFocusEnter()
		* {
		*    PracticalAPI.Instance.RecordGazeOn(HitObject.name);
		* }
		* @apiParamExample {json} Structure:
		* PracticalAPI.Instance.RecordGazeOn(string gazedObject);
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public void RecordGazeOn(string gazedObject)
		{
			viewedObject = gazedObject;
			gazeStopWatch.Start();
		}


		/**
		* @api {C# - Method} RecordGazeOff() RecordGazeOff()
		* @apiName RecordGazeOff()
		* @apiDescription RecordGazeOff() ends object tracking and reports seconds gazed.
		* 
		* @apiParamExample {json} Description:
		* Tracks: 
		* - When the GameObject is no longer being gazed at.
		* - How many seconds the user gazed at that GameObject.
		* 
		* See "PracticalGazeTracker.cs" for additional gaze tracking implementations.
		* @apiParamExample {json} Example:
		* "// This example shows how to record when a player gazes off a GameObject."
		* public void OnFocusExit()
		* {
		*    PracticalAPI.Instance.RecordGazeOff();
		* }
		* @apiParamExample {json} Structure:
		* PracticalAPI.Instance.RecordGazeOff();
		* @apiGroup PracticalAPI
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/

		public void RecordGazeOff()
		{
			gazeStopWatch.Stop();

			double milliseconds = gazeStopWatch.Elapsed.TotalSeconds;

			RecordGaze((float) milliseconds);

			gazeStopWatch.Reset();
		}

		private void RecordGaze(float gazeLength)
		{
			if (logSummary)
			{
				Debug.Log("Object Viewed: " + viewedObject + " for " + gazeLength + " seconds. \n");
			}
#if WINDOWS_UWP
		int ret = PracticalDLL.RecordGaze(viewedObject, gazeLength.ToString());

		if (ret > 0)
		{
			Debug.Log(GetErrorMessage(ret));
		}
#endif
		}

		/**
		 * @api {C# - Method} IncludeMapping() IncludeMapping()
		 * @apiName IncludeMapping()
		 * @apiDescription IncludeMapping() will add gaze tracking to mapped objects.
		 * 
		 * @apiParamExample {json} Description:
		 * This will group all the mapping objects together.
		 * 
		 * The portal will reflect a single object called "Mapping", instead of multiple mapping objects.
		 * 
		 *- For Spatial Understanding, it can be called after you finalize the mapping (recommended).
		 * 
		 *- For Spatial Mapping with a formal or timed experience, it can be called after the mapping is complete.
		 * 
		 *- For on-going Spatial Mapping, invoke on a delay that allows a significant portion of the room to be mapped.
		 * 
		 * 
		 * @apiParamExample {json} Example:
		 * "// This is from the HoloToolkit script SpatialUnderstanding.cs"
		 * 
		 * if((ScanState == ScanStates.Finishing) &&
		 *    (scanDone) &&
		 *    (!UnderstandingCustomMesh.IsImportActive) &&
		 *    (UnderstandingCustomMesh != null))
		 * {
		 *    "// Final mesh import"
		 *    StartCoroutine(UnderstnaindCustomMesh.Import_UnderstandingMesh());
		 *    
		 *    "// Mark it"
		 *    ScanState = ScanStates.Done;
		 *    
		 *    "// Group all the Mapping objects together to share one custom name 'Mapping'"
		 *    PracticalAPI.Instance.IncludeMapping();
		 * }
		 *   
		 * @apiParamExample {json} Structure:
		 * PracticalAPI.Instance.IncludeMapping();
		 * 
		 * @apiGroup PracticalAPI
		 * @apiPermission Beta
		 * @apiVersion 0.1.0
		 */

		[Tooltip("The physics layer for spatial mapping objects to be set to.")] public int MappingPhysicsLayer = 31;

		public void IncludeMapping()
		{
			var goArray = FindObjectsOfType<GameObject>();

			foreach (var obj in goArray)
			{
				if (obj.layer == MappingPhysicsLayer)
				{
					var tracker = obj.AddComponent<PracticalGazeTracker>();
					tracker.customName = true;
					tracker.customIdentifier = "Mapping";
				}
			}

		}

		/**
			* @api {C# - Class} PracticalGazeTracker.cs PracticalGazeTracker.cs
			* @apiName PracticalGazeTracker.cs
			* @apiDescription PracticalGazeTracker.cs provides gaze tracking utility for any GameObject with this script.
			* 
			* @apiExample Description:
			* Drag this script onto objects to utilize gaze tracking capabilities. 
			* 
			* Use the custom name field to use a custom name for a GameObject.
			* 
			* This script also enables you group multiple GameObject stats together:
			* - Drag this script onto objects you want to group and give them a identical custom name.
			* - Make sure the custom name on the grouped objects match. 
			* 
			* The custom name will show in the portal with the collected gaze stats from that group. 
			* 
			* See IncludeMapping() to add mapping to gaze tracking. 
			* 
			* @apiParam (Variables) {bool} customName Triggers use of a seperate custom GameObject name.
			* @apiParam (Variables) {string} customIdentifier Assigns a custom GameObject name.
		    * 
			* @apiGroup Helpers
			* @apiPermission Beta
			* @apiVersion 0.1.0
			*/

		/**
		* @api {beta release} Info Info
		* @apiName Info
		* @apiExample Getting Started:
		* Repository:
		* https://github.com/PracticalVR/PracticalAnalytics-SDK-WH
		* 
		* Portal:
		* http://analytics.practicalvr.com
		* 
		* Import the PracticalAnayltics package and drag the PracticalManager prefab into your scene.
		* 
		* PracticalManager will include:
		* - PracticalConfig.cs
		*         - API key field (Obtain from "My Apps" from portal)
		* - PracticalAPI.cs
		*         - Log Summary checkbox (Displays API results in output)
		* - PracticalDLL.cs
		*         - Feedback link
		* 
		* To set up DLL:
		* - locate the plugins folder. 
		* - Inside the folder will be the DLL.
		* - In inspector, set only WSAPlayer.
		* - Make sure CPU is set to X86.
		* - Click Apply.
		* 
		* A clean build is required when implementing DLL.
		* You must delete the content in your build folder when doing the initial build with DLL.
		* 
		* Required Capabilities: 
		* - InternetClient
		* - PicturesLibrary 
		* 
		* @apiGroup Basics
		* @apiPermission Beta
		* @apiVersion 0.1.0
		*/
	}
}

