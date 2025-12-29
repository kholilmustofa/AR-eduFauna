# Edu Fauna - AR Educational Application

## Overview
Edu Fauna is an Android Augmented Reality (AR) educational application built with Unity that introduces various fauna animals through an interactive and engaging learning experience using markerless AR technology.

## Features
- ✨ Markerless AR with plane detection
- 🦁 Multiple animal 3D models with educational information
- 📱 Interactive touch controls (drag, rotate, scale)
- 🎨 Child-friendly UI design
- 📚 Educational content in Indonesian
- 🎯 Easy animal selection interface

## Technical Requirements

### Unity Version
- **Unity 6 (6000.x)** - Recommended ⭐
- Unity 2022.3 LTS or newer (also supported)
- Unity 2021.3 LTS or newer (older version)

### Required Packages
Install these packages via Unity Package Manager (Window > Package Manager):

1. **AR Foundation** (6.0.x or newer)
   - Core AR framework for Unity
   
2. **ARCore XR Plugin** (6.0.x or newer)
   - For Android AR support
   
3. **TextMeshPro** - Built-in for Unity 6! ✅
   - No need to install package
   - Just import TMP Essentials when creating first text
   
4. **Input System** (1.7.x or newer) - Optional
   - For enhanced input handling

### Android Build Settings
- Minimum API Level: Android 7.0 (API Level 24)
- Target API Level: Android 13 (API Level 33) or newer
- Scripting Backend: IL2CPP (Required)
- Target Architectures: ARM64 (Required)
- Graphics APIs: Vulkan (default), OpenGLES3 (fallback)

## Project Structure

```
Assets/
├── Scripts/
│   ├── AnimalData.cs                    # Data structure for animal information
│   ├── GameManager.cs                   # Singleton manager for game state
│   ├── SplashScreenController.cs        # Splash/intro screen logic
│   ├── AnimalSelectionController.cs     # Animal selection menu logic
│   ├── AnimalCard.cs                    # Individual animal card component
│   ├── ARPlacementController.cs         # AR placement and plane detection
│   ├── ARAnimalInteraction.cs           # Touch interaction (drag, rotate, scale)
│   ├── ARUIController.cs                # AR scene UI management
│   └── AnimalAnimationController.cs     # Animal animation control
├── Scenes/
│   ├── SplashScreen.unity               # Initial splash screen
│   ├── AnimalSelection.unity            # Animal selection menu
│   └── ARScene.unity                    # Main AR experience
├── Prefabs/
│   ├── AnimalCard.prefab                # Animal selection card prefab
│   ├── PlacementIndicator.prefab        # AR placement indicator
│   └── Animals/                         # Animal 3D model prefabs
│       ├── Elephant.prefab
│       ├── Tiger.prefab
│       ├── Panda.prefab
│       ├── Giraffe.prefab
│       └── Penguin.prefab
├── Materials/
│   └── PlacementIndicator.mat           # Material for placement indicator
├── Textures/
│   └── AnimalIcons/                     # Icons for animal selection
└── UI/
    └── Sprites/                         # UI sprites and icons
```

## Setup Instructions

### 1. Install Required Packages

1. Open Unity Package Manager (Window > Package Manager)
2. Make sure "Unity Registry" is selected in the dropdown
3. Install the following packages:
   - **AR Foundation** (version 6.0.x or newer)
   - **ARCore XR Plugin** (version 6.0.x or newer)
4. **TextMeshPro**: Already built-in for Unity 6!
   - When you create your first TextMeshPro text, click "Import TMP Essentials"

**For detailed setup instructions, see UNITY6_SETUP.md or QUICKSTART_UNITY6.md**

### 2. Configure AR Settings

1. Go to **Edit > Project Settings > XR Plug-in Management**
2. Enable **ARCore** for Android platform
3. Go to **Player Settings > Android**
4. Set the following:
   - **Minimum API Level**: Android 7.0 (API Level 24)
   - **Scripting Backend**: IL2CPP
   - **Target Architectures**: ARM64 (check this box)
   - **Auto Graphics API**: Disable and ensure OpenGLES3 is in the list

