using System;
using System.Collections.Generic;
using System.Linq;
using Ciloci.Flee;
using Eirikb.SharePoint.Extreme;

namespace Avento
{
    internal class C
    {
        private readonly Dictionary<string, string> _types =
            new Dictionary<string, string> {{"+", "pluss"}, {"-", "minus"}, {"*", "ganger"}, {"/", "delt på"}};

        public string Q;
        public int Res;

        public C(int times)
        {
            var r = new Random();
            var context = new ExpressionContext();
            context.Imports.AddType(typeof (Math));
            var c = 0;
            var qs = Enumerable.Range(0, times).ToList().Select(i =>
                {
                    var type = _types.Keys.ToList()[r.Next(_types.Count)];
                    c++;
                    var v = "a" + c;
                    context.Variables[v] = r.Next(-2000, 10000);
                    return string.Format("{0} {1}", v, type);
                });
            Q = string.Join(" ", qs.ToArray()).Trim();
            Q = Q.Substring(0, Q.Length - 1);
            var eDynamic = context.CompileDynamic(Q);

            Res = (int) eDynamic.Evaluate();
            Console.WriteLine(Q + " = " + Res);

            _types.ToList().ForEach(type => Q = Q.Replace(type.Key, type.Value));
            context.Variables.Keys.ToList().ForEach(key => Q = Q.Replace(key, "" + context.Variables[key]));
        }
    }

    public class Calculus1 : IQuestion
    {
        private readonly string _q;
        private readonly int _res;

        public Calculus1()
        {
            var c = new C(2);
            _q = string.Format("Hva er {0}", c.Q);
            _res = c.Res;
        }

        #region IQuestion Members

        public bool Run(string line)
        {
            return line.Trim() == "" + _res;
        }

        public int Level
        {
            get { return 2; }
        }

        public string Question
        {
            get { return _q; }
        }

        #endregion
    }

    public class Calculus2 : IQuestion
    {
        private readonly string _q;
        private readonly int _res;

        public Calculus2()
        {
            var c = new C(3);
            _q = string.Format("Hva er {0}", c.Q);
            _res = c.Res;
        }

        #region IQuestion Members

        public bool Run(string line)
        {
            return line.Trim() == "" + _res;
        }

        public int Level
        {
            get { return 4; }
        }

        public string Question
        {
            get { return _q; }
        }

        #endregion
    }

    public class Calculus3 : IQuestion
    {
        private readonly string _q;
        private readonly int _res;

        public Calculus3()
        {
            var c = new C(5);
            _q = string.Format("Hva er {0}", c.Q);
            _res = c.Res;
        }

        #region IQuestion Members

        public bool Run(string line)
        {
            return line.Trim() == "" + _res;
        }

        public int Level
        {
            get { return 9; }
        }

        public string Question
        {
            get { return _q; }
        }

        #endregion
    }
}