To export a Unity project as an iOS app that you can open and build in Xcode, you’re essentially generating an Xcode project from Unity and then compiling it there. Here’s the clean, practical workflow:

---

## 1. Prerequisites (don’t skip these)

* macOS machine (Xcode only runs on macOS)
* Install Xcode from the App Store
* Install iOS Build Support in Unity (via Unity Hub → Add Modules)
* Apple Developer account (needed for signing on real devices)

---

## 2. Switch Unity to iOS platform

In Unity:

1. Go to **File → Build Settings**
2. Select **iOS**
3. Click **Switch Platform**

This may take a bit because Unity reimports assets for iOS.

---

## 3. Configure Player Settings

Still in **Build Settings → Player Settings**:

Key things to set:

* **Company Name / Product Name**
* **Bundle Identifier** (e.g. `com.yourname.yourapp`)
* **Target minimum iOS version**
* **Architecture**: usually ARM64
* **Scripting Backend**: IL2CPP (required for iOS)

Optional but common:

* Orientation (portrait/landscape)
* App icons & launch screen

---

## 4. Build the Xcode project

1. In **Build Settings**, click **Build**
2. Choose an empty folder (Unity will generate an Xcode project there)

After this, Unity creates:

```
YourProject/
 ├── Unity-iPhone.xcodeproj
 ├── Classes/
 ├── Libraries/
 └── Data/
```

---

## 5. Open in Xcode

* Double-click `Unity-iPhone.xcodeproj`
  → opens in Xcode

---

## 6. Configure signing in Xcode

Inside Xcode:

1. Select the **Unity-iPhone** target
2. Go to **Signing & Capabilities**
3. Select your **Team**
4. Ensure the **Bundle Identifier** matches your Apple Developer profile

If you see signing errors, this is where they’re fixed.

---

## 7. Build & run

* Select a device (iPhone or Simulator)
* Click **Run (▶)**

Xcode will:

* Compile the Unity-generated code
* Install the app on your device or simulator

---

## 8. (Optional) Export IPA

For distribution:

* In Xcode: **Product → Archive**
* Then export via Organizer (TestFlight / Ad Hoc / App Store)

---

## Common pitfalls (worth knowing)

### 1. “No iOS module installed”

Fix: Add iOS Build Support via Unity Hub.

### 2. Signing errors

Fix: Make sure:

* Bundle ID is unique
* Apple account is selected in Xcode

### 3. Build succeeds but app crashes

Usually:

* Missing permissions (camera, etc.)
* IL2CPP issues
* Plugin incompatibility

### 4. Rebuilding workflow

Important:
Every time you change Unity code → **rebuild from Unity**
Don’t edit generated Xcode files unless necessary (they get overwritten)

---

## Mental model (helps a lot)

Think of Unity as:

> “Game engine → exports C++ project → Xcode compiles it into an iOS app”

---

If you want, I can:

* Walk through a **minimal Unity → iOS example project**
* Help debug a **specific Xcode build error**
* Or show how to integrate **Swift/Objective-C plugins into Unity iOS builds**
