# Project_Raster

## Vision
A stealth action game inspired by the original Metal Gear Solid games, especially **MGS2**. Judge design decisions against these pillars:
- **Stealth first.** Avoiding detection is the core loop; combat is the fallback.
- **Enemy alert phases.** Patrol → Caution/Alert/Evasion → back to Patrol, with enemies reacting to sight and sound.
- **Fixed/zone cameras.** Authored per-zone cameras (MGS2 overhead feel) plus a first-person look/aim mode.
- **Non-lethal options.** KO, tranquilize and hold-up as real alternatives to killing.

When asked "how would MGS2 do this", say how sure you are about the reference; don't invent mechanics.

## Stack
Unity 6000.0.45f1, URP 17 (pixelated look via low render scale).
- Cinemachine 3: `Unity.Cinemachine` and `CinemachineCamera`, not the v2 API.
- Input System: generated `InputActions` class.
- AI Navigation (NavMesh).

Scene: `Assets/Scenes/Test.unity`. FBX files are in Git LFS.

## Architecture (`Assets/Scripts/`)
- **Actor pattern (copy it for new actors):**
  - `XController` (MonoBehaviour) builds an `XContext` (readonly refs) plus a `Dictionary<enum, IState>`.
  - It inits `StateMachine<TState,TContext>` in `Start()` (never `Awake`) and ticks it in `Update`.
  - `States/` are thin and delegate to MonoBehaviour `Systems/`.
- **Player:** states Free / Cover / FirstPerson.
  - Movement systems derive from `MovementType`.
  - `CoverMovement` detects walls (tag `Cover`) and raises `OnCoverEntered`/`OnCoverExited`.
  - FirstPerson is hold-to-aim and returns to the previous state on release.
  - Movement is camera-relative (`PlayerController.GetMovementDirection`).
- **Enemy:** Patrol via `EnemyPatrol` (a `NavMeshAgent` following a waypoint route). `Alert`/`KO` exist in the enum but aren't implemented.
- **Camera:**
  - `BaseCamera` wraps a `CinemachineCamera`.
  - `CameraManager.LateUpdate` sets priorities from the player state: Explore 10, Cover 20 (only when near an edge), FirstPerson 30.
  - A `CameraZone` trigger (tag `Player`) fires the static `ZoneEvents.onCameraZoneEnter`, which swaps in the zone's child `ExploreCamera`.
- Animators read the controller's state queries (`IsRunning`, `IsWalking`, ...).

## Conventions
- No namespaces. One class per file, named after the class.
- `[SerializeField] private` camelCase fields; tuning values get a trailing `//` comment.
- Cross-system communication goes through C# events. No `SendMessage` or `Find*`.

## Known issues (don't fix unless asked)
- **Aim from cover is undecided.** `CoverState.Exit` calls `ResetCover()`, so releasing aim returns to Cover with wiped wall data. The choice is MGS2-style "return to wall" (keep the cover data) or "aiming breaks cover" (go to Free).
- `CameraManager` subscribes to the static `ZoneEvents` and never unsubscribes.
- `FirstPersonCamera` duplicates `FIRST_PERSON_PRIORITY` and detects whether it's active by priority value.
- The `EnemyPatrol` wait coroutine survives state changes and will override the Alert destination.
- `PlayerController` mixes input, state and queries; split input out before adding more player states.
- No gravity in any movement system.
