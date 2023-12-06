# DI Tools
A set of DI tools for Unity

### How to use

- Inherit `BaseDIInstaller` to create your own DI Installer.
- Implement `IContainerConstructable` interface to automatically add your class to DI as a singleton on app's startup.
- Add any of your own interfaces to a config in a method `BaseDIInstaller.ConfigureServices()` to automatically register all their inheritors in DI on app's startup.

### How to install

In Package Manager click `+` -> `Add package from git URL...` and paste `https://github.com/Nebulate-me/DI-Tools.git`
