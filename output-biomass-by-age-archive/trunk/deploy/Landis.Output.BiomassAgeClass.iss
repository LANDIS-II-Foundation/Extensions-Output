#define PackageName      "Output Biomass Ageclass"
#define PackageNameLong  "Output Biomass Ageclass"
#define Version          "1.0"
#define ReleaseType      "official"
#define ReleaseNumber    "1"

#define CoreVersion      "5.1"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section).iss"

;#include "..\package (Setup section).iss"


[Files]
; Cohort Libraries
Source: {#LandisBuildDir}\libraries\biomass-cohort\build\release\Landis.Library.Cohorts.Biomass.dll; DestDir: {app}\bin; Flags: replacesameversion uninsneveruninstall

; Output Biomass Ageclass v1.0 plug-in
Source: {#LandisBuildDir}\OutputExtensions\output-biomass-by-age\build\release\Landis.Extension.Output.BiomassAgeClass.dll; DestDir: {app}\bin

; All the example input-files for the in examples
Source: examples\*; DestDir: {app}\examples; Flags: recursesubdirs
Source: docs\LANDIS-II Age Biomass Output v1.0 User Guide.pdf; DestDir: {app}\docs

#define BioAgeclass "output-biomass-ageclass_install.txt"
Source: {#BioAgeclass}; DestDir: {#LandisPlugInDir}


[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Output Biomass AgeClass"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#BioAgeclass}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]
;; Run plug-in admin tool to remove entries for each plug-in
; Filename: {#PlugInAdminTool}; Parameters: "remove ""Output Biomass AgeClass"" "; WorkingDir: {#LandisPlugInDir}

[Code]
#include AddBackslash(LandisDeployDir) + "package (Code section) v2.iss"

//-----------------------------------------------------------------------------

function CurrentVersion_PostUninstall(currentVersion: TInstalledVersion): Integer;
begin
  // Remove the plug-in name from database
  if StartsWith(currentVersion.Version, '1.0') then
    begin
      Exec('{#PlugInAdminTool}', 'remove "Output Biomass AgeClass"',
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
