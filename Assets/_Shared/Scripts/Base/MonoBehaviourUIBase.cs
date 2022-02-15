using UnityEngine.UI;

public abstract class MonoBehaviourUIBase : MonoBehaviourBase {
  protected Image _image => My<Image>();
  protected Button _button => My<Button>();
}