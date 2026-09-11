namespace Direct;

public class PersonDirect
{
    // TODO: store the person's name (constructor parameter).
    public String name;

    public PersonDirect(String name)
    {
        this.name = name;
    }

    // TODO: expose something a test can check afterwards, e.g. a bool HasWokenUp.
    public bool HasWokenUp = false;

    public void WakeUp(string message)
    {
        // TODO: record that this person woke up (e.g. set HasWokenUp = true).
        // TODO: print something like "{name} woke up because: {message}".

        HasWokenUp = true;
        Console.WriteLine($"{name} woke up because: {message}");
    }
}
