using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class NewObjectsAtOrigin : MonoBehaviour {
	
	[MenuItem("My Menu/Move to Origin %#o")]
	static void moveToOrigin()
	{
		Selection.activeTransform.transform.position = Vector3.zero;
		Selection.activeTransform.transform.rotation = Quaternion.identity;
	}
	[MenuItem("My Menu/Move to Origin %#o", true)]
	static bool ValidateSelection()
	{
		return Selection.activeTransform != null;
	}
	
	[MenuItem("My Menu/Create Game Object At Origin %#0")]
	static void createAtZero()
	{
		GameObject go = new GameObject("GameObject");
		go.transform.position = Vector3.zero;
		go.transform.rotation = Quaternion.identity;
	}
	 
	 
	[MenuItem("My Menu/Create Empty Parent #&e")]
	static void createEmptyParent()
	{
		GameObject go = new GameObject("GameObject");
		if (Selection.activeTransform != null)
		{
			go.transform.parent = Selection.activeTransform.parent;
			go.transform.Translate(Selection.activeTransform.position);
			Selection.activeTransform.parent = go.transform;
		}
	}
	 
	[MenuItem("My Menu/Create Empty Duplicate #&d")]
	static void createEmptyDuplicate()
	{
		GameObject go = new GameObject("GameObject");
		if (Selection.activeTransform != null)
		{
			go.transform.parent = Selection.activeTransform.parent;
			go.transform.Translate(Selection.activeTransform.position);
		}
	}
	 
	[MenuItem("My Menu/Create Empty Child #&n")]
	static void createEmptyChild()
	{
		GameObject go = new GameObject("GameObject");
		if (Selection.activeTransform != null)
		{
			go.transform.parent = Selection.activeTransform;
			go.transform.Translate(Selection.activeTransform.position);
		}
	}
	
}