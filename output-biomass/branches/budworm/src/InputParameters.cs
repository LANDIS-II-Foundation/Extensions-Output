//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.Biomass
{
    /// <summary>
    /// The input parameters for the plug-in.
    /// </summary>
    public class InputParameters
        : IInputParameters
    {
        private int timestep;
        private IEnumerable<ISpecies> selectedSpecies;
        private string speciesMapNames;
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

        public string SpeciesMapNames
        {
            get {
                return speciesMapNames;
            }
            set {
                Biomass.SpeciesMapNames.CheckTemplateVars(value);
                speciesMapNames = value;
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
                Biomass.PoolMapNames.CheckTemplateVars(value); //, selectedPools);
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
