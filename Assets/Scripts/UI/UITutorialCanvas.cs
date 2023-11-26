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

        [SerializeField] private Transform tutorialsHolder;

        private List<List<UIPerforatableMask>> tutorialSteps = new();

        private GameManager gameManager;
        private InputManager inputManager;
        private CardManager cardManager;
        private Player player;

        private Coroutine doTutorialRoutine;
        #endregion

        #region Publics
        public void EnableRaycastBlocker() {
            raycastBlocker.SetActive(true);
        }

        public void DisableRaycastBlocker() {
            raycastBlocker.SetActive(false);
        }
        #endregion

        #region Internals
        private void InitTutorials() {
            tutorialSteps.Clear();
            foreach (Transform tutorial in tutorialsHolder) {
                tutorial.gameObject.SetActive(true);

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

        private void Begin(int tutorialLevel) {
            gameObject.SetActive(true);
            DoTutorial(tutorialLevel, 0, startDelay);
        }

        private void DoTutorial(int tutorialIndex, int stepIndex, float delay) {
            if (doTutorialRoutine != null) {
                Debug.LogWarning("A tutorial routine already exists. ");
                return;
            }
            doTutorialRoutine = StartCoroutine(DoTutorialRoutine(tutorialIndex, stepIndex, delay));
        }

        private IEnumerator DoTutorialRoutine(int tutorialIndex, int stepIndex, float delay) {
            void DoNext(float delayToDoNext = 0f) {
                var nextIndex = stepIndex + 1;
                if (nextIndex >= tutorialSteps[tutorialIndex].Count) {
                    Debug.Log($"Tutorial {tutorialIndex} reaches the end. ");
                    return;
                }
                DoTutorial(tutorialIndex, nextIndex, delayToDoNext);
            }

            void Close() {
                tutorialSteps[tutorialIndex][stepIndex].gameObject.SetActive(false);
            }

            void SetUpViewOnly(float delayToDoNext = 0f) {
                EnableRaycastBlocker();

                Action handler = null;
                handler = () => {
                    Close();
                    DoNext(delayToDoNext);
                    inputManager.OnMouseLeftUp -= handler;
                };
                inputManager.OnMouseLeftUp += handler;
            }

            void SetUpCardSelection(Func<bool> validator, float delayToDoNext = 0f) {
                Action<Card> handler = null;
                handler = _ => {
                    if (!validator.Invoke()) return;

                    Close();
                    DoNext(delayToDoNext);
                    cardManager.OnCardSelected -= handler;
                };
                cardManager.OnCardSelected += handler;
            }

            void SetUpCardPlay(Func<bool> validator, float delayToDoNext = 0f) {
                Action<Card[]> handler = null;
                handler = _ => {
                    if (!validator.Invoke()) return;

                    Close();
                    DoNext(delayToDoNext);
                    cardManager.OnCardsPlayed -= handler;
                };
                cardManager.OnCardsPlayed += handler;
            }

            if (delay > 0f) {
                inputManager.Toggle(false);
                EnableRaycastBlocker();
                yield return new WaitForSeconds(delay);
            }

            inputManager.Toggle(true);
            DisableRaycastBlocker();
            tutorialSteps[tutorialIndex][stepIndex].gameObject.SetActive(true);

            switch (tutorialIndex) {

                // Tutorial level 1. 
                case 0: {
                    switch (stepIndex) {

                        // Introduce card cost. 
                        case 0: {
                            SetUpViewOnly();
                            break;
                        }

                        // Introduce card name. 
                        case 1: {
                            SetUpViewOnly();
                            break;
                        }

                        // Introduce card level. 
                        case 2: {
                            SetUpViewOnly();
                            break;
                        }

                        // Introduce card effect. 
                        case 3: {
                            SetUpViewOnly();
                            break;
                        }

                        // Introduce card selection. 
                        case 4: {
                            SetUpCardSelection(
                                () => cardManager.CurrentPattern == CardManager.Pattern.X);
                            break;
                        }

                        // Introduce play and unselect buttons.  
                        case 5: {
                            SetUpViewOnly();
                            break;
                        }

                        // Introduce selected info.  
                        case 6: {
                            SetUpViewOnly();
                            break;
                        }

                        // Wait for player to play a card.  
                        case 7: {
                            SetUpCardPlay(() => true);
                            break;
                        }

                        // Introduce tower buttons and tower placing. 
                        case 8: {
                            Action<TowerConfig> onPickedHandler = null;
                            onPickedHandler = _ => {
                                Close();
                                DoNext();
                                player.OnTowerPicked -= onPickedHandler;
                            };
                            player.OnTowerPicked += onPickedHandler;

                            break;
                        }

                        case 9: {
                            Action<TowerConfig> onPlacedHandler = null;
                            onPlacedHandler = _ => {
                                Close();
                                DoNext();
                                player.OnTowerPlaced -= onPlacedHandler;
                            };
                            player.OnTowerPlaced += onPlacedHandler;

                            break;
                        }

                        // Introduce start combat button. 
                        case 10: {
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

                // Tutorial level 2. 
                case 1: {
                    switch (stepIndex) {

                        // Introduce deck. 
                        case 0: {
                            SetUpViewOnly();
                            break;
                        }

                        // Introduce XX. 
                        case 1: {
                            SetUpViewOnly();
                            break;
                        }

                        // Wait to select XX. 
                        case 2: {
                            SetUpCardSelection(
                                () => cardManager.CurrentPattern == CardManager.Pattern.XX);
                            break;
                        }

                        // Note the change of level and effect. 
                        case 3: {
                            SetUpViewOnly();
                            break;
                        }

                        // Play XX. 
                        case 4: {
                            SetUpCardPlay(() => true, startDelay);
                            break;
                        }

                        // Introduce XXXY. 
                        case 5: {
                            SetUpViewOnly();
                            break;
                        }

                        // Wait to select XXXY. 
                        case 6: {
                            SetUpCardSelection(
                                () => cardManager.CurrentPattern == CardManager.Pattern.XXXY);
                            break;
                        }

                        // Note the reduced cost. 
                        case 7: {
                            SetUpViewOnly();
                            break;
                        }

                        // Play XXXY. 
                        case 8: {
                            SetUpCardPlay(() => true);
                            break;
                        }

                        // Wait for combat.  
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

                // Tutorial level 3. 
                case 2: {
                    switch (stepIndex) {

                        // Introduce XXXYY. 
                        case 0: {
                            SetUpViewOnly();
                            break;
                        }

                        // Wait to select XXXYY. 
                        case 1: {
                            SetUpCardSelection(
                                () => cardManager.CurrentPattern == CardManager.Pattern.XXXYY);
                            break;
                        }

                        // Note the change of level and effect and the reduced cost. 
                        case 2: {
                            SetUpViewOnly();
                            break;
                        }

                        // Play XXXYY. 
                        case 3: {
                            SetUpCardPlay(() => true, startDelay);
                            break;
                        }

                        // Introduce XXXX. 
                        case 4: {
                            SetUpViewOnly();
                            break;
                        }

                        // Wait to select XXXX. 
                        case 5: {
                            SetUpCardSelection(
                                () => cardManager.CurrentPattern == CardManager.Pattern.XXXX);
                            break;
                        }

                        // Note the change of level and effect. 
                        case 6: {
                            SetUpViewOnly();
                            break;
                        }

                        // Play XXXX. 
                        case 7: {
                            SetUpCardPlay(() => true);
                            break;
                        }

                        // Wait for combat. 
                        case 8: {
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

                // Tutorial level 4. 
                case 3: {
                    switch (stepIndex) {

                        // Introduce ABCD. 
                        case 0: {
                            SetUpViewOnly();
                            break;
                        }

                        // Wait to select ABCD. 
                        case 1: {
                            SetUpCardSelection(
                                () => 
                                cardManager.CurrentPattern == CardManager.Pattern.ABCD
                                && cardManager.NumSelected == cardManager.NumOnHand);
                            break;
                        }

                        // Note the change of level and effect. 
                        case 2: {
                            SetUpViewOnly();
                            break;
                        }

                        // Play ABCD. 
                        case 3: {
                            SetUpCardPlay(() => true);
                            break;
                        }

                        // Wait for combat. 
                        case 4: {
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
            }
            yield return null;

            doTutorialRoutine = null;
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            gameManager = GameManager.Instance;
            inputManager = InputManager.Instance;
            cardManager = CardManager.Instance;
            player = Player.Instance;

            InitTutorials();

            gameManager.OnTutorialStarted += Begin;
            gameManager.OnCurrentSceneExiting += () => {
                gameManager.OnTutorialStarted -= Begin;
            };

            gameObject.SetActive(gameManager.DoTutorial);
        }
        #endregion
    }
}
