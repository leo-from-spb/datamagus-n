Project Overview
================

DataMagus is a GUI tool for database design and modeling, as well as authoring ER/EAR diagrams.
Technically it is a .Net desktop application, compatible with Windows, MacOS and Linux.
It is written on C# and uses the AvaloniaUI framework for GUI.
                       
The layout of the project is described in the Layout.md file.


Build and Run
------------- 
                 
Build the code generator:
```bash
dotnet build Model_Gen
```
   
Generate Model implementation:
```bash
dotnet run --project Model_Gen
```

Build all:
```bash
dotnet build
```

Test:
```bash
dotnet test
```

Start the built application:
```bash
dotnet run --project DataMagus_App
```
