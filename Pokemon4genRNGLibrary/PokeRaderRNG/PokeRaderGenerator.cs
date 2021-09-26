using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    static class PokeRaderExt
    {
        private readonly static (int, int)[] d4Points = new (int, int)[32]
        {
            (-4,-4), (-4,-3), (-4,-2), (-4,-1), (-4,0), (-4,1), (-4,2), (-4,3), (-4,4),
            ( 4,-4), ( 4,-3), ( 4,-2), ( 4,-1), ( 4,0), ( 4,1), ( 4,2), ( 4,3), ( 4,4),
            (-3,-4), (-3, 4),
            (-2,-4), (-2, 4),
            (-1,-4), (-1, 4),
            ( 0,-4), ( 0, 4),
            ( 1,-4), ( 1, 4),
            ( 2,-4), ( 2, 4),
            ( 3,-4), ( 3, 4),
        };
        private readonly static (int, int)[] d3Points = new (int, int)[24]
        {
            (-3,-3), (-3,-2), (-3,-1), (-3,0), (-3,1), (-3,2), (-3,3),
            ( 3,-3), ( 3,-2), ( 3,-1), ( 3,0), ( 3,1), ( 3,2), ( 3,3),
            (-2,-3), (-2,3),
            (-1,-3), (-1,3),
            ( 0,-3), ( 0,3),
            ( 1,-3), ( 1,3),
            ( 2,-3), ( 2,3),
        };
        private readonly static (int, int)[] d2Points = new (int, int)[16]
        {
            (-2,-2), (-2,-1), (-2,0), (-2,1), (-2,2),
            ( 2,-2), ( 2,-1), ( 2,0), ( 2,1), ( 2,2),
            (-1,-2), (-1,2),
            ( 0,-2), (0,2),
            ( 1,-2), (1,2),
        };
        private readonly static (int, int)[] d1Points = new (int, int)[8]
        {
            (-1,-1), (-1,0), (-1,1),
            ( 1,-1), ( 1,0), ( 1,1),
            ( 0,-1), ( 0,1)
        };

        public static (int h, int w) ToD1Point(this uint index) => d1Points[index];
        public static (int h, int w) ToD2Point(this uint index) => d2Points[index];
        public static (int h, int w) ToD3Point(this uint index) => d3Points[index];
        public static (int h, int w) ToD4Point(this uint index) => d4Points[index];
    }

    public class PokeRaderGenerator 
        : ISideEffectiveGeneratable<PokeRaderResult, GridClipper>, 
        ISideEffectiveGeneratable<PokeRaderResult, GridClipper, uint>,
        ISideEffectiveGeneratable<PokeRaderResult, GridClipper, uint, bool>
    {
        private readonly uint adv;

        /// <summary>
        /// ポケトレ起動時の生成処理.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="gridClipper"></param>
        /// <returns></returns>
        public PokeRaderResult Generate(ref uint seed, GridClipper gridClipper)
        {
            // 揺れ位置の決定
            var d4 = (seed.GetRand() / 0x800).ToD4Point();
            var d3 = (seed.GetRand() / 0xAAB).ToD3Point();
            var d2 = (seed.GetRand() / 0x1000).ToD2Point();
            var d1 = (seed.GetRand() / 0x2000).ToD1Point();

            var res = new List<(int, int, ShakeType)>();
            if (gridClipper[d4])
                res.Add((d4.h, d4.w, ShakeTypeGenerator.GetInstance(Surround.D4).Generate(ref seed)));

            if (gridClipper[d3])
                res.Add((d3.h, d3.w, ShakeTypeGenerator.GetInstance(Surround.D3).Generate(ref seed)));

            if (gridClipper[d2])
                res.Add((d2.h, d2.w, ShakeTypeGenerator.GetInstance(Surround.D2).Generate(ref seed)));

            if (gridClipper[d1])
                res.Add((d1.h, d1.w, ShakeTypeGenerator.GetInstance(Surround.D1).Generate(ref seed)));

            return new PokeRaderResult() { Shakes = res };
        }

        /// <summary>
        /// 連鎖中にポケトレを手動起動した場合の生成処理.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="gridClipper"></param>
        /// <param name="chain"></param>
        /// <returns></returns>
        public PokeRaderResult Generate(ref uint seed, GridClipper gridClipper, uint chain)
        {
            // 揺れ位置の決定
            var d4 = (seed.GetRand() / 0x800).ToD4Point();
            var d3 = (seed.GetRand() / 0xAAB).ToD3Point();
            var d2 = (seed.GetRand() / 0x1000).ToD2Point();
            var d1 = (seed.GetRand() / 0x2000).ToD1Point();

            seed.Advance(5);

            var res = new List<(int, int, ShakeType)>();
            if (gridClipper[d4])
                res.Add((d4.h, d4.w, ShakeTypeGenerator.GetInstance(Surround.D4, chain, false).Generate(ref seed)));

            if (gridClipper[d3])
                res.Add((d3.h, d3.w, ShakeTypeGenerator.GetInstance(Surround.D3, chain, false).Generate(ref seed)));

            if (gridClipper[d2])
                res.Add((d2.h, d2.w, ShakeTypeGenerator.GetInstance(Surround.D2, chain, false).Generate(ref seed)));

            if (gridClipper[d1])
                res.Add((d1.h, d1.w, ShakeTypeGenerator.GetInstance(Surround.D1, chain, false).Generate(ref seed)));

            return new PokeRaderResult() { Shakes = res };
        }

        /// <summary>
        /// 戦闘後の連鎖処理.
        /// framesはDP: 5, Pt: 1が基本のはず.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="gridClipper"></param>
        /// <param name="chain"></param>
        /// <param name="cought"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public PokeRaderResult Generate(ref uint seed, GridClipper gridClipper, uint chain, bool cought)
        {
            // 揺れ位置の決定
            var d4 = (seed.GetRand() / 0x800).ToD4Point();
            var d3 = (seed.GetRand() / 0xAAB).ToD3Point();
            var d2 = (seed.GetRand() / 0x1000).ToD2Point();
            var d1 = (seed.GetRand() / 0x2000).ToD1Point();

            seed.Advance(adv);

            var res = new List<(int, int, ShakeType)>();
            if (gridClipper[d4] )
                res.Add((d4.h, d4.w, ShakeTypeGenerator.GetInstance(Surround.D4, chain, cought).Generate(ref seed)));

            if (gridClipper[d3] )
                res.Add((d3.h, d3.w, ShakeTypeGenerator.GetInstance(Surround.D3, chain, cought).Generate(ref seed)));

            if (gridClipper[d2] )
                res.Add((d2.h, d2.w, ShakeTypeGenerator.GetInstance(Surround.D2, chain, cought).Generate(ref seed)));

            if (gridClipper[d1] )
                res.Add((d1.h, d1.w, ShakeTypeGenerator.GetInstance(Surround.D1, chain, cought).Generate(ref seed)));

            return new PokeRaderResult() { Shakes = res };
        }

        public PokeRaderGenerator(bool platinum)
            => adv = platinum ? 1 : 5u;
    }

    public class PokeRaderResult
    {
        public IEnumerable<(int h, int w, ShakeType shakeType)> Shakes { get; set; }
    }
}
