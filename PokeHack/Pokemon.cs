using System;
using PokeAPI;
using System.Threading.Tasks;

// [...]


namespace PokeHack
{
	public class Pokemon	//Put everything in here
	{
		private PokeAPI.Pokemon Poke;
        public Move[] MoveSet = new Move[4];
        public String Name = "missing";
		private int Level;
		private int HealthMax;
		public int HealthCurr;
		public int Attack;
		public int Defense;
		public int SpecialAttack;
		public int SpecialDefense;
        public int Speed;
		public int Accuracy = 100;
		public int Evasiveness = 100;
		private Type Type1;
        private Type Type2 = Type.None;
        public String Ailment = "";
        public int AilmentTime = -1;

        // Cunstructor by name
        public Pokemon(string PokemonName, int level)
        {
            // Fetch the Pokemon by name
            Console.WriteLine("Loading pokemon " + PokemonName);
            Fetch(PokemonName);

            // Set Stats
            Level = level;
            Name = Poke.Name;

            GenerateStats();
            GenerateMoves();
        }

        // Cunstructor by ID
        public Pokemon(int PokemonID, int level)
        {

            // Fetch the Pokemon data
            Console.Write("Loading Pokemon " + PokemonID);
            Fetch(PokemonID);

            // Set Stats
            Level = level;
            Name = Poke.Name;

            Console.Write(" - " + Name + "\n");

            GenerateStats();
            GenerateMoves();
        }

        private void GenerateStats()
        {
            PokemonStats[] Stats = Poke.Stats;

            HealthMax = (Stats[5].BaseValue * 2 * Level / 100) + 10 + Level;
            HealthCurr = HealthMax;

            Attack = (Stats[4].BaseValue * 2 * Level / 100) + 5;
            Defense = (Stats[3].BaseValue * 2 * Level / 100) + 5;
            SpecialAttack = (Stats[2].BaseValue * 2 * Level / 100) + 5;
            SpecialDefense = (Stats[1].BaseValue * 2 * Level / 100) + 5;
            Speed = (Stats[0].BaseValue * 2 * Level / 100) + 5;

            // Set Types
            PokemonTypeMap[] TypeMap = Poke.Types;
            Type1 = Types.StringToType(TypeMap[0].Type.Name);
            if (TypeMap.Length > 1)
            {
                Type2 = Types.StringToType(TypeMap[1].Type.Name);
            }
        }

        private void GenerateMoves()
        {
            //Randomly selects moves
            PokemonMove[] PossibleMoves = Poke.Moves;

            if (PossibleMoves.Length > 4)
            {
                Random rand = new Random();
                int[] UsedMoves = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    bool alreadyHas;
                    int randNum;
                    do
                    {
                        alreadyHas = false;
                        randNum = rand.Next(1, PossibleMoves.Length);
                        foreach (int used in UsedMoves)
                            if (used == randNum)
                                alreadyHas = true;
                    } while (alreadyHas);

                    MoveSet[i] = new Move(PossibleMoves[randNum]);
                    UsedMoves[i] = randNum;
                }
            }
            else
                for (int i = 0; i < PossibleMoves.Length; i++)
                    MoveSet[i] = new Move(PossibleMoves[i]);
        }
	
        // Fetch by ID
        public void Fetch(int PokemonID)
        {
            if (PokemonID == 132)
                PokemonID++;
            Task<PokeAPI.Pokemon> PokeTask = FetchPokemon(PokemonID);
            Poke = PokeTask.Result;
        }

        // Fetch by name
        public void Fetch (string PokemonName)
        {
            Task<PokeAPI.Pokemon> PokeTask = FetchPokemonByName(PokemonName);
            Poke = PokeTask.Result;
        }

        // Fetch Task by ID
        public async Task<PokeAPI.Pokemon> FetchPokemon(int PokemonID)
        {
            return await DataFetcher.GetApiObject<PokeAPI.Pokemon>(PokemonID);
        }

        // Fetch Task by name
        public async Task<PokeAPI.Pokemon> FetchPokemonByName(string PokemonName)
        {
            return await DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(PokemonName);
        }

