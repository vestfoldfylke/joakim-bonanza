# Lyttere (events) — learning exercise

Owner: apprentice. Reviewer: Jørgen.

This is a warm-up for open question 3 in `ChatManager-Spec.md`:

> On `DeleteAsync` of the active chat: does `ChatManager` reach into `ChatSession` to reset it, or does it raise something and let the component handle it? What does that imply about testability?

You cannot answer that from three sentences about "the observer pattern". You need to write both versions yourself and try to test them. This exercise is that, stripped down to almost nothing.

---

## 1. Why this exists

`ChatManager` will need to tell something else that a chat got deleted. There are two shapes that can take:

- **Direct call**: `ChatManager` holds a reference to `ChatSession` and calls `session.Reset()` itself.
- **Event**: `ChatManager` exposes an `event Action` (or similar) and fires it. Whoever cares — `ChatSession`, a component, a test — subscribes and reacts on its own.

Both compile. Both "work" if you click through the UI once. The difference only shows up when you try to **test `ChatManager` in isolation**, and that's exactly the situation you're in for §8 of the spec (you're told not to mock `ChatSession`, and mocking it turned out to be awkward — that awkwardness is the point of this exercise).

## 2. What you'll be able to answer at the end

1. In the direct-call version, what does a test need in order to prove the reset happened?
2. In the event version, what does a test need instead?
3. Which version lets you test `Manager` with **zero knowledge** of what `Session` even is?
4. Map this back onto `ChatManager`/`ChatSession`: which shape does the real spec's testability requirement in §8 point you toward, and why?

## 3. Setup

- New console project: `dotnet new console -o Lyttere` (put it next to `DOTNET-MORO`, at the repo root).
- New test project: `dotnet new xunit -o Lyttere.Tests`, with a project reference to `Lyttere`.
- One file per variant in `Lyttere`: `ManagerDirect.cs` and `ManagerEvent.cs`. Small — under 20 lines each.

## 4. What to build

### Variant A — direct call

```csharp
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
```

Write this yourself, don't just paste it — type it out so the shape sticks.

### Variant B — event

Write a second pair of classes, `SessionListener` and `ManagerEvent`, where:

- `ManagerEvent` exposes an `event Action? ChatDeleted;` and raises it from `DeleteActive()`. It does **not** hold a reference to any `Session`-like class at all.
- `SessionListener` (a stand-in for what `ChatSession` would do) subscribes to `ChatDeleted` in its constructor and sets its own `IsLoaded = false` when the event fires.

Don't look up a sample "C# event tutorial" and copy it — you already have the shape from `ChatSession.StateChanged`. Open that file, look at how it declares and raises its event, and reuse the same pattern.

## 5. What to test

In `Lyttere.Tests`, write two test classes.

**`ManagerDirectTests`** — prove `DeleteActive()` puts the session in a reset state. Ask yourself before you write it: what do you have to construct and pass in to make this test compile at all?

**`ManagerEventTests`** — prove `DeleteActive()` raises `ChatDeleted`. Do **not** construct a `SessionListener` to do this. Instead, subscribe a small local delegate directly in the test (a counter, or a bool flag you flip) and assert on that.

```csharp
[Fact]
public void DeleteActive_RaisesChatDeleted_ExactlyOnce()
{
    var manager = new ManagerEvent();
    var raisedCount = 0;
    manager.ChatDeleted += () => raisedCount++;

    manager.DeleteActive();

    Assert.Equal(1, raisedCount);
}
```

Notice what that test does *not* need: no `Session`, no `SessionListener`, no mocking library.

## 6. What to observe

Run both test classes. Then write down, in your own words:

- What extra thing did `ManagerDirectTests` need that `ManagerEventTests` didn't?
- If `Session`'s constructor later needs a database connection, which test class breaks first?
- Is `ManagerEvent` easier or harder to read for someone who wants to know "what happens when a chat is deleted, everywhere"? (Hint: there's a real cost to the event version too — don't treat this as a one-sided win.)

## 7. Definition of done

- [ ] Both variants compile and run.
- [ ] Both test classes pass.
- [ ] You can answer all four questions in §2 out loud, using *your* two variants as the concrete example — not abstractly.
- [ ] You have re-read `ChatSession.cs`'s `StateChanged` event and can point at the line that raises it.

## 8. What NOT to do

- Do not add a mocking library (Moq, NSubstitute, ...) to `Lyttere.Tests`. If you feel like you need one for `ManagerDirectTests`, that feeling is the answer to question 3 — sit with it before moving on.
- Do not make `Session` an interface "to make it mockable". That's solving the symptom, not answering the question.
- Do not copy code from the internet. Look at `ChatSession.cs` for the event pattern; write everything else yourself.

## 9. Bridge back to the real spec

When you're done, go back to `ChatManager-Spec.md` §9 question 3 and write your actual answer — a few sentences, with a reason, not a guess. Use the vocabulary from this exercise: does `ChatManager` "reach into" `ChatSession`, or does it "raise and let something else react"? Which one did your tests just tell you is easier to verify in isolation?

Bring your answer to Jørgen, along with question 1 (Scoped/Singleton/Transient) from the DI sandbox.
