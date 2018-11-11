using UnityEngine;
using UnityEngine.UI;
public class ButtonDeselectUI : MonoBehaviour
{
    public Button buttonComponent;
    void Start()
    {
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
        GameManager.instance.deselectAgent();
    }
}
