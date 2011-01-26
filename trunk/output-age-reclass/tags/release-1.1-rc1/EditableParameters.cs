//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Output.Reclass
{
	/// <summary>
	/// Editable set of parameters for the plug-in.
	/// </summary>
	public class EditableParameters
		: IEditable<IParameters>
	{
		private InputValue<int> timestep;
		private EditableCoefficients coefficients;
		private ListOfEditable<IMapDefinition> mapDefns;
		private InputValue<string> mapFileNames;

		//---------------------------------------------------------------------

		/// <summary>
		/// Timestep (years)
		/// </summary>
		public InputValue<int> Timestep
		{
			get {
				return timestep;
			}

			set {
				if (value != null) {
					if (value.Actual < 0)
						throw new InputValueException(value.String,
					                                  "Value must be = or > 0.");
				}
				timestep = value;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Reclass coefficients for species
		/// </summary>
		public EditableCoefficients ReclassCoefficients
		{
			get {
				return coefficients;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Reclass maps
		/// </summary>
		public ListOfEditable<IMapDefinition> ReclassMaps
		{
			get {
				return mapDefns;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Template for the filenames for reclass maps.
		/// </summary>
		public InputValue<string> MapFileNames
		{
			get {
				return mapFileNames;
			}

			set {
				if (value != null) {
					Reclass.MapFileNames.CheckTemplateVars(value.Actual);
				}
				mapFileNames = value;
			}
		}

		//---------------------------------------------------------------------

		public EditableParameters(int speciesCount)
		{
			coefficients = new EditableCoefficients(speciesCount);
			mapDefns = new ListOfEditable<IMapDefinition>();
		}

		//---------------------------------------------------------------------

		public bool IsComplete
		{
			get {
				foreach (object parameter in new object[]{ timestep,
				                                           mapFileNames }) {
					if (parameter == null)
						return false;
				}
				return coefficients.IsComplete && mapDefns.IsEachItemComplete;
			}
		}

		//---------------------------------------------------------------------

		public IParameters GetComplete()
		{
			if (IsComplete)
				return new Parameters(timestep.Actual,
				                      coefficients.GetComplete(),
				                      mapDefns.GetComplete(),
				                      mapFileNames.Actual);
			else
				return null;
		}
	}
}
