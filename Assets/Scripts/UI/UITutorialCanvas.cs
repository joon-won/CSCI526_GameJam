using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class UITutorialCanvas : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private float startDelay;
        [SerializeField] private GameObject raycastBlocker;

        [SerializeField] private GameObject groundPathTip;

        [SerializeField] private UIPerforatableMask level1Step1;
        [SerializeField] private UIPerforatableMask level1Step2;
        [SerializeField] private UIPerforatableMask level1Step3;
        [SerializeField] private UIPerforatableMask level1Step4;
        [SerializeField] private UIPerforatableMask level1Step5;
        [SerializeField] private UIPerforatableMask level1Step6;

        private GameManager gameManager;
        private InputManager inputManager;
        private CardManager cardManager;
        private Player player;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void Begin() {
            gameObject.SetActive(true);
            StartCoroutine(BeginRoutine());
        }

        private IEnumerator BeginRoutine() {
            inputManager.Toggle(false);
            raycastBlocker.SetActive(true);

            yield return new WaitForSeconds(startDelay);

            inputManager.Toggle(true);
            raycastBlocker.SetActive(false);
            Level1Step1();
        }

        // Introduce card selecting.  
        private void Level1Step1() {
            level1Step1.gameObject.SetActive(true);

            Action<Card> handler = null;
            handler = _ => {
                if (cardManager.NumSelected != cardManager.NumOnHand) return;

                level1Step1.gameObject.SetActive(false);
                Level1Step2();
                cardManager.OnCardSelected -= handler;
            };
            cardManager.OnCardSelected += handler;
        }

        // Introduce play and unselect buttons.  
        private void Level1Step2() {
            level1Step2.gameObject.SetActive(true);
            raycastBlocker.SetActive(true);

            Action handler = null;
            handler = () => {
                level1Step2.gameObject.SetActive(false);
                Level1Step3();
                inputManager.OnMouseLeftUp -= handler;
            };
            inputManager.OnMouseLeftUp += handler;
        }

        // Introduce selected info.  
        private void Level1Step3() {
            level1Step3.gameObject.SetActive(true);

            Action handler = null;
            handler = () => {
                level1Step3.gameObject.SetActive(false);
                Level1Step4();
                inputManager.OnMouseLeftUp -= handler;
            };
            inputManager.OnMouseLeftUp += handler;
        }

        // Wait for player to play a card.  
        private void Level1Step4() {
            level1Step4.gameObject.SetActive(true);
            raycastBlocker.SetActive(false);

            Action<Card[]> handler = null;
            handler = _ => {
                if (cardManager.NumOnHand != 0) return;

                level1Step4.gameObject.SetActive(false);
                Level1Step5();
                cardManager.OnCardsPlayed -= handler;
            };
            cardManager.OnCardsPlayed += handler;
        }

        // Introduce tower buttons and tower placing. 
        private void Level1Step5() {
            level1Step5.gameObject.SetActive(true);

            Action<TowerConfig> onPickedHandler = null;
            onPickedHandler = _ => {
                level1Step5.gameObject.SetActive(false);
                groundPathTip.SetActive(true);
                player.OnTowerPicked -= onPickedHandler;
            };
            player.OnTowerPicked += onPickedHandler;

            Action<TowerConfig> onPlacedHandler = null;
            onPlacedHandler = _ => {
                Level1Step6();
                groundPathTip.SetActive(false);
                player.OnTowerPlaced -= onPlacedHandler;
            };
            player.OnTowerPlaced += onPlacedHandler;
        }

        // Introduce start combat button. 
        private void Level1Step6() {
            level1Step6.gameObject.SetActive(true);

            Action handler = null;
            handler = () => {
                level1Step6.gameObject.SetActive(false);
                //Level1Step7();
                gameManager.OnCombatStarted -= handler;
            };
            gameManager.OnCombatStarted += handler;
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            gameManager = GameManager.Instance;
            inputManager = InputManager.Instance;
            cardManager = CardManager.Instance;
            player = Player.Instance;

            foreach (var mask in GetComponentsInChildren<UIPerforatableMask>()) {
                mask.gameObject.SetActive(false);
            }
            groundPathTip.SetActive(false);

            gameManager.OnTutorialStarted += Begin;
            gameManager.OnCurrentSceneExiting += () => {
                gameManager.OnTutorialStarted -= Begin;
            };

            gameObject.SetActive(gameManager.DoTutorial);
        }
        #endregion
    }
}
