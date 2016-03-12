using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CounterController : MonoBehaviour
{

    private Text CounterText;

    public Slider PlayerSlider;

    public int PlayerCount {
        get {
            return (int)PlayerSlider.value;
        }
    }

    void Start()
    {
        PlayerSlider.onValueChanged.AddListener(delegate {
            UpdateText();
        });
        CounterText = GetComponent<Text>();
        UpdateText();
    }

    public void UpdateText()
    {
        CounterText.text = PlayerSlider.value.ToString();
    }

}
