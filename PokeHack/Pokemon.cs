using System;
using PokeAPI;


// [...]


namespace Application
{
	public class Pokemon	//Put everything in here
	{
		private PokemonSpecies p;
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

		public Pokemon (int PokemonID, int level)
		{
			p = await DataFetcher.GetApiObject<PokemonSpecies>(PokemonID);
			Level = level;
			HealthMax = p.hp;
			HealthCurr = HealthMax;
			//p = await DataFetcher.GetNamedApiObject<PokemonSpecies>("lucario");
			Attack = p.Attack;
			Defense = p.Defense;
			SpecialAttack = p.SpAtk;
			SpecialDefense = p.SpDef;
			Speed = p.Speed;
			Happiness = p.Happiness;
			Weight = p.Weight;
			Type1 = p.Types[1]["name"];
			if (Types.length > 1) {
				Type2 = p.Types[2]["name"];
			}
			//Type = p.Types[Name];

		}
		public int TakeDamage(int damage) {
			HealthCurr -= damage;
		}
		public int UseMove(Move move, Pokemon defender) {
			int damage = (int)((((float)(2 * this.Level + 10)/250 * (this.attack /	defender.defense) + 2) * GetModifier(move.Type, defender.Type1, defender.Type2)));
			int Hit = move.Accuracy * this.Accuracy / defender.Evasiveness;
			1 + Random % 100;
		}
	}
}
