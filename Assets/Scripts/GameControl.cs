using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// controls a single game of pong
public class GameControl : MonoBehaviour {
    // points necessary for winning
    public int pointsForWinning = 3;
    // where players should spawn
    public Vector2 player1Position;
    public Vector2 player2Position;
    public float coopXOffset = 1.0f;
    // position of incidents to spawn from, will auto-generate the negative values
    public Vector2 incidentsPosition;
    public Incident incident;
    // point ui objects
    public Text[] pointTexts = {null, null};
    public IntroductionScript introduction;
    public WinAnimation winAnimation;

    // used when scene is accessed directly
    public GameObject playerControllerPrefab;
    public GameObject defaultPlayer1Prefab;
    public GameObject defaultPlayer2Prefab;
    public GameObject defaultCoopPrefab;
    public GameObject ballPrefab;

    // how regularly a check for current game state is done
    public float timeCheckInterval = 5.0f;
    // uneventful time until some interruption occurs
    public float interruptionInterval = 10.0f;
    public float maxTimeWithoutInterruption = 30.0f;
    public float speedScale = 1.0f;
    public float ballSpawnDelay = 1.0f;

    public MusicController musicController;
    public AudioClip soundBallSpawn;

    private int[] points = {0, 0};
    private int winner = 0;

    // is this a human player vs human player match?
    private bool isVersus = false;
    // is this a match against ai with multiple human players helping each other?
    private bool isCoop = false;
    // main player 1, the one on the right side, always a human
    private Player player1;
    // main player 2, the one on the left side, either human or ai
    private Player player2;
    // helping players
    private List<Player> coopPlayers;



    private GameObject ball;
    private AudioSource soundSource;

    private bool active = false;
    private float timeDeltaCounter = 0.0f;
    private float timeSinceLastGoal = 0.0f;
    private float timeSinceLastInterruption = 0.0f;
    private float previousTimeScale;
    private bool clearControllersAtEnd = false;

    [HideInInspector] public static GameControl instance;

    public void ModifyPoints(int player, int amount) {
        points[player] += amount;

        // reset interruption counter
        timeSinceLastGoal = 0.0f;

        // update score text
        Text changedPointText = pointTexts[player];
        if(changedPointText != null) {
            changedPointText.text = points[player].ToString();
        }

        // check if a player won
        if(points[player] >= pointsForWinning) {
            Win(player);
        }
    }

    public void Win(int player) {
        winner = player;
        DeactivatePlayers();
        RemoveAllBalls();

        // register the result
        if(UnlocksManager.instance != null) {
            UnlocksManager.instance.RegisterMatch(player1.id, player2.id, winner==0, humanMatch: isVersus, coopMatch: isCoop);
        }
        else {
            Debug.Log("Could not register match");
        }

        if(MasterController.instance != null) {
            MasterController.instance.backToSelection = true;
        }

        winAnimation.PlayAnimation(player, player1, player2);
    }

