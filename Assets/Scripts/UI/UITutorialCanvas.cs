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
        [SerializeField] private float startDelay;
        [SerializeField] private GameObject raycastBlocker;

        [SerializeField] private GameObject groundPathTip;

        [SerializeField] private Transform tutorialsHolder;

        private List<List<UIPerforatableMask>> tutorialSteps = new();

        private GameManager gameManager;
        private InputManager inputManager;
        private CardManager cardManager;
        private Player player;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void InitTutorials() {
            tutorialSteps.Clear();
            foreach (Transform tutorial in tutorialsHolder) {
                var steps = new List<UIPerforatableMask>();
                foreach (Transform child in tutorial) {
                    if (child.TryGetComponent<UIPerforatableMask>(out var step)) {
                        step.gameObject.SetActive(false);
                        steps.Add(step);
                    }
                    else {
                        Debug.LogWarning($"{typeof(UIPerforatableMask)} is not found in {child.name}");
                    }
                }
                tutorialSteps.Add(steps);
            }
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

                        // Introduce card cost. 
                        case 0: {
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
                        
                        // Introduce card name. 
                        case 1: {
                            Action handler = null;
                            handler = () => {
                                Close();
                                DoNext();
                                inputManager.OnMouseLeftUp -= handler;
                            };
                            inputManager.OnMouseLeftUp += handler;

                            break;
                        }
                        
                        // Introduce card level. 
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
                        
                        // Introduce card effect. 
                        case 3: {
                            Action handler = null;
                            handler = () => {
                                Close();
                                DoNext();
                                inputManager.OnMouseLeftUp -= handler;
                            };
                            inputManager.OnMouseLeftUp += handler;

                            break;
                        }

                        // Introduce card selection. 
                        case 4: {
                            raycastBlocker.SetActive(false);

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
                        case 5: {
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
                        case 6: {
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
                        case 7: {
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
                        case 8: {
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
                        case 9: {
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

            InitTutorials();
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
