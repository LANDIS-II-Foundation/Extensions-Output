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

        private IInputParameters parameters;
        private static ICore modelCore;
        private OverallOutputs overalloutputs;
        private BiomassPerEcoregion biomassperecoregion;
       
        private SpeciesFrequency Establishments;
        private SpeciesFrequency DeadCohorts;

        private CohortFreq deadcohortfreq;
        private CohortFreq agecohortfreq;
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

            OutputVariables.Initialize(parameters);

            SiteVars.Initialize();

            if (parameters.CohortBalanceFileName != null) overalloutputs = new OverallOutputs(parameters.CohortBalanceFileName);
            if (parameters.BiomassPerEcoregionFileName != null) biomassperecoregion = new BiomassPerEcoregion(parameters.BiomassPerEcoregionFileName);
            
            if (parameters.SpeciesSpecEstFileName != null) Establishments = new SpeciesFrequency(parameters.SpeciesSpecEstFileName);
            if (parameters.CohortDeathFreqFileName != null) DeadCohorts = new SpeciesFrequency(parameters.CohortDeathFreqFileName);

            if (parameters.DeathAgeDistributionFileNames!=null) deadcohortfreq = new CohortFreq(parameters.Timestep, parameters.DeathAgeDistributionFileNames);
            if (parameters.AgeDistributionFileNames != null) agecohortfreq = new CohortFreq(parameters.Timestep, parameters.AgeDistributionFileNames);
        }

        //---------------------------------------------------------------------
        public ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> GetCohortAges()
        { 
            ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>>  ages = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>>();

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                ages[site] = new Library.Biomass.Species.AuxParm<List<int>>(PlugIn.ModelCore.Species);
                foreach (ISpecies species in PlugIn.ModelCore.Species)
                {
                    ages[site][species] = new List<int>();
                }
            }

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                if (SiteVars.Cohorts[site] == null) continue;
                foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
                {
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        ages[site][cohort.Species].Add(cohort.Age);
                    }
                }
            }
            return ages;
        }
        public override void Run()
        {
            OutputVariables.Update();

            if (biomassperecoregion != null) biomassperecoregion.Write();
            if (overalloutputs  != null) OverallOutputs.WriteNrOfCohortsBalance();
            if (Establishments!=null) Establishments.WriteUpdate(modelCore.CurrentTime, SiteVars.Establishments);
            if (DeadCohorts != null) DeadCohorts.WriteUpdate(modelCore.CurrentTime, SiteVars.DeadCohorts);
            if (deadcohortfreq != null) deadcohortfreq.Write(SiteVars.DeadCohortAges);
            if (agecohortfreq != null) agecohortfreq.Write(GetCohortAges());
        }

        
        
    }
}
