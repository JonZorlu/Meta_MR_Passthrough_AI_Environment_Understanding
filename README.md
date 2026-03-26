# Meta MR Passthrough AI Environment Understanding & Microgesture Hand Interactions

This project showcases an experimental mixed-reality prototype developed for the **Meta Quest 3S**, emphasizing hands-on **XR lab prototyping**, **imaging and display pipeline** integration, and solving real-time **graphics constraints**.

## 🎬 Demonstrations

*Displaying live prototype demonstrations, user study setups, and real-time interaction.*

### Feature: Smart UI Spawning & Object Detection
> *Real-time detection and anchoring of the Gallery UI using Local Inference.*
<img src="Documentation/demo1.gif" alt="Smart UI Spawning" width="800"/>

### Feature: Microgesture Hand Interactions
> *Using hand microgestures to pull up, restore, and sort through the diegetic UI.*
<img src="Documentation/demo2.gif" alt="Microgesture Navigation" width="800"/>

## 📌 Project Overview

- **What**: A dynamic mixed-reality prototype built for **Meta Quest 3S** that uses the **Meta Passthrough Camera API** and **Unity Inference Engine (Sentis)** to run local ML models (YOLO) on-device. Upon recognizing real-world objects (e.g., a smartphone), it anchors an interactable **persistent 3D UI artifact** to its precise room-scale coordinates — a direct exploration of anchoring digital objects to physical locations. Users interact with this floating UI via **Hand Tracking Microgestures** (SwipeForward, SwipeBackward, SwipeLeft, SwipeRight).

- **Why**: Built to explore foundational problems in **persistent spatial computing** — specifically: how AI context can drive real-time placement and anchoring of digital artifacts in physical environments, and how those artifacts can remain spatially coherent under the hardware constraints of a **mobile-first wearable**. The prototype acts as a proof-of-concept for future multi-user spatial systems where anchored artifacts must be consistent across sessions and devices.

- **How**: Implemented in Unity with optimized C#, the project integrates **on-device AI inference directly into the rendering loop** — tackling the core challenge of **real-time spatial rendering with AI context**. To avoid GPU/CPU bottlenecks on mobile hardware, object detection (5 FPS) is decoupled from the rendering pipeline via continuous 3D interpolation (`Slerp`/`Lerp` tracking). Color space management (Linear vs. sRGB) ensures visual fidelity in the Passthrough layer, while raw `OVRHand` microgesture heuristics enable low-latency hand interaction without external peripherals.

## 🔬 Spatial Computing Problems Explored

This prototype directly engages with foundational challenges in persistent spatial worlds:

- **Persistent spatial anchoring** — digital artifacts are anchored to real-world coordinates at room scale, exploring how AI-detected context can drive durable placement that survives session boundaries.
- **Mobile-first spatial constraints** — all tracking, AI inference, rendering, and anchor management runs within the compute budget of a standalone Meta Quest 3S, without offloading to a server or PC.
- **Real-time rendering with AI context** — the Unity rendering loop receives live contextual input from an on-device world-understanding model (YOLO via Sentis), maintaining smooth frame delivery while the AI pipeline runs concurrently.
- **Foundation for multi-user synchronization** — the spatial anchoring architecture is designed with eventual multi-user consistency in mind: because artifacts are anchored to physical coordinates rather than device-local space, the same anchor data can be shared across users seeing the same physical location.


💻 **System Recommendations**
- Unity 6000.x or newer
- Meta Quest 3S / 3
- Meta XR Core SDK and MR Utility Kit (v83+)
- Meta AI Building Blocks
