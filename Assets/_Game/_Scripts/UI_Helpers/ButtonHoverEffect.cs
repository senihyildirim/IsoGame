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

        // Eðer buton ilk buton ise baþlangýçta seçili yap
        if (transform.GetSiblingIndex() == 0)
        {
            SelectButton();
        }
        else
        {
            SetImageAlpha(normalAlpha, originalSprite);
        }
    }

    // Button'ý seçili hale getiren fonksiyon
    private void SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        SetImageAlpha(targetAlpha, changeSprite);
        currentSelectedButton = this;
    }

    // Mouse ile buton üzerine geldiðinde
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        // Eðer baþka bir buton seçiliyse onun alpha deðerini sýfýrla
        if (currentSelectedButton != null && currentSelectedButton != this)
        {
            currentSelectedButton.SetImageAlpha(normalAlpha, currentSelectedButton.originalSprite);
        }

        // Mouse ile üzerine gelinen butonu seçili yap
        SetImageAlpha(targetAlpha, changeSprite);
        currentSelectedButton = this;
    }

    // Mouse ile buton üzerinden çýktýðýnda
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;

        // Eðer buton seçilmemiþse alpha deðerini sýfýrla
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            SetImageAlpha(normalAlpha, originalSprite);
        }
    }

    // Gamepad veya klavye ile buton seçildiðinde
    public void OnSelect(BaseEventData eventData)
    {
        // Eðer baþka bir buton seçiliyse onun alpha deðerini sýfýrla
        if (currentSelectedButton != null && currentSelectedButton != this)
        {
            currentSelectedButton.SetImageAlpha(normalAlpha, currentSelectedButton.originalSprite);
        }

        // Klavye veya gamepad ile butonu seçili yap
        SetImageAlpha(targetAlpha, changeSprite);
        currentSelectedButton = this;
    }

    // Gamepad veya klavye ile buton seçimi iptal edildiðinde
    public void OnDeselect(BaseEventData eventData)
    {
        if (!isHovered)
        {
            SetImageAlpha(normalAlpha, originalSprite);
        }
    }

    // Alfa deðerini ayarlayan fonksiyon
    private void SetImageAlpha(float alphaValue, Sprite sprite)
    {
        Color color = targetImage.color;
        color.a = alphaValue;
        targetImage.color = color;

        IconImage.sprite = sprite;
    }
}
