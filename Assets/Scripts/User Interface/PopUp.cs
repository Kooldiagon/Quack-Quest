using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] private Button accept, decline;
    [SerializeField] private UIText message;
    [SerializeField] private RectTransform overlay, container;

    private Action acceptAction, declineAction;

    private void Awake()
    {
        overlay.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    public void SetUp(string messageText, Action _acceptAction, Action _declineAction)
    {
        acceptAction = _acceptAction;
        if (_declineAction != null)
        {
            decline.gameObject.SetActive(true);
        }
        else
        {
            decline.gameObject.SetActive(false);
        }
        declineAction = _declineAction;
        message.SetText(messageText);
    }

    public void Accept()
    {
        if (acceptAction != null)
        {
            acceptAction.Invoke();
        }
        gameObject.SetActive(false);
    }

    public void Decline()
    {
        if (declineAction != null)
        {
            declineAction.Invoke();
        }
        gameObject.SetActive(false);
    }
}
