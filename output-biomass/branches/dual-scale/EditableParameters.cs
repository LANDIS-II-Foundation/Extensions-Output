using Edu.Wisc.Forest.Flel.Util;
using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.Biomass
{
    /// <summary>
    /// An editable set of parameters for the plug-in.
    /// </summary>
    public class EditableParameters
        : IEditable<IParameters>
    {
        private InputValue<int> timestep;
        private IEnumerable<ISpecies> selectedSpecies;
        private InputValue<string> speciesMapNames;
        private InputValue<SelectedDeadPools> selectedPools;
        private InputValue<string> poolMapNames;

        //---------------------------------------------------------------------

        public InputValue<int> Timestep
        {
            get {
                return timestep;
            }

            set {
                if (value != null)
                    if (value.Actual < 0)
                        throw new InputValueException(value.String,
                                                      "Value must be = or > 0");
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

        public InputValue<string> SpeciesMapNames
        {
            get {
                return speciesMapNames;
            }

            set {
                if (value != null) {
                    Biomass.SpeciesMapNames.CheckTemplateVars(value.Actual);
                }
                speciesMapNames = value;
            }
        }

        //---------------------------------------------------------------------

        public InputValue<SelectedDeadPools> SelectedPools
        {
            get {
                return selectedPools;
            }

            set {
                VerifyPoolsAndMapNames(value, poolMapNames);
                selectedPools = value;
            }
        }

        //---------------------------------------------------------------------

        public InputValue<string> PoolMapNames
        {
            get {
                return poolMapNames;
            }

            set {
                VerifyPoolsAndMapNames(selectedPools, value);
                poolMapNames = value;
            }
        }

        //---------------------------------------------------------------------

        private void VerifyPoolsAndMapNames(InputValue<SelectedDeadPools> pools,
                                            InputValue<string>            mapNames)
        {
            if (pools == null || mapNames == null)
                return;
            Biomass.PoolMapNames.CheckTemplateVars(mapNames.Actual, pools.Actual);
        }

        //---------------------------------------------------------------------

        public EditableParameters()
        {
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Indicates whether the set of parameters is complete.  One or both
        /// groups of species and pool parameters must be complete.
        /// </summary>
        public bool IsComplete
        {
            get {
                if (timestep == null)
                    return false;
                bool speciesParmsComplete = (selectedSpecies != null) &&
                                            (speciesMapNames != null);
                bool poolParmsComplete = (selectedPools != null) &&
                                         (poolMapNames != null);
                if (speciesParmsComplete || poolParmsComplete)
                    return true;
                return false;
            }
        }

        //---------------------------------------------------------------------

        public IParameters GetComplete()
        {
            if (IsComplete) {
                if (selectedSpecies == null)
                    return new Parameters(timestep.Actual,
                                          selectedSpecies,
                                          null, //speciesMapNames
                                          selectedPools.Actual,
                                          poolMapNames.Actual);
                else if (selectedPools == null)
                    return new Parameters(timestep.Actual,
                                          selectedSpecies,
                                          speciesMapNames.Actual,
                                          SelectedDeadPools.None,
                                          null); // poolMapNames
                else
                    return new Parameters(timestep.Actual,
                                          selectedSpecies,
                                          speciesMapNames.Actual,
                                          selectedPools.Actual,
                                          poolMapNames.Actual);
            }
            else
                return null;
        }
    }
}
