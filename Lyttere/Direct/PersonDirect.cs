namespace Lyttere.Direct;

public class PersonDirect(string name)
{
    // TODO: store the person's name (constructor parameter).
    public string Name { get; private set; } = name;

    // TODO: expose something a test can check afterwards, e.g. a bool HasWokenUp.

    // Jeg vil at HasWokenUp skal være tilgjengelig å lese, men ikke mulig å endre - for at man skal kunne vite om Jørgen sover eller ikke
    public bool HasWokenUp { get; private set; } = false;

    public void WakeUp(string message)
    {
        // TODO: record that this person woke up (e.g. set HasWokenUp = true).
        // TODO: print something like "{name} woke up because: {message}".

        HasWokenUp = true;
        Console.WriteLine($"{Name} woke up because: {message}");
    }
}
