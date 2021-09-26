using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;
using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    static class Util
    {
        public static Gender GetGender(this uint pid, GenderRatio ratio)
        {
            if (ratio == GenderRatio.Genderless) return Gender.Genderless;

            return (pid & 0xFF) < (uint)ratio ? Gender.Female : Gender.Male;
        }

        public static uint ToCuteCharmPIDBuffer(this GenderRatio genderRatio, Gender fixedGender)
        {
            if (fixedGender != Gender.Male) return 0;

            switch (genderRatio)
            {
                case GenderRatio.M7F1: return 50;
                case GenderRatio.M3F1: return 75;
                case GenderRatio.M1F1: return 150;
                case GenderRatio.M1F3: return 200;
                default: return 0;
            }
        }
    }

    static class DPtDenominators
    {
        public const uint Rand100 = 0x290;
        public const uint Rand25 = 0xA3E;
        public const uint Rand3 = 0x5556;
    }
}
