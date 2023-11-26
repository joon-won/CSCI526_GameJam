using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace CSCI526GameJam {
    public class UIMainMenu : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private float animDuration;
        [SerializeField] private GameObject rootView;
        [SerializeField] private GameObject tutorialView;

        [SerializeField] private Button startButton;
        [SerializeField] private Button tutorialButton;
        [SerializeField] private Button[] tutorialButtons;
        [SerializeField] private Button closeButton;

        private Sequence switchSeq;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void SwitchView() {
            switchSeq.Kill();
            switchSeq = DOTween.Sequence();
            
            switchSeq.Join(transform.DOScale(0f, animDuration)
                               .SetEase(Ease.OutQuad).OnComplete(() => {
                                   rootView.SetActive(!rootView.activeSelf);
                                   tutorialView.SetActive(!tutorialView.activeSelf);
                               }));
            switchSeq.Append(transform.DOScale(1f, animDuration)
                                 .SetEase(Ease.OutBack));
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            rootView.SetActive(true);
            tutorialView.SetActive(false);

            startButton.onClick.AddListener(() => {
                GameManager.Instance.LoadScene(Configs.GameplaySceneIndex);
                GameManager.Instance.SetTutorial(false);
            });
            tutorialButton.onClick.AddListener(SwitchView);
            closeButton.onClick.AddListener(SwitchView);

            for (int i = 0; i < tutorialButtons.Length; i++) {
                var button = tutorialButtons[i];

                var index = i;
                button.onClick.AddListener(() => {
                    GameManager.Instance.LoadScene(Configs.GameplaySceneIndex);
                    GameManager.Instance.SetTutorial(true, index);
                });
            }
        }
        #endregion
    }
}
