using UnityEngine;
using UnityEngine.Events;

public class InputState : MonoBehaviour
{
    [SerializeField] private bool canPauseTime = true;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private KeyCode inputKey = KeyCode.Escape;
    [SerializeField] private UnityEvent stateEvent;
    private bool _currentState = false;

    private void Update()
    {
        if (Input.GetKeyDown(inputKey))
            InvokeStateEvent();
    }

    public void InvokeStateEvent()
    {
        stateEvent?.Invoke();
    }

    public void ChangeState()
    {
        _currentState = !_currentState;
        //SetMouseState();

        if (!_currentState)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            TimeGameState(1);
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            TimeGameState(0);
        }
    }

    private void SetMouseState()
    {
        if (!_currentState)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void TimeGameState(float time)
    {
        if (canPauseTime)
            Time.timeScale = time;
    }
}
