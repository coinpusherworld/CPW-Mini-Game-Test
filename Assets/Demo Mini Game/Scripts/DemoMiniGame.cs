//@DanielLeeMeeks
///This is a demo of a simple mini game for Coin Pusher World.
///It randomly picks one of the points values from winning
///values array and gives them to the player after showing a
///set of random points values they could win.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace DemoMiniGame
{
    public class DemoMiniGame : MonoBehaviour
    {
        //Every mini game should have a unique gameId.
        public string gameId = "Demo Mini Game";

        //Is the mini game currently active?
        private bool gameInProgress;

        //Username of the player currently playing the mini game.
        private string username;

        [SerializeField]
        private TextMeshPro displayText;
        [SerializeField]
        private int[] winningValues;
        private int winningIndex;

        [Header("Required")]
        [SerializeField]
        private Camera camera;

        // Start is called before the first frame update
        void Start()
        {
            //Required: Listen to GameManager for the StartMiniGame event.
            GameManager.StartMiniGame += OnStartMiniGame;

            //Required: Turn off camera on start.
            camera.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            //Recommended: To improve game preformace, you should have no code in the update function. 
        }

        /// <summary>
        /// This function is called by the GameManager to start mini games.  If the called gameId is the same as this game's gameId then the mini game should be started.  If the called gameId is not the same as this game's gameId then nothing should happen.
        /// </summary>
        /// <param name="gameId">GameID of the mini game to start.</param>
        /// <param name="username">Username of the player playing the mini game.</param>
        public void OnStartMiniGame(string gameId, string username)
        {
            if (gameId == this.gameId)
            {
                StartMiniGame(username);
            }
        }

        /// <summary>
        /// Start playing the mini game.
        /// </summary>
        /// <param name="username">Username of the player playing the mini game.</param>
        private void StartMiniGame(string username)
        {
            Debug.Log("<color=#ffff00>[" + gameId + "] GameManager</color>: Starting minigame.");
            gameInProgress = true;
            //Enable camera so the stream can see the main game.
            camera.enabled = true;
            this.username = username;
            //Pick index of the winnings values the player will win.
            winningIndex = Random.Range(0, winningValues.Length);
            StartCoroutine(PlayGame());
        }

        private IEnumerator PlayGame()
        {
            //Set up game
            displayText.text = "Get Ready...";
            //Move mini game into view.
            camera.transform.DOMoveY(0, 1f);
            //Wait for mini game to be moved into view before starting.
            yield return new WaitForSeconds(1f);
            //Play mini game's music.
            this.GetComponent<AudioSource>().Play();

            //Anticipation loops
            float timeLooping = 0; //How log the mini game has been looping random scores for anticipation.
            while (timeLooping < 14.9) { //Loop random numbers until music hit.
                //Random number of show player.
                displayText.text = Random.Range(500,2000).ToString();
                
                //Change the looping speed depending on how long the game has been looping to match music.
                if (timeLooping < 3.75f)
                {
                    yield return new WaitForSeconds(0.4688f);
                    timeLooping += 0.4688f;
                }
                else if (timeLooping < 7.27)
                {
                    yield return new WaitForSeconds(0.4688f / 2f);
                    timeLooping += 0.4688f/2f;
                }
                else if (timeLooping < 11.155f)
                {
                    yield return new WaitForSeconds(0.4688f / 4f);
                    timeLooping += 0.4688f / 4f;
                }
                else
                {
                    yield return new WaitForSeconds(0.4688f / 8f);
                    timeLooping += 0.4688f / 8f;
                }
            }

            //Real winning value
            displayText.text = winningValues[winningIndex].ToString();
            //Update score display in main game.
            GameManager.Instance.UpdateScoreDisplay(winningValues[winningIndex], username);
            //Wait 3 seconds so player can see the points they won.
            yield return new WaitForSeconds(3);

            //End Game
            camera.transform.DOMoveY(12, 1f); //Move mini game out of view.
            yield return new WaitForSeconds(1); //Wait for mini game to be moved out of view before ending game.
            EndMiniGame();
        }

        /// <summary>
        /// End the minigame and add points to player's account.
        /// </summary>
        private void EndMiniGame()
        {
            //Turn off mini game camera.
            camera.enabled = false;
            gameInProgress = false;
            //Add points to player's account and allow main game to move to next player in que.
            GameManager.Instance.OnMiniGameComplete(winningValues[winningIndex], username, "Demo Mini Game");
        }
    }
}
