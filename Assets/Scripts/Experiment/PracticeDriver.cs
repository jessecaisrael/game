using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PracticeDriver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);

		//Load Practice
		ActiveConditionSingleton.complete = false;
		ActiveConditionSingleton.attendedPuckColor = Color.white;
        ActiveConditionSingleton.whiteBasicTexture = (Texture2D)Resources.Load("WhiteBGBlackBorder");
        ActiveConditionSingleton.blackBasicTexture = (Texture2D)Resources.Load("BlackBG");
        ActiveConditionSingleton.whiteTargetTexture = (Texture2D)Resources.Load ("WhiteBgRedCenterBlackBorder");
		ActiveConditionSingleton.whiteChangeTexture = (Texture2D)Resources.Load ("WhiteBgRedCenterBlackBorder");
		ActiveConditionSingleton.blackTargetTexture = (Texture2D)Resources.Load ("BlackBgGreenCenter");
		ActiveConditionSingleton.blackChangeTexture = (Texture2D)Resources.Load ("BlackBgRedCenter");
		ActiveConditionSingleton.unexpectedSegment = ActiveConditionSingleton.Thirds.Five;
		ActiveConditionSingleton.unexpectedPuckChgOccurs = false;
		ActiveConditionSingleton.simulationId = 1; //if you have more than 1 practice trial you need to adjust simulationId starting val in ExperimentDriver
        ActiveConditionSingleton.unexpectedSoundOccurs = false; 

        //Set Condition IVs to log
        Logger logger = (Logger)gameObject.GetComponent<Logger>();
		logger.scenario = ActiveConditionSingleton.simulationId;
		logger.attendedPuckColor = GetColorString (ActiveConditionSingleton.attendedPuckColor);
		logger.attendedTargetColor = GetColorString (Color.red);
		logger.unattendedUnexpectedTargetColor = GetColorString (Color.red);
		logger.unexpectedTime = (int)ActiveConditionSingleton.Thirds.Five;
		if( ActiveConditionSingleton.unexpectedPuckChgOccurs ) logger.unexpectedPuckChgOccurred = "yes";
		else logger.unexpectedPuckChgOccurred = "no";

        if (ActiveConditionSingleton.unexpectedSoundOccurs) logger.unexpectedSoundOccurred = "yes";
        else logger.unexpectedSoundOccurred = "no";

        AudioManager.Instance().SetOddballOccurrence(true); //play target sound during practice
        AudioManager.Instance().SetUnexpectedSound(false);
        if (AudioManager.Instance().GetOddballOccurrence() == 1) {
            AudioManager.Instance().SetOddballPosition(10);
            AudioManager.Instance().SetOddballEar(-1);
            logger.oddballOccurred = "yes";

        } else {
                logger.oddballOccurred = "no";
        }
                        
        SceneManager.LoadScene("Trial");
        //Deprecated: Application.LoadLevel ("Trial");
	}

	string GetColorString(Color c) {
		if (c == Color.white)
			return "white";
		else if (c == Color.black)
			return "black";
		else if (c == Color.green)
			return "green";
		else if (c == Color.red)
			return "red";
		else
			return "";
	}

	// Update is called once per frame
	void Update () {
		if (ActiveConditionSingleton.complete) {
			gameObject.GetComponent<PracticeAnswerGUI>().enabled = true;
			Destroy(this);
		}
	}
}
