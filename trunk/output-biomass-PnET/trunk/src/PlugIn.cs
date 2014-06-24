//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohortsPnET;
using Landis.SpatialModeling;
using Landis.Extension.Succession.BiomassPnET;

using System;
using System.Collections.Generic;
using System.IO;

namespace Landis.Extension.Output.PnET
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly ExtensionType Type = new ExtensionType("output");
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
        static  OutputVariable AnnualTranspiration;
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
            if (parameters.AnnualTranspiration != null) AnnualTranspiration = new OutputVariable(parameters.AnnualTranspiration,  "mm");
            if (parameters.SubCanopyPAR != null) SubCanopyPAR = new OutputVariable(parameters.SubCanopyPAR,  "W/m2 pr mmol/m2");
            if (parameters.Litter != null) NonWoodyDebris = new OutputVariable(parameters.Litter, "g/m2");
            if (parameters.WoodyDebris != null) WoodyDebris = new OutputVariable(parameters.WoodyDebris,  "g/m2");
            if (parameters.DeadCohortAges != null) DeadCohortAges = new OutputVariable(parameters.DeadCohortAges, "");
            if (parameters.DeadCohortNumbers != null) DeadCohortNumbers = new OutputVariable(parameters.DeadCohortNumbers, "");
            if (parameters.AgeDistribution != null) AgeDistribution = new OutputVariable(parameters.AgeDistribution,"yr");
            if (parameters.CohortBalance != null) overalloutputs = new OverallOutputs(parameters.CohortBalance);
            
        }

       
        public override void Run()
        {
            if (BelowGround != null)
            {
                System.Console.WriteLine("Updating output variable: BelowGround");
                BelowGround.UpVarDateiable(SiteVars.BelowGroundBiomass);
            }
            if (LAI != null)
            {
                System.Console.WriteLine("Updating output variable: LAI");
                // Total LAI per site 
                LAI.UpVarDateiable(SiteVars.Lai);

                LAI.UpVarDateiable(SiteVars.AverageLAIperEcoRegion);
            }
            
            if (CohortsPerSpc != null)
            {
                System.Console.WriteLine("Updating output variable: CohortsPerSpc");
                // Nr of Cohorts per site and per species
                CohortsPerSpc.UpVarDateiable(SiteVars.Cohorts, "CohortsPerSpcPerSite", 10);

                // Nr of cohorts per species
                CohortsPerSpc.UpVarDateiable(SiteVars.Cohorts_spc, (int)Math.Round(SiteVars.Cohorts_sum, 0), (int)Math.Round(SiteVars.Cohorts_avg , 0));
            }
            if (Biomass != null)
            {
                System.Console.WriteLine("Updating output variable: Biomass");

                // write maps biomass per species per pixel
                Biomass.UpVarDateiable(SiteVars.Biomass);

                // overview table 
                // Biomass_spc
                Biomass.UpVarDateiable(SiteVars.Biomass_spc, SiteVars.Biomass_sum, (float)SiteVars.Biomass_av);
            }
            if (Water != null)
            {
                System.Console.WriteLine("Updating output variable: Water");
                Water.UpVarDateiable(SiteVars.Water);

                Water.UpVarDateiable(SiteVars.AverageWaterPerEcoregion);
            }
            if (SpeciesEstablishment != null)
            {
                System.Console.WriteLine("Updating output variable: SpeciesEstablishment");
                SpeciesEstablishment.UpVarDateiable(SiteVars.Establishments);

                SpeciesEstablishment.UpVarDateiable(SiteVars.Establishments_spc, SiteVars.Establishments_sum, SiteVars.Establishments_avg);

            
            }
            if (AnnualTranspiration != null)
            {
                System.Console.WriteLine("Updating output variable: AnnualTranspiration");
                 AnnualTranspiration.UpVarDateiable(SiteVars.AnnualTranspiration);
            }
            if (SubCanopyPAR != null)
            {
                System.Console.WriteLine("Updating output variable: SubCanopyPAR");
                SubCanopyPAR.UpVarDateiable(SiteVars.SubCanopyPARmax);
            }
            if (NonWoodyDebris != null)
            {
                System.Console.WriteLine("Updating output variable: NonWoodyDebris");
                NonWoodyDebris.UpVarDateiable(SiteVars.Litter);
            }
            if (WoodyDebris != null)
            {
                System.Console.WriteLine("Updating output variable: WoodyDebris");
                WoodyDebris.UpVarDateiable(SiteVars.WoodyDebris);
            }
            if (DeadCohortAges != null)
            {
                System.Console.WriteLine("Updating output variable: DeadCohortAges");
                DeadCohortAges.UpVarDateiable(SiteVars.DeadCohortAges, "NrOfCohortsThatDiedAtAge"   , 10);
            }
            if (DeadCohortNumbers != null)
            {
                System.Console.WriteLine("Updating output variable: DeadCohortNumbers");
                DeadCohortNumbers.UpVarDateiable(SiteVars.Deadcohorts_spc);
            }
            if (AgeDistribution != null)
            {
                System.Console.WriteLine("Updating output variable: AgeDistribution");
                AgeDistribution.UpVarDateiable(SiteVars.CohortAges, "NrOfCohortsAtAge", 10);

                System.Console.WriteLine("Updating output variable: MaxAges");
                AgeDistribution.UpVarDateiable(SiteVars.MaxAges);
            }
            if (overalloutputs != null)
            {
                System.Console.WriteLine("Updating output variable: overalloutputs");
                OverallOutputs.WriteNrOfCohortsBalance();
            }

          
        }

        
       
        
    }
}
