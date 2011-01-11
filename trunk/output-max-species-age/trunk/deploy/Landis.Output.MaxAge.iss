#define PackageName      "Max Species Age Output"
#define PackageNameLong  "Max Species Age Output"
#define Version          "1.1"
#define ReleaseType      "official"
#define ReleaseNumber    ""

#define CoreVersion      "5.1"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section).iss"

;#include "..\package (Setup section).iss"


[Files]

Source: {#LandisBuildDir}\OutputExtensions\output-max-species-age\build\release\Landis.Output.MaxSpeciesAge.dll; DestDir: {app}\bin
Source: docs\LANDIS-II Age Output v1.1 User Guide.pdf; DestDir: {app}\docs
Source: examples\*; DestDir: {app}\examples

#define MaxAgeOutput "Max Age Output 1.1.txt"
Source: {#MaxAgeOutput}; DestDir: {#LandisPlugInDir}

[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "add ""{#MaxAgeOutput}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]
;; Run plug-in admin tool to remove entries for each plug-in
Filename: {#PlugInAdminTool}; Parameters: "remove ""{#PackageName}"" "; WorkingDir: {#LandisPlugInDir}

[Code]
#include AddBackslash(LandisDeployDir) + "package (Code section) v2.iss"

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
