using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class StartController : MonoBehaviour
{

    public int PlayerNumber;
    public GameController MainGameController;

    private LongPressGesture LongPressGestureScript;

    void Start()
    {
        LongPressGestureScript = GetComponent<LongPressGesture>();
        LongPressGestureScript.LongPressed += delegate {
            Debug.Log("pressed");
        };

        LongPressGestureScript.Cancelled += delegate {
            Debug.Log("cancel");
        };

        LongPressGestureScript.StateChanged += delegate {
            Debug.Log("changed");
        };
    }

}
