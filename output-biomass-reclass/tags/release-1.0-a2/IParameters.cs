//  Copyright 2005-2006 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, Robert M. Scheller
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

namespace Landis.Output.BiomassReclass
{
    /// <summary>
    /// The parameters for the plug-in.
    /// </summary>
    public interface IParameters
    {
        /// <summary>
        /// Timestep (years)
        /// </summary>
        int Timestep
        {
            get;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Reclass maps
        /// </summary>
        IMapDefinition[] ReclassMaps
        {
            get;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Template for the filenames for reclass maps.
        /// </summary>
        string MapFileNames
        {
            get;
        }
    }
}
