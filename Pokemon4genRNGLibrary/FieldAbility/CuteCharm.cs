using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon4genRNGLibrary
{
    public class CuteCharm : FieldAbility
    {
        public Gender FixedGender { get; private set; }
        internal override WildGenerator Accept<TVersion, TEncType>(WildGeneratorFactory<TVersion, TEncType> factory)
            => factory.Resolve(this);

        public CuteCharm(Gender cuteCharmPokeGender)
            => FixedGender = cuteCharmPokeGender.Reverse();
    }
}
