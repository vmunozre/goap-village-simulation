using UnityEngine;
using UnityEngine.UI;
public class ButtonSpeedUI : MonoBehaviour {

    public float multiplier;
    public Button buttonComponent;

    void Start () {
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
        GameManager.instance.actualMuti = multiplier;
    }
}
