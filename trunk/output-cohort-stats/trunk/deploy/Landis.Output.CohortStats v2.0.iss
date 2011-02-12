#define PackageName      "Output Cohort Statistics"
#define PackageNameLong  "Output Cohort Statistics"
#define Version          "2.0"
#define ReleaseType      "official"
#define ReleaseNumber    ""

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section) v6.0.iss"


[Files]

; Output Biomass Ageclass v1.0 plug-in
;
Source: C:\Program Files\LANDIS-II\6.0\bin\Landis.Extension.Output.CohortStats.dll; DestDir: {app}\bin
Source: docs\LANDIS-II Age Cohort Statistics v2.0 User Guide.pdf; DestDir: {app}\docs
Source: examples\*; DestDir: {app}\examples\age-cohort-stats

;Source: docs\LANDIS-II Dynamic Biomass Fuel System v1.0 User Guide.pdf; DestDir: {app}\doc

#define CohortStats "Output Age Cohort Stats 2.0.txt"
Source: {#CohortStats}; DestDir: {#LandisPlugInDir}

; All the example input-files for the in examples\
Source: examples\*; DestDir: {app}\examples\cohort-stats; Flags: recursesubdirs

[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""{#PackageName}"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#CohortStats}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]
;; Run plug-in admin tool to remove entries for each plug-in

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
