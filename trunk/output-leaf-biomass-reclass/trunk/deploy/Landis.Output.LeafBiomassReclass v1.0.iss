#define PackageName      "Output Leaf Biomass Reclass"
#define PackageNameLong  "Output Leaf Biomass Reclass"
#define Version          "1.0"
#define ReleaseType      "official"
#define ReleaseNumber    "1"

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include "J:\Scheller\LANDIS-II\deploy\package (Setup section) v6.0.iss"
#define BuildDir "C:\Program Files\LANDIS-II\v6\bin\extensions"
#define AppDir "C:\Program Files\LANDIS-II\v6"

[Files]

; Output Biomass Ageclass v1.0 plug-in
Source: {#BuildDir}\Landis.Extension.Output.LeafBiomassReclass.dll; DestDir: {#BuildDir}; Flags: replacesameversion

; All the example input-files for the in examples
Source: examples\*; DestDir: {#AppDir}\examples\output-leaf-biomass-reclass; Flags: recursesubdirs
Source: docs\LANDIS-II Leaf Biomass Reclass Output v1.0 User Guide.pdf; DestDir: {#AppDir}\docs

#define BioLeaf "Leaf Biomass Reclass v1.0.txt"
Source: {#BioLeaf}; DestDir: {#LandisPlugInDir}


[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Output Leaf Biomass Reclass"" "; WorkingDir: {#LandisPlugInDir}
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