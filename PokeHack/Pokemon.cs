using System;
using PokeAPI;

// [...]


namespace PokeHack
{
	public class Pokemon	//Put everything in here
	{
		private PokeAPI.Pokemon Poke;
        private PokeAPI.PokemonSpecies Species;
		private Move[] MoveSet = new Move[4];
		private int Level;
		private int HealthMax;
		private int HealthCurr;
		private int Attack;
		private int Defense;
		private int SpecialAttack;
		private int SpecialDefense;
		private int Speed;
		private int Accuracy = 100;
		private int Evasiveness = 100;
		private int Happiness;
		private int Weight;
		private string Type1;
		private string Type2 = null;

        public Pokemon(int PokemonID, int level)
        {
            FetchPokemon(PokemonID);
            Level = level;

            
            HealthMax = p.;
            HealthCurr = HealthMax;

            Attack = p.Attack;
            Defense = p.Defense;
            SpecialAttack = p.SpAtk;
            SpecialDefense = p.SpDef;
            Speed = p.Speed;
            Happiness = p.Happiness;
            Weight = p.Weight;
            Type1 = p.Types[1]["name"];
            if (Types.length > 1)
            {
                Type2 = p.Types[2]["name"];
            }
            //Type = p.Types[Name];

        }

        public async void FetchPokemon(int PokemonID)
        {
            Poke = await DataFetcher.GetApiObject<PokeAPI.Pokemon>(PokemonID);
            Species = await DataFetcher.GetApiObject<PokemonSpecies>(PokemonID);
        }

        public int TakeDamage(int damage) {
			HealthCurr -= damage;
		}
		public int UseMove(Move move, Pokemon defender) {
			int damage = (int)((((float)(2 * this.Level + 10)/250 * (this.attack /	defender.defense) + 2) * GetModifier(move.Type, defender.Type1, defender.Type2)));
			int Hit = move.Accuracy * this.Accuracy / defender.Evasiveness;
			1 + Random % 100;
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
	}
}
