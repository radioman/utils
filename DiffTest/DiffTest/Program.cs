using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffTest
{
    static class Program
    {
        static void Main(string[] args)
        {
            string text1 = "HFT837";
            string text2 = "HTM37";

            Debug.WriteLine(text1);

            foreach (var align in text1.Align(text2))
            {
                Debug.WriteLine(align);
            }

            Console.ReadLine();
        }
    }
}
