//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Arjan de Bruijn 

using Landis.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using Landis.SpatialModeling;
 

namespace Landis.Extension.Output.PnET
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly new ExtensionType Type = new ExtensionType("output");
        public static readonly string ExtensionName = "Output-PnET";


        public static ISiteVar<Landis.Extension.Succession.BiomassPnET.ISiteCohorts> cohorts;
        public static ISiteVar<Landis.Library.Biomass.Pool> woodyDebris;
        public static ISiteVar<Landis.Library.Biomass.Pool> litter;
        
        

        private static int tstep;
        public static int TStep
        {
            get
            {
                return tstep;
            }
        }
        InputParameters parameters;
        static ICore modelCore;

        static IEnumerable<ISpecies> selectedspecies;
        static  OutputVariable Biomass;
        static  OutputVariable CohortsPerSpc;
        static  OutputVariable NonWoodyDebris;
        static  OutputVariable WoodyDebris;
        static  OutputVariable AgeDistribution;
        static OutputVariable AnnualPsn;
        static  OutputVariable BelowGround;
        static  OutputVariable LAI;
        static  OutputVariable SpeciesEstablishment;
        static OutputVariable EstablishmentProbability;
        static OutputVariable MonthlyNetPsn;
        static OutputVariable MonthlyFolResp;
        static OutputVariable MonthlyGrossPsn;
        static OutputVariable MonthlyMaintResp;
        static  OutputVariable Water;
        static  OutputVariable SubCanopyPAR;
        static OutputAggregatedTable overalloutputs;

        ISiteVar<Landis.Library.Parameters.Species.AuxParm<bool>> SpeciesWasThere;
        ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> LastBiom;

        //---------------------------------------------------------------------

        public PlugIn()
            : base(ExtensionName, Type)
        {
        }

        //---------------------------------------------------------------------

        public static ICore ModelCore
        {
            get
            {
                return modelCore;
            }
        }
        public static IEnumerable<ISpecies> SelectedSpecies
        {
            get
            {
                return selectedspecies;
            }
        }
        //---------------------------------------------------------------------

        public override void LoadParameters(string dataFile, ICore mCore)
        {
            modelCore = mCore;
            InputParametersParser parser = new InputParametersParser();
            parameters = Landis.Data.Load<InputParameters>(dataFile, parser);
        }

        //---------------------------------------------------------------------

        public override void Initialize()
        {
            Timestep = parameters.Timestep;
            selectedspecies = parameters.SelectedSpecies;

            tstep = parameters.Timestep;

            cohorts = PlugIn.ModelCore.GetSiteVar<Landis.Extension.Succession.BiomassPnET.ISiteCohorts>("Succession.CohortsPnET");
            woodyDebris = PlugIn.ModelCore.GetSiteVar<Landis.Library.Biomass.Pool>("Succession.WoodyDebris");
            litter = PlugIn.ModelCore.GetSiteVar<Landis.Library.Biomass.Pool>("Succession.Litter");
              
            if (parameters.CohortsPerSpecies != null) CohortsPerSpc = new OutputVariable(parameters.CohortsPerSpecies, "#");
            if (parameters.SpeciesBiom != null)
            {
                Biomass = new OutputVariable(parameters.SpeciesBiom, "g/m2");
                Biomass.output_table_ecoregions = new OutputTableEcoregions(Biomass.MapNameTemplate);
            }
            if (parameters.BelowgroundBiomass != null) BelowGround = new OutputVariable(parameters.BelowgroundBiomass, "g/m2");
            if (parameters.LeafAreaIndex != null)
            {
                LAI = new OutputVariable(parameters.LeafAreaIndex, "m2");
                LAI.output_table_ecoregions = new OutputTableEcoregions(LAI.MapNameTemplate);
            }
            if (parameters.MonthlyFolResp != null)
            {
                MonthlyFolResp = new OutputVariable(parameters.MonthlyFolResp, "gC/mo");
            }
            if (parameters.MonthlyGrossPsn != null)
            {
                MonthlyGrossPsn = new OutputVariable(parameters.MonthlyGrossPsn, "gC/mo");
            }
            if (parameters.MonthlyMaintResp != null)
            {
                MonthlyMaintResp = new OutputVariable(parameters.MonthlyMaintResp, "gC/mo");
            }
            if (parameters.MonthlyNetPsn != null)
            {
                MonthlyNetPsn = new OutputVariable(parameters.MonthlyNetPsn, "gC/mo");
            }
            
            if (parameters.EstablishmentProbability != null)
            {
                EstablishmentProbability = new OutputVariable(parameters.EstablishmentProbability, "");
               
            }
            if (parameters.SpeciesEst != null)
            {
                SpeciesEstablishment = new OutputVariable(parameters.SpeciesEst, "");
               
            }
            if (parameters.Water != null)
            {
                Water = new OutputVariable(parameters.Water, "mm");
                Water.output_table_ecoregions = new OutputTableEcoregions(Water.MapNameTemplate);
            }
            if (parameters.SubCanopyPAR != null) SubCanopyPAR = new OutputVariable(parameters.SubCanopyPAR,  "W/m2 pr mmol/m2");
            if (parameters.Litter != null) NonWoodyDebris = new OutputVariable(parameters.Litter, "g/m2");
            if (parameters.WoodyDebris != null) WoodyDebris = new OutputVariable(parameters.WoodyDebris,  "g/m2");
            if (parameters.AgeDistribution != null) AgeDistribution = new OutputVariable(parameters.AgeDistribution,"yr");
            if (parameters.AnnualPsn != null) AnnualPsn = new OutputVariable(parameters.AnnualPsn, "g/m2");
            if (parameters.CohortBalance != null) overalloutputs = new OutputAggregatedTable(parameters.CohortBalance);

            
        }

        
        public override void Run()
        {
            System.Console.WriteLine("Updating output variable: BelowGround");

            if (LAI != null)
            {
                System.Console.WriteLine("Updating output variable: LAI");
                // Total LAI per site 

                ISiteVar<byte> values = cohorts.GetIsiteVar(o => o.CanopyLAImax);

                string FileName = FileNames.ReplaceTemplateVars(LAI.MapNameTemplate, "", PlugIn.ModelCore.CurrentTime);

                new OutputMapSiteVar<byte, byte>(FileName, values, o => o);

                // Values per species each time step
                LAI.output_table_ecoregions.WriteUpdate(PlugIn.ModelCore.CurrentTime, values);
            }

            if (Biomass != null)
            {
                System.Console.WriteLine("Updating output variable: Biomass");


                ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> Biom = cohorts.GetIsiteVar(o => o.BiomassPerSpecies);

                foreach (ISpecies spc in PlugIn.SelectedSpecies)
                {
                    ISiteVar<int> Biom_spc = modelCore.Landscape.NewSiteVar<int>();

                    foreach (ActiveSite site in PlugIn.modelCore.Landscape)
                    {
                        Biom_spc[site] = Biom[site][spc];
                    }

                    new OutputMapSpecies(Biom_spc, spc, Biomass.MapNameTemplate);
                }

                OutputFilePerTStepPerSpecies.Write<int>(Biomass.MapNameTemplate, Biomass.units, PlugIn.ModelCore.CurrentTime, Biom);

                ISiteVar<float> Biomass_site = cohorts.GetIsiteVar(x => x.BiomassSum);

                Biomass.output_table_ecoregions.WriteUpdate<float>(PlugIn.ModelCore.CurrentTime, Biomass_site);
            }
            if (MonthlyFolResp != null)
            {
                ISiteVar<int[]> monthlyFolResp = cohorts.GetIsiteVar(site => site.FolResp);

                WriteMonthlyOutput(monthlyFolResp, MonthlyFolResp.MapNameTemplate);
            }
            if (MonthlyGrossPsn != null)
            {
                ISiteVar<int[]> monthlyGrossPsn = cohorts.GetIsiteVar(site => site.GrossPsn);

                WriteMonthlyOutput(monthlyGrossPsn, MonthlyGrossPsn.MapNameTemplate);
            }
            if (MonthlyNetPsn != null)
            {
                ISiteVar<int[]> monthlyNetPsn = cohorts.GetIsiteVar(site => site.NetPsn);

                WriteMonthlyOutput(monthlyNetPsn, MonthlyNetPsn.MapNameTemplate);
            }
            if (MonthlyMaintResp != null)
            {
                ISiteVar<int[]> monthlyMaintResp = cohorts.GetIsiteVar(site => site.MaintResp);

                WriteMonthlyOutput(monthlyMaintResp, MonthlyMaintResp.MapNameTemplate);
            }
            
            if (BelowGround != null)
            {
                System.Console.WriteLine("Updating output variable: BelowGround");

                ISiteVar<uint> values = cohorts.GetIsiteVar(o => o.BelowGroundBiomass);

                string FileName = FileNames.ReplaceTemplateVars(BelowGround.MapNameTemplate, "", PlugIn.ModelCore.CurrentTime);

                new OutputMapSiteVar<uint, uint>(FileName, values, o => o);
                
            }
            
            
            if (CohortsPerSpc != null)
            {
                System.Console.WriteLine("Updating output variable: CohortsPerSpc");
                // Nr of Cohorts per site and per species

                ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> cps =  cohorts.GetIsiteVar(x => x.CohortCountPerSpecies);

                new OutputHistogramCohort<int>(CohortsPerSpc.MapNameTemplate, "CohortsPerSpcPerSite", 10).WriteOutputHist(cps);

                foreach (ISpecies spc in PlugIn.modelCore.Species)
                {
                    string FileName = FileNames.ReplaceTemplateVars(CohortsPerSpc.MapNameTemplate, "", PlugIn.ModelCore.CurrentTime);

                    new OutputMapSiteVar<Landis.Library.Parameters.Species.AuxParm<int>, int>(FileName, cps, o => o[spc]);
                }

                OutputFilePerTStepPerSpecies.Write<int>(CohortsPerSpc.MapNameTemplate, CohortsPerSpc.units, PlugIn.ModelCore.CurrentTime, cps); 
            }
            if (EstablishmentProbability != null)
            {
                System.Console.WriteLine("Updating output variable: EstablishmentProbability");
                 
                ISiteVar<Landis.Library.Parameters.Species.AuxParm<byte>> pest = cohorts.GetIsiteVar(o => o.EstablishmentProbability.Probability);

                foreach (ISpecies spc in PlugIn.SelectedSpecies)
                {
                    ISiteVar<int> _pest = modelCore.Landscape.NewSiteVar<int>();

                    foreach (ActiveSite site in PlugIn.modelCore.Landscape)
                    {
                        _pest[site] = pest[site][spc];
                    }

                    new OutputMapSpecies(_pest, spc, EstablishmentProbability.MapNameTemplate);
                }

            }
            if (SpeciesEstablishment != null)
            {
                System.Console.WriteLine("Updating output variable: SpeciesEstablishment");

                ISiteVar<Landis.Library.Parameters.Species.AuxParm<bool>> SpeciesIsThere = cohorts.GetIsiteVar(o => o.SpeciesPresent);

                if (SpeciesWasThere != null)  
                {
                    foreach (ISpecies spc in PlugIn.modelCore.Species)
                    {
                        ISiteVar<int> comp = PlugIn.modelCore.Landscape.NewSiteVar<int>();

                        MapComparison m = new MapComparison();
                        foreach (ActiveSite site in PlugIn.modelCore.Landscape)
                        {
                            if (SpeciesWasThere[site] == null)
                            {
                                SpeciesWasThere[site] = new Library.Parameters.Species.AuxParm<bool>(PlugIn.modelCore.Species);
                            }

                            comp[site] = m[SpeciesWasThere[site][spc], SpeciesIsThere[site][spc]];

                            SpeciesWasThere[site][spc] = SpeciesIsThere[site][spc];
                        }


                        OutputMapSpecies output_map =  new OutputMapSpecies(comp, spc, SpeciesEstablishment.MapNameTemplate);

                        // map label text
                        m.PrintLabels(SpeciesEstablishment.MapNameTemplate, spc);

                        
                    }
                }
                else 
                {
                    SpeciesWasThere = modelCore.Landscape.NewSiteVar<Landis.Library.Parameters.Species.AuxParm<bool>>();

                    foreach (ActiveSite site in PlugIn.modelCore.Landscape)
                    {
                        SpeciesWasThere[site] = new Library.Parameters.Species.AuxParm<bool>(PlugIn.modelCore.Species);
                    }
                }

                ISiteVar<Landis.Library.Parameters.Species.AuxParm<bool>> Established_spc = cohorts.GetIsiteVar(x => x.SpeciesPresent);

                Landis.Library.Parameters.Species.AuxParm<int> Est_Sum = new Landis.Library.Parameters.Species.AuxParm<int>(PlugIn.modelCore.Species);

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                { 
                    foreach(ISpecies spc in PlugIn.modelCore.Species)
                    {
                        if(Established_spc[site][spc] ==true)
                        {
                            Est_Sum[spc]++;
                        }
                    }
                }

                OutputFilePerTStepPerSpecies.Write<int>(SpeciesEstablishment.MapNameTemplate, SpeciesEstablishment.units, PlugIn.ModelCore.CurrentTime, Est_Sum);
                
            }
            if (AnnualPsn != null)
            {
                System.Console.WriteLine("Updating output variable: AnnualPsn");

                ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> Biom = cohorts.GetIsiteVar(o => o.BiomassPerSpecies);

                if (LastBiom != null)
                {
                    foreach (ISpecies spc in PlugIn.modelCore.Species)
                    {
                        ISiteVar<int> comp = PlugIn.modelCore.Landscape.NewSiteVar<int>();

                        MapComparison m = new MapComparison();
                        foreach (ActiveSite site in PlugIn.modelCore.Landscape)
                        {
                            if (LastBiom[site] == null)
                            {
                                LastBiom[site] = new Library.Parameters.Species.AuxParm<int>(PlugIn.modelCore.Species);
                            }

                            comp[site] = Biom[site][spc] - LastBiom[site][spc];

                            LastBiom[site][spc] = Biom[site][spc];
                        }

                        OutputMapSpecies output_map = new OutputMapSpecies(comp, spc, AnnualPsn.MapNameTemplate);

                    }
                }
                else
                {
                    LastBiom = modelCore.Landscape.NewSiteVar<Landis.Library.Parameters.Species.AuxParm<int>>();

                    foreach (ActiveSite site in PlugIn.modelCore.Landscape)
                    {
                        LastBiom[site] = new Library.Parameters.Species.AuxParm<int>(PlugIn.modelCore.Species);
                    }
                }

            }
            
            if (Water != null)
            {
                System.Console.WriteLine("Updating output variable: Water");

                ISiteVar<ushort> Water_site = cohorts.GetIsiteVar(x => x.WaterMax);

                string FileName = FileNames.ReplaceTemplateVars(Water.MapNameTemplate, "", PlugIn.ModelCore.CurrentTime);

                new OutputMapSiteVar<ushort, ushort>(FileName, Water_site, o => o);

                Water.output_table_ecoregions.WriteUpdate(PlugIn.ModelCore.CurrentTime, Water_site);
            }
            
            if (SubCanopyPAR != null)
            {
                System.Console.WriteLine("Updating output variable: SubCanopyPAR");

                ISiteVar<float> SubCanopyRadiation = cohorts.GetIsiteVar(x => x.SubCanopyParMAX);

                string FileName = FileNames.ReplaceTemplateVars(SubCanopyPAR.MapNameTemplate, "", PlugIn.ModelCore.CurrentTime);

                new OutputMapSiteVar<float, float>(FileName, SubCanopyRadiation, o => o);

                 
            }
            if (NonWoodyDebris != null)
            {
                System.Console.WriteLine("Updating output variable: NonWoodyDebris");

                ISiteVar<double> Litter = cohorts.GetIsiteVar(x => x.Litter);

                string FileName = FileNames.ReplaceTemplateVars(NonWoodyDebris.MapNameTemplate, "", PlugIn.ModelCore.CurrentTime);

                new OutputMapSiteVar<double, double>(FileName, Litter, o => o);
              
            }
            
            if (WoodyDebris != null)
            {
                System.Console.WriteLine("Updating output variable: WoodyDebris");

                ISiteVar<double> woody_debris = cohorts.GetIsiteVar(x => x.WoodyDebris);

                string FileName = FileNames.ReplaceTemplateVars(WoodyDebris.MapNameTemplate, "", PlugIn.ModelCore.CurrentTime);

                new OutputMapSiteVar<double, double>(FileName, woody_debris, o => o);

            }
            
            if (AgeDistribution != null)
            {
                System.Console.WriteLine("Updating output variable: AgeDistribution");

                ISiteVar<Landis.Library.Parameters.Species.AuxParm<List<ushort>>> values = cohorts.GetIsiteVar(o => o.CohortAges);

                new OutputHistogramCohort<ushort>(AgeDistribution.MapNameTemplate, "NrOfCohortsAtAge", 10).WriteOutputHist(values);
                 

                System.Console.WriteLine("Updating output variable: MaxAges");

                ISiteVar<int> maxage = cohorts.GetIsiteVar(x => x.AgeMax);

                string FileName = FileNames.ReplaceTemplateVars(AgeDistribution.MapNameTemplate, "", PlugIn.ModelCore.CurrentTime);

                new OutputMapSiteVar<int, int>(FileName, maxage, o => o);
                 
            }
            if (overalloutputs != null)
            {
                System.Console.WriteLine("Updating output variable: overalloutputs");
                OutputAggregatedTable.WriteNrOfCohortsBalance();
            }

          
        }

        private static void WriteMonthlyOutput(ISiteVar<int[]> montly, string MapNameTemplate)
        {
            string[] months = new string[] { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "okt", "nov", "dec" };

            for (int mo = 0; mo < months.Count(); mo++)
            {
                ISiteVar<int> monthlyValue = PlugIn.ModelCore.Landscape.NewSiteVar<int>();

                foreach (ActiveSite site in PlugIn.modelCore.Landscape)
                {
                    monthlyValue[site] = montly[site][mo];
                }

                string FileName = FileNames.ReplaceTemplateVars(MapNameTemplate, "", PlugIn.ModelCore.CurrentTime);

                FileName = System.IO.Path.ChangeExtension(FileName, null) + months[mo] + System.IO.Path.GetExtension(FileName);

                new OutputMapSiteVar<int, int>(FileName, monthlyValue, o => o);
            }
        }

        
       
        
    }
}
