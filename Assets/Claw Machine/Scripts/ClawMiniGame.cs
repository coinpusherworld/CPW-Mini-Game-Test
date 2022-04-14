using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class ClawMiniGame : MonoBehaviour
{
    //Every mini game should have a unique gameId.
    public string gameId = "Claw Machine";

    //Is the mini game currently active?
    private bool gameInProgress;

    //Username of the player currently playing the mini game.
    private string username;

    public List<Balls> ballCounts;
    private List<Balls> ballInGame = new List<Balls>();

    public Animator theClaw;
    public MeshRenderer winningBall;
    int winAmount = 0;

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

        CommingleBalls();
    }

    //Required: Unsub from OnStartMainGame when script is disabled.  Without this, your minigame may lock up the game when called after the game is restarted/reloaded.
    void OnDisable()
    {
        //Unsubscribe from OnStartMiniGame
        GameManager.StartMiniGame -= OnStartMiniGame;
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

        StartCoroutine(game());
    }

    private IEnumerator game()
    {
        theClaw.Play("Reset");

        this.transform.GetChild(0).DOMove(new Vector3(0, 0, 0), 3);
        yield return new WaitForSeconds(3.5f);

        winningBall.material = ballInGame[0].material;
        winAmount = ballInGame[0].value;
        theClaw.Play("Drop");

        yield return new WaitForSeconds(8);

        //Update score display in main game.
        GameManager.Instance.UpdateScoreDisplay(winAmount, username);
        //Wait 3 seconds so player can see the points they won.
        yield return new WaitForSeconds(3);

        this.transform.GetChild(0).DOMove(new Vector3(0, -45, 0), 4);
        yield return new WaitForSeconds(4);

        ballInGame.RemoveAt(0);

        if (ballInGame.Count < 20)
        {
            CommingleBalls();
        }

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
        GameManager.Instance.OnMiniGameComplete(winAmount, username, gameId);
    }

    private void CommingleBalls()
    {
        foreach (Balls b in ballCounts)
        {
            int count = b.count;
            while (count > 0)
            {
                ballInGame.Add(b);
                count--;
            }
        }
        System.Random rand = new System.Random();
        ballInGame = ballInGame.OrderBy(_ => rand.Next()).ToList();
    }

    [System.Serializable]
    public struct Balls
    {
        public string id;
        public int count;
        public int value;
        public Material material;
    }
}


