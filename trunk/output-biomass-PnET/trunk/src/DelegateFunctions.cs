using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohortsPnET;
using Landis.SpatialModeling;
using Landis.Extension.Succession.BiomassPnET;
using System.IO;

namespace Landis.Extension.Output.BiomassPnET
{
    public class DelegateFunctions
    {
        public delegate Landis.Library.Biomass.Species.AuxParm<float> GetAllSpeciesSpecific();
        public delegate float GetOverallAverage();
        public delegate float GetValue(Site site);
        public delegate int GetSpeciesSpecificValue(ISpecies species,Site site);
    }
}
