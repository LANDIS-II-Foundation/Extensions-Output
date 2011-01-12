//  Copyright 2005-2006 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, Robert M. Scheller
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Output.BiomassReclass
{
    /// <summary>
    /// Editable forest type.
    /// </summary>
    public interface IEditableForestType
        : IEditable<IForestType>
    {
        /// <summary>
        /// Map name
        /// </summary>
        InputValue<string> Name
        {
            get;
            set;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Multiplier for a species
        /// </summary>
        int this[int speciesIndex]
        {
            get;
            set;
        }
    }
}
