using System;

namespace ChuikoFirstLab
{
    class SweepMethod
    {
        private const double Alpha1 = 0;
        private static double Betta1;
        private static int M;
        private static Func<int, double> a;
        private static Func<int, double> b;
        private static Func<int, double> c;
        private static Func<int, double> f;
        public static double[] Decide(double betta1, double yN, int M, 
            Func<int, double> ai, Func<int, double> bi, Func<int, double> ci, Func<int, double> fi)
        {
            Betta1 = betta1;
            a = ai;
            b = bi;
            c = ci;
            f = fi;
            DecideCoefficients(out var alpha, out var betta);
            return DecideY(alpha, betta, yN);
        }

        private static void DecideCoefficients(out double[]alpha, out double[] betta)
        {
            alpha = new double[M];
            betta = new double[M];
            alpha[0] = Alpha1;
            betta[0] = Betta1;

            for (var i = 0; i < M; i++)
            {
                alpha[i + 1] = b(i) / (c(i) - alpha[i] * a(i));
                betta[i + 1] = (a(i) * betta[i] + f(i)) / (c(i) - alpha[i] * a(i));
            }
        }

        private static double[] DecideY(double[]alpha, double[] betta, double yN)
        {
            var y = new double[M+1];
            y[M - 1] = yN;
            for (var i = M - 1; i >= 0; i--)
            {
                y[i] = alpha[i + 1] * y[i + 1] + betta[i + 1];
            }
            return y;
        }
    }
}
