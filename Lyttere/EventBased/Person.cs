namespace EventBased;

public class Person
{
    // TODO: store the person's name (constructor parameter).

    public Person(string name, AlarmClock alarmClock)
    {
        // TODO: subscribe WakeUp to alarmClock.OnAlarmTriggered here.
    }

    // TODO: expose something a test can check afterwards, e.g. a bool HasWokenUp.

    private void WakeUp(string message)
    {
        // TODO: record that this person woke up (e.g. set HasWokenUp = true).
        // TODO: print something like "{name} woke up because: {message}".
    }
}
