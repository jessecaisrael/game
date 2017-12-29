using UnityEngine;
using System.Collections;

public class TrialDriver : MonoBehaviour {

	public GameObject puck;
	private Texture2D whiteTargetTexture;
	private Texture2D whiteChangeTexture;
	private Texture2D blackTargetTexture;
	private Texture2D blackChangeTexture;
	private ActiveConditionSingleton.Thirds unexpectedSegment;
	private bool unexpectedPuckChgOccurs;
	private int simulationId;
	private Texture2D whiteBasicTexture;
	private Texture2D blackBasicTexture;

	private bool countdownActive = false;
	private bool pucksActive = false;
	private int numPucks = 20;
	private float trialLength = 20;
	private bool changed = false;
	private double countdownStartTime;
	private double puckStartTime;

	private int markedTransfers = 0; //this tracks spacebar presses

    void Start () {
		//Get Conditions
		whiteTargetTexture = ActiveConditionSingleton.whiteTargetTexture;
		whiteChangeTexture = ActiveConditionSingleton.whiteChangeTexture;
		blackTargetTexture = ActiveConditionSingleton.blackTargetTexture;
		blackChangeTexture = ActiveConditionSingleton.blackChangeTexture;
		unexpectedSegment = ActiveConditionSingleton.unexpectedSegment;
		unexpectedPuckChgOccurs = ActiveConditionSingleton.unexpectedPuckChgOccurs;
		simulationId = ActiveConditionSingleton.simulationId;
		whiteBasicTexture = ActiveConditionSingleton.whiteBasicTexture;
		blackBasicTexture = ActiveConditionSingleton.blackBasicTexture;
       
       //Read in positions and velocities
		string simulationData = "";

        using ( System.IO.StreamReader r = new System.IO.StreamReader(Application.dataPath + "/.." + "/Assets/IO/Scenarios.csv") ) {
			for( int i=0; i < simulationId; i++ ) {
				simulationData = r.ReadLine();
			}
		}

        Debug.Log("SimulationID is: " + simulationId);
        //Load pucks
		string[] simulationValues = simulationData.Split (new char[]{','});
		int valueIndex = 0;
        
        for ( int i=0; i < numPucks; i++) {
			GameObject currentPuck = (GameObject)GameObject.Instantiate(puck);

            Vector3 initialPosition = new Vector3(float.Parse(simulationValues[valueIndex++], System.Globalization.NumberStyles.Number), 0, float.Parse(simulationValues[valueIndex++],System.Globalization.NumberStyles.Number));
			Vector3 initialDirection = new Vector3(float.Parse(simulationValues[valueIndex++], System.Globalization.NumberStyles.Number), 0, float.Parse(simulationValues[valueIndex++], System.Globalization.NumberStyles.Number));
			currentPuck.GetComponent<PuckBehavior>().setInitialPosition(initialPosition); //record starting position
			currentPuck.GetComponent<PuckBehavior>().transform.position = initialPosition; //actually put the puck at that position
			currentPuck.GetComponent<PuckBehavior>().setInitialDirection(initialDirection);
			currentPuck.GetComponent<Rigidbody>().velocity = Vector3.zero;
			if( i == 0 ) {  //the first puck is the white target
				currentPuck.GetComponent<Renderer>().material.mainTexture = whiteTargetTexture;
				currentPuck.GetComponent<PuckBehavior>().isTarget = true;
				currentPuck.GetComponent<PuckBehavior>().puckColor = Color.white;
			}
			else if( i > 0 && i < 10 ) {
				currentPuck.GetComponent<Renderer>().material.mainTexture = whiteBasicTexture;
				currentPuck.GetComponent<PuckBehavior>().isTarget = false;
				currentPuck.GetComponent<PuckBehavior>().puckColor = Color.white;
			}
			else if ( i == 10 ) {  //the 11th puck is the black target
				currentPuck.GetComponent<Renderer>().material.mainTexture = blackTargetTexture;
				currentPuck.GetComponent<PuckBehavior>().isTarget = true;
				currentPuck.GetComponent<PuckBehavior>().puckColor = Color.black;
			}
			else if( i > 10 && i < 20 ) {
				currentPuck.GetComponent<Renderer>().material.mainTexture = blackBasicTexture;
				currentPuck.GetComponent<PuckBehavior>().isTarget = false; 
				currentPuck.GetComponent<PuckBehavior>().puckColor = Color.black;
			}
			valueIndex += 2; //skip both collision counts
		}

              
        StartCountdown ();
	}

