//  Copyright 2005-2006 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, Robert M. Scheller
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

namespace Landis.Output.BiomassReclass
{
    /// <summary>
    /// The definition of a reclass map.
    /// </summary>
    public class MapDefinition
        : IMapDefinition
    {
        private string name;
        private IForestType[] forestTypes;

        //---------------------------------------------------------------------

        /// <summary>
        /// Map name
        /// </summary>
        public string Name
        {
            get {
                return name;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Forest types
        /// </summary>
        public IForestType[] ForestTypes
        {
            get {
                return forestTypes;
            }
        }

        //---------------------------------------------------------------------

        public MapDefinition(string        name,
                             IForestType[] forestTypes)
        {
            this.name = name;
            this.forestTypes = forestTypes;
        }
    }
}
