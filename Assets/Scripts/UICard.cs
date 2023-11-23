using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace CSCI526GameJam {
    public class UICard : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private float animDuration;
        [SerializeField] private float startScale;

        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform content;

        [ComputedFields]
        [SerializeField] private Card card;

        private RectTransform rectTransform;
        private LayoutElement layoutElement;
        private CanvasGroup canvasGroup;

        private Vector2 size;

        private Sequence startAnimSeq;
        #endregion

        #region Publics
        public Card Card => card;

        public void Init(Card card, Action onClicked) {
            button.onClick.AddListener(() => onClicked?.Invoke());
            this.card = card;
            Refresh();

            layoutElement.preferredWidth = 0f;
            content.localScale = Vector3.one * startScale;
        }

        public void PlayStartAnim(Vector3 startPos) {
            startAnimSeq.Kill();
            startAnimSeq = DOTween.Sequence();

            canvasGroup.Toggle(true);
            startAnimSeq.Join(layoutElement.DOPreferredSize(size, animDuration));
            content.position = startPos;

            startAnimSeq.Join(content.DOLocalMove(Vector3.zero, animDuration).SetEase(Ease.OutQuad));
            startAnimSeq.Join(
                content.DOScale(Vector3.one, animDuration).SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    button.interactable = true;
                }));
        }

        public void Select() {
            content.GetComponent<Image>().color = Color.red;

        }

        public void UnSelect() {
            content.GetComponent<Image>().color = Color.white;

        }

        public void Refresh() {
            costText.text = card.Cost.ToString();
            nameText.text = card.Name;
            descriptionText.text = card.GetDescription();
            image.sprite = card.Image;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            layoutElement = GetComponent<LayoutElement>();
            canvasGroup = GetComponent<CanvasGroup>();

            button.interactable = false;
            canvasGroup.Toggle(false);
            size = new(layoutElement.preferredWidth, layoutElement.preferredHeight);
        }

        private void OnDestroy() {
            startAnimSeq.Kill();
        }
        #endregion
    }
}
