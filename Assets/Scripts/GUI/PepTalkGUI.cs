using UnityEngine;
using System.Collections;

public class PepTalkGUI : MonoBehaviour {

	void OnGUI() {
		GUIStyle labelStyle = new GUIStyle (GUI.skin.label);
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.fontSize = 14;
		labelStyle.normal.textColor = Color.black;
		labelStyle.richText = true;
		GUIStyle titleStyle = new GUIStyle (labelStyle);
		titleStyle.fontSize = 24;
		GUIStyle paragraphStyle = new GUIStyle (labelStyle);
		paragraphStyle.wordWrap = true;
		
		
		//Content
		GUILayout.BeginArea (CenteredRect ((int)(Screen.width / 1.25), (int)(Screen.height / 1.25)));
		{//block for organization
			GUILayout.Label ("Welcome!", titleStyle);
			
			string pepTalkText = "Thank you for volunteering for my experiment!  I really appreciate your time and effort.  In the spirit of good data, please remove potential distractions, such as cell phones, watch alarms, etc. Focus is important in this study, so having your full attention will help things roll smoothly.  I hope you find it interesting!  If you want to know more about the research topic, be sure to talk to the experimenter after completing the experiment.  GO SCIENCE!";
			GUILayout.Label (pepTalkText, paragraphStyle, GUILayout.ExpandWidth(true));
			
			GUILayout.Space(20);
			
			if( GUILayout.Button("Click to Continue")) {
				gameObject.GetComponent<InstructionGUI>().enabled = true;
				Destroy(this);
			}
		
		}
		
		GUILayout.EndArea ();
	}

	
	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}

}
