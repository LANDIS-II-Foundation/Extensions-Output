using Landis.Biomass;
using Edu.Wisc.Forest.Flel.Util;

using System;
using System.Collections.Generic;


namespace Landis.Output.BiomassAgeClass
{
    public class AgeClass
    {
        private ushort bin_type = 4;//Enumerated 1 = <X, 2 = X-Y, 3 = >X, 4 = X
        private ushort lwr_age = 0;
        private ushort upr_age = 0;
        private string name = "";

        public AgeClass()
        {
            bin_type = 4;
            lwr_age = 0;
            upr_age = 0;
            name = "";
        }

        public AgeClass(ushort bin_type, ushort lwr_age, ushort upr_age, string name)
        {
            //Note - make sure to set upr_age for the < and = cases, lwr_age for the > case.
            this.bin_type = bin_type;
            this.lwr_age = lwr_age;
            this.upr_age = upr_age;
            this.name = name;
        }

        public ushort BinType
        {
            get { return bin_type; }
        }
        public ushort LwrAge
        {
            get {return lwr_age;}
        }
        public ushort UprAge
        {
            get {return upr_age; }
        }
        public string Name
        {
            get { return name; }
        }

        public bool Parse(string word)
        {
            //consume ageclass text and parse into the appropriate member values
            //set this or return false if fail
            try
            {
                //must contain ( and last character must be ) 
                if (!(word.Contains("(") && word.EndsWith(")")))
                    return false;
                word = word.Replace(" ", "");
                word = word.Replace("\t", "");
                word = word.TrimEnd(")".ToCharArray());
                string[] vals = word.Split("(".ToCharArray());
                //if vals is valid, [0] will be ageclass name and [1] will be range expression
                if (vals == null)
                    return false;
                string name = vals[0];
                string range_expr = vals[1];
                if (name == null || name == "" || range_expr == null || range_expr == "")
                    return false;

                this.name = name;
                if (range_expr.StartsWith("<"))
                {
                    this.bin_type = 1;
                    this.lwr_age = 0;
                    range_expr = range_expr.Replace("<", "");
                    this.upr_age = (ushort)Convert.ToUInt16(range_expr);
                }
                else if (range_expr.StartsWith(">"))
                {
                    this.bin_type = 3;
                    this.lwr_age = 0;
                    range_expr = range_expr.Replace(">", "");
                    this.lwr_age = (ushort)Convert.ToUInt16(range_expr);
                }
                else if (range_expr.Contains("-"))
                {
                    this.bin_type = 2;
                    string[] range_vals = range_expr.Split("-".ToCharArray());
                    this.lwr_age = (ushort)Convert.ToUInt16(range_vals[0]);
                    this.upr_age = (ushort)Convert.ToUInt16(range_vals[1]);
                }
                else
                {
                    this.bin_type = 4;
                    this.lwr_age = (ushort)Convert.ToUInt16(range_expr);
                    this.upr_age = this.lwr_age;
                }
                return true;
            }
            catch
            {
                return false;
            }            
        }
    }


    /// <summary>
    /// Methods for computing biomass for groups of cohorts.
    /// </summary>
    public static class Util
    {


        //---------------------------------------------------------------------
        /*
        public static int ComputeBiomass(ISiteCohorts cohorts)
        {
            int total = 0;
            if (cohorts != null)
                foreach (ISpeciesCohorts speciesCohorts in cohorts)
                    total += ComputeBiomass(speciesCohorts);
            return total;
        }
        */


        //---------------------------------------------------------------------
        public static int ComputeAgeClassBiomass(ISpeciesCohorts cohorts, AgeClass ageclass)
        {
            int total = 0;
            if (cohorts == null)
                return total;
            ushort lwr_age = ageclass.LwrAge;
            ushort upr_age = ageclass.UprAge;

            switch (ageclass.BinType)
            {
                case 1:
                    {// <
                        foreach (ICohort cohort in cohorts)
                        {
                            if (cohort.Age < upr_age)
                            {
                                total += cohort.Biomass;
                            }
                        }
                        break;
                    }

                case 2:
                    {// Range - equivalent to (>= lwr_age and <upr_age)
                        foreach (ICohort cohort in cohorts)
                        {
                            if (cohort.Age >= lwr_age && cohort.Age < upr_age)
                            {
                                total += cohort.Biomass;
                            }
                            else if (cohort.Age < lwr_age)
                                break;//we can break here, since ages sorted descending order
                        }
                        break;
                    }

                case 3:
                    {// >  (is this equivalent to >= ??)
                        foreach (ICohort cohort in cohorts)
                        {
                            if (cohort.Age >= lwr_age)
                            {
                                total += cohort.Biomass;
                            }
                            else
                                break;//we can break here, since ages sorted descending order
                        }

                        break;
                    }
                case 4:
                    {// Single value
                        foreach (ICohort cohort in cohorts)
                        {
                            if (cohort.Age == lwr_age)
                            {
                                total += cohort.Biomass;
                            }
                            else if (cohort.Age < lwr_age)
                                break;//we can break here, since ages sorted descending order
                        }
                        break;
                    }
                    
                default:
                    {
                        throw new InputValueException("","Unhandled binning type; this should never occur");
                    }
            }

            return total;
        }     
    }
}
