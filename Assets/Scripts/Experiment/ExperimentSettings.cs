using UnityEngine;
using System.Collections;

public static class ExperimentSettings {

	public enum ResponseType {Recall=1, Spacebar=2};

	public static ResponseType responseType = ResponseType.Spacebar;

}