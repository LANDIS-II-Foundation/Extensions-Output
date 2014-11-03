//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Arjan de Bruijn 

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.PnET
{
    /// <summary>
    /// Methods for working with the template for filenames of species biomass
    /// maps.
    /// </summary>
    public static class FileNames
    {
        public const string SpeciesVar = "species";
        public const string TimestepVar = "timestep";

        public static IDictionary<string, bool> knownVars { get; private set; }
        private static IDictionary<string, string> varValues;

        //---------------------------------------------------------------------
        public static void MakeFolders(string fn)
        {
            string folder = "";
            while (fn.IndexOf('/') > 0)
            {
                string subfolder = "";
                for (int ch = 0; ch < fn.IndexOf('/') + 1; ch++)
                {
                    subfolder += fn[ch];
                }
                try
                {
                    folder += subfolder;
                    System.IO.Directory.CreateDirectory(folder);
                    fn = fn.Replace(subfolder, "");
                }
                catch (System.Exception e)
                {
                    throw e;
                }
            }
         
        }
        
         
        static FileNames()
        {
            knownVars = new Dictionary<string, bool>();
            knownVars[SpeciesVar] = true;
            knownVars[TimestepVar] = true;

            varValues = new Dictionary<string, string>();
        }

        //---------------------------------------------------------------------

        

        //---------------------------------------------------------------------
        public static string ReplaceTemplateVars(string template,
                                                 string species)
        {
            varValues[SpeciesVar] = species;
            varValues[TimestepVar] = "";

            string fn = OutputPath.ReplaceTemplateVars(template, varValues);
            MakeFolders(fn);
            return fn;
        }
         
         
        public static string ReplaceTemplateVars(string template)
        {
            varValues[SpeciesVar] = "";
            varValues[TimestepVar] = "";

            string fn = OutputPath.ReplaceTemplateVars(template, varValues);
            MakeFolders(fn);
            return fn;
        }
         
        public static string ReplaceTemplateVars(string template,
                                                 string species,
                                                 int    timestep)
        {
            varValues[SpeciesVar] = species;
            varValues[TimestepVar] = timestep.ToString();

            string fn = OutputPath.ReplaceTemplateVars(template, varValues);
            MakeFolders(fn);
            return fn;
        }
    }
}
