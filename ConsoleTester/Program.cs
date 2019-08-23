using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebserviceProxy;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            new Class1().Test().Wait();
        }
    }
}
