//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Output.Reclass
{
	/// <summary>
	/// Editable forest type.
	/// </summary>
	public class EditableForestType
		: IEditableForestType
	{
		private InputValue<string> name;
		private int[] multipliers;

		//---------------------------------------------------------------------

		/// <summary>
		/// Map name
		/// </summary>
		public InputValue<string> Name
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
		public EditableForestType(int speciesCount)
		{
			multipliers = new int[speciesCount];
		}

		//---------------------------------------------------------------------

		public bool IsComplete
		{
			get {
				foreach (object parameter in new object[]{ name }) {
					if (parameter == null)
						return false;
				}
				return true;
			}
		}

		//---------------------------------------------------------------------

		public IForestType GetComplete()
		{
			if (IsComplete)
				return new ForestType(name.Actual, multipliers);
			else
				return null;
		}
	}
}
