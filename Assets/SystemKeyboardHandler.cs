// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Assets.Util;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SystemKeyboardHandler : MonoBehaviour
{
    private MixedRealityKeyboard wmrKeyboard;

    private UserType _userType = UserType.NEW_USER;

    [SerializeField]
    private ToolTip _promptText;

#pragma warning disable 0414
    [SerializeField]
    private MixedRealityKeyboardPreview mixedRealityKeyboardPreview = null;
    private bool disableUIInteractionWhenTyping = false;

    [SerializeField]
    private InitialDataHandler _initialDataHandler;

#pragma warning restore 0414

    public void OpenSystemKeyboard(int userType)
    {
        _userType = (UserType)userType;
        _promptText.ToolTipText = _userType == UserType.NEW_USER ? "Ingresar nombre de usuario" : "Ingresar codigo de usuario";
        wmrKeyboard.ShowKeyboard(wmrKeyboard.Text, false);
    }

    void Start()
    {
        if (mixedRealityKeyboardPreview != null)
        {
            mixedRealityKeyboardPreview.gameObject.SetActive(false);
        }

        wmrKeyboard = gameObject.AddComponent<MixedRealityKeyboard>();
        wmrKeyboard.DisableUIInteractionWhenTyping = disableUIInteractionWhenTyping;
        if (wmrKeyboard.OnShowKeyboard != null)
        {
            wmrKeyboard.OnShowKeyboard.AddListener(() =>
            {
                if (mixedRealityKeyboardPreview != null)
                {
                    mixedRealityKeyboardPreview.gameObject.SetActive(true);
                }
            });
        }

        if (wmrKeyboard.OnHideKeyboard != null)
        {
            wmrKeyboard.OnHideKeyboard.AddListener(() =>
            {
                if (mixedRealityKeyboardPreview != null)
                {
                    mixedRealityKeyboardPreview.gameObject.SetActive(false);
                }
            });
        }

        if (wmrKeyboard.OnCommitText != null)
        {
            wmrKeyboard.OnCommitText.AddListener(() =>
            {
                OnCommitTextByUserType(wmrKeyboard.Text);
            });
            wmrKeyboard.OnCommitText.AddListener(() =>
            {
                wmrKeyboard.HideKeyboard();
            });
        }
    }

    void Update()
    {
        if (wmrKeyboard.Visible)
        {
            if (mixedRealityKeyboardPreview != null)
            {
                mixedRealityKeyboardPreview.Text = wmrKeyboard.Text;
                mixedRealityKeyboardPreview.CaretIndex = wmrKeyboard.CaretIndex;
            }
        }
        else
        {
            if (mixedRealityKeyboardPreview != null)
            {
                mixedRealityKeyboardPreview.Text = string.Empty;
                mixedRealityKeyboardPreview.CaretIndex = 0;
            }
        }
    }

    private void OnCommitTextByUserType(string text)
    {
        switch (_userType)
        {
            case UserType.NEW_USER:
                _initialDataHandler.OnNewUserSelected(text);
                break;
            case UserType.RETURNING_USER:
                _initialDataHandler.OnReturningUserSelected(text);
                break;
            default:
                break;
        }
    }
}
