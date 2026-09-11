# Lyttere (events) — flow-first version

Owner: apprentice. Reviewer: Jørgen.

This replaces `listeners-oppgave.md`. Same goal — warm-up for open question 3 in `ChatManager-Spec.md` — but split into two separate builds, done one at a time. Build Part 1 fully before you read Part 2. Don't skim ahead; the point is to feel the first shape as "just how you'd do it" before the second one shows up as an alternative.

No test project for this exercise — run both variants from `Program.cs` and check the console output. Testability is still the whole point of Part 3, you just reason about it instead of writing it down in code.

The project is already scaffolded for you:

- `Direct/AlarmClock.cs`, `Direct/Person.cs` — for Part 1
- `EventBased/AlarmClock.cs`, `EventBased/Person.cs` — for Part 2
- `Program.cs` — empty `Main`, with comments marking where each part's try-it-out code goes

Each class file has `// TODO` comments marking what goes where. Fill them in — don't restructure the files or rename the classes.

---

## Part 1 — the direct way

**Scenario:** an alarm clock rings. A person wakes up because of it.

**Flow:**

1. Something calls `alarm.StartAlarm(message)`.
2. Inside `StartAlarm`, the alarm clock calls `person.WakeUp(message)` — directly, by name.
3. `Person.WakeUp` runs and prints that it woke up.

```
StartAlarm(message) → person.WakeUp(message) → "Alice woke up because: ..."
```

**Build it:**

- `AlarmClock` — holds a reference to one `Person` (constructor parameter). `StartAlarm(string message)` calls that person's wake-up method directly.
- `Person` — has a name (constructor parameter) and a method that takes the alarm message and prints something like `"{name} woke up because: {message}"`.

Write it yourself from that description — don't copy the shape from anywhere else.

Wire it up in `Program.cs` and run it — confirm the message prints.

Once this compiles and runs correctly, stop. Go to Part 2.

---

## Part 2 — the event way

Same scenario, same two-word summary — "alarm rings, person wakes up" — but now `AlarmClock` is not allowed to know that `Person` exists. No field, no constructor parameter, no `using` of `Person`'s type anywhere in `AlarmClock`.

**Flow:**

1. Something calls `alarm.StartAlarm(message)`.
2. Inside `StartAlarm`, the alarm clock raises an event — `OnAlarmTriggered?.Invoke(message)`. It has no idea who, if anyone, is listening.
3. Separately, `Person` subscribed to `OnAlarmTriggered` earlier (in its constructor, given the alarm clock to subscribe to).
4. Because of that subscription, `Person`'s handler runs when the event fires.

```
StartAlarm(message) → OnAlarmTriggered?.Invoke(message)
                                    ↓
                (elsewhere, subscribed earlier) Person's handler runs
```

**Build it:**

- `AlarmClock` — declares `public event Action<string>? OnAlarmTriggered;` and invokes it from `StartAlarm`. That's the entire class. No reference to `Person` anywhere.
- `Person` — takes an `AlarmClock` in its constructor and subscribes one of its own methods to `OnAlarmTriggered` right there (`alarmClock.OnAlarmTriggered += WakeUp;`).

Use the real `event` keyword — not a bare `Action<string>` field. A bare field lets any outside code overwrite or invoke it directly, which defeats the point. If you want a second reference for the syntax, look at `ChatSession.StateChanged` in the real project — don't search the web.

**Extra step, only in this variant:** create *two* `Person` instances subscribed to the same `AlarmClock` and call `StartAlarm` once, in `Program.cs`. Both should wake up. Note what that would have taken in Part 1's shape.

---

## Common gotchas

- **`NullReferenceException` on invoke.** Before anyone subscribes, `OnAlarmTriggered` is `null`. `OnAlarmTriggered.Invoke(message)` throws; `OnAlarmTriggered?.Invoke(message)` doesn't. Always use the `?.`.
- **Signature mismatch.** `alarmClock.OnAlarmTriggered += WakeUp;` only compiles if `WakeUp` takes exactly one `string` parameter and returns `void` — matching `Action<string>`. A mismatch is a compile error, not a runtime one, so the fix is usually "look at the parameter list," not "look at the wiring."
- **Subscribing twice.** Calling `+=` on the same handler twice makes it run twice per event. If you re-wire the same `Person` into `Program.cs` more than once, you'll see duplicate wake-up lines and it'll look like a bug in `AlarmClock` when it isn't.
- **Build order in Part 2.** `Person`'s constructor needs an `AlarmClock` to subscribe to. Construct the `AlarmClock` first in `Program.cs`, then pass it into `Person` — not the other way around.
- **A bare field instead of `event`.** If `OnAlarmTriggered` compiles as a plain `Action<string>?` field (no `event` keyword), `+=` still works, so it can look correct while quietly allowing `alarm.OnAlarmTriggered = null` or `alarm.OnAlarmTriggered.Invoke(...)` from outside `AlarmClock`. Covered above — flagging it again because it's easy to type the field version out of habit and not notice the missing keyword.

---

## Part 3 — reflect

Now that both are built and running, answer these using your own two implementations as the example — not abstractly:

1. Imagine you had to write an automated test proving `StartAlarm` reached someone — no console, no eyeballing output. In Part 1's shape, what would that test have to construct? In Part 2's shape, what would it construct instead?
2. Which one lets you test `AlarmClock` with **zero knowledge** of what a `Person` even is?
3. Part 2 got a second listener almost for free. What would adding a second listener have required in Part 1's shape?
4. Is there a cost to Part 2? (Hint: try to answer "who wakes up when the alarm rings?" by reading only `AlarmClock.cs`, for each variant.)

## Bridge back to the real spec

The real question is `ChatManager-Spec.md` §9 question 3: when `ChatManager` deletes the active chat, does it reach into `ChatSession` and reset it directly, or does it raise something and let `ChatSession` handle it itself? That's exactly Part 1 vs Part 2, with `ChatManager` playing `AlarmClock` and `ChatSession` playing `Person`. Answer that question — a few sentences, with a reason drawn from what you just built, not a guess — and bring it to Jørgen along with question 1 (Scoped/Singleton/Transient) from the DI sandbox.

## Definition of done

- [ ] Part 1 compiles and runs, printing the wake-up message.
- [ ] Part 2 compiles and runs, and you've seen two listeners fire from one `StartAlarm` call.
- [ ] You've answered all four questions in Part 3 out loud, using your own code as the example.
- [ ] You've written your answer to the bridge question above.
