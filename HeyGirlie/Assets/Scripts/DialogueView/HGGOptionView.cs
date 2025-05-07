/*
Yarn Spinner is licensed to you under the terms found in the file LICENSE.md.
*/

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;


namespace Yarn.Unity
{
    public class HGGOptionView : UnityEngine.UI.Selectable, ISubmitHandler, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] public TextMeshProUGUI text;
        [SerializeField] bool showCharacterName = false;
        [SerializeField] GameObject selectionIcon;
        [SerializeField] GameObject hoverIcon;
        [SerializeField] Color hoverColor;
        [SerializeField] Color originalColor;

        public Action<DialogueOption> OnOptionSelected;
        public MarkupPalette palette;
        public KeyCode key;
        public KeyCode keyAlt;
        public int order;
        public EventTrigger soundTrigger;

        DialogueOption _option;

        bool hasSubmittedOptionSelection = false;

        public DialogueOption Option
        {
            get => _option;

            set
            {
                _option = value;

                hasSubmittedOptionSelection = false;

                // When we're given an Option, use its text and update our
                // interactibility.
                Markup.MarkupParseResult line;
                SettingManager.Instance.UpdateOptionView();
                if (showCharacterName)
                {
                    line = value.Line.Text;
                }
                else
                {
                    line = value.Line.TextWithoutCharacterName;
                }
                line.Text = order + ". " + line.Text;

                if (palette != null)
                {
                    text.text = HGGLineView.PaletteMarkedUpText(line, palette, false);
                }
                else
                {
                    text.text = line.Text;
                }

                interactable = value.IsAvailable;
            }
        }
        
        void Update()
        {
            //SettingManager.Instance.UpdateOptionView();
            if ((Input.GetKeyUp(key) || Input.GetKeyUp(keyAlt)) && !GameManager.Instance.pauseLock)
            {
                InvokeOptionSelected();
            }
        }

        // If we receive a submit or click event, invoke our "we just selected
        // this option" handler.
        public void OnSubmit(BaseEventData eventData)
        {
            InvokeOptionSelected();
            TextNormal();
        }

        public void InvokeOptionSelected()
        {
            // turns out that Selectable subclasses aren't intrinsically interactive/non-interactive
            // based on their canvasgroup, you still need to check at the moment of interaction
            if (!IsInteractable())
            {
                return;
            }
            
            // We only want to invoke this once, because it's an error to
            // submit an option when the Dialogue Runner isn't expecting it. To
            // prevent this, we'll only invoke this if the flag hasn't been cleared already.
            if (hasSubmittedOptionSelection == false)
            {
                OnOptionSelected.Invoke(Option);
                hasSubmittedOptionSelection = true;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            InvokeOptionSelected();
        }

        public override void OnSelect(BaseEventData eventData){
            TextHover();
        }

        public override void OnDeselect(BaseEventData eventData){
            TextNormal();
        }

        // If we mouse-over, we're telling the UI system that this element is
        // the currently 'selected' (i.e. focused) element. 
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.Select();
        }

        private void TextHover(){
            // disable selection indicator
            selectionIcon.SetActive(false);
            // enable hover indicator
            hoverIcon.SetActive(true);
            text.color = hoverColor;
        }

        private void TextNormal(){
            // disable selection indicator
            selectionIcon.SetActive(true);
            // enable hover indicator
            hoverIcon.SetActive(false);
            text.color = originalColor;

        }

    }
}
