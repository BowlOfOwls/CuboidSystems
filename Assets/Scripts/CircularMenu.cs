using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenu : MonoBehaviour
{
    public List<MenuButton> buttons = new List<MenuButton>();
    private Vector2 MousePosition;
    private Vector2 fromVector2M = new Vector2(0.5f, 1.0f);
    private Vector2 centerCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;

    public int menuItems;
    public int CurMenuItem;
    public int OldMenuItem;

    public event EventHandler<OnChangeSelectionEventAArgs> OnChangeSelectionEvent;
    public class OnChangeSelectionEventAArgs : EventArgs
    {
        public InteractableSO selectedInteractableSO;
    }

    private void Start()
    {
        menuItems = buttons.Count;
        foreach (MenuButton button in buttons)
        {
            button.sceneImage.color = button.NormalColor;
        }
        CurMenuItem = 0;
        OldMenuItem = 0;
    }

    private void Update()
    {
        GetCurrentMenuItem();
        if (Input.GetMouseButtonDown(0))
        {
            ButtonAction();
        }
    }

    public void GetCurrentMenuItem()
    {
        MousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

        toVector2M = new Vector2 (MousePosition.x/Screen.width, MousePosition.y/Screen.height);

        float angle = (Mathf.Atan2(fromVector2M.y - centerCircle.y, fromVector2M.x - centerCircle.x) - Mathf.Atan2(toVector2M.y - centerCircle.y, toVector2M.x - centerCircle.x)) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360;

        CurMenuItem = ((int)(angle / (360 / menuItems)));

        if(CurMenuItem != OldMenuItem)
        {
            buttons[OldMenuItem].sceneImage.color = buttons[OldMenuItem].NormalColor;
            OldMenuItem = CurMenuItem;
            buttons[CurMenuItem].sceneImage.color = buttons[OldMenuItem].HighLightedColor;
        }
    }

    public void ButtonAction()
    {
        buttons[CurMenuItem].sceneImage.color = buttons[CurMenuItem].PressedColor;
        if (CurMenuItem == buttons.Count - 1)
        {
            OnChangeSelectionEvent?.Invoke(this, new OnChangeSelectionEventAArgs{ selectedInteractableSO = null});
        }
        OnChangeSelectionEvent?.Invoke(this, new OnChangeSelectionEventAArgs { selectedInteractableSO = buttons[CurMenuItem].interactableSO });
    }
}

[System.Serializable]
public class MenuButton
{
    public string name;
    public Image sceneImage;
    public Color NormalColor = Color.white;
    public Color HighLightedColor = Color.grey;
    public Color PressedColor = Color.gray;
    public InteractableSO interactableSO;
}