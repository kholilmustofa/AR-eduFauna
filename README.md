# 🐘 eduFauna - AR Educational App

AR application for learning about animals using Unity and Vuforia Ground Plane.

## ✨ Features

- 🦁 **5 Interactive 3D Animals**: Elephant, Lion, Orca, Penguin, Zebra
- 📱 **AR Ground Plane Detection**: Place animals on real-world surfaces
- 🎮 **Touch Interactions**: 
  - Drag to rotate
  - Pinch to scale
  - Move animals around
- 📚 **Educational Information**: Learn about habitat, food, and characteristics
- 🎨 **Beautiful UI**: Modern, intuitive interface

## 🛠️ Tech Stack

- **Unity**: 2021.3 or later
- **Vuforia Engine AR**: Ground Plane detection
- **C#**: Game logic and AR interactions
- **TextMeshPro**: UI text rendering

## 📋 Requirements

### Development:
- Unity 2021.3 LTS or later
- Vuforia Engine AR package
- Android Build Support

### Device:
- Android 7.0 (API 24) or higher
- ARCore compatible device
- Good lighting conditions for plane detection

## 🚀 Setup

### 1. Clone Repository
```bash
git clone https://github.com/kholilmustofa/AR-eduFauna.git
cd AR-eduFauna
```

### 2. Open in Unity
1. Open Unity Hub
2. Click "Open" → Select project folder
3. Wait for Unity to import assets

### 3. Vuforia Setup
1. Get Vuforia license key from [Vuforia Developer Portal](https://developer.vuforia.com/)
2. In Unity: **GameObject** → **Vuforia Engine** → **AR Camera**
3. Paste license key in **Vuforia Behaviour** component

### 4. Build Settings
1. **File** → **Build Settings**
2. **Platform**: Android
3. **Switch Platform**
4. **Player Settings**:
   - Minimum API Level: Android 7.0 (API 24)
   - Target API Level: Automatic (Highest Installed)

### 5. Build & Run
1. Connect Android device via USB
2. Enable **USB Debugging** on device
3. **File** → **Build and Run**
4. Install APK on device

## 🎮 How to Use

1. **Launch App** on your Android device
2. **Select Animal** from the animal selection screen
3. **Scan Floor**: Point camera at a flat surface (floor, table)
4. **Wait for Plane Detection**: Move device slowly to help detect the plane
5. **Animal Appears**: The selected animal will appear on the detected plane
6. **Interact**:
   - **1 finger drag**: Rotate animal
   - **2 finger pinch**: Scale animal
   - **Tap "Detail Hewan"**: View educational information

## 📁 Project Structure

```
eduFauna/
├── Assets/
│   ├── Scenes/
│   │   ├── SplashScreen.unity
│   │   ├── AnimalSelection.unity
│   │   └── ARScene.unity
│   ├── Scripts/
│   │   ├── GameManager.cs              # Core game logic
│   │   ├── AnimalData.cs               # Animal data model
│   │   ├── VuforiaAnimalSpawner.cs     # Spawn animal on plane
│   │   ├── ARAnimalInteraction.cs      # Touch gestures
│   │   ├── ARUIController.cs           # AR UI management
│   │   ├── AnimalSelectionController.cs # Selection screen
│   │   └── SceneLoader.cs              # Scene management
│   ├── Prefabs/
│   │   └── Animals/                    # 3D animal models
│   ├── Resources/
│   │   └── UI/Illustrations/           # Animal icons
│   └── Materials/                      # Materials & shaders
└── README.md
```

## 🐾 Animals Included

1. **🐘 Gajah (Elephant)**
   - Habitat: Tropical forests and African savanna
   - Food: Grass, leaves, bark, fruits

2. **🦁 Singa (Lion)**
   - Habitat: African savanna and grasslands
   - Food: Carnivore - zebra, deer, buffalo

3. **🐋 Paus Orca (Orca)**
   - Habitat: Oceans worldwide, especially cold waters
   - Food: Fish, seals, penguins, even other whales

4. **🐧 Penguin**
   - Habitat: Antarctica and southern cold regions
   - Food: Fish, krill, squid

5. **🦓 Zebra**
   - Habitat: African grasslands and savanna
   - Food: Grass, leaves, bark

## 🔧 Troubleshooting

### Plane Detection is Slow
- Ensure **good lighting** (bright, natural light)
- Use **textured surfaces** (tile floor, patterned carpet)
- Avoid **reflective surfaces** (glass, shiny floors)
- Move device **slowly** in horizontal motion

### Animal Not Appearing
- Check if animal is selected in Animal Selection screen
- Ensure Vuforia license key is configured
- Check Console for error messages

### Touch Interactions Not Working
- Ensure `ARAnimalInteraction.cs` is attached to spawned animal
- Check if UI is blocking touch input
- Verify device supports multi-touch

## 📝 License

This project is for educational purposes.

## 👨‍💻 Developer

**Kholil Mustofa**
- GitHub: [@kholilmustofa](https://github.com/kholilmustofa)

## 🙏 Acknowledgments

- 3D Models: Various sources (check individual model credits)
- Vuforia Engine: PTC Inc.
- Unity Technologies

---

**Made with ❤️ for education**
