#define PackageName      "Output Leaf Biomass"
#define PackageNameLong  "Output Leaf Biomass"
#define Version          "2.0"
#define ReleaseType      "official"
#define ReleaseNumber    "2"

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section) v6.0.iss"

[Files]

; Output Biomass Ageclass v1.0 plug-in
Source: C:\Program Files\LANDIS-II\6.0\bin\Landis.Extension.Output.LeafBiomass.dll; DestDir: {app}\bin

; All the example input-files for the in examples
Source: examples\*; DestDir: {app}\examples\leaf-biomass-output; Flags: recursesubdirs
Source: docs\LANDIS-II Leaf Biomass Output v2.0 User Guide.pdf; DestDir: {app}\docs

#define BioLeaf "output-leafbiomass-install v2.0.txt"
Source: {#BioLeaf}; DestDir: {#LandisPlugInDir}


[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Output Leaf Biomass"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#BioLeaf}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]
;; Run plug-in admin tool to remove entries for each plug-in
; Filename: {#PlugInAdminTool}; Parameters: "remove ""Output Leaf Biomass"" "; WorkingDir: {#LandisPlugInDir}

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