	void StartCountdown() {
		countdownActive = true;
		countdownStartTime = Time.time;
	}

	void StopCountdown() {
		countdownActive = false;
	}

	void StartPucks() {
		pucksActive = true;
		GameObject[] puckGOs = GameObject.FindGameObjectsWithTag("Puck");
		foreach (GameObject puckGO in puckGOs) {
			puckGO.GetComponent<Rigidbody>().velocity = puckGO.GetComponent<PuckBehavior>().getInitialDirection() * puckGO.GetComponent<PuckBehavior>().getSpeed();
		}
		puckStartTime = Time.time;  //will be 0 because affected by timeScale
	}

	void StopPucks() {
		pucksActive = false;
		GameObject[] puckGOs = GameObject.FindGameObjectsWithTag("Puck");
		foreach (GameObject puckGO in puckGOs) {
			puckGO.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	void OnGUI() {
		GUIStyle labelStyle = new GUIStyle (GUI.skin.label);
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.fontSize = 20;
		labelStyle.normal.textColor = Color.black;
		labelStyle.richText = true;
		GUIStyle paragraphStyle = new GUIStyle (labelStyle);
		paragraphStyle.wordWrap = true;
		GUIStyle countdownStyle = new GUIStyle (labelStyle);
		countdownStyle.alignment = TextAnchor.MiddleCenter;
		countdownStyle.fontSize = 200;

		//Participant marked transfer count
		//GUI.Label (new Rect (Screen.width - 100, 100, 100, Screen.height - 200), "Your\n<u>Count</u>\n" + "<size=" + 2 * labelStyle.fontSize + ">" + markedTransfers + "</size>", labelStyle);

		//When counting down
		if (countdownActive) {

			//Numbers
			if( Time.time - countdownStartTime <= 1 ) GUI.Label(CenteredRect (205,205),"3", countdownStyle);
			else if( Time.time - countdownStartTime <= 2 ) GUI.Label(CenteredRect (205,205),"2", countdownStyle);
			else if( Time.time - countdownStartTime <= 3 ) GUI.Label(CenteredRect (205,205),"1", countdownStyle);

			//End countdown
			if( Time.time - countdownStartTime > 3 ){
				StopCountdown();
                //Begin background whitenoise at the beginning of the trial
                AudioManager.Instance().PlayBGM();
                StartPucks();
                AudioManager.Instance().RandomizeISI();
                AudioManager.Instance().PlaySoundStimuli();
			}
		}
	}

	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}

	// Update is called once per frame
	void Update () {
		if( pucksActive ) {
			//Handle keyboard input
			if (Input.GetKeyDown(KeyCode.Space)) { //v3: added this
				markedTransfers += 1;
			}
			//When to change unattended puck color
			if (!changed && unexpectedPuckChgOccurs && Time.time - puckStartTime >= (double)unexpectedSegment) {
				GameObject[] puckGOs = GameObject.FindGameObjectsWithTag("Puck");
				foreach ( GameObject puckGO in puckGOs) {
					PuckBehavior script = puckGO.GetComponent<PuckBehavior>();
					if ( script.isTarget ) {
						if( script.puckColor == Color.white) puckGO.GetComponent<Renderer>().material.mainTexture = whiteChangeTexture;
						else if (script.puckColor == Color.black) puckGO.GetComponent<Renderer>().material.mainTexture = blackChangeTexture;
					}
				}
				changed = true;
			}
			//When the trial is over
			if (GetTimeRemaining() <= 0) {
				StopPucks();
                foreach( GameObject go in GameObject.FindGameObjectsWithTag("Puck") ) {
					Destroy(go);
				}
                AudioManager.Instance().StopBGM();
                gameObject.GetComponent<PosttaskGUI>().enabled = true;
				gameObject.GetComponent<PosttaskGUI>().spacebarTransferCount = markedTransfers;
			}
		}
	}

	public double GetTimeRemaining() {
		return trialLength - (Time.time - puckStartTime);
	}

}
