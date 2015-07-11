#define PackageName      "Output Cohort Statistics"
#define PackageNameLong  "Output Cohort Statistics"
#define Version          "1.0"
#define ReleaseType      "official"
#define ReleaseNumber    ""

#define CoreVersion      "5.1"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section).iss"

;#include "..\package (Setup section).iss"


[Files]

; Output Biomass Ageclass v1.0 plug-in
;
Source: {#LandisBuildDir}\OutputExtensions\output-cohort-stats\build\release\Landis.Output.CohortStats.dll; DestDir: {app}\bin
Source: docs\*; DestDir: {app}\docs
Source: examples\*; DestDir: {app}\examples

;Source: docs\LANDIS-II Dynamic Biomass Fuel System v1.0 User Guide.pdf; DestDir: {app}\doc

#define CohortStats "Output Age Cohort Stats 1.0.txt"
Source: {#CohortStats}; DestDir: {#LandisPlugInDir}

; All the example input-files for the in examples\
Source: examples\*; DestDir: {app}\examples\cohort-stats; Flags: recursesubdirs

[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "add ""{#CohortStats}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]
;; Run plug-in admin tool to remove entries for each plug-in
Filename: {#PlugInAdminTool}; Parameters: "remove ""{#PackageName}"" "; WorkingDir: {#LandisPlugInDir}

[Code]
#include AddBackslash(LandisDeployDir) + "package (Code section).iss"

//-----------------------------------------------------------------------------

function CurrentVersion_PostUninstall(currentVersion: TInstalledVersion): Integer;
begin
  // Remove the plug-in name from database
  if StartsWith(currentVersion.Version, '1.0') then
    begin
      Exec('{#PlugInAdminTool}', 'remove "{#PackageName}"',
           ExtractFilePath('{#PlugInAdminTool}'),
		   SW_HIDE, ewWaitUntilTerminated, Result);
	end
  else
    Result := 0;
end;

//-----------------------------------------------------------------------------

function InitializeSetup_FirstPhase(): Boolean;
begin
  CurrVers_PostUninstall := @CurrentVersion_PostUninstall
  Result := True
end;
