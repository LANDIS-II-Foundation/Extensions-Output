//  Copyright 2005-2010 Portland State University, University of Wisconsin-Madison
//  Authors:  Robert M. Scheller, James Domingo

using Landis.Core;
using System.Collections.Generic;
using Edu.Wisc.Forest.Flel.Util;


namespace Landis.Extension.Output.MaxSpeciesAge
{
    /// <summary>
    /// The parameters for the plug-in.
    /// </summary>
    public class InputParameters
        : IInputParameters
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
            set {
                if (value < 0)
                        throw new InputValueException(value.ToString(), "Value must be = or > 0.");
                timestep = value;
            }
        }

        //---------------------------------------------------------------------

        public string MapNames
        {
            get {
                return mapNames;
            }
            set {
                MapNameTemplates.CheckTemplateVars(value);
                mapNames = value;
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

        public InputParameters()
        {
        }
    }
}
