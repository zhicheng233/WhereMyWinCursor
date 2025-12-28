# WhereMyWinCursor

<p align="center">
  <img src="Resources/icon.ico" alt="WhereMyWinCursor Logo" width="128" height="128">
</p>

<p align="center">
  <b>一款帮助你在 Windows 上像 MacOS 一样找到鼠标光标的工具</b>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-blue" alt=".NET 9.0">
  <img src="https://img.shields.io/badge/Platform-Windows-blue" alt="Windows">
  <img src="https://img.shields.io/badge/Version-1.0.0-green" alt="Version">
  <img src="https://img.shields.io/badge/License-GPL%20V3.0-yellow" alt="License">
</p>

<p align="center">
<img src="/doc/Demo.gif" alt="演示">
</p>

## ✨ 功能特点

- 🖱️ **快速定位光标** - 快速晃动鼠标即可触发光标放大动画
- 🎯 **智能检测** - 自动检测光标类型并应用相应的动画效果
- ⚙️ **高度可配置** - 自定义光标大小、动画速度等参数
- 🌍 **多语言支持** - 支持英文、简体中文等多种语言
- 🎭 **主题支持** - 提供浅色、深色和跟随系统的主题选项
- 📌 **系统托盘** - 静默运行在系统托盘中
- 🚀 **开机自启** - 可选开机自动启动功能

## 📋 待办事项
- 📝 **白名单** - 可以指定在哪些程序中禁用此功能
- 🎮 **游戏模式** - 当应用程序全屏时自动禁用功能
- 🎬 **非线性动画** - 使动画效果更加流畅

## 🔧 系统要求

- Windows 10/11
- [.NET 9.0 运行时](https://dotnet.microsoft.com/download/dotnet/9.0)

## 📥 安装方式

### 方式一：下载发行版
从 [Releases](https://github.com/your-username/WhereMyWinCursor/releases) 页面下载最新版本。

### 方式二：从源码构建

```bash
# 克隆仓库
git clone https://github.com/your-username/WhereMyWinCursor.git

# 进入项目目录
cd WhereMyWinCursor

# 构建项目
dotnet build -c Release

# 运行应用程序
dotnet run
```

## 🚀 使用方法

1. **启动应用程序** - 应用将以最小化状态在系统托盘中运行
2. **晃动鼠标** - 快速左右移动鼠标以触发光标动画
3. **找到光标** - 光标将放大显示，随后恢复正常大小

## ⚙️ 配置设置

右键点击系统托盘图标，选择"打开设置"即可进行配置。

## 🛠️ 技术栈

- **框架**: .NET 9.0 / WPF
- **UI 库**: [WPF-UI](https://github.com/lepoco/wpfui)
- **系统托盘**: [Hardcodet.NotifyIcon.Wpf](https://github.com/hardcodet/wpf-notifyicon)
- **日志**: [Serilog](https://serilog.net/)
- **JSON 处理**: [Newtonsoft.Json](https://www.newtonsoft.com/json)

## 📁 项目结构

```
WhereMyWinCursor/
├── App.xaml(.cs)              # 应用程序入口点
├── Core/
│   ├── Cursor.cs              # 通过 Windows API 操作光标
│   ├── CursorHook.cs          # 鼠标移动钩子
│   ├── Config/                # 配置管理
│   │   ├── ConfigManager.cs
│   │   ├── MainConfig.cs
│   │   └── ...
│   └── Utils/                 # 工具类
│       ├── AppInfo.cs
│       ├── FastCursorDetector.cs
│       └── ...
├── UI/
│   ├── SettingWindows.xaml(.cs)
│   └── Pages/                 # 设置页面
├── Resources/
│   ├── icon.ico
│   ├── Lang.en_US.xaml
│   └── Lang.zh-CN.xaml
└── config.json                # 用户配置文件
```

## 🤝 参与贡献

欢迎贡献代码！请随时提交 Pull Request。

1. Fork 本项目
2. 创建你的功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交你的更改 (`git commit -m '添加一些很棒的功能'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开一个 Pull Request

## 📄 开源许可

本项目采用 GNU 通用公共许可证 v3.0 进行许可 - 详情请参阅 [LICENSE](LICENSE) 文件。

## 🙏 致谢

- 灵感来源于 macOS 的"晃动鼠标找到光标"功能
- 感谢本项目中使用的所有开源库

## 📬 联系方式

如有任何问题或建议，请在 GitHub 上提交 Issue。

---

<p align="center">
  用 ❤️ 为那些经常找不到光标的 Windows 用户而制作
</p>

