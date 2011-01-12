//  Copyright 2005-2006 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, Robert M. Scheller
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Output.BiomassReclass
{
    /// <summary>
    /// Editable set of parameters for the plug-in.
    /// </summary>
    public class EditableParameters
        : IEditable<IParameters>
    {
        private InputValue<int> timestep;
        private ListOfEditable<IMapDefinition> mapDefns;
        private InputValue<string> mapFileNames;

        //---------------------------------------------------------------------

        /// <summary>
        /// Timestep (years)
        /// </summary>
        public InputValue<int> Timestep
        {
            get {
                return timestep;
            }

            set {
                if (value != null) {
                    if (value.Actual < 0)
                        throw new InputValueException(value.String,
                                                      "Value must be = or > 0.");
                }
                timestep = value;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Reclass maps
        /// </summary>
        public ListOfEditable<IMapDefinition> ReclassMaps
        {
            get {
                return mapDefns;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Template for the filenames for reclass maps.
        /// </summary>
        public InputValue<string> MapFileNames
        {
            get {
                return mapFileNames;
            }

            set {
                if (value != null) {
                    BiomassReclass.MapFileNames.CheckTemplateVars(value.Actual);
                }
                mapFileNames = value;
            }
        }

        //---------------------------------------------------------------------

        public EditableParameters(int speciesCount)
        {
            mapDefns = new ListOfEditable<IMapDefinition>();
        }

        //---------------------------------------------------------------------

        public bool IsComplete
        {
            get {
                foreach (object parameter in new object[]{ timestep,
                                                           mapFileNames }) {
                    if (parameter == null)
                        return false;
                }
                return mapDefns.IsEachItemComplete;
            }
        }

        //---------------------------------------------------------------------

        public IParameters GetComplete()
        {
            if (IsComplete)
                return new Parameters(timestep.Actual,
                                      mapDefns.GetComplete(),
                                      mapFileNames.Actual);
            else
                return null;
        }
    }
}
