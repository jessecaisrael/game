using UnityEngine;
using System.Collections;

public static class ActiveConditionSingleton {

    public enum Thirds { Five = 5, Ten = 10, Fifteen = 15 };

    public static Color attendedPuckColor;
    public static Texture2D whiteTargetTexture;
    public static Texture2D whiteChangeTexture;
    public static Texture2D blackTargetTexture;
    public static Texture2D blackChangeTexture;
    public static Thirds unexpectedSegment;
    public static bool unexpectedPuckChgOccurs;
    public static int simulationId;
    public static Texture2D whiteBasicTexture; //= (Texture2D)Resources.Load("WhiteBGBlackBorder");
    public static Texture2D blackBasicTexture; //= (Texture2D)Resources.Load("BlackBG");

    public static bool complete;

    //added for auditory conditions
    public static bool oddballOccurs;
    public static int oddballPos;
    public static int oddballEar;

    public static bool unexpectedSoundOccurs;
    public static int unexpectedSoundPosition;
    public static int unexpectedSoundEar;
    public static int unexpectedSoundOption;       
}
