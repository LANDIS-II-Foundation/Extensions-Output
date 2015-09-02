#define PackageName      "Biomass Reclassified Output"
#define PackageNameLong  "Biomass Reclassified Output Extension"
#define Version          "1.1"
#define ReleaseType      "official"
#define ReleaseNumber    "1"

#define CoreVersion      "5.1"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section).iss"

#if ReleaseType != "official"
  #define Configuration  "debug"
#else
  #define Configuration  "release"
#endif

[Files]

; Cohort Libraries
Source: {#LandisBuildDir}\libraries\biomass-cohort\build\release\Landis.Library.Cohorts.Biomass.dll; DestDir: {app}\bin; Flags: replacesameversion uninsneveruninstall

; Biomass Reclass v1.0 plug-in
Source: {#LandisBuildDir}\outputextensions\output-biomass-reclass\build\release\Landis.Extension.Output.BiomassReclass.dll; DestDir: {app}\bin
Source: {#LandisBuildDir}\outputextensions\output-biomass-reclass\deploy\docs\LANDIS-II Biomass Reclass Output v1.0 User Guide.pdf; DestDir: {app}\docs
Source: {#LandisBuildDir}\outputextensions\output-biomass-reclass\deploy\examples\output-biomass-reclass.txt; DestDir: {app}\examples

#define BiomassReclass "Biomass Reclass 1.1.txt"
Source: {#BiomassReclass}; DestDir: {#LandisPlugInDir}

[Run]
;; Run plug-in admin tool to add an entry for the plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Biomass Reclass"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#BiomassReclass}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]
;; Run plug-in admin tool to remove the entry for the plug-in
; Filename: {#PlugInAdminTool}; Parameters: "remove ""Biomass Reclass"" "; WorkingDir: {#LandisPlugInDir}

[Code]
#include AddBackslash(LandisDeployDir) + "package (Code section) v2.iss"

//-----------------------------------------------------------------------------

function CurrentVersion_PostUninstall(currentVersion: TInstalledVersion): Integer;
begin
  // Do not remove version 1.0 from the database.
  if StartsWith(currentVersion.Version, '2') then
    begin
      Exec('{#PlugInAdminTool}', 'remove "Biomass Succession v2"',
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
