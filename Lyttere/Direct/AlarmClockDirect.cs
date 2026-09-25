namespace Lyttere.Direct;

public class AlarmClockDirect(PersonDirect person)
{
    // TODO: hold a reference to one Person (constructor parameter, stored in a field).
    private PersonDirect _person = person;

    public void StartAlarm(string message) => _person.WakeUp(message);

        // TODO: call the person's wake-up method directly, passing `message`.
}
