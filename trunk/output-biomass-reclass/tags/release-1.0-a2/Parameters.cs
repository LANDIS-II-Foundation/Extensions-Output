//  Copyright 2005-2006 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, Robert M. Scheller
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

namespace Landis.Output.BiomassReclass
{
    /// <summary>
    /// The parameters for the plug-in.
    /// </summary>
    public class Parameters
        : IParameters
    {
        private int timestep;
        private IMapDefinition[] mapDefns;
        private string mapFileNames;

        //---------------------------------------------------------------------

        /// <summary>
        /// Timestep (years)
        /// </summary>
        public int Timestep
        {
            get {
                return timestep;
            }
        }

        //---------------------------------------------------------------------


        /// <summary>
        /// Reclass maps
        /// </summary>
        public IMapDefinition[] ReclassMaps
        {
            get {
                return mapDefns;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Template for the filenames for reclass maps.
        /// </summary>
        public string MapFileNames
        {
            get {
                return mapFileNames;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="timestep"></param>
        /// <param name="mapDefns"></param>
        /// <param name="mapFileNames"></param>
        public Parameters(int              timestep,
                          IMapDefinition[] mapDefns,
                          string           mapFileNames)
        {
            this.timestep = timestep;
            this.mapDefns = mapDefns;
            this.mapFileNames = mapFileNames;
        }
    }
}
