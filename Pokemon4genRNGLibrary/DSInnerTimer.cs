using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon4genRNGLibrary
{
    // 主に適当な起動時刻から初期seed候補を出すのに使う

    public readonly struct InnerClock
    {
        public uint Second { get; }
        public uint Minute { get; }
        public uint Hour { get; }
        public uint Day { get; }
        public uint Month { get; }
        public uint Year { get; }

        public uint Seed { get => (Month * Day + Minute + Second << 24) | (Hour << 16) | Year; }

        private const int FRAME_PER_10000SEC = 598261;
        private const int SEC_PER_DAY = 86400;
        private const int DAYS_PER_LOOP = 36525;

        private readonly static int[] daysOfMonth = new int[12]
        {
            31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
        };
        private readonly static int[] totalDays = new int[12]
        {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334
        };

        public uint GetDays()
        {
            var y = (int)Year;
            var m = (int)Month;
            if(m < 3)
            {
                y--;
                m += 12;
            }
            var dy = y * 365 + ((y + 4) >> 2);

            var dm = (m * 979 - 1033) >> 5;

            return (uint)(dy +  dm + Day - 1);
        }

        public InnerClock GetPrev(uint frame, int framePer10000Sec = FRAME_PER_10000SEC)
        {
            var backSec = (int)((long)frame * 10000 / framePer10000Sec);
            if (frame != (uint)(((long)backSec * framePer10000Sec + 9999) / 10000)) backSec++; // ぴったりじゃなかったら秒の境界を超える.

            var backDays = backSec / SEC_PER_DAY;
            backSec -= backDays * SEC_PER_DAY;

            // 現在時刻を秒に直す.
            var sec = (int)(Second + Minute * 60 + Hour * 3600) - backSec;

            // 日付を跨いだ
            if (sec < 0)
            {
                backDays++;
                sec += SEC_PER_DAY;
            }

            var min = sec / 60;
            sec -= min * 60;
            var hour = min / 60;
            min -= hour * 60;

            var days = GetDays() - backDays;
            if (days < 0) days += DAYS_PER_LOOP;

            return new InnerClock((uint)days, (uint)hour, (uint)min, (uint)sec);
        }

        public InnerClock GetNext(uint frame, int framePer10000Sec = FRAME_PER_10000SEC)
        {
            var advanceSec = (int)((long)frame * 10000 / framePer10000Sec);

            var advDays = advanceSec / SEC_PER_DAY;
            advanceSec -= advDays * SEC_PER_DAY;

            // 現在時刻を秒に直す.
            var sec = (int)(Second + Minute * 60 + Hour * 3600) + advanceSec;

            // 日付を跨いだ
            if (sec >= SEC_PER_DAY)
            {
                advDays++;
                sec -= SEC_PER_DAY;
            }

            var min = sec / 60;
            sec -= min * 60;
            var hour = min / 60;
            min -= hour * 60;

            var days = GetDays() + advDays;
            if (days >= DAYS_PER_LOOP) days -= DAYS_PER_LOOP;

            return new InnerClock((uint)days, (uint)hour, (uint)min, (uint)sec);
        }

        public InnerClock(uint days, uint hour, uint minute, uint second)
        {
            days %= DAYS_PER_LOOP;
            Hour = hour;
            Minute = minute;
            Second = second;

            // 2000年が閏年なので、1997年起算にして閏年を必ず4年目になるようにずらす.
            /// 1095 = 365*3
            days += 1095;

            // 4年をひと塊と見なす.
            var y4 = days / 1461;
            days -= y4 * 1461;

            // 4年未満の日数を変換していく.
            // ぴったり1460日だと閏年の影響があってめんどくさいので例外処理.
            if (days == 1460)
            {
                Year = y4 * 4; // +3年と加算した3年分で±0.
                Month = 12;
                Day = 31;
            }
            else
            {
                var y = days / 365;
                Year = y + y4 * 4 - 3; // 加算した3年分を引く.
                days -= y * 365;

                // 閏年の2/29. めんどくさいので例外処理.
                if(y==3 && days == 59)
                {
                    Month = 2;
                    Day = 29;
                }
                else
                {
                    // 1月
                    if(days < 31)
                    {
                        Month = 1;
                        Day = days + 1;
                    }
                    // 2月
                    else if(days < 59)
                    {
                        Month = 2;
                        Day = days - 30;
                    }
                    else
                    {
                        if (y == 3) days--;
                        var m_temp = days / 31; // だいたい何月か目星をつける.
                        var d_temp = days - totalDays[m_temp]; // m_temp月の何日か
                        Month = m_temp + 1;
                        Day = (uint)d_temp + 1;

                        if (d_temp < 1)
                        {
                            // ひと月多く見積もってしまっていた.
                            Month--;
                            Day += (uint)totalDays[m_temp - 1];
                        }
                        if (d_temp >= daysOfMonth[m_temp])
                        {
                            // ひと月少なく見積もってしまっていた.
                            Month++;
                            Day -= (uint)daysOfMonth[m_temp];
                        }
                    }
                }
            }
        }
        public InnerClock(uint year, uint month, uint day, uint hour, uint minute, uint second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public override string ToString()
            => $"{Year}/{Month}/{Day} {Hour:d2}:{Minute:d2}.{Second:d2}";
    }

}
