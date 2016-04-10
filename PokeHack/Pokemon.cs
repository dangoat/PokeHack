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
		private int Happiness;
		private int Weight;
		private Type Type1;
        private Type Type2 = Type.None;
        public String Ailment = "";
        public int AilmentTime = -1;

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

            // Happiness = p.Happiness;
            // Weight = p.Weight;

            // Set Types
            PokemonTypeMap[] Types = Poke.Types;
            Type1 = StringToType(Types[0].Type.Name);
            if (Types.Length > 1)
            {
                Type2 = StringToType(Types[1].Type.Name);
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
	
        public void Fetch(int PokemonID)
        {
            if (PokemonID == 132)
                PokemonID++;
            Task<PokeAPI.Pokemon> PokeTask = FetchPokemon(PokemonID);
            Poke = PokeTask.Result;
        }

        public void Fetch (string PokemonName)
        {
            Task<PokeAPI.Pokemon> PokeTask = FetchPokemonByName(PokemonName);
            Poke = PokeTask.Result;
        }

        public async Task<PokeAPI.Pokemon> FetchPokemon(int PokemonID)
        {
            return await DataFetcher.GetApiObject<PokeAPI.Pokemon>(PokemonID);
        }

        public async Task<PokeAPI.Pokemon> FetchPokemonByName(string PokemonName)
        {
            return await DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(PokemonName);
        }
       

        public void TakeDamage(int damage) {
			HealthCurr -= damage;
		}
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
            else return true;
        }
        public void GiveAilment(string Ailment, int time)
        {
            if (String.Compare(Ailment, "") != 0 && String.Compare(this.Ailment, "") != 0)
            {
                Console.WriteLine("But it's already affected by " + Ailment);
                return;
            }
            this.Ailment = Ailment;
            this.AilmentTime = time;
            if (String.Compare(Ailment, "") != 0)
                Console.WriteLine(Name + " now has the Ailment " + Ailment);
            
        }

        public void TakeHealRecoil(int healRecoil)
        {
            HealthCurr += healRecoil;
        }

		public int MoveDamage(Move move, Pokemon defender) {
            double damage = 0;
            if (String.Compare(move.DamageClass, "physical") == 0)
            {
                damage = (double)(2 * this.Level + 10) / 250.0 * (double)(this.Attack) / defender.Defense * move.Power * GetModifier(move.Type, defender.Type1, defender.Type2);
            }
            else if (String.Compare(move.DamageClass, "special") == 0)
                damage = (double)(2 * this.Level + 10) / 250.0 * (double)(this.SpecialAttack) / defender.SpecialDefense * move.Power * GetModifier(move.Type, defender.Type1, defender.Type2);
            if (move.Type == this.Type1 || move.Type == this.Type2)
                damage *= 1.5;


            return (int)damage;
		}
		
		public static double GetModifier(Type attack, Type t1, Type t2)
	        {
	            double modifier = 1;
	            //Attack is row, Defense is column
	            double[,] resistances = new double[,] {
	            {  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1, .5,  0,  1,  1, .5,  1},
	            {  1, .5, .5,  1,  2,  2,  1,  1,  1,  1,  1,  2, .5,  1, .5,  1,  2,  1},
	            {  1,  2, .5,  1, .5,  1,  1,  1,  2,  1,  1,  1,  2,  1, .5,  1,  1,  1},
	            {  1,  1,  2, .5, .5,  1,  1,  1,  0,  2,  1,  1,  1,  1, .5,  1,  1,  1},
	            {  1, .5,  2,  1, .5,  1,  1, .5,  2, .5,  1, .5,  2,  1, .5,  1, .5,  1},
	            {  1, .5, .5,  1,  2, .5,  1,  1,  2,  2,  1,  1,  1,  1,  2,  1, .5,  1},
	            {  2,  1,  1,  1,  1,  2,  1, .5,  1, .5, .5, .5,  2,  0,  1,  2,  2, .5},
	            {  1,  1,  1,  1,  2,  1,  1, .5, .5,  1,  1,  1, .5, .5,  1,  1,  0,  2},
	            {  1,  2,  1,  2, .5,  1,  1,  2,  1,  0,  1, .5,  2,  1,  1,  1,  2,  1},
	            {  1,  1,  1, .5,  2,  1,  2,  1,  1,  1,  1,  2, .5,  1,  1,  1, .5,  1},
	            {  1,  1,  1,  1,  1,  1,  2,  2,  1,  1, .5,  1,  1,  1,  1,  0, .5,  1},
	            {  1, .5,  1,  1,  2,  1, .5, .5,  1, .5,  2,  1,  1, .5,  1,  2, .5, .5},
	            {  1,  2,  1,  1,  1,  2, .5,  1, .5,  2,  1,  2,  1,  1,  1,  1, .5,  1},
	            {  0,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  1,  1,  2,  1, .5,  1,  1},
	            {  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  1, .5,  0},
	            {  1,  1,  1,  1,  1,  1, .5,  1,  1,  1,  2,  1,  1,  2,  1, .5,  1,  1},
	            {  1, .5, .5, .5,  1,  2,  1,  1,  1,  1,  1,  1,  2,  1,  1,  1, .5,  1},
	            {  1, .5,  1,  1,  1,  1,  2, .5,  1,  1,  1,  1,  1,  1,  2,  2, .5,  1}};
	
	            modifier *= resistances[(int)attack, (int)t1];
	
	            if (t2 != Type.None)
	                modifier *= resistances[(int)attack, (int)t2];
	
	            return modifier;
	        }

        private Type StringToType(string typename)
        {
            switch (typename[0])
            {
                case 'b':
                    return Type.Bug;
                case 'd':
                    switch (typename[1])
                    {
                        case 'a':
                            return Type.Dark;
                        default:
                            return Type.Dragon;
                    }
                case 'e':
                    return Type.Electric;
                case 'f':
                    switch (typename[2])
                    {
                        case 'i':
                            return Type.Fairy;
                        case 'g':
                            return Type.Fighting;
                        case 'r':
                            return Type.Fire;
                        default:
                            return Type.Flying;
                    }
                case 'g':
                    switch (typename[4])
                    {
                        case 't':
                            return Type.Ghost;
                        case 's':
                            return Type.Grass;
                        default:
                            return Type.Ground;
                    }
                case 'i':
                    return Type.Ice;
                case 'n':
                    return Type.Normal;
                case 'p':
                    switch (typename[1])
                    {
                        case 'o':
                            return Type.Poison;
                        default:
                            return Type.Psychic;
                    }
                case 'r':
                    return Type.Rock;
                case 's':
                    return Type.Steel;
                default:
                    return Type.Water;
            }
        }
    }
}
