🔫 Isometric Shooter (Prototype)  

Isometric Shooter is a top-down action game prototype developed in Unity, focused on responsive controls, modular combat systems, and performance-oriented architecture.

The project was built as a technical exploration of scalable systems and modern architectural patterns in game development. While not fully completed, it includes several production-level implementations.

🚀 Implemented Features  

  🎮 Core Gameplay  
  
  Isometric Movement System  
  Camera-relative movement  
  Frame-rate independent input handling  
  Smooth and responsive controls  
  Aiming & Shooting  
  Cursor-based aiming in isometric space  
  Continuous and single-shot firing modes  
  Decoupled input and shooting logic  
  
  🔫 Combat System  
  
  Projectile System  
  Modular bullet system (Model / View / Presenter separation)  
  Configurable parameters (speed, lifetime, damage)  
  Reusable across different weapon types  
  Object Pooling  
  Reuse of bullets and visual effects  
  Reduced GC allocations  
  Stable performance under high fire rate  
  Hit Detection  
  Physics-based collision handling  
  Extensible damage system  
  
  🧠 UI Architecture (MVVM)  
  
  MVVM Pattern  
  Separation of View, ViewModel, and Model  
  Reactive data flow  
  Reactive Programming  
  Implemented using R3  
  Observable collections and reactive properties  
  Event-driven UI updates without tight coupling  
  
  📦 Resource Management  
  
  Addressables System  
  Asset loading via Unity Addressables  
  Asynchronous resource loading  
  Decoupled asset references  
  Scalable content management  
  
  🧱 Architecture  
  
  Modular Design  
  Clear separation between systems (input, combat, UI, resources)  
  Designed for extensibility  
  Dependency Injection  
  Implemented using Zenject  
  Loose coupling between components  
  
  ⚙️ Performance  
  
  Object pooling to minimize allocations  
  Reduced GC pressure via reuse patterns  
  Efficient handling of frequently spawned objects  
  
  🛠 Tech Stack  
  
  Unity (C#)  
  Zenject  
  TextMeshPro  
  Unity Addressables  
  R3  
  MVVM (custom implementation)  
  Unity Physics & Input System  
