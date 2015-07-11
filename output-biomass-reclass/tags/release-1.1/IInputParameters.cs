//  Copyright 2005-2010 University of Wisconsin-Madison, Portland State University
//  Authors:  Jimm Domingo, Robert M. Scheller
//  License:  Available at
//  http://www.landis-ii.org/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using System.Collections.Generic;

namespace Landis.Output.BiomassReclass
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
        /// Reclass maps
        /// </summary>
        List<IMapDefinition> ReclassMaps
        {
            get;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Template for the filenames for reclass maps.
        /// </summary>
        string MapFileNames
        {
            get;set;
        }
    }
}
