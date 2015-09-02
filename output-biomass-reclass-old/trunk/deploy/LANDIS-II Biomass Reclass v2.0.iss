#define PackageName      "Biomass Reclassified Output"
#define PackageNameLong  "Biomass Reclassified Output Extension"
#define Version          "2.0"
#define ReleaseType      "official"
#define ReleaseNumber    "2"

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include "J:\Scheller\LANDIS-II\deploy\package (Setup section) v6.0.iss"
#define ExtDir "C:\Program Files\LANDIS-II\v6\bin\extensions"
#define AppDir "C:\Program Files\LANDIS-II\v6"

[Files]

; Biomass Reclass v2.0 plug-in
Source: ..\src\bin\debug\Landis.Extension.Output.BiomassReclass.dll; DestDir: {#ExtDir}; Flags: replacesameversion
Source: docs\LANDIS-II Biomass Reclass Output v2.0 User Guide.pdf; DestDir: {#AppDir}\docs
Source: examples\ecoregions.gis; DestDir: {#AppDir}\examples\output-biomass-reclass
Source: examples\initial-communities.gis; DestDir: {#AppDir}\examples\output-biomass-reclass
Source: examples\*.txt; DestDir: {#AppDir}\examples\output-biomass-reclass
Source: examples\*.bat; DestDir: {#AppDir}\examples\output-biomass-reclass

#define BiomassReclass "Biomass Reclass 2.0.txt"
Source: {#BiomassReclass}; DestDir: {#LandisPlugInDir}

[Run]
;; Run plug-in admin tool to add an entry for the plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Biomass Reclass"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#BiomassReclass}"" "; WorkingDir: {#LandisPlugInDir}

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
