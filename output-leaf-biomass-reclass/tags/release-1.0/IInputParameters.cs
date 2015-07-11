//  Copyright 2013 Portland State University
//  Authors:  Robert M. Scheller

using System.Collections.Generic;

namespace Landis.Extension.Output.LeafBiomassReclass
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
