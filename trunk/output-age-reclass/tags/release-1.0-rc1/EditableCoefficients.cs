using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Output.Reclass
{
	/// <summary>
	/// Editable set of reclass coefficients for species.
	/// </summary>
	public class EditableCoefficients
		: IEditable<double[]>
	{
		private double[] coefficients;

		//---------------------------------------------------------------------

		/// <summary>
		/// Coefficient for a species
		/// </summary>
		public double this[int speciesIndex]
		{
			get {
				return coefficients[speciesIndex];
			}

			set {
				/*if (value < 0.0 )
					throw new DoubleException(value,
						"Value must be = or > 0.0");
				if (value > 1)
					throw new InputValueException(value,
						"Value must be = or < 1.0");
				*/
				coefficients[speciesIndex] = value;
			}
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Initialize a new instance.
		/// </summary>
		/// <remarks>
		/// All the coefficients are initially 0.
		/// </remarks>
		public EditableCoefficients(int speciesCount)
		{
			coefficients = new double[speciesCount];
		}

		//---------------------------------------------------------------------

		public bool IsComplete
		{
			get {
				return true;
			}
		}

		//---------------------------------------------------------------------

		public double[] GetComplete()
		{
			return coefficients;
		}
	}
}
