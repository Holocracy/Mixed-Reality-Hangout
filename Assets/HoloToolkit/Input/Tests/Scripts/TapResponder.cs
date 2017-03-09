// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;



namespace HoloToolkit.Unity.InputModule.Tests
{
    /// <summary>
    /// This class implements IInputClickHandler to handle the tap gesture.
    /// It increases the scale of the object when tapped.
    /// </summary>
    public class TapResponder : MonoBehaviour, IInputClickHandler
    {
        public GameObject object1;

        
        public void OnInputClicked(InputEventData eventData)
        {
            // Increase the scale of the object just as a response.
            

            if (object1.activeInHierarchy == true)
            {
                object1.SetActive(false);
                gameObject.transform.localScale -= 0.2f * gameObject.transform.localScale;
            }
            else
            {
                object1.SetActive(true);
                gameObject.transform.localScale += 0.2f * gameObject.transform.localScale;
            }

        }
    }
}