using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VolumeToggler : MonoBehaviour {
    [Header("Toggle Attributes")]
    [SerializeField] RectTransform uiHandleRectTransform;
    [SerializeField] Color backgroundActiveColor;
    [SerializeField] Color handleActiveColor;
    [Header("Mute Audio")]
    private bool isMuted;
    [SerializeField] AudioSource BGM;
    Image backgroundImage, handleImage;

    Color backgroundDefaultColor, handleDefaultColor;

    Toggle toggle;

    Vector2 handlePosition;

    void Awake() {
        toggle = GetComponent<Toggle>();

        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn) {
            OnSwitch(true);
            isMuted = false;
            BGM.mute = isMuted;
        }
    }

    public void ExitClicked() {
        BGM.mute = true;
    }

    public void NoClicked() {
        BGM.mute = isMuted;
    }

    void OnSwitch(bool on) {
        //uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition ; // no anim
        uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);

        //backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor ; // no anim
        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, .6f);

        //handleImage.color = on ? handleActiveColor : handleDefaultColor ; // no anim
        handleImage.DOColor(on ? handleActiveColor : handleDefaultColor, .4f);

        isMuted = !isMuted;
        BGM.mute = isMuted;
    }

    void OnDestroy() {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}

