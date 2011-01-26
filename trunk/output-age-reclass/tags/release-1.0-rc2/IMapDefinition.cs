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
