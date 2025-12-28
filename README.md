# WhereMyWinCursor

<p align="center">
  <img src="Resources/icon.ico" alt="WhereMyWinCursor Logo" width="128" height="128">
</p>

<p align="center">
  <b>A utility to help you find your Windows cursor like MacOS</b>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-blue" alt=".NET 9.0">
  <img src="https://img.shields.io/badge/Platform-Windows-blue" alt="Windows">
  <img src="https://img.shields.io/badge/Version-1.0.0-green" alt="Version">
  <img src="https://img.shields.io/badge/License-GPL%20V3.0-yellow" alt="License">
</p>

<p align="center">
<img src="/doc/Demo.gif" alt="Demo">
</p>
<p align="center">
<a href="README_zh-CN.md">简体中文</a>
</p>

## ✨ Features

- 🖱️ **Quick Cursor Locator** - Shake your mouse rapidly to trigger a cursor enlargement animation
- 🎯 **Smart Detection** - Automatically detects cursor type and applies appropriate animation
- ⚙️ **Highly Configurable** - Customize cursor size, animation speed, and more
- 🌍 **Multi-language Support** - English, Simplified Chinese and more!
- 🎭 **Theme Support** - Light, Dark, and System theme options
- 📌 **System Tray** - Runs quietly in the system tray
- 🚀 **Start with Windows** - Optional auto-start with system boot

## 🎯TODO
- 🚫 **Whitelist** - You can decide on which program to disable
- 🎮 **GameMode** - Disable functionality when app is full screen
- 💫 **non-linear animation** - Makes animations smoother

## 🔧 Requirements

- Windows 10/11
- [.NET 9.0 Runtime](https://dotnet.microsoft.com/download/dotnet/9.0)

## 📥 Installation

### Option 1: Download Release
Download the latest release from the [Releases](https://github.com/zhicheng233/WhereMyWinCursor/releases) page.

### Option 2: Build from Source

```bash
# Clone the repository
git clone https://github.com/zhicheng233/WhereMyWinCursor.git

# Navigate to the project directory
cd WhereMyWinCursor

# Build the project
dotnet build -c Release

# Run the application
dotnet run
```

## 🚀 Usage

1. **Launch the application** - The app will start minimized in the system tray
2. **Shake your mouse** - Quickly move your mouse left and right to trigger the cursor animation
3. **Find your cursor** - The cursor will enlarge and then return to normal size

## ⚙️ Configuration

Right-click the system tray icon and select "Open Settings" to configure:

## 🛠️ Tech Stack

- **Framework**: .NET 9.0 / WPF
- **UI Library**: [WPF-UI](https://github.com/lepoco/wpfui)
- **System Tray**: [Hardcodet.NotifyIcon.Wpf](https://github.com/hardcodet/wpf-notifyicon)
- **Logging**: [Serilog](https://serilog.net/)
- **JSON Handling**: [Newtonsoft.Json](https://www.newtonsoft.com/json)

## 📁 Project Structure

```
WhereMyWinCursor/
├── App.xaml(.cs)              # Application entry point
├── Core/
│   ├── Cursor.cs              # Cursor manipulation via Windows API
│   ├── CursorHook.cs          # Mouse movement hook
│   ├── Config/                # Configuration management
│   │   ├── ConfigManager.cs
│   │   ├── MainConfig.cs
│   │   └── ...
│   └── Utils/                 # Utility classes
│       ├── AppInfo.cs
│       ├── FastCursorDetector.cs
│       └── ...
├── UI/
│   ├── SettingWindows.xaml(.cs)
│   └── Pages/                 # Settings pages
├── Resources/
│   ├── icon.ico
│   ├── Lang.en_US.xaml
│   └── Lang.zh-CN.xaml
└── config.json                # User configuration
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

![Alt](https://repobeats.axiom.co/api/embed/ad8d7771b0760a21a30b5db2a001c74029151f83.svg "Repobeats analytics image")

<a style="margin-top: 15px" href="https://github.com/zhicheng233/WhereMyWinCursor/graphs/contributors">
<img src="https://contrib.rocks/image?repo=zhicheng233/WhereMyWinCursor" />
</a>

## 📄 License

This project is licensed under the GNU General Public License v3.0  License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Inspired by the macOS "shake to find cursor" feature
- Thanks to all the open-source libraries used in this project

## 📬 Contact

If you have any questions or suggestions, please open an issue on GitHub.

---

<p align="center">
  Made with ❤️ for Windows users who lose their cursor
</p>

