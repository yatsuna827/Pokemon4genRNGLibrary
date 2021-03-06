using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    class StandardItemSelector : IHoldItemSelector
    {
        private readonly GameVersion gameVersion;
        public string SelectItem(ref uint seed, Pokemon.Species species)
        {
            var r = seed.GetRand(100);
            if (r < 45) return species.HoldItems(gameVersion).Must;
            if (r < 95) return species.HoldItems(gameVersion).Common;
            return species.HoldItems(gameVersion).Rare;
        }
        public StandardItemSelector(GameVersion gameVersion)
            => this.gameVersion = gameVersion;
    }
}
