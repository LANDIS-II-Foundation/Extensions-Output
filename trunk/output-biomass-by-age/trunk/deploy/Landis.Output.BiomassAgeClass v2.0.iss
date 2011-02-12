#define PackageName      "Output Biomass Ageclass"
#define PackageNameLong  "Output Biomass Ageclass"
#define Version          "2.0"
#define ReleaseType      "official"
#define ReleaseNumber    "2"

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section) v6.0.iss"

[Files]
; Cohort Libraries
#define BuildDir "C:\Program Files\LANDIS-II\6.0\bin"

; Output Biomass Ageclass v1.0 plug-in
Source: {#BuildDir}\Landis.Extension.Output.BiomassByAge.dll; DestDir: {app}\bin

; All the example input-files for the in examples
Source: examples\*; DestDir: {app}\examples\biomass-age-output; Flags: recursesubdirs
Source: docs\LANDIS-II Age Biomass Output v2.0 User Guide.pdf; DestDir: {app}\docs

#define BioAgeclass "output-biomass-ageclass v2.0.txt"
Source: {#BioAgeclass}; DestDir: {#LandisPlugInDir}


[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Output Biomass-by-Age"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#BioAgeclass}"" "; WorkingDir: {#LandisPlugInDir}

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
