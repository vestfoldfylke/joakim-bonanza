using Direct;
using EventBased;

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

        AlarmClockEvent clockEvent1 = new AlarmClockEvent();
        PersonEvent personEvent1 = new PersonEvent("Joakim", clockEvent1);
        PersonEvent personEvent2 = new PersonEvent("Rune", clockEvent1);

        AlarmClockEvent clockEvent2 = new AlarmClockEvent();
        PersonEvent personEvent3 = new PersonEvent("Jørgen", clockEvent2);

        clockEvent1.StartAlarm("God morgen fra event!");
        Console.WriteLine($"{personEvent3.name} {(personEvent3.hasWokenUp ? "er våken" : "sover fremdeles!")}");
    }
}
