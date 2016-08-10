using Landis.Species;
using Edu.Wisc.Forest.Flel.Util;

using System.Collections.Generic;

namespace Landis.Extension.Output.Biomass
{
    public interface IInputParameters
    {
        int Timestep {get;}
        IEnumerable<ISpecies> SelectedSpecies {get;}
        string SpeciesMapNames {get;}
        bool MakeMaps {get;}
        bool MakeTable {get;}
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
        private bool makeTable;
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

        public string SpeciesMapNames
        {
            get {
                return speciesMapNames;
            }
            set {
                Biomass.SpeciesMapNames.CheckTemplateVars(value);
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

        public bool MakeTable
        {
            get {
                return makeTable;
            }
            set {
                makeTable = value;
            }
        }
        //---------------------------------------------------------------------

        public InputParameters()
        {
        }
        //---------------------------------------------------------------------

        /*public Parameters(int                   timestep,
                          IEnumerable<ISpecies> selectedSpecies,
                          string                speciesMapNames,
                          bool makeMaps,
                          bool makeTable
                          )
        {
            this.timestep = timestep;
            this.selectedSpecies = selectedSpecies;
            this.speciesMapNames = speciesMapNames;
            this.makeMaps = makeMaps;
            this.makeTable = makeTable;
        }*/
    }
}
