---
trigger: always_on
---

# Meta MR Passthrough AI Environment Understanding - Always On Knowledge Base

## 🎯 Project Goal
Building a Unity 6 URP mixed reality prototype project for **Meta Quest 3S** where the user wears the headset, sees their real room via passthrough, and using local models like **YOLO model (via Unity Sentis)** detects and labels real-world objects in real time with floating 3D bounding boxes. We will add more functionality to this, like using another local object detection model, like moondream2, add local SLM inference for chat, local TTS and local STT using Building Blocks (BB) Meta provides. User will add the BBs himself, agent can do the rest, search, suggest, fix, improve, simplify code, add other type of Game Objects, like for UI. This project will be pushed to a public repo as a POC for showcasing.

## 📌 Important Notes & Architecture rules
- **Coroutines over UniTask:** For simplicity, we use Coroutines instead of UniTask in this project, at least for now.
- **Unity MCP Server:** Available for editing things in the Unity world. Do not try to make edits directly to Unity Engine related files manually when MCP can be used.
- **Context7:** Use it to get the latest code and best practice examples on the features we want to implement.

## 🔗 Key References
- **Meta Horizon Unity Samples:** [developers.meta.com/horizon/code-samples/unity/](https://developers.meta.com/horizon/code-samples/unity/)
- **Meta Horizon Unity API (v85):** [developers.meta.com/horizon/reference/unity/v85/](https://developers.meta.com/horizon/reference/unity/v85/)
- **Passthrough & Snapshot Tutorial Video:** [youtube.com/watch?v=2gSVPW8L6ps](https://www.youtube.com/watch?v=2gSVPW8L6ps)
- **Quest Passthrough Color Picker Example:** [XRDevRob/QuestCameraKit](https://github.com/xrdevrob/QuestCameraKit)
- **Official samples repo:** [oculus-samples/Unity-PassthroughCameraApiSamples](https://github.com/oculus-samples/Unity-PassthroughCameraApiSamples)
- **AI Building Blocks Video Tutorial:** [youtube.com/watch?v=QIa6frPMcSQ](https://www.youtube.com/watch?v=QIa6frPMcSQ)
- **AI Building Blocks Demos Repo (Active Reference):** [dilmerv/AIBuildingBlocksDemos](https://github.com/dilmerv/AIBuildingBlocksDemos)
- **Hand Tracking with Microgestures Video:** [youtube.com/watch?v=fJgvIxkFABU&t=858s](https://www.youtube.com/watch?v=fJgvIxkFABU&t=858s)
- **Microgestures Demos Repo:** [dilmerv/MicrogesturesDemos](https://github.com/dilmerv/MicrogesturesDemos)
- **Custom UI with LLM Features Video:** [youtube.com/watch?v=Q8BFLkRYOy0&t=1949s](https://www.youtube.com/watch?v=Q8BFLkRYOy0&t=1949s)
- **Public GitHub Repo (Ours):** [JonZorlu/Meta_MR_Passthrough_AI_Environment_Understanding](https://github.com/JonZorlu)
- **AI Building Blocks Guide (Active Reference):** [Quest_3S_AI_Building_Blocks_Guide.md](file:///f:/Unity_Stuff/Repos/Meta_MR_Object_Detection/Quest_3S_AI_Building_Blocks_Guide.md)

## 💻 Coding Style & User Preferences
These rules must be strictly adhered to:

- **Variable Naming:** Meaningful, descriptive names.
- **Instantiation (Use var):** Always use `var` for the instance type.
  - *Good:* `var playerData = new PlayerData();`
- **Delegates over Lambdas:** Avoid inline lambdas (`() => { ... }`). Create explicitly named methods and pass them as delegates for better memory profiling.
  - *Good:* `button.onClick.AddListener(OnNextSceneButtonClicked);`
- **No C# Properties (Get/Set):** Do not use `{ get; set; }`. Create explicit `Get...()` and `Set...()` methods.
  - *Good:*
    ```csharp
    private int _health;
    public int GetHealth() { return _health; }
    public void SetHealth(int amount) { _health = amount; }
    ```
- **Use TryGet over Getters:** Instead of returning a value directly with a `Get()`, use `TryGet(out var valueToReturn)` and return a `bool`, if it is not a simple Getter. This allows for internal error handling without costly exceptions.
  - *Good:*
    ```csharp
    if (TryGetPlayerData(out var playerData)) {
        // use playerData safely
    }
    ```
- **Avoid Tuples for Multiple Returns:** Do not use Tuples when a function needs to return more than one thing. Use specific `out` parameters.
  - *Bad:* `public (int score, bool isDead) GetStatus()`
  - *Good:* `public void GetStatus(out int score, out bool isDead)`
- **UI Framework:** Always use Unity Canvas (uGUI) for UI elements, do NOT use UI Toolkit.

## 🚀 Project Roadmap & Priorities
- **P0 (Current):** User sets up the scene with Meta Building Blocks. Help him add more functionality to the scene and make code changes and fix issues. Don't add Building Blocks, let the user add them, guide the user.
- **P1:** Use Hand interactions instead of controllers. Write our own clean, simplified scripts, adding functionality progressively if it makes sense to do so.
- **P2:** 
  - Polished Mixed Reality desk scene: Virtual objects and 3D UI that adjust shading based on real-world environment brightness.
  - Diegetic UI: Pop-up interfaces when looking at specific objects (e.g., keyboard, smartphone) using gaze and hand gestures (pinch/swipe).
  - Clean up project structure.

## 🛠️ Environment & Optimization Rules
- **ADB Connection:** If `adb` isn't in PATH, navigate to `C:\Program Files\Unity\Hub\Editor\6000.x\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools` and run `.\adb devices`. If unauthorized, toggle Developer Mode on the Meta Horizon mobile app.
- **Passthrough Rendering Optimization:** 
  - **Lighting -> Environment:** Skybox Material = `None`, Sun Source = `None`.
  - **Environment Lighting:** Source = `Color` (Medium Gray).
  - **CenterEyeAnchor Camera:** Clear Flags = `Solid Color`, Background Color = `RGBA(0, 0, 0, 0)` (Pure transparent black).
  *This prevents the GPU from rendering an invisible skybox behind the passthrough layer.*