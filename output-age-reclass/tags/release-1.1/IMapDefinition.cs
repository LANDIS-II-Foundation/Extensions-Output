//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

namespace Landis.Output.Reclass
{
	/// <summary>
	/// The definition of a reclass map.
	/// </summary>
	public interface IMapDefinition
	{
		/// <summary>
		/// Map name
		/// </summary>
		string Name
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Forest types
		/// </summary>
		IForestType[] ForestTypes
		{
			get;
		}
	}
}
