using System.IO.Pipes;

namespace Direct;

public class AlarmClockDirect
{
    // TODO: hold a reference to one Person (constructor parameter, stored in a field).
    public PersonDirect person;

    public AlarmClockDirect(PersonDirect person)
    {
        this.person = person;
    }

    public void StartAlarm(string message)
    {
        // TODO: call the person's wake-up method directly, passing `message`.
        person.WakeUp(message);
    }
}
