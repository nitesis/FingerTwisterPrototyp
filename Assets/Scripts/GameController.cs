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

    private Color[] Colors = { Color.red, Color.blue, Color.green, Color.yellow };
    private float FrustumHeight;
    private float FrustumWidth;
    private float HalfFrustumHeight;
    private float HalfFrustumWidht;

    void Start()
    {
        FrustumHeight = 2.0f * Mathf.Abs(CurrentCamera.transform.position.y) * Mathf.Tan(CurrentCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        FrustumWidth = FrustumHeight * CurrentCamera.aspect;
        HalfFrustumHeight = FrustumHeight / 2;
        HalfFrustumWidht = FrustumWidth / 2;
    }

    public void StartGame()
    {
        StartMenu.enabled = false;
        InstantiatePlayers(PlayerCounter.PlayerCount);
    }

    private void InstantiatePlayers(int count)
    {
        for (int i = 0; i < count; i++)
            InstantiatePlayer(i);
    }

    private void InstantiatePlayer(int number)
    {
        var startObject = (GameObject)Instantiate(StartPrefab, GetPlayerPosition(number), Quaternion.identity);
        var startMaterial = Instantiate<Material>(StartMaterial);
        startMaterial.color = Colors[number];
        startObject.GetComponent<MeshRenderer>().material = startMaterial;
        StartController startController = startObject.GetComponent<StartController>();
        startController.PlayerNumber = number;
        startController.MainGameController = this;
    }

    private Vector3 GetPlayerPosition(int number)
    {
        number %= 4;
        float x = 0;
        float z = 0;
        switch (number) {
        case 0:
            x = GetRandomX();
            z = HalfFrustumHeight;
            break;
        case 1:
            x = GetRandomX();
            z = -HalfFrustumHeight;
            break;
        case 2:
            x = HalfFrustumWidht;
            z = GetRandomZ();
            break;
        case 3:
            x = -HalfFrustumWidht;
            z = GetRandomZ();
            break;
        }
        return new Vector3(x, 0, z);
    }

    public float GetRandomX()
    {
        return Random.Range(-HalfFrustumWidht, HalfFrustumWidht);
    }

    public float GetRandomZ()
    {
        return Random.Range(-HalfFrustumHeight, HalfFrustumHeight);
    }

    public void StartPressed(int number)
    {

    }

}
