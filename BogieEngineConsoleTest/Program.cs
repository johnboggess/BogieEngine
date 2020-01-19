using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore;
using BogieEngineCore.Nodes;

namespace BogieEngineConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Game window = new Game(800, 800, "Render");
            Console.Read();
        }
    }
}
