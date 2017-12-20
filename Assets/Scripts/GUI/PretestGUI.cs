using UnityEngine;
using System.Collections;

public class PretestGUI : MonoBehaviour {

	private string ageString = "";
	private int genderChoice = -1;
	private int collegeChoice = -1;
	private string warningText = "";

	void OnGUI() {
		GUIStyle labelStyle = new GUIStyle (GUI.skin.label);
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.fontSize = 14;
		labelStyle.normal.textColor = Color.black;
		GUIStyle warningStyle = new GUIStyle (labelStyle);
		warningStyle.normal.textColor = Color.red;
		GUIStyle textFieldStyle = new GUIStyle (GUI.skin.textField);
		textFieldStyle.alignment = TextAnchor.MiddleCenter;
		textFieldStyle.fontSize = 14;
		textFieldStyle.normal.textColor = Color.black;


		//Content
		GUILayout.BeginArea (CenteredRect ((int)(Screen.width / 1.5), Screen.height / 2));
		{//block for organization
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("What is your age in years?", labelStyle);
			ageString = GUILayout.TextField (ageString, 2, textFieldStyle, GUILayout.Width(labelStyle.CalcSize(new GUIContent("000")).x));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("What is your gender?", labelStyle);
			genderChoice = GUILayout.Toolbar (genderChoice, new string[]{"Female","Male"});
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("In what college are you currently enrolled?", labelStyle);
			collegeChoice = GUILayout.SelectionGrid (collegeChoice, new string[]{"CHASS","CALS","Design","Education","Engineering","Management","Natural Resources", "Textiles", "First Year College"},3);
			GUILayout.EndHorizontal ();

			GUILayout.Space(100);
			
			if( GUILayout.Button("Click to Continue")) {
				if( ValidateInput()) {
					//load next
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

		int age = 0;
		if (!int.TryParse (ageString, out age)) {
			valid = false;
			warningText += "Invalid age\n";
		}
		
		if (genderChoice == -1) {
			valid = false;
			warningText += "You must choose a gender\n";
		}

		if (collegeChoice == -1) {
			valid = false;
			warningText += "You must choose a college\n";
		}
		
		return valid;
	}

	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}

}
