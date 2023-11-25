using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace CSCI526GameJam {
    public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        #region Fields
        [MandatoryFields]
        
        [Title("Anim")]
        [SerializeField] private float startAnimDuration;
        [SerializeField] private float startScale;

        [SerializeField] private float playAnimDuration;
        [SerializeField] private float playOffsetY;

        [SerializeField] private Color selectedColor;
        [SerializeField] private Color costRegularColor;
        [SerializeField] private Color costReducedColor;
        [SerializeField] private Color[] levelColors;

        [Title("UI")]
        [SerializeField] private string onHoveredSortingLayer;
        [SerializeField] private Image costIcon;
        [SerializeField] private Image levelIcon;
        [SerializeField] private Image imageBackground;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform body;

        [ComputedFields]
        [SerializeField] private Card card;

        private RectTransform rectTransform;
        private LayoutElement layoutElement;
        private Canvas canvas;
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
            body.localScale = Vector3.one * startScale;
        }

        public void DoStartAnim(Vector3 startPos) {
            startAnimSeq.Kill();
            startAnimSeq = DOTween.Sequence();

            canvasGroup.Toggle(true);
            startAnimSeq.Join(layoutElement.DOPreferredSize(size, startAnimDuration).SetEase(Ease.OutQuad));
            body.position = startPos;

            startAnimSeq.Join(body.DOAnchorPos(Vector2.zero, startAnimDuration).SetEase(Ease.OutQuad));
            startAnimSeq.Join(
                body.DOScale(Vector3.one, startAnimDuration).SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    button.interactable = true;
                }));
        }

        public void DoPlayAnim(Action onCompleted) {
            playAnimSeq.Kill();
            playAnimSeq = DOTween.Sequence();

            canvasGroup.interactable = false;

            playAnimSeq.Join(canvasGroup.DOFade(0f, playAnimDuration).SetEase(Ease.OutQuad));
            playAnimSeq.Join(body.DOAnchorPosY(playOffsetY, playAnimDuration).SetEase(Ease.OutQuad));
            playAnimSeq.Append(
                layoutElement.DOPreferredSize(new(0f, size.y), playAnimDuration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => onCompleted?.Invoke()));
        }

        public void Select() {
            body.GetComponent<Image>().color = selectedColor;
        }

        public void UnSelect() {
            body.GetComponent<Image>().color = Color.white;
        }

        public void Refresh() {
            costIcon.color = card.IsCostHalved ? costReducedColor : costRegularColor;
            costText.text = card.Cost.ToString();

            var level = (int)card.CurrentLevel;
            levelIcon.color = levelColors[level];
            levelText.text = (level + 1).ToString();

            imageBackground.color = levelColors[level];
            nameText.text = card.Name;
            descriptionText.text = card.GetDescription();
            image.sprite = card.Image;
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            canvas.overrideSorting = true;
            canvas.sortingLayerName = onHoveredSortingLayer;
            canvas.sortingOrder = 1;
        }

        public void OnPointerExit(PointerEventData eventData) {
            canvas.overrideSorting = false;
            canvas.sortingOrder = 0;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            layoutElement = GetComponent<LayoutElement>();
            canvas = GetComponent<Canvas>();
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
