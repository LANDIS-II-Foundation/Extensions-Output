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
    public class Parameters
        : IParameters
    {
        private int timestep;
        private string mapNames;
        private IEnumerable<ISpecies> selectedSpecies;

        //---------------------------------------------------------------------

        public int Timestep
        {
            get {
                return timestep;
            }
        }

        //---------------------------------------------------------------------

        public string MapNames
        {
            get {
                return mapNames;
            }
        }

        //---------------------------------------------------------------------

        public IEnumerable<ISpecies> SelectedSpecies
        {
            get {
                return selectedSpecies;
            }
        }

        //---------------------------------------------------------------------

        public Parameters(int                   timestep,
                          string                mapNames,
                          IEnumerable<ISpecies> selectedSpecies)
        {
            this.timestep = timestep;
            this.mapNames = mapNames;
            this.selectedSpecies = selectedSpecies;
        }
    }
}
