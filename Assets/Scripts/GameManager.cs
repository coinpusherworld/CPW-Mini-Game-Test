//Do not edit this script.
///This is a stripped down version of the Coin Pusher World
///Game Manager that only contains the function needed to
///start and respond to mini games ending.
///
///This script calles StartMiniGame(gameID, username) to start
///a mini game.  Each mini game's controller script should
///listen for this event and only start their mini game if
///it's game ID matches the game ID from the event.
///
/// When the player's score changes in the mini game, the 
/// mini game controller should call
/// GameManager.Instance.UpdateScoreDisplay(int score, string username);
/// to update the main games score display.  This function
/// does not add points to the player's account so if can be
/// call as many time as needed during a mini game.
/// 
///After the minigame is complete, call the
///GameManager.Instance.OnMiniGameComplete(int score, string username, string gameId);
///function to add the player's points won during a mini
///game to their account and allow the main game to
///move to the next player in que.  This function should
///only be called once per mini game and should be the very
///last function called during a mini game.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool dropperEnabled = true;

    //[Header("Events")]
    public delegate void StartGame(string gameId, string username);
    public static event StartGame StartMiniGame;

    public static GameManager Instance;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI scoreDisplay;
    [SerializeField]
    private TextMeshProUGUI usernameDisplay;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Start a mini game for testing.  This function does not exist in the real game.
    /// </summary>
    /// <param name="gameID">ID of the minigame to start.</param>
    /// <param name="username">Username of the player playing the mini game.</param>
    public void TestMiniGame(string gameID, string username)
    {
        Debug.Log("<color=#00ffff>[Game Manager]</color> Starting mini game " + gameID + " for player " + username + ".");
        UpdateScoreDisplay(0,username);
        StartMiniGame(gameID, username);
    }

    /// <summary>
    /// Call this function after mini game is complete.  Points will be added to the players account and the game will move on to the next player in que.  This should be the last function called in your mini game.
    /// </summary>
    /// <param name="score">Amount of points to add to a players account.</param>
    /// <param name="username">Twitch username of player.</param>
    /// <param name="gameName">ID of mini game points where earned in.</param>
    public void OnMiniGameComplete(int score, string username, string gameName)
    {
        Debug.Log("<color=#00ffff>[Game Manager]</color> OnMiniGameComplete called.");
        dropperEnabled = true;
        Debug.Log("<color=#00ffff>[Game Manager]</color> " + score.ToString() + " points where added to " + username.ToString() + "'s account by " + gameName + " minigame." );
        UpdateScoreDisplay(score, username);
        TestMiniGameUI.Instance.OnMiniGameComplete();
    }

    /// <summary>
    /// Change the current score and player box.  This function does not add points to a players account and can be called mutiple times per mini game.
    /// </summary>
    /// <param name="score">Player's current score.</param>
    /// <param name="username">Player's username.</param>
    public void UpdateScoreDisplay(int score, string username = "")
    {
        scoreDisplay.text = score.ToString();
        usernameDisplay.text = username;
    }
}
