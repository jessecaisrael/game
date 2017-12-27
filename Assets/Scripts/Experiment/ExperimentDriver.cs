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

        //Experimental variables below
        Color[] attendedTargetColors = new Color[] { Color.red, Color.green };
        bool[] unexpectedOccurOptions = new bool[] { true, false };  //v4: Add back unexpected events.
        bool[] oddballOccurOptions = new bool[] { true, false };
        bool[] unexpectedSoundsOptions = new bool[] { true, false };
        //int[] unexpectedSoundConds = new int[] { 1, 2 };

        //Extraneous variables below will be randomly accessed in nested loop
        Color[] attendedPuckColors = new Color[]{Color.white, Color.black};
        Color[] unattendedUnexpectedTargetColors = new Color[]{Color.clear, Color.white, Color.black};  //clear is code for whatever the attended target color is (R/G)
        ActiveConditionSingleton.Thirds[] unexpectedTimes = new ActiveConditionSingleton.Thirds[] {
						ActiveConditionSingleton.Thirds.Five,
						ActiveConditionSingleton.Thirds.Ten,
						ActiveConditionSingleton.Thirds.Fifteen
		};
        
        int indexOddballPosition;
        int indexOddballEar;
        int indexUnexpectedSoundPosition;
        int indexUnexpectedSoundEar;


        Random.InitState(gameObject.GetComponent<Logger>().pid);
        conditions = new ArrayList();
        foreach (Color attendedTargetColor in attendedTargetColors)
        {   
            foreach (bool unexpectedOccurOption in unexpectedOccurOptions)
            {  
                foreach (bool oddballOccurOption in oddballOccurOptions)
                {     
                    foreach (bool unexpectedSoundsOption in unexpectedSoundsOptions)
                    {    
    //                  foreach (int unexpectedSoundCond in unexpectedSoundConds) {    //experimental
                        
                            Color aPuckColor = attendedPuckColors[GetRandomIndex(0, attendedPuckColors.Length)];
                            Color uUnexpectedTargetColor = unattendedUnexpectedTargetColors[GetRandomIndex(0, unattendedUnexpectedTargetColors.Length)];
                      
                            ActiveConditionSingleton.Thirds uTime = unexpectedTimes[GetRandomIndex(0, unexpectedTimes.Length)];

                            indexOddballEar = GetRandomIndex(-1, 2);
                            indexOddballPosition = GetRandomIndex(0, 13);

                            indexUnexpectedSoundPosition = GetRandomIndex(2, 11);
                            indexUnexpectedSoundEar = GetRandomIndex(-1, 2);

                            conditions.Add(new Condition(simulationId++, aPuckColor, attendedTargetColor, uUnexpectedTargetColor, uTime, unexpectedOccurOption, unexpectedSoundsOption, oddballOccurOption, indexOddballPosition, indexOddballEar, indexUnexpectedSoundPosition, indexUnexpectedSoundEar));
          //            }
                    }
                }
            }
        }
        Debug.Log ("Condition count: " + conditions.Count);

        //Randomize condition order
        //This call has been moved to earlier point because Random is needed Random.InitState(gameObject.GetComponent<Logger>().pid);
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
            ActiveConditionSingleton.oddballOccurs = currentCondition.oddballPresent;
            ActiveConditionSingleton.oddballPos = currentCondition.oddballPos;
            ActiveConditionSingleton.oddballEar = currentCondition.oddballLocale;
            ActiveConditionSingleton.unexpectedSoundPosition = currentCondition.unexpectedSoundPos;
            ActiveConditionSingleton.unexpectedSoundEar = currentCondition.unexpectedSoundLocale;

            AudioManager.Instance().SetOddballOccurrence(currentCondition.oddballPresent);
            if (AudioManager.Instance().GetOddballOccurrence() == 1) {
                AudioManager.Instance().SetOddballPosition(currentCondition.oddballPos);
                AudioManager.Instance().SetOddballEar(currentCondition.oddballLocale);
            } 
            AudioManager.Instance().SetUnexpectedSound(currentCondition.unexpectedSound);
            AudioManager.Instance().SetUnexpectedSoundPos(currentCondition.unexpectedSoundPos);

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

            logger.oddballPosition = currentCondition.oddballPos;
            logger.oddballEar = currentCondition.oddballLocale;
            logger.unexpectedSoundPosition = currentCondition.unexpectedSoundPos;
            logger.unexpectedSoundEar = currentCondition.unexpectedSoundLocale;

            if ( currentCondition.unexpectedPuckChgOccurs ) logger.unexpectedPuckChgOccurred = "yes";
			else logger.unexpectedPuckChgOccurred = "no";

            if ( currentCondition.unexpectedSound) logger.unexpectedSoundOccurred = "yes";
            else logger.unexpectedSoundOccurred = "no";
		}
		else {
            //Change to call the FullAttentionDriver.cs - this will be similar to PracticeDriver except the unexpected stimuli will occur
            //Then from FullAttentionDriver call FullAttentionPosttaskGUI which will also call ExperimentCompleteGUI there
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
	
    int GetRandomIndex(int start, int end) {
        return Random.Range(start, end);
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
        public bool oddballPresent;
        public int oddballPos;
        public int oddballLocale;
        public int unexpectedSoundPos;
        public int unexpectedSoundLocale;

        // public int unexpectedEar = 1;

        public Condition ( int simulationId, Color attendedPuckColor, Color attendedTargetColor, Color unattendedUnexpectedTargetColor, ActiveConditionSingleton.Thirds unexpectedTime, bool unexpectedOccurs, bool unexpectedSound, bool oddballOccurs, int oddballPosition, int oddballEar, int unexpectedSoundPosition, int unexpectedSoundEar) {
			this.simulationId = simulationId;
			this.attendedPuckColor = attendedPuckColor;
			this.attendedTargetColor = attendedTargetColor;
			this.unattendedUnexpectedTargetColor = unattendedUnexpectedTargetColor;
			this.unexpectedTime = unexpectedTime;
			this.unexpectedPuckChgOccurs = unexpectedOccurs;
            this.unexpectedSound = unexpectedSound;
            this.oddballPresent = oddballOccurs;
            this.oddballPos = oddballPosition;
            this.oddballLocale = oddballEar;
            this.unexpectedSoundPos = unexpectedSoundPosition;
            this.unexpectedSoundLocale = unexpectedSoundEar;
        }
    }
}
