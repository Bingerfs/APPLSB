// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    /// <summary>
    /// An example script that delegates keyboard API access either to the WMR workaround
    /// (MixedRealityKeyboard) or Unity's TouchScreenKeyboard API depending on the platform.
    /// </summary>
    /// <remarks>
    /// <para>Note that like Unity's TouchScreenKeyboard API, this script only supports WSA, iOS, and Android.</para>
    /// <para>If using Unity 2019 or 2020, make sure the version >= 2019.4.25 or 2020.3.2 to ensure the latest fixes for Unity keyboard bugs are present.</para>
    /// </remarks>
    [AddComponentMenu("Scripts/MRTK/Examples/SystemKeyboardExample")]
    public class SystemKeyboardExample : MonoBehaviour
    {

        private MixedRealityKeyboard wmrKeyboard;


        [SerializeField]
        private TextMeshPro debugMessage = null;

#pragma warning disable 0414
        [SerializeField]
        private MixedRealityKeyboardPreview mixedRealityKeyboardPreview = null;
        [SerializeField, Tooltip("Whether disable user's interaction with other UI elements while typing. Use this option to decrease the chance of keyboard getting accidentally closed.")]
        private bool disableUIInteractionWhenTyping = false;
#pragma warning restore 0414

        /// <summary>
        /// Opens a platform specific keyboard.
        /// </summary>
        public void OpenSystemKeyboard()
        {
            wmrKeyboard.ShowKeyboard(wmrKeyboard.Text, false);

        }

        #region MonoBehaviour Implementation

        private void Start()
        {
            // Initially hide the preview.
            if (mixedRealityKeyboardPreview != null)
            {
                mixedRealityKeyboardPreview.gameObject.SetActive(false);
            }


            // Windows mixed reality keyboard initialization goes here
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

        }

        private void Update()
        {

            // Windows mixed reality keyboard update goes here
            if (wmrKeyboard.Visible)
            {
                if (debugMessage != null)
                {
                    debugMessage.text = "Typing: " + wmrKeyboard.Text;
                }

                if (mixedRealityKeyboardPreview != null)
                {
                    mixedRealityKeyboardPreview.Text = wmrKeyboard.Text;
                    mixedRealityKeyboardPreview.CaretIndex = wmrKeyboard.CaretIndex;
                }
            }
            else
            {
                var keyboardText = wmrKeyboard.Text;

                if (string.IsNullOrEmpty(keyboardText))
                {
                    if (debugMessage != null)
                    {
                        debugMessage.text = "Open keyboard to type text.";
                    }
                }
                else
                {
                    if (debugMessage != null)
                    {
                        debugMessage.text = "Typed: " + keyboardText;
                    }
                }

                if (mixedRealityKeyboardPreview != null)
                {
                    mixedRealityKeyboardPreview.Text = string.Empty;
                    mixedRealityKeyboardPreview.CaretIndex = 0;
                }
            }

        }

        #endregion MonoBehaviour Implementation
    }
}
