//  Copyright 2005-2010 Portland State University, University of Wisconsin-Madison
//  Authors:  Robert M. Scheller, James Domingo

using Landis.Core;
using System.Collections.Generic;

namespace Landis.Extension.Output.MaxSpeciesAge
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
