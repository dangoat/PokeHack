using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PokeAPI;

namespace PokeHack
{
    class main
    {


        public static void Main()
        {
            string command;

            Pokemon p1 = new Pokemon(number, level);


            while (true)
            {
                command = Console.ReadLine();

                Console.WriteLine(command);

            }

        }

        public void CombatRound(Pokemon p1, Pokemon p2)
        {
            Move p1m, p2m;
            int maxpower = 0;

            for (Move m: p1.GetMoves())
            {
                if (p1.UseMove(m, p2) > maxpower)
                {
                    p1m = m;
                    maxpower = p1.UseMove(m, p2);
                }
            }

            maxpower = 0;

            for (Move m: p2.GetMoves())
            {
                if (p2.UseMove(m, p1) > maxpower)
                {
                    p2m = m;
                    maxpower = p2.UseMove(m, p1);
                }
            }


        }

    }
}
