using System;
using System.Collections.Generic;
using Landis.Core;


namespace Landis.Extension.Output.PnET
{
    public class MapComparison
    {
        public enum values
        {
            No_Presence = 0,
            Continued_Presence = 1,
            Discontinued_Presence = 2,
            New_Presence = 3
        }
        public void PrintLabels(string MapNameTemplate, ISpecies Species)
        {
            string FileName = System.IO.Path.ChangeExtension(FileNames.ReplaceTemplateVars(MapNameTemplate.Replace("{timestep}", "Label"), Species.Name), "txt");

            List<string> Content = new List<string>();

            foreach (var value in Enum.GetValues(typeof(values)))
            {
                Content.Add((int)value +"\t" + value.ToString() +"\n");
             
            }
            System.IO.File.WriteAllLines(FileName, Content.ToArray());
        }
        public int this[bool before, bool after]
        {
            get
            {
                if (before == false && after == false)
                {
                    return (int)values.No_Presence;
                }
                else if (before == true && after == true)
                {
                    return (int)values.Continued_Presence;
                }
                else if (before == true && after == false)
                {
                    return (int)values.Discontinued_Presence;
                }
                else if (before == false && after == true)
                {
                    return (int)values.New_Presence;
                }
                else throw new System.Exception("No implementation for " + before + " and " + after + " in MapComparison values");
            }
        
        }

        


      
 
    }
}
