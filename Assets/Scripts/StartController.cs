using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Behaviors;

public class StartController : MonoBehaviour
{

    public GameObject PointPrefab;
    public GameObject ParticleSys;

    public string POINT_TAG = "Point";

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
    private LongPressGesture LongPressGestureScript;
    private bool Ready;
    private int PointsCounter;
    private int PointsCollected;

    void Start()
    {
        MetaGestureScript = GetComponent<MetaGesture>();
        LongPressGestureScript = GetComponent<LongPressGesture>();
        LongPressGestureScript.LongPressed += delegate {
            Ready = true;
            ParticleSys.SetActive(true);
            MGameController.PlayerReady();
        };
        MetaGestureScript.TouchEnded += delegate {
            if (!Ready)
                return;
            Ready = false;
            ParticleSys.SetActive(false);
            MGameController.PlayerNotReady();
        };
    }

    public void InstantiatePoints(int number)
    {
        PointsCounter = number;
        for (int i = 0; i < number; i++) {
            var point = (GameObject) Instantiate(PointPrefab, MGameController.GetRandomPosition(), Quaternion.identity);
            point.GetComponent<Light>().color = PlayerInstantiator.Colors[PlayerN];
            point.GetComponent<PointController>().Tag = POINT_TAG + PlayerN;
        }
    }

    public void Go()
    {
        GetComponent<Transformer>().enabled = true;
        ParticleSys.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        var pointController = other.gameObject.GetComponent<PointController>();
        if (pointController == null)
            return;
        if (pointController.Tag.Equals(POINT_TAG + PlayerN)) {
            Destroy(other.gameObject);
            PointsCollected++;
            if (PointsCollected == PointsCounter) {
                MGameController.PlayerWon();
                Ready = false;
                ParticleSys.SetActive(true);
            }
        } else {
            Destroy(gameObject);
            MGameController.PlayerDestroyed();
        }
    }

}
