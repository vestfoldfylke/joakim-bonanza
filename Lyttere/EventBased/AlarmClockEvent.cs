namespace EventBased;

public class AlarmClockEvent
{
    // TODO: declare an event here, e.g. public event Action<string>? OnAlarmTriggered;
    // No reference to Person anywhere in this class — not a field, not a using, nothing.
    public event Action<string>? OnAlarmTriggered;

    public void StartAlarm(string message)
    {
        // TODO: raise the event, e.g. OnAlarmTriggered?.Invoke(message);
        OnAlarmTriggered?.Invoke(message);
    }
}
