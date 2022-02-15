using System;

namespace EventQueuePattern.Case1.Base1 {
  public class ThirdCommand : ICommand {
    public Action OnFinished { get; set; }

    public void Execute() {
      PopupManager.Instance.ThirdPopup.gameObject.SetActive(true);
      PopupManager.Instance.ThirdPopup.OnClosed += OnPopupClosed;
    }

    private void OnPopupClosed() {
      PopupManager.Instance.ThirdPopup.OnClosed -= OnPopupClosed;
      PopupManager.Instance.ThirdPopup.gameObject.SetActive(false);
      OnFinished?.Invoke();
    }
  }
}