### 3. Create Scenes

#### A. Splash Screen Scene
1. Create new scene: **SplashScreen.unity**
2. Add Canvas (UI > Canvas)
3. Add the following UI elements:
   - Logo Image (top/center)
   - Title Text (TextMeshPro)
   - Description Text (TextMeshPro)
   - "Pilih Hewan" Button (bottom)
4. Attach `SplashScreenController.cs` to Canvas
5. Assign references in Inspector

#### B. Animal Selection Scene
1. Create new scene: **AnimalSelection.unity**
2. Add Canvas
3. Add the following:
   - Title Text: "Pilih Hewan"
   - Scroll View with Grid Layout Group
   - Back Button
4. Create **AnimalCard Prefab**:
   - Panel with Image (Icon) and Text (Name)
   - Add Button component
   - Attach `AnimalCard.cs` script
5. Attach `AnimalSelectionController.cs` to Canvas
6. Assign references

#### C. AR Scene (Unity 6)
1. Create new scene: **ARScene.unity**
2. Delete Main Camera
3. Add **XR Origin** (GameObject > XR > XR Origin (Mobile AR))
   - This includes AR Camera automatically
   - For Unity 6: Use XR Origin, NOT AR Session Origin
4. Add **AR Session** (GameObject > XR > AR Session)
5. Add **AR Plane Manager** to XR Origin
6. Add **AR Raycast Manager** to XR Origin
7. Create **Placement Indicator**:
   - Create a simple 3D plane or circle
   - Add a material with semi-transparent shader
   - Save as prefab
8. Create UI Canvas:
   - Instruction Panel (top)
   - Button Panel (bottom):
     - "Pilih Hewan" button
     - "Detail Hewan" button
   - Detail Panel (initially hidden):
     - Animal name, habitat, food, characteristics
     - Close button
9. Create empty GameObject named "ARController"
10. Attach `ARPlacementController.cs` to ARController
11. Attach `ARUIController.cs` to Canvas
12. Assign all references in Inspector

**Note for Unity 6**: The hierarchy will be:
```
XR Origin (NOT AR Session Origin)
├── Camera Offset
│   └── Main Camera
├── AR Plane Manager (component)
└── AR Raycast Manager (component)
```

### 4. Create Animal Prefabs

For each animal:
1. Import 3D model (FBX or OBJ)
2. Add to scene and configure:
   - Add Collider (Box or Mesh Collider)
   - Add `ARAnimalInteraction.cs` script
   - Add `AnimalAnimationController.cs` if model has animations
3. Save as prefab in `Assets/Prefabs/Animals/`
4. Create icon sprite for selection menu

### 5. Configure GameManager

1. Create empty GameObject in SplashScreen scene
2. Name it "GameManager"
3. Attach `GameManager.cs` script
4. In Inspector, configure animal list:
   - Set size to number of animals (e.g., 5)
   - For each animal:
     - Assign model prefab
     - Assign icon sprite
     - Data (name, habitat, food, characteristics) is set in code

### 6. Build Settings

1. Go to **File > Build Settings**
2. Add scenes in order:
   - SplashScreen
   - AnimalSelection
   - ARScene
3. Switch platform to **Android**
4. Click **Player Settings** and configure:
   - Company Name
   - Product Name: "Edu Fauna"
   - Package Name: com.yourcompany.edufauna
   - Version: 1.0
   - Bundle Version Code: 1

### 7. Android Manifest Configuration

The AR Foundation package should automatically configure the Android manifest, but verify:

1. Check `Assets/Plugins/Android/AndroidManifest.xml` exists
2. Ensure it contains:
```xml
<uses-permission android:name="android.permission.CAMERA" />
<uses-feature android:name="android.hardware.camera.ar" android:required="true"/>
```

