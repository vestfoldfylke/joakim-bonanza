namespace EventBased;

public class PersonEvent
{
    // TODO: store the person's name (constructor parameter).
    public String name;
    public AlarmClockEvent alarmClock;
    public bool hasWokenUp = false;

    public PersonEvent(string name, AlarmClockEvent alarmClock)
    {
        this.name = name;
        this.alarmClock = alarmClock;

        // TODO: subscribe WakeUp to alarmClock.OnAlarmTriggered here.
        alarmClock.OnAlarmTriggered += WakeUp;
    }

    // TODO: expose something a test can check afterwards, e.g. a bool HasWokenUp.

    private void WakeUp(string message)
    {
        // TODO: record that this person woke up (e.g. set HasWokenUp = true).
        // TODO: print something like "{name} woke up because: {message}".

        hasWokenUp = true;
        Console.WriteLine($"{name} woke up because: {message}");
    }
}
