using System;
using PokeAPI;
using System.Threading.Tasks;

namespace PokeHack
{
    public class Move
    {
        private PokeAPI.Move move;
        public string Name;
        public int Accuracy = 100;
        public int EffectChance = 100;
        public int PowerPoints;
        public int Power;
        public string DamageClass;
        public Type Type;
        public string EffectType;

        public Move(int MoveID)
        {
            Fetch(MoveID);
            Name = move.Name;
            Console.WriteLine(Name);

            if (move.Accuracy != null)
                Accuracy = (int)move.Accuracy;
            if (move.EffectChance != null)
                EffectChance = (int)move.EffectChance;
            if (move.PP != null)
                PowerPoints = (int)move.PP;
            if (move.Power != null)
                Power = (int)move.Power;

            EffectType = move.Meta.Value.Ailment.Name;
            if (EffectType.CompareTo("none") == 0)
                EffectType = "";
            DamageClass = move.DamageClass.Name;
            Type = Types.StringToType(move.Type.Name);
        }

        public Move(PokemonMove ThisMove)
        {
            Name = ThisMove.Move.Name;
            Fetch(Name);
            Console.WriteLine(Name);

            if (move.Accuracy != null)
                Accuracy = (int)move.Accuracy;
            if (move.EffectChance != null)
                EffectChance = (int)move.EffectChance;
            if (move.PP != null)
                PowerPoints = (int)move.PP;
            if (move.Power != null)
                Power = (int)move.Power;

            EffectType = move.Meta.Value.Ailment.Name;
            if (EffectType.CompareTo("none") == 0)
                EffectType = "";
            DamageClass = move.DamageClass.Name;
            Type = Types.StringToType(move.Type.Name);

        }

        public void Fetch(String MoveName)
        {
            Task<PokeAPI.Move> MoveTask = FetchMove(MoveName);
            move = MoveTask.Result;
        }

        public void Fetch(int MoveID)
        {
            Task<PokeAPI.Move> MoveTask = FetchMove(MoveID);
            move = MoveTask.Result;
        }

        public async Task<PokeAPI.Move> FetchMove(String MoveName)
        {
            return await DataFetcher.GetNamedApiObject<PokeAPI.Move>(MoveName);
        }

        public async Task<PokeAPI.Move> FetchMove(int MoveID)
        {
            return await DataFetcher.GetApiObject<PokeAPI.Move>(MoveID);
        }

        public bool HasStatus()
        {
            return EffectType != null;
        }

        public int GetTime()
        {
            Random rand = new Random();
            if(String.Compare(this.EffectType, "sleep") == 0)
            {
                return rand.Next(1, 5);
            }
            if (String.Compare(this.EffectType, "confusion") == 0)
            {
                return rand.Next(1, 4);
            }
            else return -1;
        }

        public int HealOrRecoil()
        {
            return move.Meta.Value.DrainRecoil;
        }

        public string GetStatus()
        {
            Random random = new Random();
            int EffectHappens = random.Next(1, 100);
            if (EffectHappens > this.EffectChance)
                return "";

            return this.EffectType;
        }

    }
}