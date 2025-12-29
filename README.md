# 🦁 Edu Fauna - AR Educational Application

An Android Augmented Reality (AR) educational application built with Unity that introduces various fauna animals through an interactive and engaging learning experience using markerless AR technology.

![Unity Version](https://img.shields.io/badge/Unity-6.0-blue)
![Platform](https://img.shields.io/badge/Platform-Android-green)
![AR Foundation](https://img.shields.io/badge/AR%20Foundation-6.0-orange)

## ✨ Features

- 🎯 **Markerless AR** with plane detection
- 🦁 **Multiple animal 3D models** with educational information
- 📱 **Interactive touch controls** (drag, rotate, scale)
- 🎨 **Child-friendly UI** design
- 📚 **Educational content** in Indonesian
- 🔊 **Audio support** for immersive experience

## 🛠️ Technical Requirements

### Unity & Packages
- **Unity 6 (6000.x)** - Recommended ⭐
- **AR Foundation** (6.0.x or newer)
- **ARCore XR Plugin** (6.0.x or newer)
- **TextMeshPro** (Built-in for Unity 6)

### Android Build Settings
- **Minimum API Level**: Android 7.0 (API Level 24)
- **Target API Level**: Android 13 (API Level 33) or newer
- **Scripting Backend**: IL2CPP (Required)
- **Target Architectures**: ARM64 (Required)

## 📁 Project Structure

```
Assets/
├── Scenes/
│   ├── SplashScreen.unity
│   ├── AnimalSelection.unity
│   └── ARScene.unity
├── Scripts/
│   ├── GameManager.cs
│   ├── ARPlacementController.cs
│   ├── ARAnimalInteraction.cs
│   └── ARUIController.cs
├── Prefabs/
│   ├── Animals/
│   └── AnimalCard.prefab
└── UI/
    ├── Fonts/
    ├── Icons/
    └── Buttons/
```

## 🚀 Quick Start

### 1. Clone Repository
```bash
git clone https://github.com/kholilmustofa/AR-eduFauna.git
cd AR-eduFauna
```

### 2. Open in Unity
- Open Unity Hub
- Click "Add" and select the project folder
- Open with Unity 6 (6000.x)

### 3. Install Required Packages
1. Open **Window > Package Manager**
2. Install:
   - AR Foundation (6.0.x)
   - ARCore XR Plugin (6.0.x)

### 4. Configure AR Settings
1. Go to **Edit > Project Settings > XR Plug-in Management**
2. Enable **ARCore** for Android
3. Configure **Player Settings > Android**:
   - Scripting Backend: IL2CPP
   - Target Architectures: ARM64

### 5. Build & Run
1. **File > Build Settings**
2. Switch to **Android**
3. Click **Build and Run**

## 📱 How to Use

1. **Launch** the app on your Android device
2. **Select** an animal from the menu
3. **Point** camera at a flat surface (floor/table)
4. **Tap** to place the animal in AR
5. **Interact**:
   - Drag to move
   - Two-finger rotate
   - Pinch to scale
6. **Learn** by tapping the info button

## 🎮 Controls

| Action | Gesture |
|--------|---------|
| Place Animal | Single Tap |
| Move Animal | Drag |
| Rotate Animal | Two-finger Rotate |
| Scale Animal | Pinch |
| View Info | Info Button |
| Change Animal | Menu Button |

## 🐛 Troubleshooting

**AR Not Working?**
- Ensure device supports ARCore
- Check camera permissions
- Test in well-lit environment

**Build Errors?**
- Verify IL2CPP is selected
- Check ARM64 is enabled
- Clear Library folder and reimport

## 📚 Resources

- [Unity Documentation](https://docs.unity3d.com)
- [AR Foundation Docs](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest)
- [ARCore Developer Guide](https://developers.google.com/ar)

## 📄 License

This project is created for educational purposes.

## 👨‍💻 Developer

**Kholi Mustofa**
- GitHub: [@kholilmustofa](https://github.com/kholilmustofa)
- Email: kholilmoestofa954@gmail.com

---

**Version**: 1.0  
**Last Updated**: December 2025  
**Built with**: Unity 6 & AR Foundation 6.0