## Usage Guide

### For Developers

#### Adding New Animals

1. **Prepare 3D Model**:
   - Import model to `Assets/Models/`
   - Ensure model is optimized for mobile (low poly count)
   - Add animations if available

2. **Create Prefab**:
   - Add model to scene
   - Add Box Collider or Mesh Collider
   - Add `ARAnimalInteraction.cs`
   - Add `AnimalAnimationController.cs` (if animated)
   - Save as prefab

3. **Create Icon**:
   - Create or import icon image (512x512 recommended)
   - Set Texture Type to "Sprite (2D and UI)"
   - Save in `Assets/Textures/AnimalIcons/`

4. **Update GameManager**:
   - Open `GameManager.cs`
   - Add new animal data in `InitializeAnimals()` method:
   ```csharp
   animals.Add(new AnimalData(
       "Animal Name",
       "Habitat description",
       "Food description",
       "Characteristics description"
   ));
   ```

5. **Assign References**:
   - Select GameManager in SplashScreen scene
   - In Inspector, expand Animals list
   - Assign Model Prefab and Icon for new animal

#### Customizing UI

All UI can be customized in the Unity Editor:
- Colors: Modify Image and Text components
- Fonts: Assign TextMeshPro fonts
- Layout: Adjust RectTransform properties
- Animations: Modify animation parameters in scripts

### For End Users

1. **Launch App**: Open Edu Fauna
2. **Splash Screen**: Tap "Pilih Hewan"
3. **Select Animal**: Choose an animal from the grid
4. **AR Experience**:
   - Point camera at flat surface (floor/table)
   - Wait for surface detection (dots/grid appears)
   - Tap to place animal
5. **Interact**:
   - **Drag**: Move animal on surface
   - **Two-finger rotate**: Rotate animal
   - **Pinch**: Scale animal size
6. **Learn**: Tap "Detail Hewan" to see information
7. **Change Animal**: Tap "Pilih Hewan" to select different animal

## Troubleshooting

### AR Not Working
- Ensure device supports ARCore
- Check camera permissions are granted
- Verify ARCore is installed on device
- Test on well-lit environment with textured surfaces

### Animals Not Appearing
- Check GameManager has animal prefabs assigned
- Verify prefabs have colliders
- Check console for errors

### Touch Controls Not Working
- Ensure ARAnimalInteraction script is attached
- Check colliders are present on models
- Verify EventSystem exists in scene

### Build Errors
- Ensure IL2CPP is selected
- Check ARM64 architecture is enabled
- Verify all required packages are installed
- Clear Library folder and reimport

## Performance Optimization

1. **3D Models**:
   - Keep polygon count under 10,000 per model
   - Use texture atlases
   - Compress textures (ASTC format for Android)

2. **AR Settings**:
   - Limit plane detection to horizontal planes only
   - Disable plane detection after placement
   - Use LOD (Level of Detail) for complex models

3. **UI**:
   - Use sprite atlases
   - Minimize overdraw
   - Pool UI elements when possible

## Credits & Resources

### Recommended 3D Model Sources
- **Sketchfab**: https://sketchfab.com (Free and paid models)
- **TurboSquid**: https://www.turbosquid.com
- **Unity Asset Store**: https://assetstore.unity.com

### AR Foundation Documentation
- Official Docs: https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest
- ARCore: https://developers.google.com/ar

### Learning Resources
- Unity Learn: https://learn.unity.com
- AR Foundation Samples: https://github.com/Unity-Technologies/arfoundation-samples

## License
This project is created for educational purposes.

## Support
For issues and questions, please refer to Unity documentation or AR Foundation community forums.

---

**Version**: 1.0  
**Last Updated**: December 2025  
**Unity Version**: Unity 6 (6000.x) - Recommended | Unity 2022.3 LTS or newer

**📚 For Unity 6 setup, read: QUICKSTART_UNITY6.md or UNITY6_SETUP.md**

