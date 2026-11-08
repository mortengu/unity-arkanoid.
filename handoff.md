# Project handoff

## Goal and collaboration style

This is a simple 2D Arkanoid-style hobby game. Read `AGENTS.md` first.

The user wants to build the game incrementally, understand each change, and stay in control of the next feature. Keep scripts short and straightforward, with only essential error checks. Complete the requested piece, explain how it works, and let the user try it before expanding the game.

## Environment

- Unity **6000.6.8f1**.
- Universal Render Pipeline **17.6.0**, with the **2D Renderer**.
- Unity Input System **1.20.1**, with the new Input System enabled.
- uGUI Canvas and TextMeshPro for the win message; TMP essentials are imported.
- Unity Pipeline package **0.10.0-exp.1** provides live Editor commands through the Unity CLI.
- Gameplay scripts: `Assets/Scripts/`.
- Main scene: `Assets/Scenes/SampleScene.unity`.

Ignore `Library/`, `Temp/`, `Logs/`, `obj/`, and `*.meta` files when searching, reviewing, or manually editing. Let Unity manage metadata and generated files.

## What currently works

1. Move the paddle with **A/D** or **Left/Right Arrow**.
2. The ball starts above the paddle and follows it until **Space** is pressed.
3. Each launch chooses a random angle between **-45 and +45 degrees from straight up**. Ball speed remains constant.
4. The ball bounces off the paddle, side walls, top wall, and bricks.
5. A missed ball returns visibly to the paddle and waits for Space again. Existing brick damage is retained.
6. There are **24 bricks**, arranged in three colored rows of eight. A brick disappears when hit by the ball.
7. Clearing the last brick displays **YOU WIN!** with a "Press R to restart" line, deactivates the ball, and disables paddle movement.
8. After the win, pressing **R** reloads the scene for a fresh game with all 24 bricks.

There is no score, lives counter, game-over screen, sound, power-up system, or level progression yet. These are possible future steps, not approved work to implement automatically.

## Scripts

| Script | Responsibility |
| --- | --- |
| `Paddle.cs` | Reads keyboard input in `FixedUpdate()`, moves a kinematic `Rigidbody2D` with `MovePosition()`, and clamps horizontal movement. Defaults: speed 8, center limit ±6. |
| `Ball.cs` | Handles waiting, randomized Space launches, constant speed after collisions, and resetting after a miss. Default speed: 7. Holds an Inspector reference to the paddle Transform. |
| `Brick.cs` | In `OnCollisionEnter2D()`, checks for a `Ball` component and destroys its own GameObject. |
| `WinCondition.cs` | Attached to the `Bricks` parent. Detects zero remaining children and activates the win state. After the win, **R** reloads the active scene by build index. Holds references to the win message, ball, and paddle. |

## Scene structure and dimensions

```text
Main Camera
Global Light 2D
Paddle
  Paddle Art
Play Area
  Left Edge
  Right Edge
  Top Edge
Ball
  Ball Art
Bricks                         [WinCondition]
  Brick 1-1 ... Brick 3-8       [24 direct children]
Game UI                        [Canvas]
  Win Message                  [TextMeshPro, initially inactive]
```

- Camera: orthographic, size 5, dark background.
- Paddle: position `(0, -3.8, 0)`, visual and collider dimensions **2 × 0.3** world units. Kinematic Rigidbody2D and BoxCollider2D.
- Ball: diameter **0.32** world units, CircleCollider2D, dynamic Rigidbody2D, zero gravity, continuous collision detection, interpolation, and frozen rotation.
- Waiting ball offset: **0.45 units above the paddle**.
- Side wall inner edges: x = ±7. The bottom is open.
- Bricks: width 1.45, height 0.45; columns spaced 1.6 units apart. Rows are at y = 3.3, 2.65, and 2.0.
- `Assets/Physics/Bounce.physicsMaterial2D`: friction 0, bounciness 1. Used by the ball, paddle, walls, and bricks.

Keep only brick objects as direct children of `Bricks`: the win condition uses `transform.childCount`.

## Artwork

- `Assets/Art/paddle_image.png` and `Assets/Art/ball_image.png` are user-provided artwork.
- Their imported sprites use cropped rectangles around the visible artwork to exclude transparent padding. The original PNG files were not rewritten.
- `Paddle Art` and `Ball Art` are child GameObjects with white-tinted SpriteRenderers. Their local scales fit the imported sprites to the existing gameplay dimensions.
- The original SpriteRenderer on each parent is disabled. Parent scales, colliders, and gameplay scripts retain their existing settings.
- `Square.png` is the original reusable white sprite, still used by walls and bricks. `Circle.png` is the original ball sprite and remains in the project.
- Change the art children when replacing artwork; avoid changing parent dimensions or colliders unless the user requests gameplay size changes.

## Important fixes to preserve

- **Miss detection:** reset when `body.position.y < paddle.position.y - 0.3f`. The original fixed line at y = -5.5 caused shallow or horizontal misses to linger below the paddle.
- **Visible reset position:** while waiting, `body.simulated` is false, so position the visible ball through `transform.position`. Changing only `body.position` previously failed to show the ball back on the paddle reliably.
- **Launch position:** after re-enabling simulation, synchronize `body.position = transform.position` before assigning velocity.
- **Input settings:** an earlier automated test replaced temporary Input System settings and caused stale native-object assertions. This was repaired. Do not replace or destroy the live `InputSystem.settings` object for tests; restore any temporary property changes.

## Working with Unity

Use the Unity CLI skill when driving the open Editor, and the appropriate Unity UI or sprite skill when those assets change. Prefer live Editor commands for scene and asset modifications instead of manually editing scene YAML or metadata.

Useful commands, run from the project root:

```sh
unity status --format json
unity command scene get_hierarchy --caller plugin --skill unity-cli --format json
unity recompile --format json
unity command console get --caller plugin --skill unity-cli --format json -- --level error --tail 10
```

Discover the connected Editor's command schemas rather than assuming names from older documentation. Pass `--project-path` if multiple Editors are open. Put Editor-specific parameters after `--`. Use the invoking skill's name for `--skill`.

The shell sandbox may block localhost access, making a running Editor appear absent. In this session, scoped elevated CLI commands were needed to reach it. A failed sandboxed status check alone does not prove Unity is closed.

Before changing a scene, check whether Play mode is active and whether there are unsaved user changes. Save intended scene changes in Edit mode. Finish playtests in Edit mode and restore any temporary background-play or simulation overrides.

## Verification and current status

The scene was last inspected in Edit mode, saved, with all 24 bricks present and the win message hidden. Both imported artwork replacements were visually checked at their original gameplay sizes.

Previous checks covered paddle controls and limits, wall/paddle/brick bounces, repeated misses and visible resets, 40 randomized launches within the angle bounds, destruction of only the hit brick, and the win state after the last brick. There is no permanent automated test suite for this game; earlier temporary test/setup scripts under `/private/tmp/` are not project dependencies.

For future changes, use a focused Play-mode check: move, launch, hit a brick, miss, confirm the ball visibly returns to the paddle, and relaunch. For win-condition changes, verify both that one remaining brick does not win and that clearing the final brick does. Inspect the Game view and new console errors as well as physics values; a correct Rigidbody position alone did not catch the previous visible-reset bug.
