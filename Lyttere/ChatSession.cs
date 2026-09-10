namespace balleB;
public class SessionListener()
{
    private ManagerEvent _manager = new ManagerEvent();
    public int count = 0;

    public void InitRunAction()
    {
        _manager.ChatDeleted += Notify;

        _manager.DeleteActive();

        _manager.ChatDeleted -= Notify;
    }

    private static void Notify()
    {
        Console.WriteLine("Balle!!!");
    }
}
