﻿using System;
using PokeAPI;
using Type;

namespace PokeHack
{
	public class Move
	{
		private string move;
		private int Identifier;
		private int Accuracy;
		private int EffectChance;
		private int PowerPoints;
		private int Power;
		private string DamageClass;
		private int Type;

		public Move ()
		{
			FetchMove;
		}

		public async void FetchMove(String name)
		{
			move = await DataFetcher.GetApiObject<Move>(name);
			Identifer = move.Id;
			Accuracy = move.Accuracy;
			EffectChance = move.EffectChance;
			PowerPoints = move.Pp;
			Power = move.Power;
			DamageClass = move.DamageClass["name"];
			Type = StringToType(move.Type["name"]);

		}
	}
	private static Type StringToType (string typename) {
		int TypeNum;
		switch (typename [0]) {
		case 'b':
			return Type.Bug;
		case 'd':
			switch (typename [1]) {
			case 'a':
				return Type.Dark;
			default:
				return Type.Dragon;
			}
		case 'e':
			return Type.Electric;
		case 'f':
			switch (typename [2]) {
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
			switch (typename [4]) {
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
			switch (typename [1]) {
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

