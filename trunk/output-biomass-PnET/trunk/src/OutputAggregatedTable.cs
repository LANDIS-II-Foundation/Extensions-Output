using System.Collections.Generic;
using System;
using System.Linq;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{
    
    class OutputAggregatedTable
    {
        static List<string> FileContent = null;
        private static string FileName;

        public OutputAggregatedTable(string Template)
        {
            
            FileName = FileNames.ReplaceTemplateVars(Template, "Overall", PlugIn.ModelCore.CurrentTime).Replace(".img", ".txt");
            FileContent = new List<string>();
            FileContent.Add("Time" + "\t" + "#Cohorts" + "\t" +  "AverageAge" + "\t" + "AverageB(g/m2)" + "\t" + "AverageLAI(m2)" + "\t" + "AverageWater(mm)" + "\t" + "SubCanopyPAR(W/m2)" + "\t" + "Litter(kgDW/m2)" + "\t" + "WoodyDebris(kgDW/m2)");
        }
        public static void WriteNrOfCohortsBalance()
        {
            try
            {

                ISiteVar<int> CohortsPerSite = PlugIn.cohorts.GetIsiteVar(x => x.CohortCount);
                ISiteVar<float> CohortBiom = PlugIn.cohorts.GetIsiteVar(x => x.BiomassSum);
                ISiteVar<int> CohortAge = PlugIn.cohorts.GetIsiteVar(x => (x.CohortCount >0) ? x.AverageAge : -1);
                ISiteVar<byte> CohortLAI = PlugIn.cohorts.GetIsiteVar(x => x.CanopyLAImax);
                ISiteVar<ushort> WaterPerSite = PlugIn.cohorts.GetIsiteVar(x => x.Water);
                ISiteVar<ushort> SubCanopyRAD = PlugIn.cohorts.GetIsiteVar(x => x.SubCanopyParMAX);
                ISiteVar<double> Litter = PlugIn.cohorts.GetIsiteVar(x => x.Litter);
                ISiteVar<double> WoodyDebris = PlugIn.cohorts.GetIsiteVar(x => x.WoodyDebris);

                ushort Water_SUM = 0;
                double CohortBiom_SUM = 0;
                double CohortAge_SUM = 0;
                double CohortLAI_SUM = 0;
                int CohortCount = 0;
                int SubCanopyRad_SUM = 0;
                double Litter_SUM = 0;
                double Woody_debris_SUM = 0;

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    CohortCount += CohortsPerSite[site];
                    CohortBiom_SUM += CohortBiom[site];
                    Water_SUM += WaterPerSite[site];
                    SubCanopyRad_SUM += SubCanopyRAD[site];
                    Litter_SUM += Litter[site];
                    Woody_debris_SUM += WoodyDebris[site];

                    if (CohortsPerSite[site] > 0)
                    {
                        CohortAge_SUM += CohortAge[site];
                        CohortLAI_SUM += CohortLAI[site];
                       
                    }
                }

                string c = CohortCount.ToString();
                string CohortAge_av = (CohortAge_SUM / (float)CohortCount).ToString();
                string CohortBiom_av = (CohortBiom_SUM / (float)CohortCount).ToString();
                string CohortLAI_av = (CohortLAI_SUM / (float)CohortCount).ToString();
                string Water_av = (Water_SUM / (float)CohortCount).ToString();
                string SubCanopyRad_av = (SubCanopyRad_SUM / (float)CohortCount).ToString();
                string Litter_av = (Litter_SUM / (float)CohortCount).ToString();
                string Woody_debris_ave = (Woody_debris_SUM / (float)CohortCount).ToString();

                FileContent.Add(PlugIn.ModelCore.CurrentTime.ToString() + "\t" + c + "\t" + CohortAge_av + "\t" + CohortBiom_av + "\t" + CohortLAI_av + "\t" + Water_av + "\t" + SubCanopyRad_av + "\t" + Litter_av + "\t" + Woody_debris_ave);

                System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write to " +FileName +" "+ e.Message);
            }
            

        }
    }
    
}
