public enum StatName : int {
  [StringValue(nameof(Health))]
  Health,

  [StringValue(nameof(Level))]
  Level,

  [StringValue(nameof(Mana))]
  Mana,

  [StringValue(nameof(Stamia))]
  Stamia,

  [StringValue(nameof(Experience))]
  Experience,

  /// <summary>
  /// Used for enemy.
  /// </summary>
  [StringValue(nameof(ExperienceReward))]
  ExperienceReward, // static stat

  /// <summary>
  /// Used for player to convert amount of XP to level for progression.
  /// </summary>
  [StringValue(nameof(ExperienceToLevel))]
  ExperienceToLevel, // stat to stat

  [StringValue(nameof(Scores))]
  Scores,

  [StringValue(nameof(Lives))]
  Lives,

  /// <summary>
  /// Static/fixed stat on per level.
  /// </summary>
  [StringValue(nameof(Strength))]
  Strength,
}