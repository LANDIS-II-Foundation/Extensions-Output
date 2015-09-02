//  Copyright 2005-2010 Portland State University, University of Wisconsin-Madison
//  Authors:  Robert M. Scheller, Jimm Domingo

using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.BiomassReclass
{
    /// <summary>
    /// The parameters for the plug-in.
    /// </summary>
    public class InputParameters
        : IInputParameters
    {
        private int timestep;
        private List<IMapDefinition> mapDefns;
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
            set {
                if (value < 0)
                    throw new InputValueException(value.ToString(),"Value must be = or > 0.");
                timestep = value;
            }
        }

        //---------------------------------------------------------------------


        /// <summary>
        /// Reclass maps
        /// </summary>
        public List<IMapDefinition> ReclassMaps
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
            set {
                BiomassReclass.MapFileNames.CheckTemplateVars(value);
                mapFileNames = value;
            }
        }

        //---------------------------------------------------------------------

        public InputParameters(int speciesCount)
        {
            mapDefns = new List<IMapDefinition>();
        }
        //---------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="timestep"></param>
        /// <param name="mapDefns"></param>
        /// <param name="mapFileNames"></param>
/*        public Parameters(int              timestep,
                          IMapDefinition[] mapDefns,
                          string           mapFileNames)
        {
            this.timestep = timestep;
            this.mapDefns = mapDefns;
            this.mapFileNames = mapFileNames;
        }*/
    }
}
