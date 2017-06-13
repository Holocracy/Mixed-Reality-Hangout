using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Practical.Analytics;

[CustomEditor(typeof(PracticalDLL))]
public class CustomHelpInspector : Editor
{
	public override void OnInspectorGUI()
	{
		GUILayout.Label("Have feedback?");
		if (GUILayout.Button("Give Feedback"))
			GiveFeedback();
	}

	void GiveFeedback()
	{
		var helpButton = new PracticalDLL();
		helpButton.SendEmail();
	}
}
