//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.BiomassPnET
{
    /// <summary>
    /// The input parameters for the plug-in.
    /// </summary>
    public class InputParameters
        : IInputParameters
    {
        private int timestep;
        private IEnumerable<ISpecies> selectedSpecies;
        private string speciesBiomMapNames;
        private string speciesLAIMapNames;
        private string speciesEestMapNames;
        private string annualtranspirationmapNames;
        private string belowgroundmapnames;
        private string subcanopyPARmapnames;
        private string selectedPools;
        private string poolMapNames;
        private bool makeTable;

        
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

        //---------------------------------------------------------------------

        public string BelowgroundMapNames
        {
            get {
                return belowgroundmapnames ;
            }
            set {
                belowgroundmapnames = value;
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

        //---------------------------------------------------------------------

        public string AnnualTranspirationMapNames
        {
            get {
                return annualtranspirationmapNames;
            }
            set {
                annualtranspirationmapNames = value;
            }
        }
        public string SubCanopyPARMapNames
        {
            get {
                return subcanopyPARmapnames;
            }
            set {
                subcanopyPARmapnames = value;
            }
        }


        public string SpeciesEstMapNames
        {
            get {
                return speciesEestMapNames;
            }
            set {
                speciesEestMapNames = value;
            }
        }
        public string SpeciesLAIMapNames
        {
            get {
                return speciesLAIMapNames;
            }
            set {
                //Biomass.SpeciesMapNames.CheckTemplateVars(value);
                speciesLAIMapNames = value;
            }
        }
        string waterMapNameTemplate;
        public string WaterMapNameTemplate
        {
            get
            {
                return waterMapNameTemplate;
            }
            set 
            {
                waterMapNameTemplate = value;
            }
        }

        public string SpeciesBiomMapNames
        {
            get {
                return speciesBiomMapNames;
            }
            set {
                BiomassPnET.SpeciesMapNames.CheckTemplateVars(value);
                speciesBiomMapNames = value;
            }
        }

        //---------------------------------------------------------------------

        public string SelectedPools
        {
            get {
                return selectedPools;
            }
            set {
            	if(value != "woody" && value != "non-woody" && value != "both")
                	throw new InputValueException(selectedPools, "The dead pools {0} must be either 'woody' or 'non-woody' or 'both'");
                selectedPools = value;
            }
        }

        //---------------------------------------------------------------------

        public string PoolMapNames
        {
            get {
                return poolMapNames;
            }
            set {
                BiomassPnET.PoolMapNames.CheckTemplateVars(value); //, selectedPools);
                poolMapNames = value;
            }
        }

        public InputParameters()
        {
        }
        //---------------------------------------------------------------------

        public bool MakeTable
        {
            get
            {
                return makeTable;
            }
            set
            {
                makeTable = value;
            }
        }
    }
}
