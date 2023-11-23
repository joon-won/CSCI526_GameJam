using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {
    public class UITowerButton : MonoBehaviour {

        #region Fields
        [MandatoryFields]

        [Title("Anim")]
        [SerializeField] private float bounceDuration;
        [SerializeField] private float bounceScale;
        [SerializeField] private Color invalidColor;

        [Title("UI")]
        [SerializeField] private TowerConfig config;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text numText;

        private Player player;
        private int prevNum;

        private Tween bounceTween;
        #endregion

        #region Publics
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            player = Player.Instance;
            button.onClick.AddListener(() => Player.Instance.PickTower(config));
            prevNum = int.MaxValue;
        }

        private void Update() {
            var num = player.GetTowerNum(config);
            if (prevNum == num) return;

            if (num == 0) {
                numText.enabled = false;
                button.enabled = false;
                icon.color = invalidColor;
            }
            else {
                numText.enabled = true;
                button.enabled = true;
                icon.color = Color.white;
                numText.text = num.ToString();
                if (num > prevNum) {
                    bounceTween.Kill();
                    bounceTween = transform.DOScale(bounceScale, bounceDuration * 0.5f).SetEase(Ease.OutQuad)
                        .OnComplete(() => transform.DOScale(1f, bounceDuration * 0.5f).SetEase(Ease.InQuad));
                }
            }
            prevNum = num;
        }

        private void OnValidate() {
            if (config && icon) {
                icon.sprite = config.Image;
            }
        }
        #endregion
    }
}
