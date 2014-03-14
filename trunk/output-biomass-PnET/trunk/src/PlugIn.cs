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

namespace Landis.Extension.Output.BiomassPnET
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly ExtensionType Type = new ExtensionType("output");
        public static readonly string ExtensionName = "Output PnET";

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
        static OutputVariable Biomass;
        static OutputVariable CohortsPerSpc;
        static OutputVariable NonWoodyDebris;
        static OutputVariable WoodyDebris;
        static OutputVariable DeadCohortNumbers;
        static OutputVariable DeadCohortAges;
        static OutputVariable AgeDistribution;
        static OutputVariable BelowGround;
        static OutputVariable LAI;
        static OutputVariable SpeciesEstablishment;
        static OutputVariable Water;
        static OutputVariable AnnualTranspiration;
        static OutputVariable SubCanopyPAR;
        static OverallOutputs overalloutputs;

       
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
            if (parameters.SpeciesBiom != null) Biomass = new OutputVariable(parameters.SpeciesBiom, "kg/m2");
            if (parameters.BelowgroundBiomass != null) BelowGround = new OutputVariable(parameters.BelowgroundBiomass, "kg/m2");
            if (parameters.LeafAreaIndex != null) LAI = new OutputVariable(parameters.LeafAreaIndex, "m2");
            if (parameters.SpeciesEst != null) SpeciesEstablishment = new OutputVariable(parameters.SpeciesEst, "");
            if (parameters.Water != null) Water = new OutputVariable(parameters.Water, "mm");
            if (parameters.AnnualTranspiration != null) AnnualTranspiration = new OutputVariable(parameters.AnnualTranspiration,  "mm");
            if (parameters.SubCanopyPAR != null) SubCanopyPAR = new OutputVariable(parameters.SubCanopyPAR,  "W/m2");
            if (parameters.Litter != null) NonWoodyDebris = new OutputVariable(parameters.Litter, "kg/m2");
            if (parameters.WoodyDebris != null) WoodyDebris = new OutputVariable(parameters.WoodyDebris,  "kg/m2");
            if (parameters.DeadCohortAges != null) DeadCohortAges = new OutputVariable(parameters.DeadCohortAges, "kg/m2");
            if (parameters.DeadCohortNumbers != null) DeadCohortNumbers = new OutputVariable(parameters.DeadCohortNumbers, "kg/m2");
            if (parameters.AgeDistribution != null) AgeDistribution = new OutputVariable(parameters.AgeDistribution,"yr");
            if (parameters.CohortBalance != null) overalloutputs = new OverallOutputs(parameters.CohortBalance);
            
        }

       
        public override void Run()
        {
         
            if (BelowGround != null)
            {
                // Sum bgb per site 
                ISiteVar<int> bgb = SiteVars.GetBelowGroundBiomass();
                BelowGround.UpdateVariable(bgb);
            }
            if (LAI != null)
            {
                // Total LAI per site 

                LAI.UpdateVariable(SiteVars.FloatToInt(SiteVars.CanopyLAImax));
            }
            if (Water != null)
            {
                Water.UpdateVariable(SiteVars.FloatToInt(SiteVars.SoilWater));
            }
            if (CohortsPerSpc != null)
            {
                // Nr of Cohorts per site and per species
                ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Values = SiteVars.GetNrOfCohorts();
                CohortsPerSpc.UpdateVariable(Values, SiteVars.GetMaxCohorts());

                // Nr of cohorts per species
                Landis.Library.Biomass.Species.AuxParm<int> Values_spc = SiteVars.SpeciesPerSiteToSpecies(Values);
                
                CohortsPerSpc.UpdateVariable(Values_spc);
            }
            if (Biomass != null)
            {
                ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Values = SiteVars.GetBiomass();
                Biomass.UpdateVariable(Values);

                Landis.Library.Biomass.Species.AuxParm<int> Values_spc = SiteVars.SpeciesPerSiteToSpecies(Values);
                Biomass.UpdateVariable(Values_spc);
            }
            
            if (SpeciesEstablishment != null)
            {
                //SpeciesEstablishment.UpdateVariable(SiteVars.Establishments);

                Landis.Library.Biomass.Species.AuxParm<int> Values_spc = SiteVars.SpeciesPerSiteToSpecies(SiteVars.Establishments);
                SpeciesEstablishment.UpdateVariable(Values_spc);
            }
            
            if (AnnualTranspiration != null)
            {
                AnnualTranspiration.UpdateVariable(SiteVars.FloatToInt(SiteVars.AnnualTranspiration));
            }
            if (SubCanopyPAR != null)
            {
                SubCanopyPAR.UpdateVariable(SiteVars.FloatToInt(SiteVars.SubCanopyPARmax));
            }
            if (NonWoodyDebris != null)
            {

                //NonWoodyDebris.UpdateVariable(SiteVars.PoolToInt(SiteVars.WoodyDebris ));
            }
            if (WoodyDebris != null)
            {
                WoodyDebris.UpdateVariable(SiteVars.PoolToInt(SiteVars.WoodyDebris));
            }
            if (DeadCohortAges != null)
            {
                DeadCohortAges.UpdateVariable(SiteVars.DeadCohortAges, 10);
            }
            if (DeadCohortNumbers != null)
            {
                DeadCohortNumbers.UpdateVariable(SiteVars.DeadCohorts, SiteVars.GetMax(SiteVars.DeadCohorts));
            }
            if (AgeDistribution != null)
            {
                ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> Values = SiteVars.GetCohortAges();
                AgeDistribution.UpdateVariable(Values, 10);
            }
            if (overalloutputs != null)
            {
                OverallOutputs.WriteNrOfCohortsBalance();
            }

          
        }

        
       
        
    }
}
