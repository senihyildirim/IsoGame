using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverTransparency : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Image targetImage;
    public Image IconImage;
    public Sprite originalSprite;
    public Sprite changeSprite;
    private Color originalColor;
    private float targetAlpha = 1f;
    private float normalAlpha = 0f;

    private static ButtonHoverTransparency currentSelectedButton;
    private bool isHovered = false;

    private void Start()
    {
        originalColor = targetImage.color;

        // E�er buton ilk buton ise ba�lang��ta se�ili yap
        if (transform.GetSiblingIndex() == 0)
        {
            SelectButton();
        }
        else
        {
            SetImageAlpha(normalAlpha, originalSprite);
        }
    }

    // Button'� se�ili hale getiren fonksiyon
    private void SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        SetImageAlpha(targetAlpha, changeSprite);
        currentSelectedButton = this;
    }

    // Mouse ile buton �zerine geldi�inde
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        // E�er ba�ka bir buton se�iliyse onun alpha de�erini s�f�rla
        if (currentSelectedButton != null && currentSelectedButton != this)
        {
            currentSelectedButton.SetImageAlpha(normalAlpha, currentSelectedButton.originalSprite);
        }

        // Mouse ile �zerine gelinen butonu se�ili yap
        SetImageAlpha(targetAlpha, changeSprite);
        currentSelectedButton = this;
    }

    // Mouse ile buton �zerinden ��kt���nda
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;

        // E�er buton se�ilmemi�se alpha de�erini s�f�rla
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            SetImageAlpha(normalAlpha, originalSprite);
        }
    }

    // Gamepad veya klavye ile buton se�ildi�inde
    public void OnSelect(BaseEventData eventData)
    {
        // E�er ba�ka bir buton se�iliyse onun alpha de�erini s�f�rla
        if (currentSelectedButton != null && currentSelectedButton != this)
        {
            currentSelectedButton.SetImageAlpha(normalAlpha, currentSelectedButton.originalSprite);
        }

        // Klavye veya gamepad ile butonu se�ili yap
        SetImageAlpha(targetAlpha, changeSprite);
        currentSelectedButton = this;
    }

    // Gamepad veya klavye ile buton se�imi iptal edildi�inde
    public void OnDeselect(BaseEventData eventData)
    {
        if (!isHovered)
        {
            SetImageAlpha(normalAlpha, originalSprite);
        }
    }

    // Alfa de�erini ayarlayan fonksiyon
    private void SetImageAlpha(float alphaValue, Sprite sprite)
    {
        Color color = targetImage.color;
        color.a = alphaValue;
        targetImage.color = color;

        IconImage.sprite = sprite;
    }
}
