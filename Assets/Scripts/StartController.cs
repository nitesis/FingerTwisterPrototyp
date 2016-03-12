using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class StartController : MonoBehaviour
{

    public int ReadyDelay = 1;
    public GameObject PointPrefab;

    public GameController MainGameController {
        set {
            MGameController = value;
        }
    }
    public int PlayerNumber {
        set {
            PlayerN = value;
        }
    }

    private int PlayerN;
    private GameController MGameController;
    private MetaGesture MetaGestureScript;

    void Start()
    {
        MetaGestureScript = GetComponent<MetaGesture>();

        MetaGestureScript.StateChanged += (sender, e) => Debug.Log(e.State);
        
    }

    public void InstantiatePoints(int number)
    {
        for (int i = 0; i < number; i++) {
            var point = (GameObject) Instantiate(PointPrefab, MGameController.GetRandomPosition(), Quaternion.identity);
            point.GetComponent<Light>().color = PlayerInstantiator.Colors[PlayerN];
        }
    }

}
