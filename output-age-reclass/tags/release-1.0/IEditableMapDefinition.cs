using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Output.Reclass
{
	/// <summary>
	/// Editable definition of a reclass map.
	/// </summary>
	public interface IEditableMapDefinition
		: IEditable<IMapDefinition>
	{
		/// <summary>
		/// Map name
		/// </summary>
		InputValue<string> Name
		{
			get;
			set;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Forest types
		/// </summary>
		ListOfEditable<IForestType> ForestTypes
		{
			get;
		}
	}
}
