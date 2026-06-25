# FakeOutBoss

A stealth office comedy game: slack off at your desk, but switch back to "work" before the boss catches you looking. Push your luck between **overwork pressure** and **suspicion** — let either max out and you lose.

<img width="800" height="450" alt="FakeOutBOSS-ezgif com-video-to-gif-converter" src="https://github.com/user-attachments/assets/6b20cb55-0bc2-4459-b70c-b7ce977bee91" />

## Built with
- **Unity 6** (`6000.3.9f1`), C#
- 3D, POLYGON Office asset pack, Timeline-driven intro

## Highlights
- **NPC AI — finite state machine.** Each character runs sit → stand → walk → look / drink, then returns to its desk. Idle decisions are made with a **suspicion-weighted random** so the boss patrols more aggressively as suspicion rises.
- **Pathfinding — strategy pattern.** Four interchangeable algorithms behind one interface (`IPathfindingStrategy`): **A\***, **BFS**, **DFS**, and Direct. Swap per-character from the inspector.
- **Tension systems.** Two opposing stat bars (overwork vs. suspicion) pace the round; getting spotted triggers a slow-motion **key-press QTE** to scramble back to work.

## Project layout
```
Assets/Scripts/
├── Ai/
│   ├── AiBrain.cs            # state-machine driver
│   ├── States/              # sitting, standing, walking, looking, drinking
│   └── Pathfinding/         # grid, nodes, A*/BFS/DFS/Direct strategies
├── Gameplay/                # player, stats, key challenge, game data
└── Misc/                    # managers, UI bars, timeline, time scaling
```
