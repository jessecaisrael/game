using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class ExperimentDriver : MonoBehaviour {

	private ArrayList conditions;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);

        //Generate 32 unique simulations from 72 existing ones to correspond with 2 X 2 X 2 X 2 X 2 design
        List<int> simulationList = GenerateRandom(32, 2, 72);

        //Debug.Log("List of simulation values return: \n");

        //foreach (int item in simulationList)
          //  Debug.Log(item + "\n");

        //List all possible conditions
        //int simulationId = 2;  //id 1 is for practice.  Condition permutation needs to always occur with the same simulation conditions
        int simulationListIndex = 0;

        //Experimental variables below
        bool[] unexpectedOccurOptions = new bool[] { true, false };  //v4: Add back unexpected events.
        bool[] oddballOccurOptions = new bool[] { true, false };
        bool[] unexpectedSoundsOptions = new bool[] { true, false };
        int[] unexpectedSoundConds = new int[] { 1, 2 };

        //Extraneous variables below will be randomly accessed in nested loop
        Color[] attendedTargetColors = new Color[] { Color.red, Color.green };
        Color[] attendedPuckColors = new Color[] {Color.white, Color.black};
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

        int[] earSettings = new int[] { -1, 1 }; //-1 is for left ear, 1 is for right ear

        Random.InitState(gameObject.GetComponent<Logger>().pid);

        conditions = new ArrayList();
        for (int j=0; j<2; j++) {    
            foreach (bool unexpectedOccurOption in unexpectedOccurOptions) {  
                foreach (bool oddballOccurOption in oddballOccurOptions) {     
                    foreach (bool unexpectedSoundsOption in unexpectedSoundsOptions) {    
                        foreach (int unexpectedSoundCond in unexpectedSoundConds) {

                            Color aTargetColor = attendedTargetColors[Random.Range(0, attendedTargetColors.Length)];
                            Color aPuckColor = attendedPuckColors[Random.Range(0, attendedPuckColors.Length)];
                            Color uUnexpectedTargetColor = unattendedUnexpectedTargetColors[Random.Range(0, unattendedUnexpectedTargetColors.Length)];
                      
                            ActiveConditionSingleton.Thirds uTime = unexpectedTimes[Random.Range(0, unexpectedTimes.Length)];

                            indexOddballEar = earSettings[Random.Range(0, earSettings.Length)];
                            indexOddballPosition = Random.Range(0, 14); 

                            indexUnexpectedSoundPosition = Random.Range(1, 9);
                            indexUnexpectedSoundEar = earSettings[Random.Range(0, earSettings.Length)];

                            conditions.Add(new Condition(simulationList[simulationListIndex++], aPuckColor, aTargetColor, uUnexpectedTargetColor, uTime, unexpectedOccurOption, unexpectedSoundsOption, oddballOccurOption, indexOddballPosition, indexOddballEar, indexUnexpectedSoundPosition, indexUnexpectedSoundEar, unexpectedSoundCond));
                        }
                    }
                }
            }
        }
        
        //Randomize condition order
        //This call has been moved to earlier point because Random is needed Random.InitState(gameObject.GetComponent<Logger>().pid);
		/*for (int i=0; i < conditions.Count; i++) {
			Condition c = (Condition)conditions[i];
			int randomIndex = Random.Range(i, conditions.Count);
			conditions[i] = conditions[randomIndex];
			conditions[randomIndex] = c;
		}*/

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
            ActiveConditionSingleton.unexpectedSoundOption = currentCondition.unexpectedSoundOption;

            SceneManager.LoadScene("Trial");
            //Application.LoadLevel ("Trial");

			//Set Condition IVs to log
			Logger logger = (Logger)gameObject.GetComponent<Logger>();
			logger.scenario = currentCondition.simulationId;
			logger.attendedPuckColor = GetColorString(currentCondition.attendedPuckColor);
			logger.attendedTargetColor = GetColorString(currentCondition.attendedTargetColor);
			logger.unattendedUnexpectedTargetColor = GetColorString(currentCondition.unattendedUnexpectedTargetColor);
			logger.unexpectedTime = (int)currentCondition.unexpectedTime;

            if (currentCondition.oddballPresent) logger.oddballOccurred = "yes";
            else logger.oddballOccurred = "no";

            logger.oddballPosition = currentCondition.oddballPos;
            logger.oddballEar = currentCondition.oddballLocale;
            AudioManager.Instance().SetOddballOccurrence(currentCondition.oddballPresent);
            AudioManager.Instance().SetOddballPosition(currentCondition.oddballPos);
            AudioManager.Instance().SetOddballEar(currentCondition.oddballLocale);
    
            if ( currentCondition.unexpectedPuckChgOccurs ) logger.unexpectedPuckChgOccurred = "yes";
            else logger.unexpectedPuckChgOccurred = "no";

            if (currentCondition.unexpectedSound) logger.unexpectedSoundOccurred = "yes";
            else logger.unexpectedSoundOccurred = "no";

            logger.unexpectedSoundPosition = currentCondition.unexpectedSoundPos;
            logger.unexpectedSoundEar = currentCondition.unexpectedSoundLocale;
            logger.unexpectedSoundOption = currentCondition.unexpectedSoundOption;
            AudioManager.Instance().SetUnexpectedSound(currentCondition.unexpectedSound);
            AudioManager.Instance().SetUnexpectedSoundPos(currentCondition.unexpectedSoundPos);
            AudioManager.Instance().SetUnexpectedEar(currentCondition.unexpectedSoundLocale);
            AudioManager.Instance().SetUnexpectedSoundCondition(currentCondition.unexpectedSoundOption);

            //Debug.Log("currentCondition.oddballPos: " + currentCondition.oddballPos + " currentCondition.oddballLocale: " + currentCondition.oddballLocale + "\n");
            //Debug.Log("logger.oddballPosition: " + logger.oddballPosition + " logger.oddballEar: " + logger.oddballEar + "\n");
            //Debug.Log("AudioManager::GetOddballPos(): " + AudioManager.Instance().GetOddballPosition() + " AudioManager::GetOddballEar: " + AudioManager.Instance().GetOddballEar() + "\n\n");

            //Debug.Log("currentCondition.unexpectedSoundPos: " + currentCondition.unexpectedSoundPos + " currentCondition.unexpectedSoundLocale: " + currentCondition.unexpectedSoundLocale + "\n");
            //Debug.Log("logger.unexpectedSoundPosition: " + logger.unexpectedSoundPosition + " logger.unexpectedSoundEar: " + logger.unexpectedSoundEar + "\n");
            //Debug.Log("AudioManager::GetUnexpectedSoundPos(): " + AudioManager.Instance().GetUnexpectedSoundPos() + " AudioManager::GetUnexpectedSoundEar(): " + AudioManager.Instance().GetUnexpectedSoundEar() + "\n\n"); */
        }
        else {
            //Move to the full attention trial after the main experiment block completes
            ActiveConditionSingleton.fullAttentionReady = true;
			gameObject.GetComponent<FullAttentionPretaskGUI>().enabled = true;
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

    private static List<int> GenerateRandom(int count, int min, int max) {
        System.Random rand = new System.Random();
        
        HashSet<int> candidates = new HashSet<int>();
        for (int top = max - count; top < max; top++) {
             if(!candidates.Add(rand.Next(min, top + 1))) { 
                 candidates.Add(top);
             }
         }

         List<int> result = candidates.ToList();

        /* for (int i = result.Count - 1; i > 0; i--) {
             int k = rand.Next(i + 1);
             int tmp = result[k];
             result[k] = result[i];
             result[i] = tmp;
         }*/
         return result;
        //for (int i = 2; i < count; i++)
          //  while (!candidates.Add(rand.Next()) && count < max);
        //return candidates;
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
        public int unexpectedSoundOption;

        public Condition ( int simulationId, Color attendedPuckColor, Color attendedTargetColor, Color unattendedUnexpectedTargetColor, ActiveConditionSingleton.Thirds unexpectedTime, bool unexpectedOccurs, bool unexpectedSound, bool oddballOccurs, int oddballPosition, int oddballEar, int unexpectedSoundPosition, int unexpectedSoundEar, int unexpectedSoundOption) {
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
            this.unexpectedSoundOption = unexpectedSoundOption;
        }
    }
}
