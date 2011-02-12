#define PackageName      "Biomass Ouput"
#define PackageNameLong  "Biomass Output Extensions"
#define Version          "2.0"
#define ReleaseType      "official"
#define ReleaseNumber    "2"

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section) v6.0.iss"

[Files]
#define BuildDir "C:\Program Files\LANDIS-II\6.0\bin"

; Biomass Output v1.2 plug-in
Source: {#BuildDir}\Landis.Extension.Output.Biomass.dll; DestDir: {app}\bin
Source: docs\LANDIS-II Biomass Output v2.0 User Guide.pdf; DestDir: {app}\docs
Source: examples\*; DestDir: {app}\examples\output-biomass

#define BiomassOutput "Output Biomass 2.0.txt"
Source: {#BiomassOutput}; DestDir: {#LandisPlugInDir}

[Run]
;; Run plug-in admin tool to add an entry for the plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Output Biomass"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#BiomassOutput}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]

[Code]
#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Code section) v3.iss"

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
