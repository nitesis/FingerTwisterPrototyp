using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class PointController : MonoBehaviour
{

    private MetaGesture MetaGestureScript;

    void Start()
    {
        MetaGestureScript = GetComponent<MetaGesture>();
        MetaGestureScript.StateChanged += (sender, e) => Debug.Log(e.State);
    }
	
}
