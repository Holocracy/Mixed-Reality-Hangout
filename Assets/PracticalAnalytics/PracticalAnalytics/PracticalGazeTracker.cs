using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Practical;

namespace Practical.Analytics
{
    public class PracticalGazeTracker : MonoBehaviour, IFocusable
    {
        [Header("Custom Identifier (default is gameObject.name)")]
        [Tooltip("Use this only if you want to use a different name than the object this script is attached to.")]
        public bool customName = false;
        [Tooltip("This is the unique identifier that stats will be grouped by and labeled in the portal")]
        public string customIdentifier;

        public void OnFocusEnter()
        {
            if (customName)
                PracticalAPI.Instance.RecordGazeOn(customIdentifier);
            else
                PracticalAPI.Instance.RecordGazeOn(gameObject.name);
        }

        public void OnFocusExit()
        {
            PracticalAPI.Instance.RecordGazeOff();
        }
    }
}