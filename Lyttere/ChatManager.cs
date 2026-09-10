namespace balleB;

public class ManagerEvent
{
    public event Action? ChatDeleted;

    public void DeleteActive()
    {
        Console.WriteLine("Deleting chat..");
        ChatDeleted?.Invoke();
    }
}