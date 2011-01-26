//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

namespace Landis.Output.Reclass
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
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Multiplier for a species
		/// </summary>
		int this[int speciesIndex]
		{
			get;
		}
	}
}
