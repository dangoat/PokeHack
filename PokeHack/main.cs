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
            Pokemon p1 = null, p2 = null;
            Random rand = new Random();
            string Pokemon1 = null, Pokemon2 = null;
            bool ValidPokemon = true;
            int level = 0;

            do
            {
                try
                {
                    Console.Write("Select pokemon one (or type random): ");
                    Pokemon1 = Console.ReadLine();

                    Console.Write("Select pokemon two (or type random): ");
                    Pokemon2 = Console.ReadLine();

                    if (level <= 0 || level > 100)
                    {
                        level = rand.Next(1, 100);
                        Console.WriteLine("Invalid level, using random level: " + level);
                    }

                    if (Pokemon1.CompareTo("random") == 0)
                        p1 = new Pokemon(rand.Next(1, 721), level);
                    else
                        p1 = new Pokemon(Pokemon1, level);
                    if (Pokemon1.CompareTo("random") == 0)
                        p2 = new Pokemon(rand.Next(1, 721), level);
                    else
                        p2 = new Pokemon(Pokemon2, level);
                }
                catch (NullReferenceException nre)
                {
                    Console.WriteLine("Oops, something went wrong. Try again./n");
                    ValidPokemon = false;
                }
            } while (!ValidPokemon) ;

            Console.WriteLine(p1.Name + " " + p1.Attack + " " + p1.Defense + " " + p1.HealthCurr + " " + p1.Speed);

            Console.WriteLine(p2.Name + " " + p2.Attack + " " + p2.Defense + " " + p2.HealthCurr + " " + p2.Speed);

            while (true)
            {
                if(p1.HealthCurr > 0 && p2.HealthCurr > 0)
                {
                    CombatRound(p1, p2);
                }
                if (p1.HealthCurr < 0)
                {
                    Console.WriteLine(p1.Name + " has fainted");
                    break;
                }
                if (p2.HealthCurr < 0)
                {
                    Console.WriteLine(p2.Name + " has fainted");
                    break;
                }
                  
                    
                

            }
            Console.ReadLine();
        }

        //Given p1 and p2, returns how many turns p1 needs to win with a greedy strategy
        public static int CalculateClock(Pokemon p1, Pokemon p2)
        {
            int health = p2.HealthCurr, clock = 0;
            Move move = PickMoveHighestPower(p1, p2);

            while(health > 0)
            {
                health -= p1.MoveDamage(move, p2);
                clock++;
            }
            return clock;
        }

        public static void CombatRound(Pokemon p1, Pokemon p2)
        {
            Move p1m = null, p2m = null;
            int clock1 = CalculateClock(p1, p2);
            int clock2 = CalculateClock(p1, p2);
            if (clock1 < clock2)
            {

                p1m = PickMoveHighestPower(p1, p2);
            }
            else if (clock2 < clock1)
            {
                p2m = PickMoveHighestPower(p2, p1);
            }
            else
            {
                if (p1.Speed >= p2.Speed)
                {
                    p1m = PickMoveHighestPower(p1, p2);
                    p2m = PickMoveIncreaseClock(p2, p1);
                }

            }
        }

        public static void PrimitiveCombatRound(Pokemon p1, Pokemon p2)
        {
            Move p1m = null, p2m = null;
            Random rand = new Random();

            p1m = PickMoveHighestPower(p1, p2);
            p2m = PickMoveHighestPower(p2, p1);
            if (p1.Speed > p2.Speed)
            {
                if (rand.Next(1, 100) < HitChance(p1m, p1, p2)) //Check for miss
                {
                    int damage = p1.MoveDamage(p1m, p2);
                    p2.TakeDamage(damage);
                    Console.WriteLine(p1.Name + " used " + p1m.Name + " for " + damage + " damage");
                }
                else Console.WriteLine(p1.Name + "'s " +  p1m.Name + " missed");
                if (p2.HealthCurr > 0) //Check for kill
                { 
                    if (rand.Next(1, 100) < HitChance(p2m, p2, p1))
                    {
                        int damage = p2.MoveDamage(p2m, p1);
                        p1.TakeDamage(damage);
                        Console.WriteLine(p2.Name + " used " + p2m.Name + " for " + damage + " damage");
                    }
                    else Console.WriteLine(p2.Name + "'s " + p2m.Name + " missed");
                }
            }
            else
            {
                if (rand.Next(1, 100) < HitChance(p2m, p2, p1))
                {
                    p1.TakeDamage(p2.MoveDamage(p2m, p1));
                    Console.WriteLine(p2.Name + " used " + p2m.Name + " for " + p2.MoveDamage(p2m, p1) + " damage");
                    if (p2m.HasStatus())
                    {
                        p1.Ailment = p2m.GetStatus();
                        if (p1.Ailment.CompareTo("") != 0)
                            Console.WriteLine(p1.Name + " has become " + p1.Ailment);
                    }
                    p2.TakeHealRecoil(p2m.HealOrRecoil());
                }
                else Console.WriteLine(p2.Name + "'s " + p2m.Name + " missed");
                if (p1.HealthCurr > 0)
                {
                    if (rand.Next(1, 100) < HitChance(p1m, p1, p2))
                    {
                        p2.TakeDamage(p1.MoveDamage(p1m, p2));
                        Console.WriteLine(p1.Name + " used " + p1m.Name + " for " + p1.MoveDamage(p1m, p2) + " damage");
                        if (p1m.HasStatus())
                        {
                            p2.Ailment = p1m.GetStatus();
                            if (p2.Ailment.CompareTo("") != 0)
                                Console.WriteLine(p2.Name + " has become " + p2.Ailment);
                        }
                        p1.TakeHealRecoil(p1m.HealOrRecoil());
                    }
                    else Console.WriteLine(p1.Name + "'s " + p1m.Name + " missed");
                }
            }
        }
        
        //Picks move based on highest damage possible.
        //p1 is attacker, p2 is defender
        private static Move PickMoveHighestPower(Pokemon p1, Pokemon p2)
        {
            int maxpower = 0;
            Move p1m = null;
            foreach (Move m in p1.MoveSet)
            {
                if (p1.MoveDamage(m, p2) * HitChance(m, p1, p2) > maxpower)
                {
                    p1m = m;
                    maxpower = p1.MoveDamage(m, p2) * HitChance(m, p1, p2);
                }
            }
            if (p1m == null)
                p1m = PickMoveIncreaseClock(p1, p2);
            return p1m;
        }

        private static Move PickMoveIncreaseClock(Pokemon pDefense, Pokemon pOffense)
        {
            Move p1m = null;
            string status;
            double value, maxVal = 0;
            int clockDefense = CalculateClock(pDefense, pOffense);
            int clockOffense = CalculateClock(pOffense, pDefense);

            foreach(Move m in pDefense.MoveSet)
            {
                if (m.HasStatus())
                {
                    status = m.GetStatus();
                    if (String.Compare(status, "paralysis") == 0)
                    {
                        value = NegBi(clockDefense, clockOffense, 0.25);
                        if (value > maxVal)
                        {
                            p1m = m;
                            maxVal = value;
                        }
                    }
                
                    else if (String.Compare(status, "sleep") == 0)
                    {
                        value = (5 - clockDefense + clockOffense) / 5;
                        if (value > maxVal)
                        {
                            p1m = m;
                            maxVal = value;
                        }
                    }
                    else if (String.Compare(status, "confusion") == 0)
                    {
                        value = NegBi(clockDefense, clockOffense, .5) * ((4 - clockDefense + clockOffense) / 4);
                        if (value > maxVal)
                        {
                            p1m = m;
                            maxVal = value;
                        }
                    }
                    else if (String.Compare(status, "freeze") == 0)
                    {
                        value = Math.Pow(.8, clockDefense - clockOffense);
                        if (value > maxVal)
                        {
                            p1m = m;
                            maxVal = value;
                        }
                    }
                    else if (String.Compare(status, "infatuation") == 0)
                    {
                        value = NegBi(clockDefense, clockOffense, .5);
                        if (value > maxVal)
                        {
                            p1m = m;
                            maxVal = value;
                        }
                    }
                    
                }
            }
        }

        private static double NegBi(int clockDefense, int clockOffense, double prob)
        {
            int diff = clockDefense - clockOffense;
            double ToReturn = (double)Fact(clockDefense - 1) / (double)(Fact(diff)) * 
                Math.Pow(prob, diff) * Math.Pow(1 - prob, clockOffense);
            return ToReturn;
        }

        private static int Fact(int n)
        {
            int ToReturn = 1;
            while (n > 1)
            {
                ToReturn *= n;
                n--;
            }
            return ToReturn;
        }

        private  static int HitChance(Move m, Pokemon att, Pokemon def)
        {
            return  m.Accuracy * att.Accuracy / def.Evasiveness;
        }

    }
}
