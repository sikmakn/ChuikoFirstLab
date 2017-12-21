using System;

namespace ChuikoFirstLab
{
    class TemperatureWave
    {
        private static int N;
        private static int M;
        private static double EpsAbs;
        private static double EpsRel;
        private static Func<double, double, double> _F;
        private static int _h;
        private static int _alpha;
        private static int _n;
        private static Func<double, double> _k = u => _alpha * Math.Pow(u, _n);
        private static int _tau;
        public static double[][] Decide(int alpha, int coef, Func<double, double> fi, Func<double, double> csi1, Func<double, double> csi2, Func<double, double>k,
            int h, int tau,int L, int T, Func<double, double, double> F)
        {
            N = L / h;
            M = T / tau;
            _F = F;
            _h = h;
            _k = k;
            _alpha = alpha;
            _n = N;
            _tau = tau;

            var solution = new double[M][];

            solution[0] = DecideU0(fi);
            for (var n = 0; n <= M; n++)
            {
                var Un = new double[N+1];
                var Un1 = solution[n];

                for (var m = 1; DecideError(Un1, Un); m++)
                {
                    Un = Un1;
                    Un1 = SweepMethod.Decide(csi1(n), csi2(n), N, a(Un1),  b(Un1), c(Un1), i => Un[i] + F(i*h, tau*(n+1)));

                }
                solution[n+1] = Un;
            }
            return solution;
        }

        private static Func<double, double, double> ai = (ui_1, ui) => 0.5 * (_k(ui_1) + _k(ui));

        private static double[] DecideU0(Func<double, double> fi)
        {
            var result = new double[N+1];
            for (var i = 0; i <= N; i++)
            {
                result[i] = fi(i*_h);
            }
            return result;
        }

        private static Func<int, double> a(double[] Un)
        {
            return i => _tau * ai(Un[i], Un[i+1]) / _h / _h;
        } 

        private static Func<int, double> b(double[] Un)
        {
            return i => _tau * ai(Un[i - 1], Un[i]);
        }

        private static Func<int, double> c(double[] Un)
        {
            return i => 1 + a(Un)(i) + b(Un)(i);
        }

        private static bool DecideError(double[] um1, double[] um)
        {
            for (var i = 0; i < N+1; i++)
            {
                var flag = Math.Abs(um[i] - um1[i]) <= EpsAbs*Math.Abs(um[i]) + EpsRel;
                if (!flag)
                    return false;

            }
            return true;
        }
    }
}
