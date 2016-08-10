//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.PnET
{
    /// <summary>
    /// The input parameters for the plug-in.
    /// </summary>
    public class InputParameters
      
    {
        public int Timestep;

        public string CohortsPerSpecies;

        public string AnnualPsn;

        public string CohortBalance;

        public IEnumerable<ISpecies> SelectedSpecies;

        public string WoodyDebris;

        public string AgeDistribution;
 
        public string Litter;
         
        public string BelowgroundBiomass;
         
        public string SubCanopyPAR;
         
        public string EstablishmentProbability;

        public string SpeciesEst;

        public string MonthlyNetPsn;


        public string MonthlyFolResp;
        public string MonthlyGrossPsn;
        public string MonthlyMaintResp;

           

        public string LeafAreaIndex;


        public string Water;

        public string SpeciesBiom;
        public string SpeciesAbovegroundBiom;
        public string SpeciesWoodySenescence;
        public string SpeciesFoliageSenescence;
         
       
        
    }
}
