//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Arjan de Bruijn 

using Landis.Core;

using System;
using System.Collections.Generic;
using Landis.Core;
using Landis.SpatialModeling;
using System;

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
        IInputParameters parameters;
        static ICore modelCore;

        static IEnumerable<ISpecies> selectedspecies;
        static  OutputVariable Biomass;
        static  OutputVariable CohortsPerSpc;
        static  OutputVariable NonWoodyDebris;
        static  OutputVariable WoodyDebris;
        static  OutputVariable DeadCohortNumbers;
        static  OutputVariable DeadCohortAges;
        static  OutputVariable AgeDistribution;
        static  OutputVariable BelowGround;
        static  OutputVariable LAI;
        static  OutputVariable SpeciesEstablishment;
        static  OutputVariable Water;
        //static  OutputVariable AnnualTranspiration;
        static  OutputVariable SubCanopyPAR;
        static  OverallOutputs overalloutputs;

       
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
            parameters = Landis.Data.Load<IInputParameters>(dataFile, parser);
        }

        //---------------------------------------------------------------------

        public override void Initialize()
        {
            Timestep = parameters.Timestep;

            tstep = parameters.Timestep;
            
            SiteVars.Initialize();
            
            selectedspecies = parameters.SelectedSpecies;

            if (parameters.CohortsPerSpecies != null) CohortsPerSpc = new OutputVariable(parameters.CohortsPerSpecies, "#");
            if (parameters.SpeciesBiom != null) Biomass = new OutputVariable(parameters.SpeciesBiom, "g/m2");
            if (parameters.BelowgroundBiomass != null) BelowGround = new OutputVariable(parameters.BelowgroundBiomass, "g/m2");
            if (parameters.LeafAreaIndex != null) LAI = new OutputVariable(parameters.LeafAreaIndex, "m2");
            if (parameters.SpeciesEst != null) SpeciesEstablishment = new OutputVariable(parameters.SpeciesEst, "");
            if (parameters.Water != null) Water = new OutputVariable(parameters.Water, "mm");
            //if (parameters.AnnualTranspiration != null) AnnualTranspiration = new OutputVariable(parameters.AnnualTranspiration,  "mm");
            if (parameters.SubCanopyPAR != null) SubCanopyPAR = new OutputVariable(parameters.SubCanopyPAR,  "W/m2 pr mmol/m2");
            if (parameters.Litter != null) NonWoodyDebris = new OutputVariable(parameters.Litter, "g/m2");
            if (parameters.WoodyDebris != null) WoodyDebris = new OutputVariable(parameters.WoodyDebris,  "g/m2");
            if (parameters.DeadCohortAges != null) DeadCohortAges = new OutputVariable(parameters.DeadCohortAges, "");
            if (parameters.DeadCohortNumbers != null) DeadCohortNumbers = new OutputVariable(parameters.DeadCohortNumbers, "");
            if (parameters.AgeDistribution != null) AgeDistribution = new OutputVariable(parameters.AgeDistribution,"yr");
            if (parameters.CohortBalance != null) overalloutputs = new OverallOutputs(parameters.CohortBalance);
            
        }
        
        public static ISiteVar<int> SpeciesSum(ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>>  SpeciesSpecific)
        {
            ISiteVar<int> TotalBiomass = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
            foreach (ActiveSite site in PlugIn.modelCore.Landscape)
            {
                foreach (ISpecies species in PlugIn.modelCore.Species)
                {
                    TotalBiomass[site] += SpeciesSpecific[site][species];
                }
            }
            return TotalBiomass;
        }

        public override void Run()
        {
            if (BelowGround != null)
            {
                System.Console.WriteLine("Updating output variable: BelowGround");
                new OutputMapSiteVar(BelowGround.MapNameTemplate, "", SiteVars.BelowGroundBiomass);
                
            }
            if (LAI != null)
            {
                System.Console.WriteLine("Updating output variable: LAI");
                // Total LAI per site 

                new OutputMapSiteVar(LAI.MapNameTemplate,"", SiteVars.CanopyLAImax);

                // Values per species each time step
                new OutputTableEcoregions(LAI.MapNameTemplate).WriteUpdate(PlugIn.ModelCore.CurrentTime, SiteVars.AverageLAIperEcoRegion);

                
            }
            
            if (CohortsPerSpc != null)
            {
                System.Console.WriteLine("Updating output variable: CohortsPerSpc");
                // Nr of Cohorts per site and per species
                
                new OutputHistogramCohort(CohortsPerSpc.MapNameTemplate, "CohortsPerSpcPerSite", 10).WriteOutputHist(SiteVars.Cohorts);      

                new OutputMapSiteVar(CohortsPerSpc.MapNameTemplate, "",SiteVars.CohortsPerSite);
                 
                // Nr of cohorts per species
                OutputFilePerTStepPerSpecies.Write<int>(CohortsPerSpc.MapNameTemplate, CohortsPerSpc.units, PlugIn.ModelCore.CurrentTime, SiteVars.Cohorts_spc, (int)Math.Round(SiteVars.Cohorts_sum, 0), (int)Math.Round(SiteVars.Cohorts_avg, 0));


                 
            }
             
            if (Biomass != null)
            {
                System.Console.WriteLine("Updating output variable: Biomass");

                // write maps biomass per species per pixel
                // Variable per species and per site (multiple maps)
                foreach (ISpecies spc in PlugIn.SelectedSpecies)
                {
                    new OutputMapSpecies(SiteVars.Biomass, spc, Biomass.MapNameTemplate);
                }

                
                new OutputMapSiteVar(Biomass.MapNameTemplate,"Total", SpeciesSum(SiteVars.Biomass));

                // overview table 
                OutputFilePerTStepPerSpecies.Write<float>(Biomass.MapNameTemplate, Biomass.units, PlugIn.ModelCore.CurrentTime, SiteVars.Biomass_spc, (int)Math.Round(SiteVars.Biomass_sum, 0), (int)Math.Round(SiteVars.Biomass_av, 0));

            }
            if (Water != null)
            {
                System.Console.WriteLine("Updating output variable: Water");
                
                new OutputMapSiteVar(Water.MapNameTemplate,"", SiteVars.Water);

                new OutputTableEcoregions(Water.MapNameTemplate).WriteUpdate(PlugIn.ModelCore.CurrentTime, SiteVars.AverageWaterPerEcoregion);

                 
            }
            if (SpeciesEstablishment != null)
            {
                System.Console.WriteLine("Updating output variable: SpeciesEstablishment");
                foreach (ISpecies spc in PlugIn.SelectedSpecies)
                {
                    new OutputMapSpecies(SiteVars.newcohortcount, spc, SpeciesEstablishment.MapNameTemplate);
                }

                
               // OutputFilePerTStepPerSpecies.Write<int>(SpeciesEstablishment.MapNameTemplate, SpeciesEstablishment.units, SiteVars.Establishments_spc, SiteVars.Establishments_sum, SiteVars.Establishments_avg);

            }
            
            if (SubCanopyPAR != null)
            {
                System.Console.WriteLine("Updating output variable: SubCanopyPAR");

                new OutputMapSiteVar(SubCanopyPAR.MapNameTemplate, "",SiteVars.ToInt<float>(SiteVars.SubCanopyRadiation));

                 
            }
            if (NonWoodyDebris != null)
            {
                System.Console.WriteLine("Updating output variable: NonWoodyDebris");

                

                new OutputMapSiteVar(NonWoodyDebris.MapNameTemplate, "",SiteVars.ToInt(SiteVars.Litter));
              
            }
            if (WoodyDebris != null)
            {
                System.Console.WriteLine("Updating output variable: WoodyDebris");

                 
                new OutputMapSiteVar(WoodyDebris.MapNameTemplate, "",SiteVars.ToInt(SiteVars.WoodyDebris));

                
             
            }
            if (DeadCohortAges != null)
            {
                System.Console.WriteLine("Updating output variable: DeadCohortAges");

                new OutputHistogramCohort(DeadCohortAges.MapNameTemplate, "NrOfCohortsThatDiedAtAge", 10).WriteOutputHist(SiteVars.DeadCohortAges);
               
            }
            if (DeadCohortNumbers != null)
            {
                System.Console.WriteLine("Updating output variable: DeadCohortNumbers");

                OutputTableSpecies.WriteUpdate(DeadCohortNumbers.MapNameTemplate, PlugIn.ModelCore.CurrentTime, SiteVars.Deadcohorts_spc);
 
                 
            }
            if (AgeDistribution != null)
            {
                System.Console.WriteLine("Updating output variable: AgeDistribution");

                new OutputHistogramCohort(AgeDistribution.MapNameTemplate, "NrOfCohortsAtAge", 10).WriteOutputHist(SiteVars.CohortAges);
                 

                System.Console.WriteLine("Updating output variable: MaxAges");

                new OutputMapSiteVar(AgeDistribution.MapNameTemplate,"", SiteVars.MaxAges);
                 
            }
            if (overalloutputs != null)
            {
                System.Console.WriteLine("Updating output variable: overalloutputs");
                OverallOutputs.WriteNrOfCohortsBalance();
            }

          
        }

        
       
        
    }
}
