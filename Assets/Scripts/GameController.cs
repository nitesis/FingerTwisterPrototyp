using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TouchScript.Behaviors;

public class GameController : MonoBehaviour
{

    public Camera CurrentCamera;
    public GameObject StartMenu;
    public GameObject CountdownCanvas;
    public CounterController PlayerCounter;
    public GameObject StartPrefab;
    public Material StartMaterial;
    public int PointCount = 4;

    public float HalfFrustumHeight {
        get {
            return FrustumHeight / 2;
        }
    }
    public float HalfFrustumWidht {
        get {
            return FrustumWidth / 2;
        }
    }

    private float FrustumHeight;
    private float FrustumWidth;
    private int PlayersReady;
    private int PlayersDestroyed;
    private GameObject[] Starts;

    void Start()
    {
        InitiateFrustums();
    }

    public void StartGame()
    {
        StartMenu.SetActive(false);
        Starts = PlayerInstantiator.InstantiateStarts(StartPrefab, StartMaterial, PlayerCounter.PlayerCount, this);
        InstantiatePoints();
    }

    public float GetRandomX()
    {
        return Random.Range(-HalfFrustumWidht, HalfFrustumWidht);
    }

    public float GetRandomZ()
    {
        return Random.Range(-HalfFrustumHeight, HalfFrustumHeight);
    }

    public Vector3 GetRandomPosition()
    {
        return new Vector3(GetRandomX(), 0, GetRandomZ());
    }

    public void PlayerReady()
    {
        PlayersReady++;
        if (PlayersReady == PlayerCounter.PlayerCount)
            Go();
    }

    public void PlayerNotReady()
    {
        PlayersReady--;
    }

    public void PlayerDestroyed()
    {
        PlayersDestroyed++;
        if (PlayersDestroyed == PlayerCounter.PlayerCount)
            Restart();
    }

    public void PlayerWon()
    {
        Handheld.Vibrate();
        Restart();
    }
    
    private void InitiateFrustums()
    {
        FrustumHeight = 2.0f * Mathf.Abs(CurrentCamera.transform.position.y) * Mathf.Tan(CurrentCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        FrustumWidth = FrustumHeight * CurrentCamera.aspect;
    }
    
    private void InstantiatePoints()
    {

		var numberOfPoints = Starts.Length * PointCount;
		var pointsX = Mathf.Ceil(Mathf.Sqrt(numberOfPoints * CurrentCamera.aspect));
		var pointsY = Mathf.Ceil(numberOfPoints / pointsX);

		var radius = Mathf.Min (FrustumWidth / pointsX, FrustumHeight / pointsY) * 0.6f;

		var freePoints = new List<Vector2> (numberOfPoints);

		var X = (int) Mathf.Ceil(pointsX / 2) + 1;
		var Y = (int)  Mathf.Ceil(pointsY / 2) + 1;
		
		var diffX = FrustumWidth / X / 2;
		var diffY = FrustumHeight / Y / 2;

		for (int i = -X; i < X; i++) {
			for (int j = -Y; j < Y; j++) {
				freePoints.Add (new Vector2 (i * diffX, j * diffY));
				GameObject.CreatePrimitive (PrimitiveType.Sphere);
			}
		}

		Debug.Log (freePoints.Count);

        foreach (var start in Starts)
			start.GetComponent<StartController>().InstantiatePoints(PointCount, freePoints, radius);
    }

    private void Go()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        var countdownText = CountdownCanvas.GetComponent<CountdownController>().CountdownText;
        countdownText.text = "3";
        Debug.Log("go");
        CountdownCanvas.SetActive(true);
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "Go";
        Handheld.Vibrate();
        foreach (GameObject start in Starts)
            start.GetComponent<StartController>().Go();
        yield return new WaitForSeconds(1);
        CountdownCanvas.SetActive(false);
    }

    private void Restart()
    {
        foreach (GameObject start in Starts)
            if (start != null)
                start.GetComponent<Transformer>().enabled = false;
        StartCoroutine(Rest());
    }

    private IEnumerator Rest()
    {
        yield return new WaitForSeconds(3);
        Application.LoadLevel(Application.loadedLevel);
    }

}
