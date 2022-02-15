using System;

namespace EventQueuePattern.Case1.Base1 {
  public class SecondCommand : ICommand {
    public Action OnFinished { get; set; }

    public void Execute() {
      PopupManager.Instance.SecondPopup.gameObject.SetActive(true);
      PopupManager.Instance.SecondPopup.OnClosed += OnPopupClosed;
    }

    private void OnPopupClosed() {
      PopupManager.Instance.SecondPopup.OnClosed -= OnPopupClosed;
      PopupManager.Instance.SecondPopup.gameObject.SetActive(false);
      OnFinished?.Invoke();
    }
  }
}