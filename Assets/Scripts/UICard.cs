using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {
    public class UICard : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        
        [Title("Anim")]
        [SerializeField] private float startAnimDuration;
        [SerializeField] private float startScale;

        [SerializeField] private float playAnimDuration;
        [SerializeField] private float playOffsetY;

        [Title("UI")]
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
        private Sequence playAnimSeq;
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

        public void DoStartAnim(Vector3 startPos) {
            startAnimSeq.Kill();
            startAnimSeq = DOTween.Sequence();

            canvasGroup.Toggle(true);
            startAnimSeq.Join(layoutElement.DOPreferredSize(size, startAnimDuration).SetEase(Ease.OutQuad));
            content.position = startPos;

            startAnimSeq.Join(content.DOLocalMove(Vector3.zero, startAnimDuration).SetEase(Ease.OutQuad));
            startAnimSeq.Join(
                content.DOScale(Vector3.one, startAnimDuration).SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    button.interactable = true;
                }));
        }

        public void DoPlayAnim(Action onCompleted) {
            playAnimSeq.Kill();
            playAnimSeq = DOTween.Sequence();

            canvasGroup.interactable = false;

            playAnimSeq.Join(canvasGroup.DOFade(0f, playAnimDuration).SetEase(Ease.OutQuad));
            playAnimSeq.Join(content.DOLocalMoveY(playOffsetY, playAnimDuration).SetEase(Ease.OutQuad));
            playAnimSeq.Append(
                layoutElement.DOPreferredSize(new(0f, size.y), playAnimDuration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => onCompleted?.Invoke()));
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