        // Inflict damage
        public void TakeDamage(int damage) {
			HealthCurr -= damage;
		}

        // Is the capable of attacking
        public bool CanAttack()
        {
            Random rand = new Random();
            if (Ailment == "")
                return true;
            if (String.Compare(Ailment, "paralysis") == 0)
            {
                if (rand.Next(1, 4) == 1)
                {
                    Console.WriteLine(Name + " cannot operate due to paralysis");
                    return false;
                }
                else
                {
                    Console.WriteLine(Name + " wasn't affected by paralysis");
                    return true;
                }
            }
            if (String.Compare(Ailment, "infatuation") == 0)
            {
                if (rand.Next(1, 2) == 1)
                {
                    Console.WriteLine(Name + " cannot operate due to infatuation");
                    return false;
                }
                else
                {
                    Console.WriteLine(Name + " wasn't affected by infatuation");
                    return true;
                }
            }
            if (String.Compare(Ailment, "sleep") == 0)
            {
                if(AilmentTime > 0)
                {
                    AilmentTime--;
                    Console.WriteLine(Name + " is snoozing");
                    return false;
                }
                else
                {
                    AilmentTime = -1;
                    Ailment = "";
                    Console.WriteLine(Name + " woke up");
                    return true;
                }
            }
            if (String.Compare(Ailment, "freeze") == 0)
            {
                if (rand.Next(1, 5) == 1)
                {
                    Ailment = "";
                    AilmentTime = -1;
                    Console.WriteLine(Name + " thawed");
                    return true;
                }
                else
                {
                    Console.WriteLine(Name + " remains frozen");
                    return false;
                }
            }
            if (String.Compare(Ailment, "confusion") == 0)
            {
                if (AilmentTime > 0)
                {
                    AilmentTime--;
                }
                else
                {
                    Ailment = "";
                    AilmentTime = -1;
                    Console.WriteLine(Name + " has shed their confusion");
                    return true;
                }
                if (rand.Next(1, 2) == 1)
                {
                    this.TakeDamage((int)((double)(2 * this.Level + 10) / 250.0 * (double)(this.Attack) / this.Defense * 40));
                    Console.WriteLine(Name + " took damage due to confusion");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            if (String.Compare(Ailment, "poison") == 0 || 
                String.Compare(Ailment, "burn") == 0 || 
                String.Compare(Ailment, "leech-seed") == 0)
            {
                this.TakeDamage(HealthMax / 8);
            }

            return true;
        }

        // Give this pokemon an ailment
        public void GiveAilment(string Ailment, int time)
        {
            if (String.Compare(Ailment, "") == 0)
            {
                return;
            }
            else if (String.Compare(this.Ailment, "") != 0)
            {
                Console.WriteLine("But it's already affected by " + Ailment);
                return;
            }
            this.Ailment = Ailment;
            this.AilmentTime = time;
            if (String.Compare(Ailment, "") != 0)
                Console.WriteLine(Name + " now has the Ailment " + Ailment);
            
        }

        // Take either heal or recoil
        public void TakeHealRecoil(int healRecoil)
        {
            HealthCurr += healRecoil;
        }

        // Determine the damage dealt by a move
		public int MoveDamage(Move move, Pokemon defender)
        {
            double damage = 0;
            if (String.Compare(move.DamageClass, "physical") == 0)
            {
                damage = (double)(2 * this.Level + 10) / 250.0 * (double)(this.Attack) / defender.Defense * move.Power * Types.GetModifier(move.Type, defender.Type1, defender.Type2);
            }
            else if (String.Compare(move.DamageClass, "special") == 0)
                damage = (double)(2 * this.Level + 10) / 250.0 * (double)(this.SpecialAttack) / defender.SpecialDefense * move.Power * Types.GetModifier(move.Type, defender.Type1, defender.Type2);
            if (move.Type == this.Type1 || move.Type == this.Type2)
                damage *= 1.5;


            return (int)damage;
		}

    }
}
