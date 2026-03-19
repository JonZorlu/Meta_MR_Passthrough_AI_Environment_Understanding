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

- **What**: A dynamic MR prototype that leverages the **Meta Passthrough Camera API** and **Unity Inference Engine (Sentis)** to run local machine learning models (YOLO). Upon recognizing specific real-world objects (e.g., a smartphone), it anchors an interactable 3D UI component to its precise room-scale coordinates. Users interact with this floating UI using **Hand Tracking Microgestures** (SwipeForward, SwipeBackward, SwipeLeft, SwipeRight) to navigate images like a physical deck of cards.
- **Why**: Designed to bridge the gap between experimental concepts and tangible user studies. This prototype serves as a foundational piece for future AR glasses research—proving concepts around **display color pipelines**, ensuring **image processing** visual fidelity (managing gamma/linear tone mapping for Passthrough layers), building robust diegetic UI, and analyzing hardware/software performance constraints in real-time.
- **How**: Built in Unity with heavily optimized C#, the project integrates **AI Building Blocks** directly into the rendering loop. To mitigate GPU/CPU bottlenecks natively, it decouples the demanding 5FPS object-detection frame rate from the rendering display pipeline by employing continuous 3D interpolation (`Slerp`/`Lerp` tracking). It manages color space conversions (Linear vs. sRGB graphics formats) to maintain visual fidelity and prevent washed-out Passthrough colors, while manipulating raw `OVRHand` microgesture heuristics.

💻 **System Recommendations**
- Unity 6000.x or newer
- Meta Quest 3S / 3
- Meta XR Core SDK and MR Utility Kit (v83+)
- Meta AI Building Blocks
