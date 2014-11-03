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
        : IInputParameters
    {
        private int timestep;
        private IEnumerable<ISpecies> selectedSpecies;
        private string speciesBiom;
        private string leafareaindex;
        private string speciesEst;
       // private string annualtranspiration;
        private string woodydebris;
        private string litter;
        private string agedistribution;
        private string deadcohortages;
        private string deadcohortnumbers;
        private string belowgroundbiomass;
        private string subcanopyPAR;
       
        private string cohortbalance;
        private string cohortsperspecies;
        string water;
        
        //---------------------------------------------------------------------

        public int Timestep
        {
            get {
                return timestep;
            }
            set {
                if (value < 0)
                    throw new InputValueException(value.ToString(), "Value must be = or > 0");
                timestep = value;
            }
        }

        public string CohortsPerSpecies
        {
            get {
                return cohortsperspecies;
            }
            set {
                cohortsperspecies = value;
            }
        }
        
        public string CohortBalance
        {
            get {
                return cohortbalance;
            }
            set {
                cohortbalance = value;
            }
        }
        public IEnumerable<ISpecies> SelectedSpecies
        {
            get {
                return selectedSpecies;
            }
            set {
                selectedSpecies = value;
            }
        }
        public string WoodyDebris
        {
            get
            {
                return woodydebris;
            }
            set
            {
                woodydebris = value;
            }
        }
        public string DeadCohortNumbers
        {
            get
            {
                return deadcohortnumbers;
            }
            set
            {
                deadcohortnumbers = value;
            }

        }
        public string DeadCohortAges
        {
            get
            {
                return deadcohortages;
            }
            set
            {
                deadcohortages = value;
            }

        }
         
        public string AgeDistribution
        {
            get
            {
                return agedistribution;
            }
            set
            {
                agedistribution = value;
            }
        }
       
        public string Litter
        {
            get
            {
                return litter;
            }
            set
            {
                litter = value;
            }
        }
        public string BelowgroundBiomass
        {
            get
            {
                return belowgroundbiomass;
            }
            set
            {
                belowgroundbiomass = value;
            }
        }

        //public string AnnualTranspiration
        //{
        //    get {
        //        return annualtranspiration;
        //    }
         //   set {
        //        annualtranspiration = value;
        //    }
        //}
        public string SubCanopyPAR
        {
            get {
                return subcanopyPAR;
            }
            set {
                subcanopyPAR = value;
            }
        }


        public string SpeciesEst
        {
            get {
                return speciesEst;
            }
            set {
                speciesEst = value;
            }
        }
        public string LeafAreaIndex
        {
            get {
                return leafareaindex;
            }
            set {
                //Biomass.SpeciesMapNames.CheckTemplateVars(value);
                leafareaindex = value;
            }
        }
        
        public string Water
        {
            get
            {
                return water;
            }
            set 
            {
                water = value;
            }
        }

        public string SpeciesBiom
        {
            get {
                return speciesBiom;
            }
            set {
                OutputPath.CheckTemplateVars(value, FileNames.knownVars);
                 
                speciesBiom = value;
            }
        }

       
        //---------------------------------------------------------------------

       

        public InputParameters()
        {
        }
        //---------------------------------------------------------------------

      
    }
}
