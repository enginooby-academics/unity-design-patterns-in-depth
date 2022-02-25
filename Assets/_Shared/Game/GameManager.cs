using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// ? CONSIDER
// ? Time stat
// ? Difficulty stat

public class GameManager : MonoBehaviourSingleton<GameManager> {
  // ===================================================================================================================

  #region GAME LOAD

  private const string GameLoadGroupName = "GAME LOAD";

  [FoldoutGroup("$" + nameof(GameLoadGroupName))]
  public UnityEvent OnGameLoad = new();

  private void Setup() {
    SetupStats();
    if (gameOverLabel) gameOverLabel.gameObject.SetActive(false);
    if (restartButton) restartButton.SetActive(false);

    if (backgroundMusic && audioSource) {
      audioSource.clip = backgroundMusic;
      audioSource.Play();
    }

    if (FindObjectsOfType<EventSystem>().Length > 1) Destroy(eventSystem.gameObject);
    OnGameLoad.Invoke();
  }

  #endregion

  // ===================================================================================================================

  #region GAME START

  private void Start() {
    Setup();
    if (autoStartGame) StartGame();
  }

  private const string GameStartGroupName = "GAME START";

  [FoldoutGroup("$GameStartGroupName")] [SerializeField]
  private bool autoStartGame = true;

  [FoldoutGroup("$GameStartGroupName")] [EnumToggleButtons] [LabelText("Enable Spawners")] [SerializeField]
  private Target spawnersToEnableTarget = Target.All;

  [FoldoutGroup("$GameStartGroupName")]
  [SceneObjectsOnly]
  [ShowIf(nameof(spawnersToEnableTarget), Target.Custom)]
  [SerializeField]
  private Spawner[] spawnersToEnable;

  [FoldoutGroup("$GameStartGroupName")] public UnityEvent OnGameStart = new();

  // TODO: Object to activate

  [FoldoutGroup("$GameStartGroupName")]
  [Button]
  [GUIColor(1f, .6f, .6f)]
  public void StartGame() {
    gameOver = false;
    if (spawnersToEnableTarget == Target.All) spawnersToEnable = FindObjectsOfType<Spawner>();
    foreach (var obj in spawnersToEnable) {
      // TODO
      // if (!obj.AutoSpawnMode.) return; // only trigger for which spawner is setup enable
      // obj.StartAutoSpawning();
    }

    OnGameStart.Invoke();
    if (timerStat.enable) StartCoroutine(TimerEnumerator());
  }

  public void RestartGame() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  #endregion

  // ===================================================================================================================

  #region GAME OVER

  private const string gameOverGroupName = "GAME OVER";

  [DisplayAsString] [HideInInspector] public bool gameOver;

  private enum Target {
    All,
    Custom,
  }

  [FoldoutGroup("$gameOverGroupName")] [EnumToggleButtons] [LabelText("Stop Projectiles")] [SerializeField]
  private Target projectilesToStopTarget = Target.All; // TODO: replace all Projectile by Transforming

  [FoldoutGroup("$gameOverGroupName")]
  [SceneObjectsOnly]
  [ShowIf(nameof(projectilesToStopTarget), Target.Custom)]
  [SerializeField]
  private ArchievedProjectile[] projectilesToStop;

  [FoldoutGroup("$gameOverGroupName")] [EnumToggleButtons] [LabelText("Stop Transforming")] [SerializeField]
  private Target transformingToStopTarget = Target.All;

  [FoldoutGroup("$gameOverGroupName")]
  [SceneObjectsOnly]
  [ShowIf(nameof(transformingToStopTarget), Target.Custom)]
  [SerializeField]
  private TransformOperator[] transformingToStop;

  [FoldoutGroup("$gameOverGroupName")] [EnumToggleButtons] [LabelText("Stop Spawners")] [SerializeField]
  private Target spawnersToStopTarget = Target.All;

  [FoldoutGroup("$gameOverGroupName")]
  [SceneObjectsOnly]
  [ShowIf(nameof(spawnersToStopTarget), Target.Custom)]
  [SerializeField]
  private Spawner[] spawnersToStop;

  [FoldoutGroup("$gameOverGroupName")] [SceneObjectsOnly] [LabelText("Destroy Objects")] [SerializeField]
  private GameObject[] objectsToDestroy;

  [FoldoutGroup("$gameOverGroupName")] public UnityEvent OnGameOver = new();

  [FoldoutGroup("$gameOverGroupName")]
  [Button]
  [GUIColor(1f, .6f, .6f)]
  public void SetGameOver() {
    if (gameOver) return;

    gameOver = true;
    OnGameOver.Invoke();
    gameOverLabel.gameObject.SetActive(true);
    restartButton.SetActive(true);

    if (projectilesToStopTarget == Target.All) projectilesToStop = FindObjectsOfType<ArchievedProjectile>();
    if (transformingToStopTarget == Target.All) transformingToStop = FindObjectsOfType<TransformOperator>();
    if (spawnersToStopTarget == Target.All) spawnersToStop = FindObjectsOfType<Spawner>();

    foreach (var obj in projectilesToStop) obj.Stop();
    foreach (var obj in transformingToStop) obj.Stop();
    foreach (var obj in spawnersToStop) obj.AutoSpawnScheduler.Disable();
    foreach (var obj in objectsToDestroy) Destroy(obj);
  }

  #endregion

  // ===================================================================================================================

  #region STATS & UI

  private const string statsGroupName = "STATS & UI";

  [FoldoutGroup("$statsGroupName")] [GUIColor("@Color.cyan")] [SerializeField] [HideLabel]
  public StatGame livesStat = new("Lives", triggerGameOverValue: StatGame.StatEvent.Zero);

  [FoldoutGroup("$statsGroupName")] [GUIColor("@Color.yellow")] [SerializeField] [HideLabel]
  public StatGame scoresStat = new("Scores");

  [FoldoutGroup("$statsGroupName")] [GUIColor("@Color.cyan")] [SerializeField] [HideLabel]
  public StatGame timerStat = new("Timer", triggerGameOverValue: StatGame.StatEvent.Zero);

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

  // ===================================================================================================================

  #region GAME UI

  private const string gameUIGroupName = "GAME UI";

  [FoldoutGroup("$gameUIGroupName")] [SerializeField]
  private EventSystem eventSystem;

  [FoldoutGroup("$gameUIGroupName")] [SerializeField]
  private TextMeshProUGUI gameOverLabel;

  [FoldoutGroup("$gameUIGroupName")] [SerializeField]
  private GameObject restartButton;

  #endregion

  // ===================================================================================================================

  #region AUDIO

  private const string audioGroupName = "AUDIO";

  [FoldoutGroup("$audioGroupName")] [SerializeField]
  public AudioSource audioSource;

  // ! FIX: clip link with other scene in same project if apply to prefab
  [FoldoutGroup("$audioGroupName")] [SerializeField]
  private AudioClip backgroundMusic;

  #endregion

  // ===================================================================================================================
}