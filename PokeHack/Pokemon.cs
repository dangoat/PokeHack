using System;
using PokeAPI;

// [...]

namespace PokeHack
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
		private string Type;

		public Pokemon (int PokemonID, int level)
		{
			p = await DataFetcher.GetApiObject<PokemonSpecies>(PokemonID);
			Level = level;
			HealthMax = p.
			//p = await DataFetcher.GetNamedApiObject<PokemonSpecies>("lucario");
			float cRate = p.CaptureRate;
			Attack = p.Attack;
			Defense = p.Defense;
			SpecialAttack = p.SpAtk;
			SpecialDefense = p.SpDef;
			Speed = p.Speed;
			Happiness = p.Happiness;
			Weight = p.Weight;
			Type = p.Types["name"];
			//Type = p.Types[Name];

		}
	}
}
