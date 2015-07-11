//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, FLEL
//  License:  Available at
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Edu.Wisc.Forest.Flel.Util;
using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.MaxSpeciesAge
{
    /// <summary>
    /// An editable set of parameters for the plug-in.
    /// </summary>
    public class EditableParameters
        : IEditable<IParameters>
    {
        private InputValue<int> timestep;
        private InputValue<string> mapNamesTemplate;
        private IEnumerable<ISpecies> selectedSpecies;

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
                                                      "Value must be = or > 0.");
                timestep = value;
            }
        }

        //---------------------------------------------------------------------

        public InputValue<string> MapNamesTemplate
        {
            get {
                return mapNamesTemplate;
            }

            set {
                if (value != null) {
                    MapNames.CheckTemplateVars(value.Actual);
                }
                mapNamesTemplate = value;
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

        public EditableParameters()
        {
        }

        //---------------------------------------------------------------------

        public bool IsComplete
        {
            get {
                foreach (object parameter in new object[]{ timestep,
                                                           mapNamesTemplate,
                                                           selectedSpecies }) {
                    if (parameter == null)
                        return false;
                }
                return true;
            }
        }

        //---------------------------------------------------------------------

        public IParameters GetComplete()
        {
            if (IsComplete)
                return new Parameters(timestep.Actual,
                                      mapNamesTemplate.Actual,
                                      selectedSpecies);
            else
                return null;
        }
    }
}
