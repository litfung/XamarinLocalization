using System;
using System.Collections.Generic;
using System.Linq;
using BLELocalization.Abstractions;
using MathNet.Numerics;


namespace BLELocalization.Localization
{
    public class Localization
    {
        public class Algorithm
        {
            public static void maxLH(List<BleDevice> BleDevices, double certainty)
            {


                List<double[]> bounds = new List<double[]>(); //List of array size 2 for bounds calculation
                List<double> Lp = new List<double>(); //Lp for each distance bounds
                List<int> x_sol = new List<int>();
                List<int> y_sol = new List<int>();
                double solution_x = 0, solution_y = 0;
                var x = Enumerable.Range(-100, 200).ToList();
                var y = Enumerable.Range(-100, 200).ToList();
                var CertainRange = certainty; //Certainty Range

                //Use 802.11 channel model C to find bound for certainty range
                int sigma = 8; //Shadow Fading
                double dbp = 5; //??
                int Pt = 13; //Transmit Power
                double fc = 2.4e9; //Frequency
                double c = 3e8; //Speed of Light


                var L0 = -20 * Math.Log10(c / 4 / Math.PI / fc);
                double FadeMargin = sigma * Math.Sqrt(2) * SpecialFunctions.ErfcInv(1 + CertainRange);


                foreach (var b in BleDevices)
                {
                    int i = 0;


                    Lp.Add(L0 + 20 * Math.Log10(dbp) + 35 * Math.Log10(b.Distance / dbp));

                    i++;
                }

                int t = 0;
                foreach (var l in Lp)
                {

                    double[] temp = new double[] {
                    dbp*Math.Pow(10,((Lp[t] + FadeMargin - L0 - 20* Math.Log10(dbp))/35)),
                    dbp*Math.Pow(10,((Lp[t] - FadeMargin - L0 - 20*Math.Log10(dbp))/35))
                    };

                    bounds.Add(temp);

                    t++;
                }

                int k = 0;
                for (int i = 0; i < x.Count(); i++)
                {
                    for (int j = 0; j < y.Count(); j++)
                    {
                        bool cir1 = TestRange((Math.Pow(x[i] - BleDevices[0].Location[0], 2) + System.Math.Pow(y[j] - BleDevices[0].Location[1], 2)), System.Math.Pow(bounds[0][0], 2), System.Math.Pow(bounds[0][1], 2));
                        bool cir2 = TestRange((Math.Pow(x[i] - BleDevices[1].Location[0], 2) + System.Math.Pow(y[j] - BleDevices[1].Location[1], 2)), System.Math.Pow(bounds[1][0], 2), System.Math.Pow(bounds[1][1], 2));
                        bool cir3 = TestRange((Math.Pow(x[i] - BleDevices[2].Location[0], 2) + System.Math.Pow(y[j] - BleDevices[2].Location[1], 2)), System.Math.Pow(bounds[2][0], 2), System.Math.Pow(bounds[2][1], 2));
                        if (cir1 && cir2 && cir3)
                        {
                            k = k + 1;
                            x_sol.Add(x[i]);
                            y_sol.Add(y[j]);
                        }
                    }
                }


                try
                {
                    solution_x = x_sol.Average();
                    solution_y = y_sol.Average();

                }
                catch
                {

                }

            }

            public static bool TestRange(double numberToCheck, double bottom, double top)
            {
                return (numberToCheck >= bottom && numberToCheck <= top);
            }
        }
    }
}
