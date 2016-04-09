using System;
using PokeAPI;

namespace PokeHack
{
    public class Move
    {
        private PokeAPI.Move move;
        public string Name;
        public int Accuracy;
        public int EffectChance;
        public int PowerPoints;
        public int Power;
        public string DamageClass;
        public Type Type;

        public Move(int id)
        {
            FetchMove(id);
            Accuracy = (int)move.Accuracy;
            EffectChance = (int)move.EffectChance;
            PowerPoints = (int)move.PP;
            Power = (int)move.Power;
            DamageClass = move.DamageClass.Name;
            Type = StringToType(move.Type.Name);

        }

        public async void FetchMove(int id)
        {
            move = await DataFetcher.GetApiObject<PokeAPI.Move>(id);
        }

        private Type StringToType(string typename)
        {
            int TypeNum;
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

