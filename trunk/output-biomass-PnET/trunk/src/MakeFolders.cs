using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Landis.Extension.Output.BiomassPnET
{
    public class MakeFolders
    {
        public static void Make(string fn)
        {
            string folder = "";
            while(fn.IndexOf('/')>0)
            {
                string subfolder = "";
                for (int ch = 0; ch < fn.IndexOf('/')+1; ch++)
                {
                    subfolder += fn[ch];
                }
                try
                {
                    folder += subfolder;
                    System.IO.Directory.CreateDirectory(folder);
                    fn = fn.Replace(subfolder, "");
                }
                catch(System.Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
