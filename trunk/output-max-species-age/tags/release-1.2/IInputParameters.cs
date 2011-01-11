//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, FLEL
//  License:  Available at
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.MaxSpeciesAge
{
    /// <summary>
    /// The parameters for the plug-in.
    /// </summary>
    public interface IInputParameters
    {
        /// <summary>
        /// Timestep (years)
        /// </summary>
        int Timestep
        {
            get;set;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Template for the filenames for output maps.
        /// </summary>
        string MapNames
        {
            get;set;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Collection of species for which output maps are generated.
        /// </summary>
        IEnumerable<ISpecies> SelectedSpecies
        {
            get;set;
        }
    }
}
