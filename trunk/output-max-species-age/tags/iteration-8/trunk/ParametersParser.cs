using Edu.Wisc.Forest.Flel.Util;
using Landis.Util;
using System.Collections.Generic;

namespace Landis.Output.MaxSpeciesAge
{
	/// <summary>
	/// A parser that reads the plug-in's parameters from text input.
	/// </summary>
	public class ParametersParser
		: Landis.TextParser<IParameters>
	{
		public override string LandisDataValue
		{
			get {
				return "Maximum Species Age";
			}
		}

		//---------------------------------------------------------------------

		public ParametersParser()
		{
		}

		//---------------------------------------------------------------------

		protected override IParameters Parse()
		{
			ReadLandisDataVar();

			//IEditableParameters parameters = new EditableParameters();

			InputVar<int> timestep = new InputVar<int>("Timestep");
			ReadVar(timestep);
			//parameters.Timestep = timestep.Value;

			CheckNoDataAfter("the " + timestep.Name + " parameter");

			//return parameters.GetComplete();
			return new Parameters(timestep.Value.Actual);
		}
	}
}
