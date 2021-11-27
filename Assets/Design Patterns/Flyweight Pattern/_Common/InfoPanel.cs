using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Flyweight {
  public class InfoPanel : MonoBehaviourSingleton<InfoPanel> {
    public TextMeshProUGUI specieLabel;
    public TextMeshProUGUI endangeredLabel;
    public TextMeshProUGUI massLabel;
    public TextMeshProUGUI genderLabel;
  }
}
