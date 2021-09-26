using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    public enum ShakeType
    {
        None,
        Small,
        Big,
        Continuous,
        Shiny
    }

    // 揺れ位置の周
    enum Surround
    {
        D1,
        D2,
        D3,
        D4
    }

    // 揺れの種類を決定する.
    class ShakeTypeGenerator : ISideEffectiveGeneratable<ShakeType>
    {
        private protected readonly uint thresh;

        public virtual ShakeType Generate(ref uint seed)
        {
            if (seed.GetRand() / 0x290 < thresh)
                return ShakeType.Small;

            return seed.GetRand() / 0x290 < 50 ? ShakeType.Small : ShakeType.Big;
        }
        private  ShakeTypeGenerator(uint thresh) => this.thresh = thresh;

        public static ShakeTypeGenerator GetInstance(Surround surround)
        {
            switch (surround)
            {
                case Surround.D1: return firstD1;
                case Surround.D2: return firstD2;
                case Surround.D3: return firstD3;
                case Surround.D4: return firstD4;
                default: throw new ArgumentException();
            }
        }
        public static ShakeTypeGenerator GetInstance(Surround surround, uint chain, bool cought)
        {
            switch (surround)
            {
                case Surround.D1: return cought ? chainD1[chain][1] : chainD1[chain][0];
                case Surround.D2: return cought ? chainD2[chain][1] : chainD2[chain][0];
                case Surround.D3: return cought ? chainD3[chain][1] : chainD3[chain][0];
                case Surround.D4: return cought ? chainD4[chain][1] : chainD4[chain][0];
                default: throw new ArgumentException();
            }
        }

        private readonly static ShakeTypeGenerator firstD1;
        private readonly static ShakeTypeGenerator firstD2;
        private readonly static ShakeTypeGenerator firstD3;
        private readonly static ShakeTypeGenerator firstD4;

        private readonly static ShakeTypeGenerator[][] chainD1;
        private readonly static ShakeTypeGenerator[][] chainD2;
        private readonly static ShakeTypeGenerator[][] chainD3;
        private readonly static ShakeTypeGenerator[][] chainD4;
        static ShakeTypeGenerator()
        {
            firstD4 = new ShakeTypeGenerator(88);
            firstD3 = new ShakeTypeGenerator(68);
            firstD2 = new ShakeTypeGenerator(48);
            firstD1 = new ShakeTypeGenerator(28);

            var shinyThresh = Enumerable.Range(0, 40).Select(chain => 1 + 0xFFFF / (8000u - (uint)chain * 200u));

            chainD4 = shinyThresh.Select(_ => new[]
            {
                new ChainShakeTypeGenerator(88, _),
                new ChainShakeTypeGenerator(98, _),
            }).ToArray();
            chainD3 = shinyThresh.Select(_ => new[]
            {
                new ChainShakeTypeGenerator(68, _),
                new ChainShakeTypeGenerator(78, _),
            }).ToArray();
            chainD2 = shinyThresh.Select(_ => new[]
            {
                new ChainShakeTypeGenerator(48, _),
                new ChainShakeTypeGenerator(58, _),
            }).ToArray();
            chainD1 = shinyThresh.Select(_ => new[]
            {
                new ChainShakeTypeGenerator(28, _),
                new ChainShakeTypeGenerator(38, _),
            }).ToArray();
        }

        class ChainShakeTypeGenerator : ShakeTypeGenerator
        {
            private readonly uint shinyThresh;
            public override ShakeType Generate(ref uint seed)
            {
                if (seed.GetRand() / 0x290 < thresh)
                    return (seed.GetRand() < shinyThresh) ? ShakeType.Shiny : ShakeType.Continuous;

                return seed.GetRand() / 0x290 < 50 ? ShakeType.Small : ShakeType.Big;
            }

            public ChainShakeTypeGenerator(uint thresh, uint shinyThresh) : base(thresh)
                => (this.shinyThresh) = (shinyThresh);
        }
    }
}
