namespace Lyttere.EventBased;

public class AlarmClockEvent
{
    // TODO: declare an event here, e.g. public event Action<string>? OnAlarmTriggered;
    // No reference to Person anywhere in this class — not a field, not a using, nothing.
    public event Action<string>? OnAlarmTriggered;

    public void StartAlarm(string message)
    {
        // TODO: raise the event, e.g. OnAlarmTriggered?.Invoke(message);
        OnAlarmTriggered?.Invoke(message);

        /* Til Jørgen:

        "?" Sjekker om det finnes noen subscribers, og returnerer null hvis det ikke er noen

        Alternativ måte ?:

        Action OnAlarmTriggered = action;
        if (OnAlarmTriggered != null)
        {
            OnAlarmTriggered();
        }

        */
    }
}
