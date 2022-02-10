#if ENVIRO_LW || ENVIRO_HD
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour {
  private EnviroSkyMgr weatherMgr;
  private List<EnviroWeatherPreset> weatherPresets;
  private EnviroWeatherPreset currentWeatherPreset;

  void Start() {
    weatherMgr = EnviroSkyMgr.instance;
    weatherPresets = weatherMgr.Weather.weatherPresets;
    currentWeatherPreset = weatherMgr.Weather.currentActiveWeatherPreset;
  }

  void Update() {
    if (Input.GetKeyUp(KeyCode.Keypad0)) SwitchWeather();
    if (Input.GetKeyUp(KeyCode.Keypad1)) ToggleFog();
    if (Input.GetKeyUp(KeyCode.Keypad2)) ToggleFlatClouds();
    if (Input.GetKeyUp(KeyCode.Keypad3)) ToggleParticleClouds();
    if (Input.GetKeyUp(KeyCode.Keypad4)) ToggleAurora();
    if (Input.GetKeyUp(KeyCode.Keypad7)) ToggleTimeProgression();
    if (Input.GetKeyUp(KeyCode.Keypad8)) SetDayTime();
    if (Input.GetKeyUp(KeyCode.Keypad9)) SetNightTime();
  }

  public void ToggleFog() {
    weatherMgr.useFog = !weatherMgr.useFog;
  }

  public void ToggleFlatClouds() {
    weatherMgr.useFlatClouds = !weatherMgr.useFlatClouds;
  }

  public void ToggleParticleClouds() {
    weatherMgr.useParticleClouds = !weatherMgr.useParticleClouds;
  }

  public void ToggleAurora() {
    weatherMgr.useAurora = !weatherMgr.useAurora;
  }

  public void SetDayTime() {
    int currentYear = weatherMgr.Time.Years;
    int currentDay = weatherMgr.Time.Days;
    weatherMgr.SetTime(year: currentYear, dayOfYear: currentDay, hour: 12, minute: 0, seconds: 0);
  }

  public void SetNightTime() {
    int currentYear = weatherMgr.Time.Years;
    int currentDay = weatherMgr.Time.Days;
    weatherMgr.SetTime(year: currentYear, dayOfYear: currentDay, hour: 24, minute: 0, seconds: 0);
  }

  public void ToggleTimeProgression() {
    weatherMgr.Time.ProgressTime = weatherMgr.Time.ProgressTime == EnviroTime.TimeProgressMode.None
      ? EnviroTime.TimeProgressMode.Simulated
      : EnviroTime.TimeProgressMode.None;
  }

  public void SwitchWeather() {
    currentWeatherPreset = weatherPresets.NavNext(currentWeatherPreset);
    weatherMgr.ChangeWeather(currentWeatherPreset);
  }
}
#endif