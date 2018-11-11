using UnityEngine;
using UnityEngine.UI;
public class ActionPlanUI : MonoBehaviour {

    public Text actionText;
    public Image actionImage;

    public void setContent(string _text, Sprite _image)
    {
        actionText.text = _text;        
        actionImage.sprite = _image;
    }
}
