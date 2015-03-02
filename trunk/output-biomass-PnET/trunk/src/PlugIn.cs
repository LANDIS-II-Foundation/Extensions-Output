//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Arjan de Bruijn 

using Landis.Core;

using System;
using System.Collections.Generic;
 
using Landis.SpatialModeling;
 

namespace Landis.Extension.Output.PnET
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly new ExtensionType Type = new ExtensionType("output");
        public static readonly string ExtensionName = "Output-PnET";

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
        static  OutputVariable BelowGround;
        static  OutputVariable LAI;
        static  OutputVariable SpeciesEstablishment;
        static  OutputVariable Water;
        static  OutputVariable SubCanopyPAR;
        static  OverallOutputs overalloutputs;

        Landis.Library.Parameters.Species.AuxParm<ISiteVar<bool>> SpeciesWasThere;
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

            tstep = parameters.Timestep;
            
            SiteVars.Initialize();
            
            selectedspecies = parameters.SelectedSpecies;

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
            if (parameters.SpeciesEst != null)
            {
                SpeciesEstablishment = new OutputVariable(parameters.SpeciesEst, "");
                SpeciesWasThere = new Library.Parameters.Species.AuxParm<ISiteVar<bool>>(PlugIn.modelCore.Species);
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
            if (parameters.CohortBalance != null) overalloutputs = new OverallOutputs(parameters.CohortBalance);
            
            
        }
        
        public static ISiteVar<int> SpeciesSum(Landis.Library.Parameters.Species.AuxParm<ISiteVar<int>>  SpeciesSpecific)
        {
            ISiteVar<int> TotalBiomass = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
            foreach (ActiveSite site in PlugIn.modelCore.Landscape)
            {
                foreach (ISpecies species in PlugIn.modelCore.Species)
                {
                    TotalBiomass[site] += SpeciesSpecific[species][site];
                }
            }
            return TotalBiomass;
        }

        public override void Run()
        {
            if (BelowGround != null)
            {
                System.Console.WriteLine("Updating output variable: BelowGround");
                new OutputMapSiteVar<int>(BelowGround.MapNameTemplate, "", SiteVars.BelowGroundBiomass);
                
            }
            if (LAI != null)
            {
                System.Console.WriteLine("Updating output variable: LAI");
                // Total LAI per site 

                new OutputMapSiteVar<byte>(LAI.MapNameTemplate,"", SiteVars.CanopyLAImax);

                // Values per species each time step
                LAI.output_table_ecoregions.WriteUpdate (PlugIn.ModelCore.CurrentTime, SiteVars.CanopyLAImax);
            }
            
            if (CohortsPerSpc != null)
            {
                System.Console.WriteLine("Updating output variable: CohortsPerSpc");
                // Nr of Cohorts per site and per species
                
                new OutputHistogramCohort(CohortsPerSpc.MapNameTemplate, "CohortsPerSpcPerSite", 10).WriteOutputHist(SiteVars.Cohorts);      

                new OutputMapSiteVar<int>(CohortsPerSpc.MapNameTemplate, "",SiteVars.CohortsPerSite);
                 
                // Nr of cohorts per species
                OutputFilePerTStepPerSpecies.Write<int>(CohortsPerSpc.MapNameTemplate, CohortsPerSpc.units, PlugIn.ModelCore.CurrentTime, SiteVars.Cohorts_spc); 
            }
            
            if (SpeciesEstablishment != null)
            {
                System.Console.WriteLine("Updating output variable: SpeciesEstablishment");

                 Landis.Library.Parameters.Species.AuxParm<ISiteVar<int>> SpeciesIsThere = SiteVars.Cohorts;

                foreach(ISpecies spc in PlugIn.modelCore.Species)
                {
                    if (SpeciesWasThere[spc] != null)
                    {

                        MapComparison m = new MapComparison();

                        ISiteVar<int> comp = SpeciesWasThere[spc].Compare(SpeciesIsThere[spc].ToBool(), m);

                        OutputMapSpecies output_map =  new OutputMapSpecies(comp, spc, SpeciesEstablishment.MapNameTemplate);

                        // map label text
                        m.PrintLabels(System.IO.Path.ChangeExtension(output_map.FileName, "mlt"));

                        ISiteVar<bool> SpeciesIsThereBool = SpeciesIsThere[spc].ToBool();
                    
                    }
                    SpeciesWasThere[spc] = SiteVars.Cohorts[spc].Copy<int>().ToBool();
                }


                
            }
            
            if (Biomass != null)
            {
                System.Console.WriteLine("Updating output variable: Biomass");

                // write maps biomass per species per pixel
                // Variable per species and per site (multiple maps)
                
                Landis.Library.Parameters.Species.AuxParm<Landis.SpatialModeling.ISiteVar<int>> BiomassPerSiteSpecies = SiteVars.Biomass;
                   
                foreach (ISpecies spc in PlugIn.SelectedSpecies)
                {
                    new OutputMapSpecies(BiomassPerSiteSpecies[spc], spc, Biomass.MapNameTemplate);
                }


                new OutputMapSiteVar<int>(Biomass.MapNameTemplate, "Total", SpeciesSum(BiomassPerSiteSpecies));

                // overview table 
                OutputFilePerTStepPerSpecies.Write<float>(Biomass.MapNameTemplate, Biomass.units, PlugIn.ModelCore.CurrentTime, SiteVars.Biomass_spc);

                // Values per species each time step
                Biomass.output_table_ecoregions.WriteUpdate<int>(PlugIn.ModelCore.CurrentTime, SiteVars.Biomass_per_site);
            }
            if (Water != null)
            {
                System.Console.WriteLine("Updating output variable: Water");
                
                new OutputMapSiteVar<ushort>(Water.MapNameTemplate,"", SiteVars.Water);

                Water.output_table_ecoregions.WriteUpdate(PlugIn.ModelCore.CurrentTime, SiteVars.Water);
            }
            
            if (SubCanopyPAR != null)
            {
                System.Console.WriteLine("Updating output variable: SubCanopyPAR");

                new OutputMapSiteVar<int>(SubCanopyPAR.MapNameTemplate, "", SiteVars.SubCanopyRadiation.ToInt());

                 
            }
            if (NonWoodyDebris != null)
            {
                System.Console.WriteLine("Updating output variable: NonWoodyDebris");

                new OutputMapSiteVar<int>(NonWoodyDebris.MapNameTemplate, "", SiteVars.Litter.ToInt());
              
            }
            if (WoodyDebris != null)
            {
                System.Console.WriteLine("Updating output variable: WoodyDebris");


                new OutputMapSiteVar<int>(WoodyDebris.MapNameTemplate, "", SiteVars.WoodyDebris.ToInt());

                
             
            }
            
            if (AgeDistribution != null)
            {
                System.Console.WriteLine("Updating output variable: AgeDistribution");

                new OutputHistogramCohort(AgeDistribution.MapNameTemplate, "NrOfCohortsAtAge", 10).WriteOutputHist(SiteVars.CohortAges);
                 

                System.Console.WriteLine("Updating output variable: MaxAges");

                new OutputMapSiteVar<int>(AgeDistribution.MapNameTemplate,"", SiteVars.MaxAges);
                 
            }
            if (overalloutputs != null)
            {
                System.Console.WriteLine("Updating output variable: overalloutputs");
                OverallOutputs.WriteNrOfCohortsBalance();
            }

          
        }

        
       
        
    }
}
