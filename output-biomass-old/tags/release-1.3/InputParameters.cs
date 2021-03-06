using Edu.Wisc.Forest.Flel.Util;
using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.Biomass
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
        //private SelectedDeadPools selectedPools;
        private string selectedPools;
        private string poolMapNames;

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

        //private void VerifyPoolsAndMapNames(InputValue<SelectedDeadPools> pools,
        //                                    InputValue<string>            mapNames)
        //private void VerifyPoolsAndMapNames(SelectedDeadPools pools,
        //                                    string            mapNames)
        //{
            //if (pools == null || mapNames == null)
            //    return;
        //    Biomass.PoolMapNames.CheckTemplateVars(mapNames, pools);
        //}
    }
}
