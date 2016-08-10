//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.LeafBiomassCohorts;
using Landis.SpatialModeling;

using System.Collections.Generic;

namespace Landis.Extension.Output.LeafBiomass
{
    public interface IInputParameters
    {
        int Timestep {get;}
        IEnumerable<ISpecies> SelectedSpecies {get;}
        string SpeciesMaps {get;}
        bool MakeMaps {get;}
        //bool MakeTable {get;}
    }
    /// <summary>
    /// The parameters for the plug-in.
    /// </summary>
    public class InputParameters
        : IInputParameters
    {
        private int timestep;
        private IEnumerable<ISpecies> selectedSpecies;
        private string speciesMapNames;
        private bool makeMaps;
        //private bool makeTable;
        //---------------------------------------------------------------------

        public int Timestep
        {
            get {
                return timestep;
            }
            set {
                if (value < 0)
                    throw new InputValueException(value.ToString(), "Value must be = or > 0");
                timestep = value;
            }
        }

        //---------------------------------------------------------------------

        public IEnumerable<ISpecies> SelectedSpecies
        {
            get {
                return selectedSpecies;
            }
            set {
                selectedSpecies = value;
            }
        }

        //---------------------------------------------------------------------

        public string SpeciesMaps
        {
            get {
                return speciesMapNames;
            }
            set {
                MapNames.CheckTemplateVars(value);
                speciesMapNames = value;
            }
        }

        //---------------------------------------------------------------------

        public bool MakeMaps
        {
            get {
                return makeMaps;
            }
            set {
                makeMaps = value;
            }
        }
        //---------------------------------------------------------------------

        //public bool MakeTable
        //{
        //    get {
        //        return makeTable;
        //    }
        //    set {
        //        makeTable = value;
        //    }
        //}
        //---------------------------------------------------------------------

        public InputParameters()
        {
        }
    }
}
