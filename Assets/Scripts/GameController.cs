using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Camera CurrentCamera;
    public Canvas StartMenu;
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
    private int PlayerCount;
    private int PlayersReady;
    private GameObject[] Starts;

    void Start()
    {
        InitiateFrustums();
    }

    public void StartGame()
    {
        StartMenu.enabled = false;
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

    public void InitiateFrustums()
    {
        FrustumHeight = 2.0f * Mathf.Abs(CurrentCamera.transform.position.y) * Mathf.Tan(CurrentCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        FrustumWidth = FrustumHeight * CurrentCamera.aspect;
    }

    public void InstantiatePoints()
    {
        foreach (var start in Starts)
            start.GetComponent<StartController>().InstantiatePoints(PointCount);
    }

}
