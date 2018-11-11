using UnityEngine;
using UnityEngine.UI;
public class ButtonMoveCameraUI : MonoBehaviour
{
    public Button buttonComponent;
    MovementCamera movCam;
    void Start()
    {
        buttonComponent.onClick.AddListener(HandleClick);
        movCam = Camera.main.gameObject.GetComponent<MovementCamera>();
    }

    public void HandleClick()
    {
        movCam.activeMovement = !movCam.activeMovement;
    }
}
