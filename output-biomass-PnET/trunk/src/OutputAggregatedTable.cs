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
            FileContent.Add("Time" + "\t" + "#Cohorts" + "\t" + "AverageAge" + "\t" + "AverageB(g/m2)" + "\t" + "AverageLAI(m2)" + "\t" + "AverageWater(mm)" + "\t" + "SubCanopyPAR(W/m2)" + "\t" + "Litter(kgDW/m2)" + "\t" + "WoodyDebris(kgDW/m2)" + "\t" + "AverageBelowGround(g/m2)" + "\t" + "AverageFoliage(g/m2)" + "\t" + "AverageNSC(gC)");
        }
        public static void WriteNrOfCohortsBalance()
        {
            try
            {

                ISiteVar<int> CohortsPerSite = PlugIn.cohorts.GetIsiteVar(x => x.CohortCount);
                ISiteVar<float> CohortBiom = PlugIn.cohorts.GetIsiteVar(x => x.BiomassSum);
                ISiteVar<float> CohortWoodySenescence = PlugIn.cohorts.GetIsiteVar(x => x.WoodySenescenceSum);
                ISiteVar<float> FoliageWoodySenescence = PlugIn.cohorts.GetIsiteVar(x => x.FoliageSenescenceSum);
                ISiteVar<int> CohortAge = PlugIn.cohorts.GetIsiteVar(x => (x.CohortCount >0) ? x.AverageAge : -1);
                ISiteVar<byte> CohortLAI = PlugIn.cohorts.GetIsiteVar(x => x.CanopyLAImax);
                ISiteVar<ushort> WaterPerSite = PlugIn.cohorts.GetIsiteVar(x => x.WaterMax);
                ISiteVar<float> SubCanopyRAD = PlugIn.cohorts.GetIsiteVar(x => x.SubCanopyParMAX);
                ISiteVar<double> Litter = PlugIn.cohorts.GetIsiteVar(x => x.Litter);
                ISiteVar<double> WoodyDebris = PlugIn.cohorts.GetIsiteVar(x => x.WoodyDebris);
                ISiteVar<uint> BelowGroundBiom = PlugIn.cohorts.GetIsiteVar(x => x.BelowGroundBiomass);
                ISiteVar<float> FoliageBiom = PlugIn.cohorts.GetIsiteVar(x => x.FoliageSum);
                ISiteVar<float> NSCBiom = PlugIn.cohorts.GetIsiteVar(x => x.NSCSum);

                double Water_SUM = 0;
                double CohortBiom_SUM = 0;
                double BelowGroundBiom_SUM = 0;
                double FoliageBiom_SUM = 0;
                double NSCBiom_SUM = 0;
                double CohortAge_SUM = 0;
                double CohortLAI_SUM = 0;
                int CohortCount = 0;
                int siteCount = 0;
                double SubCanopyRad_SUM = 0;
                double Litter_SUM = 0;
                double Woody_debris_SUM = 0;

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    siteCount++;
                    CohortCount += CohortsPerSite[site];
                    CohortBiom_SUM += CohortBiom[site];
                    BelowGroundBiom_SUM += BelowGroundBiom[site];
                    FoliageBiom_SUM += FoliageBiom[site];
                    NSCBiom_SUM += NSCBiom[site];
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
                string CohortAge_av = (CohortAge_SUM / (float)siteCount).ToString();
                string CohortBiom_av = (CohortBiom_SUM / (float)siteCount).ToString();
                string BelowGroundBiom_av = (BelowGroundBiom_SUM / (float)siteCount).ToString();
                string FoliageBiom_av = (FoliageBiom_SUM / (float)siteCount).ToString();
                string NSCBiom_av = (NSCBiom_SUM / (float)siteCount).ToString();
                string LAI_av = (CohortLAI_SUM / (float)siteCount).ToString();
                string Water_av = (Water_SUM / (float)siteCount).ToString();
                string SubCanopyRad_av = (SubCanopyRad_SUM / (float)siteCount).ToString();
                string Litter_av = (Litter_SUM / (float)siteCount).ToString();
                string Woody_debris_ave = (Woody_debris_SUM / (float)siteCount).ToString();

                FileContent.Add(PlugIn.ModelCore.CurrentTime.ToString() + "\t" + c + "\t" + CohortAge_av + "\t" + CohortBiom_av + "\t" + LAI_av + "\t" + Water_av + "\t" + SubCanopyRad_av + "\t" + Litter_av + "\t" + Woody_debris_ave + "\t" + BelowGroundBiom_av + "\t" + FoliageBiom_av + "\t" + NSCBiom_av);

                System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write to " +FileName +" "+ e.Message);
            }
            

        }
    }
    
}
