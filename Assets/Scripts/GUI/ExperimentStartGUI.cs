using UnityEngine;
using System.Collections;

public class ExperimentStartGUI : MonoBehaviour {

	private string pidString = "";
	private string warningText = "";

	void OnGUI() {
        GUIStyle labelStyle = new GUIStyle (GUI.skin.label);
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.fontSize = 14;
		labelStyle.normal.textColor = Color.black;
		GUIStyle titleStyle = new GUIStyle (labelStyle);
		titleStyle.fontSize = 24;
		GUIStyle warningStyle = new GUIStyle (labelStyle);
		warningStyle.normal.textColor = Color.red;
		GUIStyle textFieldStyle = new GUIStyle (GUI.skin.textField);
		textFieldStyle.alignment = TextAnchor.MiddleCenter;
		textFieldStyle.fontSize = 14;
		textFieldStyle.normal.textColor = Color.black;		
		
		//Content
		GUILayout.BeginArea (CenteredRect ((int)(Screen.width / 1.25), (int)(Screen.height / 2)));
		{//block for organization
			GUILayout.Label ("Experiment", titleStyle);

			GUILayout.BeginHorizontal ();
			string label = "Enter PID (from Qualtrics survey):";
			GUILayout.Label (label, labelStyle, GUILayout.Width(labelStyle.CalcSize(new GUIContent(label)).x), GUILayout.ExpandWidth(false));
			pidString = GUILayout.TextField (pidString, 3, textFieldStyle, GUILayout.Width(labelStyle.CalcSize(new GUIContent("000")).x));
			GUILayout.EndHorizontal ();
						
			GUILayout.Space(20);
			
			if( GUILayout.Button("Click to Continue")) {
				if( ValidateInput()) {
					gameObject.GetComponent<Logger>().pid = int.Parse(pidString);
					gameObject.GetComponent<PepTalkGUI>().enabled = true;
					Destroy(this);
				}
			}
			
			if (warningText != "") {
				GUILayout.Label(warningText, warningStyle, GUILayout.ExpandHeight(true));
			}
		}
		
		GUILayout.EndArea ();
	}
	
	bool ValidateInput() {
		warningText = "";
		bool valid = true;
		
		int pid = 0;
		if (!int.TryParse (pidString, out pid)) {
			valid = false;
			warningText += "Invalid pid\n";
		}
		
		return valid;
	}
	
	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}

}
