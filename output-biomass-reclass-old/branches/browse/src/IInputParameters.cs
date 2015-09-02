//  Copyright 2005-2010 Portland State University, University of Wisconsin-Madison
//  Authors:  Robert M. Scheller, Jimm Domingo

using System.Collections.Generic;

namespace Landis.Extension.Output.BiomassReclass
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
