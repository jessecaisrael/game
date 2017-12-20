using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExperimentDriver : MonoBehaviour {

	private ArrayList conditions;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);

		//List all possible conditions
		int simulationId = 2;  //id 1 is for practice.  Condition permutation needs to always occur with the same simulation conditions
		Color[] attendedPuckColors = new Color[]{Color.white, Color.black};
		Color[] attendedTargetColors = new Color[]{Color.red, Color.green};
		Color[] unattendedUnexpectedTargetColors = new Color[]{Color.clear, Color.white, Color.black};  //clear is code for whatever the attended target color is (R/G)
		ActiveConditionSingleton.Thirds[] unexpectedTimes = new ActiveConditionSingleton.Thirds[] {
						ActiveConditionSingleton.Thirds.Five,
						ActiveConditionSingleton.Thirds.Ten,
						ActiveConditionSingleton.Thirds.Fifteen
				};

        bool[] unexpectedSoundsOptions = new bool[] { true, false };
        //int[] unexpectedEars = new int[] { -1, 1 };
        bool[] unexpectedOccurOptions = new bool[]{true, false};  //v4: Add back unexpected events.
		conditions = new ArrayList ();
		foreach (Color attendedPuckColor in attendedPuckColors) {
			foreach (Color attendedTargetColor in attendedTargetColors) {
				foreach (Color unattendedTargetColor in unattendedUnexpectedTargetColors) {
					foreach (ActiveConditionSingleton.Thirds unexpectedTime in unexpectedTimes) {
						foreach (bool unexpectedOccurOption in unexpectedOccurOptions) {
          //                  foreach (int unexpectedEar in unexpectedEars) { 
                                foreach (bool unexpectedSoundsOption in unexpectedSoundsOptions) { 
                                    conditions.Add(new Condition(simulationId++, attendedPuckColor, attendedTargetColor, unattendedTargetColor, unexpectedTime, unexpectedOccurOption, unexpectedSoundsOption));
                                }   
                //            }
                        }
					}
				}
			}
		}
		Debug.Log ("Condition count: " + conditions.Count);

        //Randomize condition order
        Random.InitState(gameObject.GetComponent<Logger>().pid);
		//Deprecated: Random.seed = gameObject.GetComponent<Logger> ().pid;
		for (int i=0; i < conditions.Count; i++) {
			Condition c = (Condition)conditions[i];
			int randomIndex = Random.Range(i, conditions.Count);
			conditions[i] = conditions[randomIndex];
			conditions[randomIndex] = c;
		}

		LoadNextTrial ();
	}

	void LoadNextTrial() {
		if( conditions.Count > 0 ) {
			ActiveConditionSingleton.complete = false;
			gameObject.GetComponent<Logger>().actualTransfers = 0;

			//Set condition IVs to Active Condition
			Condition currentCondition = (Condition)conditions [0];
			conditions.RemoveAt (0);
			ActiveConditionSingleton.attendedPuckColor = currentCondition.attendedPuckColor;
			if (currentCondition.attendedPuckColor == Color.white) {
				//Starting colors
				if (currentCondition.attendedTargetColor == Color.red) {
					ActiveConditionSingleton.whiteTargetTexture = (Texture2D)Resources.Load ("WhiteBgRedCenterBlackBorder");
					ActiveConditionSingleton.blackTargetTexture = (Texture2D)Resources.Load ("BlackBgGreenCenter");
				}
				else if (currentCondition.attendedTargetColor == Color.green) {
					ActiveConditionSingleton.whiteTargetTexture = (Texture2D)Resources.Load ("WhiteBgGreenCenterBlackBorder");
					ActiveConditionSingleton.blackTargetTexture = (Texture2D)Resources.Load ("BlackBgRedCenter");
				}
				else throw new UnityException("Attended target color invalid");
				//Changing colors
				ActiveConditionSingleton.whiteChangeTexture = ActiveConditionSingleton.whiteTargetTexture;
				if(currentCondition.unattendedUnexpectedTargetColor == Color.clear) {
					if (currentCondition.attendedTargetColor == Color.red) {
						ActiveConditionSingleton.blackChangeTexture = (Texture2D)Resources.Load ("BlackBgRedCenter");
					}
					else if( currentCondition.attendedTargetColor == Color.green) {
						ActiveConditionSingleton.blackChangeTexture = (Texture2D)Resources.Load ("BlackBgRedCenter");
					}
				}
				else if(currentCondition.unattendedUnexpectedTargetColor == Color.white) {
					ActiveConditionSingleton.blackChangeTexture = ActiveConditionSingleton.whiteBasicTexture;
				}
				else if(currentCondition.unattendedUnexpectedTargetColor == Color.black) {
					ActiveConditionSingleton.blackChangeTexture = ActiveConditionSingleton.blackBasicTexture;
				}
			}
			else if (currentCondition.attendedPuckColor == Color.black) {
				//Starting colors
				if (currentCondition.attendedTargetColor == Color.red) {
					ActiveConditionSingleton.whiteTargetTexture = (Texture2D)Resources.Load ("WhiteBgGreenCenterBlackBorder");
					ActiveConditionSingleton.blackTargetTexture = (Texture2D)Resources.Load ("BlackBgRedCenter");
				}
				else if (currentCondition.attendedTargetColor == Color.green) {
					ActiveConditionSingleton.whiteTargetTexture = (Texture2D)Resources.Load ("WhiteBgRedCenterBlackBorder");
					ActiveConditionSingleton.blackTargetTexture = (Texture2D)Resources.Load ("BlackBgGreenCenter");
				}
				else throw new UnityException("Attended target color invalid");
				//Changing colors
				ActiveConditionSingleton.blackChangeTexture = ActiveConditionSingleton.blackTargetTexture;
				if(currentCondition.unattendedUnexpectedTargetColor == Color.clear) {
					if (currentCondition.attendedTargetColor == Color.red) {
						ActiveConditionSingleton.whiteChangeTexture = (Texture2D)Resources.Load ("WhiteBgRedCenterBlackBorder");
					}
					else if( currentCondition.attendedTargetColor == Color.green) {
						ActiveConditionSingleton.whiteChangeTexture = (Texture2D)Resources.Load ("WhiteBgGreenCenterBlackBorder");
					}
				}
				else if(currentCondition.unattendedUnexpectedTargetColor == Color.white) {
					ActiveConditionSingleton.whiteChangeTexture = ActiveConditionSingleton.whiteBasicTexture;
				}
				else if(currentCondition.unattendedUnexpectedTargetColor == Color.black) {
					ActiveConditionSingleton.whiteChangeTexture = ActiveConditionSingleton.blackBasicTexture;
				}
			}
			else throw new UnityException("Attended puck color invalid");

			//Changing colors
			if(currentCondition.unattendedUnexpectedTargetColor == Color.clear) {
				if (currentCondition.attendedTargetColor == Color.red) {
					currentCondition.unattendedUnexpectedTargetColor = Color.red;
				}
				else if (currentCondition.attendedTargetColor == Color.green) {
					currentCondition.unattendedUnexpectedTargetColor = Color.green;
				}
			}

			ActiveConditionSingleton.unexpectedSegment = currentCondition.unexpectedTime;
			ActiveConditionSingleton.unexpectedPuckChgOccurs = currentCondition.unexpectedPuckChgOccurs;
			ActiveConditionSingleton.simulationId = currentCondition.simulationId;
            ActiveConditionSingleton.unexpectedSoundOccurs = currentCondition.unexpectedSound;
            // ActiveConditionSingleton.unexpectedEar = currentCondition.unexpectedEar;
            AudioManager.Instance().SetOddballOccurrence();
            if(AudioManager.Instance().GetOddballOccurrence() == 1)
              AudioManager.Instance().SetOddballPosition();
    
            SceneManager.LoadScene("Trial");
            //Application.LoadLevel ("Trial");

			//Set Condition IVs to log
			Logger logger = (Logger)gameObject.GetComponent<Logger>();
			logger.scenario = currentCondition.simulationId;
			logger.attendedPuckColor = GetColorString(currentCondition.attendedPuckColor);
			logger.attendedTargetColor = GetColorString(currentCondition.attendedTargetColor);
			logger.unattendedUnexpectedTargetColor = GetColorString(currentCondition.unattendedUnexpectedTargetColor);
			logger.unexpectedTime = (int)currentCondition.unexpectedTime;

            if (AudioManager.Instance().GetOddballOccurrence() == 1) logger.oddballOccurred = "yes";
            else logger.oddballOccurred = "no";

            logger.oddBallPosition = AudioManager.Instance().GetOddballPosition();

            if ( currentCondition.unexpectedPuckChgOccurs ) logger.unexpectedPuckOccurred = "yes";
			else logger.unexpectedPuckOccurred = "no";

            if ( currentCondition.unexpectedSound) logger.unexpectedSoundOccurred = "yes";
            else logger.unexpectedSoundOccurred = "no";
		}
		else {
			gameObject.GetComponent<ExperimentCompleteGUI>().enabled = true;
			Destroy(this);
		}
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
			LoadNextTrial();
		}
	}

	class Condition {
		public int simulationId;
		public Color attendedPuckColor;
		public Color attendedTargetColor;
		public Color unattendedUnexpectedTargetColor;
		public ActiveConditionSingleton.Thirds unexpectedTime;
		public bool unexpectedPuckChgOccurs;
        public bool unexpectedSound;

        // public int unexpectedEar = 1;

        public Condition ( int simulationId, Color attendedPuckColor, Color attendedTargetColor, Color unattendedUnexpectedTargetColor, ActiveConditionSingleton.Thirds unexpectedTime, bool unexpectedOccurs, bool unexpectedSound) {
			this.simulationId = simulationId;
			this.attendedPuckColor = attendedPuckColor;
			this.attendedTargetColor = attendedTargetColor;
			this.unattendedUnexpectedTargetColor = unattendedUnexpectedTargetColor;
			this.unexpectedTime = unexpectedTime;
			this.unexpectedPuckChgOccurs = unexpectedOccurs;
            this.unexpectedSound = unexpectedSound;
           // this.unexpectedEar = unexpectedEar;
        }
    }
}
