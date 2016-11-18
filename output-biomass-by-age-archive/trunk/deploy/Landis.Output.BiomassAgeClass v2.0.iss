#define PackageName      "Output Biomass Ageclass"
#define PackageNameLong  "Output Biomass Ageclass"
#define Version          "2.0.1"
#define ReleaseType      "official"
#define ReleaseNumber    "2"

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include "J:\Scheller\LANDIS-II\deploy\package (Setup section) v6.0.iss"
#define ExtDir "C:\Program Files\LANDIS-II\v6\bin\extensions"
#define AppDir "C:\Program Files\LANDIS-II\v6"

[Files]

; Output Biomass Ageclass v1.0 plug-in
Source: ..\src\bin\debug\Landis.Extension.Output.BiomassByAge.dll; DestDir: {#ExtDir}; Flags: replacesameversion

; All the example input-files for the in examples
Source: docs\LANDIS-II Age Biomass Output v2.0 User Guide.pdf; DestDir: {#AppDir}\docs
Source: examples\ecoregions.gis; DestDir: {#AppDir}\examples\biomass-age-output
Source: examples\initial-communities.gis; DestDir: {#AppDir}\examples\biomass-age-output
Source: examples\*.txt; DestDir: {#AppDir}\examples\biomass-age-output
Source: examples\*.bat; DestDir: {#AppDir}\examples\biomass-age-output

#define BioAgeclass "output-biomass-ageclass v2.0.txt"
Source: {#BioAgeclass}; DestDir: {#LandisPlugInDir}


[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Output Biomass-by-Age"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#BioAgeclass}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]

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
