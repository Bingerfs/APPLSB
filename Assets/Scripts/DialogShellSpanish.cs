using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class DialogShellSpanish : Dialog
    {
        private GameObject[] twoButtonSet;

        [SerializeField]
        [Tooltip("Title text of the dialog")]
        private TextMeshPro titleText = null;

        /// <summary>
        /// Title text of the dialog
        /// </summary>
        public TextMeshPro TitleText
        {
            get { return titleText; }
            set { titleText = value; }
        }

        [SerializeField]
        [Tooltip("Description text of the dialog")]
        private TextMeshPro descriptionText = null;

        /// <summary>
        /// Description text of the dialog
        /// </summary>
        public TextMeshPro DescriptionText
        {
            get { return descriptionText; }
            set { descriptionText = value; }
        }

        /// <inheritdoc />
        protected override void FinalizeLayout() { }

        /// <inheritdoc />
        protected override void GenerateButtons()
        {
            // Get List of ButtonTypes that should be created on Dialog
            List<DialogButtonType> buttonTypes = new List<DialogButtonType>();
            foreach (DialogButtonType buttonType in Enum.GetValues(typeof(DialogButtonType)))
            {
                // If this button type flag is set
                if (buttonType != DialogButtonType.None && result.Buttons.IsMaskSet(buttonType))
                {
                    buttonTypes.Add(buttonType);
                }
            }

            twoButtonSet = new GameObject[2];

            // Find all buttons on dialog...
            List<DialogButton> buttonsOnDialog = GetAllDialogButtons();

            // Set desired buttons active and the rest inactive
            SetButtonsActiveStates(buttonsOnDialog, buttonTypes.Count);

            // Set titles and types
            if (buttonTypes.Count > 0)
            {
                // If we have two buttons then do step 1, else 0
                int step = buttonTypes.Count >= 2 ? 1 : 0;
                for (int i = 0; i < buttonTypes.Count && i < 2; ++i)
                {
                    twoButtonSet[i] = buttonsOnDialog[i + step].gameObject;
                    buttonsOnDialog[i + step].SetTitle(TranslateButtonTypeNames(buttonTypes[i]));
                    buttonsOnDialog[i + step].ButtonTypeEnum = buttonTypes[i];
                }
            }
        }

        private string TranslateButtonTypeNames(DialogButtonType type)
        {
            string name = "";
            switch (type)
            {
                case DialogButtonType.Close:
                    name = "Cerrar";
                    break;
                case DialogButtonType.Confirm:
                    name = "Confirmar";
                    break;
                case DialogButtonType.Cancel:
                    name = "Cancelar";
                    break;
                case DialogButtonType.Accept:
                    name = "Aceptar";
                    break;
                case DialogButtonType.Yes:
                    name = "Sí";
                    break;
                case DialogButtonType.No:
                    name = "No";
                    break;
                case DialogButtonType.OK:
                    name = "Ok";
                    break;
                default:
                    break;
            }

            return name;
        }

        private void SetButtonsActiveStates(List<DialogButton> buttons, int count)
        {
            for (int i = 0; i < buttons.Count; ++i)
            {
                var flag1 = (count == 1) && (i == 0);
                var flag2 = (count >= 2) && (i > 0);
                buttons[i].ParentDialog = this;
                buttons[i].gameObject.SetActive(flag1 || flag2);
            }
        }

        private List<DialogButton> GetAllDialogButtons()
        {
            List<DialogButton> buttonsOnDialog = new List<DialogButton>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.name == "ButtonParent")
                {
                    var buttons = child.GetComponentsInChildren<DialogButton>();
                    if (buttons != null)
                    {
                        buttonsOnDialog.AddRange(buttons);
                    }
                }
            }
            return buttonsOnDialog;
        }

        /// <summary>
        /// Set Title and Text on the Dialog.
        /// </summary>
        protected override void SetTitleAndMessage()
        {
            if (titleText != null)
            {
                titleText.text = Result.Title;
            }

            if (descriptionText != null)
            {
                descriptionText.text = Result.Message;
            }
        }

        /// <summary>
        /// Function to destroy the Dialog.
        /// </summary>
        public override void DismissDialog()
        {
            State = DialogState.InputReceived;
        }
    }
}
