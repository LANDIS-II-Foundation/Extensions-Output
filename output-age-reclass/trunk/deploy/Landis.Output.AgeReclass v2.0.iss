#define PackageName      "Age Reclass Output"
#define PackageNameLong  "Age Reclass Output"
#define Version          "2.0.1"
#define ReleaseType      "official"
#define ReleaseNumber    ""

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include "J:\Scheller\LANDIS-II\deploy\package (Setup section) v6.0.iss"
#define ExtDir "C:\Program Files\LANDIS-II\v6\bin\extensions"
#define AppDir "C:\Program Files\LANDIS-II\v6\"


[Files]

Source: ..\src\bin\debug\Landis.Extension.Output.AgeReclass.dll; DestDir: {#ExtDir}; Flags: replacesameversion
Source: docs\LANDIS-II Age Reclass Output v2.0 User Guide.pdf; DestDir: {#AppDir}\docs
Source: examples\*.txt; DestDir: {#AppDir}\examples\age-reclass-output
Source: examples\*.gis; DestDir: {#AppDir}\examples\age-reclass-output
Source: examples\*.bat; DestDir: {#AppDir}\examples\age-reclass-output

#define AgeReclassOutput "Age Reclass Output 2.0.txt"
Source: {#AgeReclassOutput}; DestDir: {#LandisPlugInDir}

[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""{#PackageName}"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#AgeReclassOutput}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]
;; Run plug-in admin tool to remove entries for each plug-in

[Code]
#include "J:\Scheller\LANDIS-II\deploy\package (Code section) v3.iss"

//-----------------------------------------------------------------------------

function CurrentVersion_PostUninstall(currentVersion: TInstalledVersion): Integer;
begin
    Result := 0;
end;

//-----------------------------------------------------------------------------

function InitializeSetup_FirstPhase(): Boolean;
begin
  CurrVers_PostUninstall := @CurrentVersion_PostUninstall
  Result := True
end;
