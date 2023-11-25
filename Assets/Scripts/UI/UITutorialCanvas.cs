using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace CSCI526GameJam {
    public class UITutorialCanvas : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private string tutorialStepPrefix;
        [SerializeField] private float startDelay;
        [SerializeField] private GameObject raycastBlocker;

        [SerializeField] private GameObject groundPathTip;

        [SerializeField] private Transform[] tutorialStepsHolders;

        private UIPerforatableMask[][] tutorialSteps;

        private GameManager gameManager;
        private InputManager inputManager;
        private CardManager cardManager;
        private Player player;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private UIPerforatableMask[] InitTutorial(Transform stepsHolder) {
            stepsHolder.gameObject.SetActive(true);

            var steps = new UIPerforatableMask[stepsHolder.childCount];
            foreach (Transform child in stepsHolder) {
                var name = child.name;
                if (int.TryParse(name.Replace(tutorialStepPrefix, ""), out var parsedNum)) {
                    if (child.TryGetComponent<UIPerforatableMask>(out var step)) {
                        step.gameObject.SetActive(false);
                        steps[parsedNum - 1] = step;
                    }
                    else {
                        Debug.LogWarning($"{typeof(UIPerforatableMask)} is not found in {child.name}");
                    }
                }
                else {
                    Debug.LogWarning($"{child.name} can not be parsed. ");
                }
            }
            return steps;
        }

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
            DoTutorial(0, 0);
        }


        private void DoTutorial(int tutorialIndex, int stepIndex) {
            void DoNext() {
                DoTutorial(tutorialIndex, stepIndex + 1);
            }
            void Close() {
                tutorialSteps[tutorialIndex][stepIndex].gameObject.SetActive(false);
            }

            tutorialSteps[tutorialIndex][stepIndex].gameObject.SetActive(true);

            switch (tutorialIndex) {

                // Tutorial level 1
                case 0: {
                    switch (stepIndex) {

                        // Introduce card selecting.  
                        case 0: {
                            Action<Card> handler = null;
                            handler = _ => {
                                if (cardManager.NumSelected != cardManager.NumOnHand) return;

                                Close();
                                DoNext();
                                cardManager.OnCardSelected -= handler;
                            };
                            cardManager.OnCardSelected += handler;

                            break;
                        }

                        // Introduce play and unselect buttons.  
                        case 1: {
                            raycastBlocker.SetActive(true);

                            Action handler = null;
                            handler = () => {
                                Close();
                                DoNext();
                                inputManager.OnMouseLeftUp -= handler;
                            };
                            inputManager.OnMouseLeftUp += handler;

                            break;
                        }

                        // Introduce selected info.  
                        case 2: {
                            Action handler = null;
                            handler = () => {
                                Close();
                                DoNext();
                                inputManager.OnMouseLeftUp -= handler;
                            };
                            inputManager.OnMouseLeftUp += handler;

                            break;
                        }

                        // Wait for player to play a card.  
                        case 3: {
                            raycastBlocker.SetActive(false);

                            Action<Card[]> handler = null;
                            handler = _ => {
                                if (cardManager.NumOnHand != 0) return;

                                Close();
                                DoNext();
                                cardManager.OnCardsPlayed -= handler;
                            };
                            cardManager.OnCardsPlayed += handler;

                            break;
                        }

                        // Introduce tower buttons and tower placing. 
                        case 4: {
                            Action<TowerConfig> onPickedHandler = null;
                            onPickedHandler = _ => {
                                Close();
                                groundPathTip.SetActive(true);
                                player.OnTowerPicked -= onPickedHandler;
                            };
                            player.OnTowerPicked += onPickedHandler;

                            Action<TowerConfig> onPlacedHandler = null;
                            onPlacedHandler = _ => {
                                DoNext();
                                groundPathTip.SetActive(false);
                                player.OnTowerPlaced -= onPlacedHandler;
                            };
                            player.OnTowerPlaced += onPlacedHandler;

                            break;
                        }

                        // Introduce start combat button. 
                        case 5: {
                            Action handler = null;
                            handler = () => {
                                Close();
                                gameManager.OnCombatStarted -= handler;
                            };
                            gameManager.OnCombatStarted += handler;

                            break;
                        }
                    }

                    break;
                }

                // Tutorial level 2
                case 1: {

                    break;
                }

                // Tutorial level 3
                case 2: {

                    break;
                }
            }
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            gameManager = GameManager.Instance;
            inputManager = InputManager.Instance;
            cardManager = CardManager.Instance;
            player = Player.Instance;

            tutorialSteps = tutorialStepsHolders.Select(x => InitTutorial(x)).ToArray();

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
