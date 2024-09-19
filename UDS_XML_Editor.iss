#include "Infrastructure\Inno Setup\CompareIssVersionToCurrent.iss"  

#define Major 1
#define Minor 0
#define Rev 0
#define Build 0
#define MyAppVersion Str(Major) + "." + Str(Minor)  + "." + Str(Rev)  + "." + Str(Build)


                                             
[Setup]                       
AppVersion={#MyAppVersion}
AppName=TrueDrive Manager
WizardStyle=modern
DefaultDirName={autopf}\UDS_XML_Editor
DefaultGroupName=UDS_XML_Editor
SourceDir=UDS_XML_Editor\bin\Release\net6.0-windows
OutputDir=..\..\..\..\Output
OutputBaseFilename=UDS_XML_EditorSetup
UsePreviousAppDir=no


[Files]
Source: "*.*"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion;


[Icons]
Name: "{group}\UDS_XML_Editor" ; Filename: "{app}\UDS_XML_Editor.exe";  Tasks: startmenu; IconFilename: {app}\Resources\XML.ico
Name: "{commondesktop}\UDS_XML_Editor {#MyAppVersion}"; Filename: "{app}\UDS_XML_Editor.exe"; Tasks: desktopicon; IconFilename: {app}\Resources\XML.ico

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional icons:"; Flags: unchecked
Name: "startmenu"; Description: "Add a shortcut to the start menu"; GroupDescription: "Additional icons:"; Flags: unchecked

[Run]
Filename: {app}\UDS_XML_Editor.exe; Description: "Launch TrueDrive Manager"; Flags: postinstall skipifsilent hidewizard

[Code] 
 
var
  DownloadPage: TDownloadWizardPage;
  AppsToInstall : array of array of String;

function InitializeSetup(): Boolean;
var
  newMajor, newMinor, newRev, newBuild: Cardinal;
  tempPath : String;
Begin
  

  newMajor := {#Major};
  newMinor := {#Minor};
  newRev := {#Rev};
  newBuild := {#Build}; 
  
  Result := IsNewVersionLower_endRun(newMajor, newMinor, newRev, newBuild,'C:\Program Files (x86)\UDS_XML_Editor\UDS_XML_Editor.exe');

  

End;


