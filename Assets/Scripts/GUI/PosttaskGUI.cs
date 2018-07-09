using UnityEngine;
using System.Collections;

public class PosttaskGUI : MonoBehaviour {

	public int spacebarTransferCount; //number of spacebar presses (passed from TrialDriver)
	private string mentalTransferCountString = ""; //number of recalled transfers
	private int transferConfidence = 50;
	private int trackedBaseColor = -1;
	private int unexpectedChoice = -1;  //v4: added this back in
	private int unexpectedConfidence = 50;
	private string unexpectedDescription = "";
	//private string mentalTrialTime = "";
	private string warningText = "";
    private int oddballSoundChoice = -1;
    private int oddballConfidence = 50;

	void OnGUI() {
		//Style
		GUIStyle labelStyle = new GUIStyle (GUI.skin.label);
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.fontSize = 14;
		labelStyle.normal.textColor = Color.black;
		labelStyle.padding = new RectOffset (0, 0, 0, 10);
		GUIStyle textFieldStyle = new GUIStyle (GUI.skin.textField);
		textFieldStyle.alignment = TextAnchor.MiddleCenter;
		textFieldStyle.fontSize = 14;
		textFieldStyle.normal.textColor = Color.black;
		GUIStyle textAreaStyle = textFieldStyle;
		textAreaStyle.alignment = TextAnchor.UpperLeft;
		textAreaStyle.wordWrap = true;
		GUIStyle sliderStyle = new GUIStyle (GUI.skin.horizontalSlider);
		sliderStyle.margin = new RectOffset (4, 4, 15, 4);
		GUIStyle warningStyle = new GUIStyle (labelStyle);
		warningStyle.normal.textColor = Color.red;

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), (Texture2D)Resources.Load ("WhiteBg"), ScaleMode.StretchToFill);

		//Content
		GUILayout.BeginArea (CenteredRect ((int)(Screen.width / 1.25), (int)(Screen.height / 1.25)));
		{//block for organization

            if (!ActiveConditionSingleton.fullAttentionReady) {
                switch (ExperimentSettings.responseType)
                {
                    case ExperimentSettings.ResponseType.Recall:
                        GUILayout.BeginHorizontal();  //v6: added this back //v3: Removed recall of transfer question
                        GUILayout.Label("How many transfers did you count?", labelStyle);
                        mentalTransferCountString = GUILayout.TextField(mentalTransferCountString, 3, textFieldStyle, GUILayout.Width(labelStyle.CalcSize(new GUIContent("00000")).x));
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("How confident are you in your transfer count for this trial?", labelStyle);
                        transferConfidence = (int)GUILayout.HorizontalSlider((float)transferConfidence, 0f, 100f, sliderStyle, GUI.skin.horizontalSliderThumb, GUILayout.Width(200));
                        GUILayout.Label(transferConfidence.ToString() + "%", labelStyle, GUILayout.Width(labelStyle.CalcSize(new GUIContent("100%")).x));
                        GUILayout.EndHorizontal();
                        break;
                    case ExperimentSettings.ResponseType.Spacebar:
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("How confident are you that you recorded the correct number of transfers (by hitting the spacebar) during this trial?", labelStyle);
                        transferConfidence = (int)GUILayout.HorizontalSlider((float)transferConfidence, 0f, 100f, sliderStyle, GUI.skin.horizontalSliderThumb, GUILayout.Width(200));
                        GUILayout.Label(transferConfidence.ToString() + "%", labelStyle, GUILayout.Width(labelStyle.CalcSize(new GUIContent("100%")).x));
                        GUILayout.EndHorizontal();
                        break;
                }

                GUILayout.BeginHorizontal(); //v3: Added new question
                GUILayout.Label("Which base color target puck were you tracking?", labelStyle);
                trackedBaseColor = GUILayout.Toolbar(trackedBaseColor, new string[] { "black", "white" });
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(); //Added new question for auditory task
                GUILayout.Label("Was the target sound presented during the task?", labelStyle);
                oddballSoundChoice = GUILayout.Toolbar(oddballSoundChoice, new string[] { "no", "yes" });
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("How confident are you that you heard (or didn't hear) the target sound in this trial?", labelStyle);
                oddballConfidence = (int)GUILayout.HorizontalSlider((float)oddballConfidence, 0f, 100f, sliderStyle, GUI.skin.horizontalSliderThumb, GUILayout.Width(200));
                GUILayout.Label(oddballConfidence.ToString() + "%", labelStyle, GUILayout.Width(labelStyle.CalcSize(new GUIContent("100%")).x));
                GUILayout.EndHorizontal();
            }
            
            GUILayout.BeginHorizontal ();  //v4: Add back option question if saw unexpected.
			GUILayout.Label ("Did you notice anything unexpected?", labelStyle);
			unexpectedChoice = GUILayout.Toolbar (unexpectedChoice, new string[]{"no","yes"});
			GUILayout.EndHorizontal ();
			
			GUILayout.BeginHorizontal ();  //v4: Add back slider question on confidence.
			GUILayout.Label ("How confident are you in that assertion?", labelStyle);
			unexpectedConfidence = (int)GUILayout.HorizontalSlider((float)unexpectedConfidence, 0f, 100f, sliderStyle, GUI.skin.horizontalSliderThumb, GUILayout.Width (200));
			GUILayout.Label (unexpectedConfidence.ToString()+"%", labelStyle, GUILayout.Width(labelStyle.CalcSize(new GUIContent("100%")).x));
			GUILayout.EndHorizontal ();

			//GUILayout.BeginHorizontal ();  //v7: Add time question
			//GUILayout.Label ("How long (in seconds) do you think the previous trial was?", labelStyle);
			//mentalTrialTime = GUILayout.TextField (mentalTrialTime, 3, textFieldStyle, GUILayout.Width(labelStyle.CalcSize(new GUIContent("00000")).x));
			//GUILayout.EndHorizontal ();

			if (unexpectedChoice == 1) {

				GUILayout.Space(5);

				GUILayout.BeginHorizontal ();  //v7: Added new question
				GUILayout.Label ("Please describe what you noticed that was unexpected in the previous trial.", labelStyle);
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal ();  //v7: Added new question
				unexpectedDescription = GUILayout.TextArea(unexpectedDescription, 400, textAreaStyle, GUILayout.ExpandHeight(true), GUILayout.MaxHeight(70));
				GUILayout.EndHorizontal ();

			}

			GUILayout.Space(10);

			if( GUILayout.Button("Click to Continue to Next Trial")) {
				if( ValidateInput()) {
                    if(!ActiveConditionSingleton.fullAttentionReady)
                        LogResponses();
					ActiveConditionSingleton.complete = true;
					Destroy(this);
				}
			}
			if (warningText != "") {
				GUILayout.Label(warningText, warningStyle, GUILayout.ExpandHeight(false));
			}
		}

		GUILayout.EndArea ();
	}

	void LogResponses() {
		Logger logger = (Logger)GameObject.FindGameObjectWithTag ("ExperimentScripts").GetComponent<Logger> ();

		// user input
		switch(ExperimentSettings.responseType)
		{
		case ExperimentSettings.ResponseType.Recall:
			logger.observedTransfers = int.Parse (mentalTransferCountString);
			break;
		case ExperimentSettings.ResponseType.Spacebar:
			logger.observedTransfers = spacebarTransferCount;
			break;
		}
		logger.transferConfidence = transferConfidence;
		if (unexpectedChoice == 0)  //v4: added back  //v2: A lot of logic removed from this class to not account for or validate this
				logger.observedUnexpected = "no";
		else if (unexpectedChoice == 1)
				logger.observedUnexpected = "yes";
		logger.unexpectedConfidence = unexpectedConfidence;  //v4: Back to int  //v2: This logger variable used to be an int; now string
		logger.unexpectedDescription = unexpectedDescription;
        logger.oddballConfidence = oddballConfidence;

		/*logger.observedTrialTime = int.Parse (mentalTrialTime);*/

        if (oddballSoundChoice == 0)
            logger.observedOddball = "no";
        else if (oddballSoundChoice == 1)
            logger.observedOddball = "yes";

		// trial factors
		if (trackedBaseColor == 0)
						logger.trackedBaseColor = "black";
				else if (trackedBaseColor == 1)
						logger.trackedBaseColor = "white";
				else
						throw new UnityException ("Invalid tracked base color option: " + trackedBaseColor);
		if (ActiveConditionSingleton.attendedPuckColor.Equals(Color.black))
						logger.actualBaseColor = "black";
		else if (ActiveConditionSingleton.attendedPuckColor.Equals(Color.white))
						logger.actualBaseColor = "white";
				else
			throw new UnityException ("Invalid actual base color: " + ActiveConditionSingleton.attendedPuckColor);

		logger.LogTrial ();
	}

	bool ValidateInput() {
		warningText = "";
		bool valid = true;

        if (!ActiveConditionSingleton.fullAttentionReady)
        {
            switch (ExperimentSettings.responseType)
            {
                case ExperimentSettings.ResponseType.Recall:
                    int mentalTransferCount = 0;
                    if (!int.TryParse(mentalTransferCountString, out mentalTransferCount) || mentalTransferCount < 0)
                    {
                        valid = false;
                        warningText += "Transfer count must be a positive integer.\n";
                    }
                    break;
                case ExperimentSettings.ResponseType.Spacebar:
                    if (spacebarTransferCount < 0)
                    {
                        valid = false;
                        warningText += "Transfer count must be a positive integer.\n";
                    }
                    break;
            }

            if (trackedBaseColor == -1)
            {
                valid = false;
                warningText += "You must choose black or white.\n";
            }

            if (oddballSoundChoice == -1)
            { //Added in for auditory task
                valid = false;
                warningText += "You must choose yes or no\n";
            }
        }

        if (unexpectedChoice == -1) {  //v4: added back  //v2:  removed validation logic for unexpectedChoice
			valid = false;
			warningText += "You must choose yes or no\n";
		}

        //int observedTrialTime = 0;
		//if (!int.TryParse (mentalTrialTime, out observedTrialTime) || observedTrialTime < 0) {
			//valid = false;
			//warningText += "Estimated trial time must be a positive integer.\n";
		//} 

		if (unexpectedChoice == 1 && unexpectedDescription == "") {
			valid = false;
			warningText += "Please enter a description of what you observed.\n";
		}

		return valid;
	}
	
	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}
}
