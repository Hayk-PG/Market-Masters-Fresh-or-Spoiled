using System.Collections;
using UnityEngine;
using TMPro;
using Pautik;

public class ItemHoverInfoDisplayManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _text;

    private PlayerInventoryItemButton _currentDisplayedItemButton;
    private IEnumerator _closeAfterDelayCoroutine;

    private string _partiallyTransparentText = "#FFFFFFC8";
    private string _whiteText = "#FFFFFF";
    private string _itemId;
    private string _itemOriginalPrice;
    private string _itemHealth;




    private void OnEnable()
    {
        GameEventHandler.OnEvent += OnGameEvent;
    }

    private void Update()
    {
        UpdateHoverInfoPositionWithOffset();
    }

    private void OnGameEvent(GameEventType gameEventType, object[] data)
    {
        DisplayInfo(gameEventType, data);
    }

    private void UpdateHoverInfoPositionWithOffset()
    {
        bool isNoItemButtonActive = _currentDisplayedItemButton == null;

        if (isNoItemButtonActive)
        {
            return;
        }

        transform.position = (Vector2)Input.mousePosition + (Vector2.up * 200);
    }

    private void DisplayInfo(GameEventType gameEventType, object[] data)
    {
        if(gameEventType != GameEventType.InventoryItemHoverInfoDisplay)
        {
            return;
        }

        AssignCurrentDisplayedItemButton(playerInventoryItemButton: (PlayerInventoryItemButton)data[0]);
        bool isNoItemButtonOrNoAssociatedItem = _currentDisplayedItemButton == null || _currentDisplayedItemButton.AssociatedItem == null;

        if (isNoItemButtonOrNoAssociatedItem)
        {
            AssignCurrentDisplayedItemButton(null);
            StartCloseAfterDelayCoroutine();
            return;
        }

        SetCanvasGroupActive(true);
        UpdateText();
    }

    private void AssignCurrentDisplayedItemButton(PlayerInventoryItemButton playerInventoryItemButton)
    {
        _currentDisplayedItemButton = playerInventoryItemButton;
    }

    private void StartCloseAfterDelayCoroutine()
    {
        bool hasCloseAfterDelayCoroutine = _closeAfterDelayCoroutine != null;

        if (hasCloseAfterDelayCoroutine)
        {
            return;
        }

        _closeAfterDelayCoroutine = CloseAfterDelay();
        StartCoroutine(_closeAfterDelayCoroutine);
    }

    private IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);

        if(_currentDisplayedItemButton == null)
        {
            SetCanvasGroupActive(false);
        }

        _closeAfterDelayCoroutine = null;
    }

    private void UpdateText()
    {
        _itemId = $"{GlobalFunctions.TextWithColorCode(_whiteText, "ID:")} {GlobalFunctions.TextWithColorCode(_partiallyTransparentText, _currentDisplayedItemButton.AssociatedItem.ID.ToString())}";
        _itemOriginalPrice = $"{GlobalFunctions.TextWithColorCode(_whiteText, "Original Price:")} {GlobalFunctions.TextWithColorCode(_partiallyTransparentText, "$" + _currentDisplayedItemButton.AssociatedItem.Price.ToString())}";
        _itemHealth = $"{GlobalFunctions.TextWithColorCode(_whiteText, "Health:")} {GlobalFunctions.TextWithColorCode(_partiallyTransparentText, (100 - _currentDisplayedItemButton.ItemSpoilPercentage).ToString() + "%")}";
        _text.text = $"{_itemId}\n{_itemOriginalPrice}\n{_itemHealth}";
    }

    private void SetCanvasGroupActive(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}