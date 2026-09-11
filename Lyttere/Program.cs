using Direct;
internal class Program
{
    private static void Main(string[] args)
    {
        // Part 1: once Direct.AlarmClock / Direct.Person are implemented, try them here.
        // Create a Person, create an AlarmClock that holds it, call StartAlarm.

        PersonDirect personDirect1 = new PersonDirect("Joakim");
        AlarmClockDirect clockDirect1 = new AlarmClockDirect(personDirect1);
        clockDirect1.StartAlarm("God morgen fra direct!");


        Console.WriteLine();
        // Part 2: once EventBased.AlarmClock / EventBased.Person are implemented, try them here.
        // Create an AlarmClock, create a Person that subscribes to it, call StartAlarm.

        // Part 2 extra: create a second Person subscribed to the same AlarmClock and call
        // StartAlarm once — both should wake up.
    }
}
