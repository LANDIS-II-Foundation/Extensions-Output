//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using System.Collections.Generic;
using Edu.Wisc.Forest.Flel.Util;


namespace Landis.Extension.Output.BiomassByAge
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public class InputParameters
		: IInputParameters
	{
		private int timestep;
		private IEnumerable<ISpecies> selectedSpecies;
		private string speciesMapNames;
        private Dictionary<string, List<AgeClass>> ageClasses;

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

        public Dictionary<string, List<AgeClass>> AgeClasses
        {
            get
            {
                return ageClasses;
            }
            set
            {
                ageClasses = value;
            }
        }

		//---------------------------------------------------------------------

		public string SpeciesMapNames
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

        public InputParameters()
        {
        }
	}
}
