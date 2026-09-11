namespace EventBased;

public class AlarmClock
{
    // TODO: declare an event here, e.g. public event Action<string>? OnAlarmTriggered;
    // No reference to Person anywhere in this class — not a field, not a using, nothing.

    public void StartAlarm(string message)
    {
        // TODO: raise the event, e.g. OnAlarmTriggered?.Invoke(message);
    }
}
