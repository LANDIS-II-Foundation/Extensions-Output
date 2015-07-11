using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Output.Reclass
{
	/// <summary>
	/// Editable forest type.
	/// </summary>
	public interface IEditableForestType
		: IEditable<IForestType>
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
		/// Multiplier for a species
		/// </summary>
		int this[int speciesIndex]
		{
			get;
			set;
		}
	}
}