    public void PauseGame() {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void UnpauseGame() {
        Time.timeScale = previousTimeScale;
    }

    public void SpawnBall(bool playSound) {
        if(playSound) soundSource.PlayOneShot(soundBallSpawn, 0.7f);
        ball = Instantiate(ballPrefab, new Vector3(0.0f, 0.0f), Quaternion.identity);
        ball.GetComponent<Ball>().Launch();
    }

    IEnumerator SpawnBallDelayed(float delay) {
        yield return new WaitForSeconds(delay);
        SpawnBall(true);
    }

    public void RemoveAllBalls() {
        Ball[] balls = FindObjectsOfType<Ball>();
        foreach(Ball ball in balls) {
            Destroy(ball.gameObject);
        }
    }

    [ContextMenu("Incident")]
    public void SpawnIncident(string spawnId="") {
        Vector2 position = Vector2.zero;
        bool flipped = false;
        if(Random.value > 0.5) position.x = incidentsPosition.x;
        else position.x = -incidentsPosition.x;

        if(Random.value > 0.5) position.y = incidentsPosition.y;
        else {
            position.y = -incidentsPosition.y;
            flipped = true;
        }

        if(spawnId == "") {
            incident.CreateIncident(position, flipped);
        }
        else {
            incident.CreateIncident(position, flipped, spawnId);
        }
        
        timeSinceLastInterruption = 0;
    }

    public void StartGame() {
        print("START GAME");
        Time.timeScale = speedScale;
        SpawnBall(true);
        active = true;
        ActivatePlayers();

        // play music
        AudioClip overrideMusic = null;
        if(player2.themeSong != null) {
            overrideMusic = player2.themeSong;
        }

        musicController.PlayMusic(player2.bossIntroduction, overrideMusic: overrideMusic);
    }

    public void ActivatePlayers() {
        player1.active = true;
        player2.active = true;

        if (coopPlayers != null) {
            foreach (Player player in coopPlayers) {
                player.active = true;
            }
        }

        active = true;
    }

    public void AnimatePlayer(int player) {
        if(player == 0) {
            player1.StartAnimation();

            // also activate all coop players, they are on the same side as player 0
            if (coopPlayers != null) {
                print("Animate coop");
                foreach (Player playerController in coopPlayers) {
                    playerController.StartAnimation();
                }
            }
        }
        else {
            player2.StartAnimation();
        }
    }

    public void DeactivatePlayers() {
        player1.active = false;
        player2.active = false;

        if(coopPlayers != null) {
            foreach(Player player in coopPlayers) {
                player.active = false;
            }
        }

        active = false;
    }

    public void EndGame() {
        Time.timeScale = 1.0f;

        // remove the player controlled paddles
        if (PlayerManagerController.instance != null) {
            PlayerManagerController.instance.ClearPlayerControlledPaddles();

            // also remove the saved controllers if this was a single player game
            if(clearControllersAtEnd) {
                PlayerManagerController.instance.Clear();
            }
        }

        instance = null;
        SceneManager.LoadScene("Menu");
    }

    public void ModTimeScale(float diff) {
        Time.timeScale = speedScale + diff;
    }

    public void RestoreTimeScale() {
        Time.timeScale = speedScale;
    }

    public Player GetPlayer1() {
        return player1;
    }

    public Player GetPlayer2() {
        return player2;
    }

    void Start() {
        instance = this;
        soundSource = gameObject.GetComponent<AudioSource>();

        SpawnPlayers();
        introduction.BeginAnimation(player2.name, player2.bossIntroduction);
    }

    void FixedUpdate() {
        if(active) {
            // do a check regularly (but not all the time to save performance)
            timeDeltaCounter += Time.deltaTime;
            if(timeDeltaCounter > timeCheckInterval) {
                // spawn new ball if no balls active
                GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
                if(balls.Length == 0) {
                    StartCoroutine(SpawnBallDelayed(ballSpawnDelay));
                    // SpawnBall();
                }
                else {
                    // see if any of the existing balls prevents us from creating new ones
                    bool preventSpawning = false;
                    foreach(var ballObj in balls) {
                        Ball ball = ballObj.GetComponent<Ball>();
                        if(ball.preventSpawning) {
                            preventSpawning = true;
                            break;
                        }
                    }

                    if(!preventSpawning) {
                        StartCoroutine(SpawnBallDelayed(ballSpawnDelay));
                    }
                }

                timeDeltaCounter = 0.0f;
            }

            // see if we should spawn an incident
            bool spawnIncident = false;

            // if no goals have been scored for a long time, mix things up
            timeSinceLastGoal += Time.deltaTime;
            if (timeSinceLastGoal >= interruptionInterval) {
                spawnIncident = true;

                // reset timer, but not completely (only scoring a goal will do that)
                timeSinceLastGoal = Random.Range(0, timeSinceLastGoal / 2);
            }

            // if no events happened for a long time, just spawn one
            timeSinceLastInterruption += Time.deltaTime;
            if(timeSinceLastInterruption >= maxTimeWithoutInterruption) {
                spawnIncident = true;
            }

            if(spawnIncident) {
                SpawnIncident();
            }
        }
    }

    private void SpawnPlayers() {
        GameObject player1Prefab;
        GameObject player2Prefab;

        // load main player choices if available
        if (MasterController.instance != null) {
            player1Prefab = MasterController.instance.player1Choice;
            player2Prefab = MasterController.instance.player2Choice;
        }
        else {
            player1Prefab = defaultPlayer1Prefab;
            player2Prefab = defaultPlayer2Prefab;
        }

        // if no specific inputs are mapped just use a default PlayerController for one player and a match vs AI
        // default case
        if (PlayerManagerController.instance != null) print("Number of players " + PlayerManagerController.instance.GetNumberOfPlayers());

        if (MasterController.instance == null || PlayerManagerController.instance == null || PlayerManagerController.instance.GetNumberOfPlayers() == 0) {
            isVersus = false;
            isCoop = false;
            GameObject playerControllerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
            player1 = SpawnPlayer(player1Prefab, false, playerControllerObj);
            player2 = SpawnPlayer(player2Prefab, true);
            clearControllersAtEnd = true;
            print("DEFAULT");
        }
        else {
            isVersus = MasterController.instance.versusActive;
            isCoop = MasterController.instance.coopActive;

            if (isVersus == false && isCoop == false) {
                isVersus = false;
                isCoop = false;
                var inputs = PlayerManagerController.instance.GetPlayerInputs();
                player1 = SpawnPlayer(player1Prefab, false, inputs[0].gameObject);
                player2 = SpawnPlayer(player2Prefab, true);
                clearControllersAtEnd = false;
                print("DEFAULT MOD");
            }
            // otherwise instantiate each player seperately
            else {
                print("MULTIPLAYER");
                clearControllersAtEnd = false;

                var inputs = PlayerManagerController.instance.GetPlayerInputs();
                Assert.IsTrue(inputs.Count >= 2);

                if (isVersus) {
                    // just spawn the first 2 players against each other
                    player1 = SpawnPlayer(player1Prefab, false, inputs[0].gameObject);
                    player2 = SpawnPlayer(player2Prefab, true, inputs[1].gameObject);
                }
                else if (isCoop) {
                    // spawn enemy
                    player2 = SpawnPlayer(player2Prefab, true);

                    // spawn players
                    coopPlayers = new List<Player>();
                    float currentXOffset = coopXOffset;

                    int index = 0;
                    foreach (var input in inputs) {
                        // first player is main
                        if (index == 0) {
                            player1 = SpawnPlayer(player1Prefab, false, input.gameObject);
                        }
                        // other players are coops
                        else {
                            coopPlayers.Add(SpawnCoopPlayer(defaultCoopPrefab, input.gameObject, new Vector2(player1Position.x - currentXOffset, player2Position.y)));
                            currentXOffset += coopXOffset;
                        }
                        index++;
                    }
                    Debug.Log("Coop wtih " + coopPlayers.Count + "coop players registered");
                }
                else {
                    Debug.LogError("Gotten into local multiplayer spawn behaviour with versus and coop disabled.");
                }
            }
        }

        DeactivatePlayers();
    }

    private Player SpawnPlayer(GameObject prefab, bool spawnLeft, GameObject playerController = null) {
        Vector2 position = player1Position;
        Quaternion rotation = Quaternion.identity;

        // if the prefab spawns left and needs to be rotated do that now
        if (spawnLeft) {
            position = player2Position;
            if (prefab.GetComponent<Player>().player2Rotated) {
                rotation = Quaternion.Euler(0, 0, 180.0f);
            }
        }

        GameObject playerObj = Instantiate(prefab, new Vector3(position.x, position.y), rotation);
        Player player = playerObj.GetComponent<Player>();
        player.player2 = false;

        if (spawnLeft) {
            player.player2 = true;
            if (!player.IsAI()) {
                player.name = "Player 2";
            }
        }

        // deactivate renderer for now
        player.Hide();

        // if playerController is given then set that up
        if (playerController != null) {
            playerObj.transform.parent = playerController.transform;
        }

        return player;
    }

    private Player SpawnCoopPlayer(GameObject prefab, GameObject playerController, Vector2 spawnPosition) {
        GameObject playerObj = Instantiate(prefab, new Vector3(spawnPosition.x, spawnPosition.y), Quaternion.identity);
        Player player = playerObj.GetComponent<Player>();
        player.player2 = false;
        player.Hide();
        playerObj.transform.parent = playerController.transform;
        return player;
    }

}
