namespace balleA;
public class Session
{
    public bool IsLoaded { get; private set; } = true;
    public void Reset() => IsLoaded = false;
}

public class ManagerDirect
{
    private readonly Session _session;
    public ManagerDirect(Session session) => _session = session;

    public void DeleteActive() => _session.Reset();
}