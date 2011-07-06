#define PackageName      "Age Reclass Output"
#define PackageNameLong  "Age Reclass Output"
#define Version          "2.0"
#define ReleaseType      "official"
#define ReleaseNumber    ""

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section) v6.0.iss"

[Files]

Source: C:\Program Files\LANDIS-II\6.0\bin\Landis.Extension.Output.AgeReclass.dll; DestDir: {app}\bin; Flags: replacesameversion
Source: docs\LANDIS-II Age Reclass Output v2.0 User Guide.pdf; DestDir: {app}\docs
Source: examples\*; DestDir: {app}\examples\age-reclass-output

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
#include AddBackslash(GetEnv("LANDIS_DEPLOY"))  + "package (Code section) v3.iss"

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
