using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Output.Reclass
{
	/// <summary>
	/// Editable definition of a reclass map.
	/// </summary>
	public class EditableMapDefinition
		: IEditableMapDefinition
	{
		private InputValue<string> name;
		private ListOfEditable<IForestType> forestTypes;

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
		/// Forest types
		/// </summary>
		public ListOfEditable<IForestType> ForestTypes
		{
			get {
				return forestTypes;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Initialize a new instance.
		/// </summary>
		public EditableMapDefinition()
		{
			forestTypes = new ListOfEditable<IForestType>();
		}

		//---------------------------------------------------------------------

		public bool IsComplete
		{
			get {
				foreach (object parameter in new object[]{ name }) {
					if (parameter == null)
						return false;
				}
				return forestTypes.IsEachItemComplete;
			}
		}

		//---------------------------------------------------------------------

		public IMapDefinition GetComplete()
		{
			if (IsComplete)
				return new MapDefinition(name.Actual,
				                         forestTypes.GetComplete());
			else
				return null;
		}
	}
}
