# Project guide

- Unity Editor: **6000.6.8f1** (Unity 6).
- Render pipeline: **Universal Render Pipeline (URP) 17.6.0**, using the **2D Renderer**. Pipeline settings live in `Assets/Settings/UniversalRP.asset` and `Assets/Settings/Renderer2D.asset`.
- Input: **Unity Input System 1.20.1** (`com.unity.inputsystem`). Active Input Handling is set to the new Input System; use `UnityEngine.InputSystem` for gameplay input.

## Code and workflow

- Gameplay C# scripts live in `Assets/Scripts/`, including `Paddle.cs`, `Ball.cs`, `Brick.cs`, and `WinCondition.cs`. Add new gameplay scripts here.
- The main scene is `Assets/Scenes/SampleScene.unity`.
- This is a hobby project. Build functionality incrementally, explain each change, and keep code simple with only essential error checks.

## Ignore

Ignore `Library/`, `Temp/`, `Logs/`, `obj/`, and all `*.meta` files when searching, reviewing, or making manual edits. Leave generated files and Unity-managed metadata to Unity; do not delete them as cleanup.
