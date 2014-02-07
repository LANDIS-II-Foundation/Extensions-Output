#define PackageName      "Output PnET"
#define PackageNameLong  "Output PnET Extension"
#define Version          "1.0"
#define ReleaseType      "alpha"
#define ReleaseNumber    "1"

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#define DeployFolder AddBackslash("C:\Users\adebruij\Desktop\PnET_Succession\InstallerFolder\PnET_Output\deploy")
#include DeployFolder + "package (Setup section) v6.0.iss"

;#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section) v6.0.iss"

#if ReleaseType != "official"
  #define Configuration  "debug"
#else
  #define Configuration  "release"
#endif


[Files]
 
Source: C:\Program Files\LANDIS-II\v6\bin\extensions\Landis.Extension.Output.BiomassPnET.dll; DestDir: {app}\bin; Flags: replacesameversion


#define Output_PnET "PnET Output 1.0.txt"
Source: {#Output_PnET}; DestDir: {#LandisPlugInDir}

[Run]
;; Run plug-in admin tool to add the entry for the plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Output_PnET"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#Output_PnET}"" "; WorkingDir: {#LandisPlugInDir}


[Code]
{ Check for other prerequisites during the setup initialization }

#include DeployFolder + "package (Code section) v3.iss"

//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------

function CurrentVersion_PostUninstall(currentVersion: TInstalledVersion): Integer;
begin
end;


//-----------------------------------------------------------------------------

function InitializeSetup_FirstPhase(): Boolean;
begin
  CurrVers_PostUninstall := @CurrentVersion_PostUninstall
  Result := True
end;
