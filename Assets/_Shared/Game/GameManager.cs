using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

// ? CONSIDER
// ? Time stat
// ? Difficulty stat

public class GameManager : MonoBehaviourSingleton<GameManager> {
  #region GAME LOAD ===================================================================================================================================
  private const string gameLoadGroupName = "GAME LOAD";

  [FoldoutGroup("$gameLoadGroupName")]
  public UnityEvent OnGameLoad = new UnityEvent();

  private void Setup() {
    SetupStats();
    gameOverLabel.gameObject.SetActive(false);
    restartButton.SetActive(false);

    if (backgroundMusic && audioSource) {
      audioSource.clip = backgroundMusic;
      audioSource.Play();
    }
    if (FindObjectsOfType<EventSystem>().Length > 1) Destroy(eventSystem.gameObject);
    OnGameLoad.Invoke();
  }
  #endregion

  #region GAME START ===================================================================================================================================
  void Start() {
    Setup();
    if (autoStartGame) StartGame();
  }

  private const string gameStartGroupName = "GAME START";

  [FoldoutGroup("$gameStartGroupName")]
  [SerializeField] bool autoStartGame = true;

  [FoldoutGroup("$gameStartGroupName")]
  [EnumToggleButtons, LabelText("Enable Spawners")]
  [SerializeField] Target spawnersToEnableTarget = Target.All;

  [FoldoutGroup("$gameStartGroupName")]
  [SceneObjectsOnly, ShowIf(nameof(spawnersToEnableTarget), Target.Custom)]
  [SerializeField] Spawner[] spawnersToEnable;

  [FoldoutGroup("$gameStartGroupName")]
  public UnityEvent OnGameStart = new UnityEvent();

  // TODO: Object to activate

  [FoldoutGroup("$gameStartGroupName")]
  [Button]
  [GUIColor(1f, .6f, .6f)]
  public void StartGame() {
    gameOver = false;
    if (spawnersToEnableTarget == Target.All) spawnersToEnable = FindObjectsOfType<Spawner>();
    foreach (var obj in spawnersToEnable) {
      if (!obj.autoSpawnEnabled) return; // only trigger for which spawner is setup enable
      obj.StartAutoSpawning();
    }
    OnGameStart.Invoke();
    if (timerStat.enable) StartCoroutine(TimerEnumerator());
  }

  public void RestartGame() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
  #endregion

  #region GAME OVER ===================================================================================================================================
  private const string gameOverGroupName = "GAME OVER";

  [DisplayAsString, HideInInspector] public bool gameOver = false;

  private enum Target { All, Custom }

  [FoldoutGroup("$gameOverGroupName")]
  [EnumToggleButtons, LabelText("Stop Projectiles")]
  [SerializeField] Target projectilesToStopTarget = Target.All; // TODO: replace all Projectile by Transforming

  [FoldoutGroup("$gameOverGroupName")]
  [SceneObjectsOnly, ShowIf(nameof(projectilesToStopTarget), Target.Custom)]
  [SerializeField] ArchievedProjectile[] projectilesToStop;

  [FoldoutGroup("$gameOverGroupName")]
  [EnumToggleButtons, LabelText("Stop Transforming")]
  [SerializeField] Target transformingToStopTarget = Target.All;

  [FoldoutGroup("$gameOverGroupName")]
  [SceneObjectsOnly, ShowIf(nameof(transformingToStopTarget), Target.Custom)]
  [SerializeField] TransformOperator[] transformingToStop;

  [FoldoutGroup("$gameOverGroupName")]
  [EnumToggleButtons, LabelText("Stop Spawners")]
  [SerializeField] Target spawnersToStopTarget = Target.All;

  [FoldoutGroup("$gameOverGroupName")]
  [SceneObjectsOnly, ShowIf(nameof(spawnersToStopTarget), Target.Custom)]
  [SerializeField] Spawner[] spawnersToStop;

  [FoldoutGroup("$gameOverGroupName")]
  [SceneObjectsOnly, LabelText("Destroy Objects")]
  [SerializeField] GameObject[] objectsToDestroy;

  [FoldoutGroup("$gameOverGroupName")]
  public UnityEvent OnGameOver = new UnityEvent();

  [FoldoutGroup("$gameOverGroupName")]
  [Button]
  [GUIColor(1f, .6f, .6f)]
  public void SetGameOver() {
    if (gameOver) return;

    gameOver = true;
    OnGameOver.Invoke();
    if (projectilesToStopTarget == Target.All) projectilesToStop = FindObjectsOfType<ArchievedProjectile>();
    if (transformingToStopTarget == Target.All) transformingToStop = FindObjectsOfType<TransformOperator>();
    if (spawnersToStopTarget == Target.All) spawnersToStop = FindObjectsOfType<Spawner>();
    foreach (var obj in projectilesToStop) obj.Stop();
    foreach (var obj in transformingToStop) obj.Stop();
    foreach (var obj in spawnersToStop) obj.StopAutoSpawning();
    foreach (var obj in objectsToDestroy) Destroy(obj);
    gameOverLabel.gameObject.SetActive(true);
    restartButton.SetActive(true);
  }
  #endregion ===================================================================================================================================

  #region STATS & UI ===================================================================================================================================
  private const string statsGroupName = "STATS & UI";

  [FoldoutGroup("$statsGroupName")]
  [GUIColor("@Color.cyan")]
  [SerializeField, HideLabel] public StatGame livesStat = new StatGame(statName: "Lives", triggerGameOverValue: StatGame.StatEvent.Zero);

  [FoldoutGroup("$statsGroupName")]
  [GUIColor("@Color.yellow")]
  [SerializeField, HideLabel] public StatGame scoresStat = new StatGame(statName: "Scores");
  [FoldoutGroup("$statsGroupName")]

  [GUIColor("@Color.cyan")]
  [SerializeField, HideLabel] public StatGame timerStat = new StatGame(statName: "Timer", triggerGameOverValue: StatGame.StatEvent.Zero);

  public void UpdateLives(int amountToAdd) {
    livesStat.Update(amountToAdd);
    // Color.grey
  }

  public void UpdateScores(int amountToAdd) {
    scoresStat.Update(amountToAdd);
  }

  private void SetupStats() {
    livesStat.SetupGameEvents();
    scoresStat.SetupGameEvents();
    timerStat.SetupGameEvents();
  }

  public IEnumerator TimerEnumerator() {
    while (timerStat.CurrentValue > timerStat.MinValue) {
      yield return new WaitForSeconds(1.0f);
      timerStat.Descrease();
    }
  }
  #endregion

  #region GAME UI ===================================================================================================================================
  private const string gameUIGroupName = "GAME UI";

  [FoldoutGroup("$gameUIGroupName")]
  [SerializeField] EventSystem eventSystem;

  [FoldoutGroup("$gameUIGroupName")]
  [SerializeField] TextMeshProUGUI gameOverLabel;

  [FoldoutGroup("$gameUIGroupName")]
  [SerializeField] GameObject restartButton;
  #endregion ===================================================================================================================================

  #region AUDIO ===================================================================================================================================
  private const string audioGroupName = "AUDIO";

  [FoldoutGroup("$audioGroupName")]
  [SerializeField] public AudioSource audioSource;

  // ! FIX: clip link with other scene in same project if apply to prefab
  [FoldoutGroup("$audioGroupName")]
  [SerializeField] AudioClip backgroundMusic;
  #endregion ===================================================================================================================================
}
