/*
Yarn Spinner is licensed to you under the terms found in the file LICENSE.md.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


namespace Yarn.Unity
{
    public class HGGOptionsListView : DialogueViewBase
    {
        [SerializeField] CanvasGroup canvasGroup;

        [SerializeField] Canvas gridLayout;
        [SerializeField] Canvas verticalLayout;

        [SerializeField] Scrollbar scrollbar;

        [SerializeField] HGGOptionView optionViewPrefab;

        [SerializeField] MarkupPalette palette;

        [SerializeField] float fadeTime = 0.1f;

        [SerializeField] bool showUnavailableOptions = false;

        [Header("Last Line Components")]
        [SerializeField] public TextMeshProUGUI lastLineText;
        [SerializeField] GameObject lastLineContainer;

        [SerializeField] TextMeshProUGUI lastLineCharacterNameText;
        [SerializeField] GameObject lastLineCharacterNameContainer;

        [SerializeField] GameObject dialogueBubblePrefab;
        Color purple = new Color(0.4313726f, 0.2f, 0.6470588f, 1f), white = Color.white;

        // A cached pool of OptionView objects so that we can reuse them
        List<HGGOptionView> optionViewsVertical = new List<HGGOptionView>();
        List<HGGOptionView> optionViewsGrid = new List<HGGOptionView>();

        // The method we should call when an option has been selected.
        Action<int> OnOptionSelected;

        // The line we saw most recently.
        LocalizedLine lastSeenLine;

        public void Awake()
        {
            SettingManager.Instance.SetOptionsListView(this, this.optionViewsVertical);
            SettingManager.Instance.UpdateOptionView();
        }

        public void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            //SettingManager.Instance.SetOptionsListView(this, this.optionViewsVertical);
            //SettingManager.Instance.UpdateOptionView();
        }

        public void Update(){
            if(!GameManager.Instance.pauseLock){
                float value = scrollbar.value + (Input.GetAxis("Mouse ScrollWheel"));
                scrollbar.value = (value <= 0f) ? 0f : ((value >= 1f) ? 1f : value);
            }
        }

        public void Reset()
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            // Don't do anything with this line except note it and
            // immediately indicate that we're finished with it. RunOptions
            // will use it to display the text of the previous line.
            lastSeenLine = dialogueLine;
            onDialogueLineFinished();
        }
        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            // scrollWheel.Select();
            // If we don't already have enough option views, create more
            bool dialogueGrid = dialogueOptions.Length > 3;
            if (dialogueGrid)
            {
                gridLayout.gameObject.SetActive(true);
                verticalLayout.gameObject.SetActive(false);

                while (dialogueOptions.Length > optionViewsGrid.Count)
                {
                    var optionView = CreateNewOptionView(gridLayout);
                    optionView.gameObject.SetActive(false);
                }

            }
            else
            {
                verticalLayout.gameObject.SetActive(true);
                gridLayout.gameObject.SetActive(false);
                while (dialogueOptions.Length > optionViewsVertical.Count)
                {
                    var optionView = CreateNewOptionView(verticalLayout);
                    optionView.gameObject.SetActive(false);
                }

            }

            // Set up all of the option views
            int optionViewsCreated = 0;

            HGGOptionView prev = null;
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;
            nav.selectOnLeft = scrollbar;

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                var optionView = dialogueGrid ? optionViewsGrid[i] : optionViewsVertical[i];
                var option = dialogueOptions[i];
                //SettingManager.Instance.SetOptionsListView(this, this.optionViewsVertical);
                //SettingManager.Instance.UpdateOptionView();

                if (option.IsAvailable == false && showUnavailableOptions == false)
                {
                    // Don't show this option.
                    continue;
                }
                optionView.gameObject.SetActive(true);
                
                optionView.key = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Alpha" + (i + 1));
                optionView.keyAlt = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Keypad" + (i + 1));
                optionView.order = i+1;

                optionView.palette = this.palette;
                optionView.Option = option;

                if(prev != null){
                    nav.selectOnDown = optionView;
                    prev.navigation = nav;
                    nav.selectOnUp = prev;
                }
                prev = optionView;

                // The first available option is selected by default
                if (optionViewsCreated == 0)
                {
                    Navigation scrollNav = scrollbar.navigation;
                    scrollNav.selectOnRight = optionView;
                    scrollbar.navigation = scrollNav;

                    optionView.Select();
                }

                optionViewsCreated += 1;
            }

            nav.selectOnDown = null;
            prev.navigation = nav;

            // Update the last line, if one is configured
            // if (lastLineContainer != null)
            // {
            //     if (lastSeenLine != null)
            //     {
            //         // if we have a last line character name container
            //         // and the last line has a character then we show the nameplate
            //         // otherwise we turn off the nameplate
            //         var line = lastSeenLine.Text;
            //         if (lastLineCharacterNameContainer != null)
            //         {
            //             if (string.IsNullOrWhiteSpace(lastSeenLine.CharacterName))
            //             {
            //                 lastLineCharacterNameContainer.SetActive(false);
            //             }
            //             else
            //             {
            //                 line = lastSeenLine.TextWithoutCharacterName;
            //                 lastLineCharacterNameContainer.SetActive(true);
            //                 lastLineCharacterNameText.text = lastSeenLine.CharacterName;
            //             }
            //         }
            //         else if (line.Text[0] == ':')
            //         {
            //             line.Text = line.Text.Substring(1);
            //         }

            //         if (palette != null)
            //         {
            //             lastLineText.text = HGGLineView.PaletteMarkedUpText(line, palette);
            //         }
            //         else
            //         {
            //             lastLineText.text = line.Text;
            //         }

            //         lastLineContainer.SetActive(true);
            //     }
            //     else
            //     {
            //         lastLineContainer.SetActive(false);
            //     }
            // }
            
            // disable last line from appearing
            lastLineContainer.SetActive(false);

            // Note the delegate to call when an option is selected
            OnOptionSelected = onOptionSelected;

            // sometimes (not always) the TMP layout in conjunction with the
            // content size fitters doesn't update the rect transform
            // until the next frame, and you get a weird pop as it resizes
            // just forcing this to happen now instead of then
            Relayout();

            // Fade it all in
            StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeTime));

            /// <summary>
            /// Creates and configures a new <see cref="OptionView"/>, and adds
            /// it to <see cref="optionViewsVertical"/>.
            /// </summary>
            HGGOptionView CreateNewOptionView(Canvas layout)
            {
                var optionView = Instantiate(optionViewPrefab);
                //SettingManager.Instance.SetOptionsListView(this, this.optionViewsVertical);
                //SettingManager.Instance.UpdateOptionView();
                optionView.transform.SetParent(layout.gameObject.transform, false);
                optionView.transform.SetAsLastSibling();

                optionView.OnOptionSelected = OptionViewWasSelected;

                if (dialogueGrid)
                {
                    optionViewsGrid.Add(optionView);
                }
                else
                {
                    optionViewsVertical.Add(optionView);
                }

                return optionView;
            }

            /// <summary>
            /// Called by <see cref="OptionView"/> objects.
            /// </summary>
            void OptionViewWasSelected(DialogueOption option)
            {
                StartCoroutine(OptionViewWasSelectedInternal(option));

                IEnumerator OptionViewWasSelectedInternal(DialogueOption selectedOption)
                {
                    CloneMessageBoxToHistory(selectedOption.Line);
                    yield return StartCoroutine(FadeAndDisableOptionViews(canvasGroup, 1, 0, fadeTime));
                    OnOptionSelected(selectedOption.DialogueOptionID);
                }
            }
        }

        public void CloneMessageBoxToHistory(LocalizedLine dialogueLine)
        {
            dialogueBubblePrefab.SetActive(true);
            var line = dialogueLine.Text.Text;
            if (line[0] == ':')
            {
                line = line.Substring(1);
            }
            dialogueBubblePrefab.transform.Find("TextBG/Text").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "SELECTED: " + dialogueLine.Text.Text;

            var oldClone = Instantiate(
                    dialogueBubblePrefab,
                    dialogueBubblePrefab.transform.position,
                    dialogueBubblePrefab.transform.rotation,
                    dialogueBubblePrefab.transform.parent
                );
            dialogueBubblePrefab.transform.SetAsLastSibling();

            dialogueBubblePrefab.SetActive(false);
            // reset message box and configure based on current settings
            //dialogueBubblePrefab.SetActive(true);
            string character = dialogueLine.CharacterName;
            var bg = oldClone.GetComponentInChildren<Image>();
            var message = oldClone.transform.Find("TextBG/Text").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            // message.text = "";
            if (String.IsNullOrEmpty(character) || character == "Kristen")
            {
                bg.color = purple;
                message.color = white;
            }
            else
            {
                bg.color = white;
                message.color = purple;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// If options are still shown dismisses them.
        /// </remarks>
        public override void DialogueComplete()
        {   
            // do we still have any options being shown?
            if (canvasGroup.alpha > 0)
            {
                StopAllCoroutines();
                lastSeenLine = null;
                OnOptionSelected = null;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

                StartCoroutine(FadeAndDisableOptionViews(canvasGroup, canvasGroup.alpha, 0, fadeTime));
            }
        }

        /// <summary>
        /// Fades canvas and then disables all option views.
        /// </summary>
        private IEnumerator FadeAndDisableOptionViews(CanvasGroup canvasGroup, float from, float to, float fadeTime)
        {
            yield return Effects.FadeAlpha(canvasGroup, from, to, fadeTime);

            // Hide all existing option views
            foreach (var optionView in optionViewsVertical)
            {
                optionView.gameObject.SetActive(false);
            }
        }

        public void OnEnable()
        {
            Relayout();
        }

        private void Relayout()
        {
            // Force re-layout
            var layouts = GetComponentsInChildren<UnityEngine.UI.LayoutGroup>();

            // Perform the first pass of re-layout. This will update the inner horizontal group's sizing, based on the text size.
            foreach (var layout in layouts)
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());
            }
            
            // Perform the second pass of re-layout. This will update the outer vertical group's positioning of the individual elements.
            foreach (var layout in layouts)
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());
            }
        }
    }
}
