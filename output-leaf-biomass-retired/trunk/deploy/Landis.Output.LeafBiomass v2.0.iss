#define PackageName      "Output Leaf Biomass"
#define PackageNameLong  "Output Leaf Biomass"
#define Version          "2.1"
#define ReleaseType      "official"
#define ReleaseNumber    "2"

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include "J:\Scheller\LANDIS-II\deploy\package (Setup section) v6.0.iss"
#define ExtDir "C:\Program Files\LANDIS-II\v6\bin\extensions"
#define AppDir "C:\Program Files\LANDIS-II\v6\"

[Files]

; Output Biomass Ageclass v1.0 plug-in
Source: ..\src\bin\Debug\Landis.Extension.Output.LeafBiomass.dll; DestDir: {#ExtDir}; Flags: replacesameversion
Source: ..\src\bin\Debug\Landis.Library.Metadata.dll; DestDir: {#ExtDir}; Flags: replacesameversion uninsneveruninstall

; All the example input-files for the in examples
Source: docs\LANDIS-II Leaf Biomass Output v2.1 User Guide.pdf; DestDir: {#AppDir}\docs
Source: examples\*.txt; DestDir: {#AppDir}\examples\leaf-biomass-output; 
Source: examples\ecoregions.gis; DestDir: {#AppDir}\examples\leaf-biomass-output; 
Source: examples\initial-communities.gis; DestDir: {#AppDir}\examples\leaf-biomass-output; 
Source: examples\*.bat; DestDir: {#AppDir}\examples\leaf-biomass-output; 

#define BioLeaf "output-leafbiomass-install v2.1.txt"
Source: {#BioLeaf}; DestDir: {#LandisPlugInDir}


[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Output Leaf Biomass"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#BioLeaf}"" "; WorkingDir: {#LandisPlugInDir}

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
