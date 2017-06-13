using System.Runtime.InteropServices;
using UnityEngine;
using Practical;

namespace Practical.Analytics
{
	public class PracticalDLL : MonoBehaviour
	{
		[DllImport("PracticalDLL", CallingConvention = CallingConvention.StdCall)]
		public static extern System.IntPtr GetErrorMessage(int errorCode);

		[DllImport("PracticalDLL", CallingConvention = CallingConvention.StdCall)]
		public static extern System.IntPtr GetLastErrorMessage();
		
		// Records how many long a user is looking at an object. Passes timestamp of report, gameobject name & length of time gazing
		[DllImport("PracticalDLL", CallingConvention = CallingConvention.StdCall)]
		public static extern int Startup(string apiKey);

		[DllImport("PracticalDLL", CallingConvention = CallingConvention.StdCall)]
		public static extern int RecordGaze(string uniqueIdentifier, string value);

		// Records what keywords are being used.
		[DllImport("PracticalDLL", CallingConvention = CallingConvention.StdCall)]
		public static extern int RecordKeyword(string uniqueIdentifier, string value, string interactedObject);

		// Records what gestures are being used.
		[DllImport("PracticalDLL", CallingConvention = CallingConvention.StdCall)]
		public static extern int RecordGesture(string uniqueIdentifier, string value, string gestureType, string interactedObject, string holdLength);

		// Records customized state
		[DllImport("PracticalDLL", CallingConvention = CallingConvention.StdCall)]
		public static extern int RecordCustomStat(string uniqueIdentifier, string value, string measurement, string formula, string interactedObject);

		// Records when gaining resources
		[DllImport("PracticalDLL", CallingConvention = CallingConvention.StdCall)]
		public static extern int RecordGain(string uniqueIdentifier, string value);

		// Records when exhausting resources
		[DllImport("PracticalDLL", CallingConvention = CallingConvention.StdCall)]
		public static extern int RecordLoss(string uniqueIdentifier, string value);


		public void SetAPIKey(string LoadedAPIKey)
		{
#if WINDOWS_UWP
			LoadedAPIKey = LoadedAPIKey.Replace(" ","");
			int errorCode = Startup(LoadedAPIKey);
			if (errorCode > 0)
			{
				string errorMessage = Marshal.PtrToStringAnsi(GetErrorMessage(errorCode));
				Debug.Log(errorMessage);
			}
#endif
		}

		public void SendEmail()
		{
			string email = "support@practicalvr.com";
			string subject = MyEscapeURL("Practical Feedback");
			string body = ""; 

			Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
		}

		string MyEscapeURL(string url)
		{
			return WWW.EscapeURL(url).Replace("+", "%20");
		}
	}
}
