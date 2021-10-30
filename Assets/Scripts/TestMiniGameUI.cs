//Do not edit this script.
///This script does not exist in the real game.
///It is only used to link the text mini game
///button and input box to the game manager.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TestMiniGameUI : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField gameIdInput;
    [SerializeField]
    private Button testMiniGameButton;
    private int playCount = 0;

    public static TestMiniGameUI Instance;

    void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// Starts a mini game with the gameId that is typed in the game Id input box.
    /// </summary>
    public void OnClickTestMiniGameButton()
    {
        playCount++;
        string minigameId = gameIdInput.text;
        GameManager.Instance.TestMiniGame(minigameId, "TestPlayer" + playCount.ToString());
        testMiniGameButton.interactable = false;
        gameIdInput.interactable = false;
    }

    /// <summary>
    /// Re-enable test buttons.
    /// </summary>
    public void OnMiniGameComplete()
    {
        testMiniGameButton.interactable = true;
        gameIdInput.interactable = true;
    }
}
