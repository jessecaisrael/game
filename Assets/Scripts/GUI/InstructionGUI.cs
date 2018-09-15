using UnityEngine;
using System.Collections;

public class InstructionGUI : MonoBehaviour {

	public Texture2D puckSample;
    public string scenarioText;

	void Start() {
		puckSample = (Texture2D)Resources.Load ("PuckSample");

        scenarioText = "You will complete 2 tasks.  For the <b>first task</b>, a group of pucks (some with a black base color and some with a white base color) will be bouncing around.  One of the black pucks will have a colored dot in its center indicating that puck is a target puck.  One of the white pucks will also be a target puck.  When a target puck collides with another puck of the same base color (i.e. black-black or white-white), sometimes (not always!) the target will transfer to the colliding puck of the same base color.";
        
    }

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
			GUILayout.Label ("Instructions", titleStyle);

            GUILayout.Label (scenarioText, paragraphStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));

			GUILayout.Label (puckSample, GUILayout.ExpandHeight(false), GUILayout.Height(75));

			string taskText;
			switch(ExperimentSettings.responseType)
			{
			case ExperimentSettings.ResponseType.Recall:
				taskText = "<b>Your tasks are to:</b> \n 1.) Track one (<i>only one!</i>) of the target pucks, either white or black, which will be specified at the beginning of each trial.  Be sure to track the correct puck! \n 2.) Count the number of times the target transfers between pucks.";
				GUILayout.Label (taskText, paragraphStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
				break;
			case ExperimentSettings.ResponseType.Spacebar:
				taskText = "<b>Your tasks are to:</b> \n 1.) Track one (<i>only one!</i>) of the target pucks, either white or black, which will be specified at the beginning of each trial.  Be sure to track the correct puck! \n 2.) Quickly press the Spacebar every time you observe a transfer of the target you are tracking.";
				GUILayout.Label (taskText, paragraphStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
				break;
			}

            GUILayout.Label("\n\n For the <b>second task</b>, you will hear a series of sounds that differ in pitch. \n\n " +
                "<b>Your tasks are to:</b> \n 1.) Listen for the target sound.  This sound will play at a <i>higher pitch</i> than the other sounds.\n " +
                "2.) At the end of each trial, you will be asked to note the presence or absence of the target sound.\n\n",
                paragraphStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));

            if (GUILayout.Button("Click to Hear Target Sound")) {
                AudioManager.Instance().PlayOddball();
            } 
            if (GUILayout.Button("Click to Hear Distractor Sound 1")) {
                AudioManager.Instance().PlayStdSound2();
            }
            if (GUILayout.Button("Click to Hear Distractor Sound 2")) {
                AudioManager.Instance().PlayStdSound3();
            }

            GUILayout.Label("\n\n You will complete these two tasks at the same time. Please spend equal time on both tasks and shoot for 100% accuracy on both tasks. White noise will play to reduce background noise and aid in concentration.\n\n",
                paragraphStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));

            GUILayout.Label("\n Next, you will receive one practice trial to get accustomed to the format.", paragraphStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));

            if ( GUILayout.Button("Click to Continue")) {
				gameObject.GetComponent<PracticeDriver>().enabled = true;
				Destroy(this);
			}
		}
		GUILayout.EndArea ();
	}
	
	
	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}

}
