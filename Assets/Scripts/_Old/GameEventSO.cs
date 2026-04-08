using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Game/Event")]
public class GameEventSO : ScriptableObject
{
    public UnityEvent onInvoke;

    public void Invoke() => onInvoke?.Invoke();
}