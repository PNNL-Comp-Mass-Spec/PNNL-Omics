using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.UnitTests
{
    class TestUtils
    {
        private static void ShowMessage(string methodName, string message)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + @", {0}, {1}", message, methodName);
        }

        public static void ShowStarting(string methodName)
        {
            ShowMessage(methodName, "Starting");
        }

    }
}
