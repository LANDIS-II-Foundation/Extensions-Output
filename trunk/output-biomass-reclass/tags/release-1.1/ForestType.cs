//  Copyright 2005-2010 University of Wisconsin-Madison, Portland State University
//  Authors:  Jimm Domingo, Robert M. Scheller
//  License:  Available at
//  http://www.landis-ii.org/developers/LANDIS-IISourceCodeLicenseAgreement.pdf


namespace Landis.Output.BiomassReclass
{
    /// <summary>
    /// A forest type.
    /// </summary>
    public interface IForestType
    {
        /// <summary>
        /// Name
        /// </summary>
        string Name
        {
            get;set;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Multiplier for a species
        /// </summary>
        int this[int speciesIndex]
        {
            get;set;
        }
    }

    /// <summary>
    /// A forest type.
    /// </summary>
    public class ForestType
        : IForestType
    {
        private string name;
        private int[] multipliers;

        //---------------------------------------------------------------------

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Multiplier for a species
        /// </summary>
        public int this[int speciesIndex]
        {
            get {
                return multipliers[speciesIndex];
            }
            set {
                multipliers[speciesIndex] = value;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Initialize a new instance.
        /// </summary>
        public ForestType(int speciesCount)
        {
            multipliers = new int[speciesCount];
        }
        //---------------------------------------------------------------------

/*        public ForestType(string name,
                          int[]  multipliers)
        {
            this.name = name;
            this.multipliers = multipliers;
        }*/
    }
}
