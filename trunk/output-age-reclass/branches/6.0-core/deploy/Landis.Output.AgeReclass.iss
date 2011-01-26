#define PackageName      "Age Reclass Output"
#define PackageNameLong  "Age Reclass Output"
#define Version          "1.1"
#define ReleaseType      "official"
#define ReleaseNumber    ""

#define CoreVersion      "5.1"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section).iss"

;#include "..\package (Setup section).iss"


[Files]

Source: {#LandisBuildDir}\OutputExtensions\output-reclass\src\output-reclass\obj\release\Landis.Extension.Output.Reclass.dll; DestDir: {app}\bin
Source: docs\LANDIS-II Reclass Output v1.1 User Guide.pdf; DestDir: {app}\docs
Source: examples\*; DestDir: {app}\examples

#define AgeReclassOutput "Age Reclass Output 1.1.txt"
Source: {#AgeReclassOutput}; DestDir: {#LandisPlugInDir}

[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""{#PackageName}"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#AgeReclassOutput}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]
;; Run plug-in admin tool to remove entries for each plug-in

[Code]
#include AddBackslash(LandisDeployDir) + "package (Code section) v3.iss"

//-----------------------------------------------------------------------------

function CurrentVersion_PostUninstall(currentVersion: TInstalledVersion): Integer;
begin
  // Remove the plug-in name from database
  //if StartsWith(currentVersion.Version, '1.0') then
  //  begin
  //    Exec('{#PlugInAdminTool}', 'remove "{#PackageName}"',
  //         ExtractFilePath('{#PlugInAdminTool}'),
	//	   SW_HIDE, ewWaitUntilTerminated, Result);
	//end
  //else
    Result := 0;
end;

//-----------------------------------------------------------------------------

function InitializeSetup_FirstPhase(): Boolean;
begin
  CurrVers_PostUninstall := @CurrentVersion_PostUninstall
  Result := True
end;
