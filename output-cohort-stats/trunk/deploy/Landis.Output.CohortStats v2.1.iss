#define PackageName      "Output Cohort Statistics"
#define PackageNameLong  "Output Cohort Statistics"
#define Version          "2.1.2"
#define ReleaseType      "official"
#define ReleaseNumber    ""

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include "J:\Scheller\LANDIS-II\deploy\package (Setup section) v6.0.iss"
#define ExtDir "C:\Program Files\LANDIS-II\v6\bin\extensions"
#define AppDir "C:\Program Files\LANDIS-II\v6\"

[Files]

; Output Biomass Ageclass v1.0 plug-in
;
Source: ..\src\bin\debug\Landis.Extension.Output.CohortStats.dll; DestDir: {#ExtDir}; Flags: replacesameversion

Source: docs\LANDIS-II Age Cohort Statistics v2.1 User Guide.pdf; DestDir: {#AppDir}\docs
Source: examples\*.txt; DestDir: {#AppDir}\examples\output-cohort-stats
Source: examples\ecoregions.gis; DestDir: {#AppDir}\examples\output-cohort-stats
Source: examples\initial-communities.gis; DestDir: {#AppDir}\examples\output-cohort-stats
Source: examples\*.bat; DestDir: {#AppDir}\examples\output-cohort-stats

#define CohortStats "Output Age Cohort Stats 2.1.txt"
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
