[Setup]
AppName=QuickNote
AppVersion=1.0
DefaultDirName={autopf}\QuickNote
DefaultGroupName=QuickNote
OutputBaseFilename=QuickNoteInstaller
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: "Source\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\QuickNote"; Filename: "{app}\QuickNoteApp.exe"; WorkingDir: "{app}"
Name: "{group}\Uninstall QuickNote"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\QuickNoteApp.exe"; Description: "Launch QuickNote"; Flags: nowait postinstall skipifsilent