# DoorMonitor
A responsive C#.NET WPF client/server application which provides a simple door management system for a facility.

### UPDATED SOLUTION STRUCTURE
1. DoorMonitor.Common (.NET Standard project for shared classes)
2. DoorMonitor.SignalR (ASP.NET Core API project for the Server)
3. DoorMonitor.WPF (WPF .NET Framework project for the Client)

### Instructions providing simple steps to run the application
Open the DoorMonitor.sln in visual studio and first build `DoorMonitor.WPF` project in debug mode. The debug executable can be found in:
```
~\DoorMonitor.WPF\bin\Debug\DoorMonitor.WPF.exe
```
Next, build the `DoorMonitor.SignalR` project in debug mode and run “DoorMonitor.SignalR”. Once the server is up and running, you can run `DoorMonitor.exe` to connect the client to the server and test out the application.
**Note:** The `DoorMonitor.SignalR` is configured to use a SQLite database which will work only if you run the server as stated above. If instead you want to use the debug
executable of the server from the bin folder of `DoorMonitor.SignalR`, then please comment out line 27 in `Startup.cs` and uncomment line 28 for using InMemory Database before building.

### Summary of the overall design of the application.
1. The `DoorMonitor.SignalR` project in addition to REST APIs, configures a SignalR hub for responsive broadcasting.
2. I chose a Data to UI development approach i.e. -
Database setup -> Server implementation -> UI implementations
3. Took the MVVM approach for an improved development experience between the UI and Business logic.
4. The solution can use an InMemory or Local SQLite database.
5. The `DoorMonitor.Common` project was added to maintain shared endpoints and classes that both the server and client use.
6. Decided to keep 3 separate PUT methods (`changeName`, `changeLockState`, `changeOpenState`) in the server for the door modification even though the body of the code is same, anticipating any chained feature implementation or validation that might be required.

**Ad Astra**
