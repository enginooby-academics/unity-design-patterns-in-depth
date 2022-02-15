using System;

namespace EventQueuePattern.Case1.Base1 {
  public class FirstCommand : ICommand {
    public Action OnFinished { get; set; }

    public void Execute() {
      PopupManager.Instance.FirstPopup.gameObject.SetActive(true);
      PopupManager.Instance.FirstPopup.OnClosed += OnPopupClosed;
    }

    private void OnPopupClosed() {
      PopupManager.Instance.FirstPopup.OnClosed -= OnPopupClosed;
      PopupManager.Instance.FirstPopup.gameObject.SetActive(false);
      OnFinished?.Invoke();
    }
  }
}