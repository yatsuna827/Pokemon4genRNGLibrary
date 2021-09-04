using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon4genRNGLibrary
{
    public static class DateTimeDetector
    {
        /// <summary>
        /// 起動時の秒を指定して計算します.
        /// 日付は適当に選ばれます.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="blank"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static (InnerClock innerClock, uint frameCount, uint d_sec) DetectDateTime(uint seed, uint blank, uint sec)
        {
            if (sec > 59) throw new ArgumentException("secの指定が不正です");

            var h8 = seed >> 24;
            var u24 = seed & 0xFFFFFF;

            var hour = u24 > 0x180000 ? 23 : (u24 >> 16);
            u24 -= hour << 16;
            var frame = blank + u24;

            // ここでフレームレートを正確な値にすればいいのでは
            var d_sec = (uint)((long)frame * 10000 / 598261);

            var year = frame - (uint)((long)(d_sec * 598261 + 9999) / 10000);
            frame -= year;

            h8 = (h8 - (sec + d_sec) % 60) & 0xFF;

            var month = h8 < 10 ? 1u : 10;
            var day = h8 / month;

            var min = h8 - month * day;

            return (new InnerClock(year, month, day, hour, min, (sec + d_sec) % 60), frame - blank, d_sec);
        }

        /// <summary>
        /// 起動時の日付を指定して計算します.
        /// 起動時の秒は10秒以降で最小のものが選ばれます.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="blank"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="extendsFrame"></param>
        /// <returns></returns>
        public static (InnerClock innerClock, uint frameCount, uint d_sec) DetectDateTime(uint seed, uint blank, uint year, uint month, uint day, bool extendsFrame = false)
        {
            // 日付のバリデーションを行う.
            if (year > 99) throw new ArgumentException("yearの指定が不正です");
            if (month == 0 || month > 12) throw new ArgumentException("monthの指定が不正です");
            if (day == 0 || day > 31) throw new ArgumentException("dayの指定が不正です");
            if (month == 2)
            {
                if (day > 29) throw new ArgumentException("dayの指定が不正です");
                else if (year % 4 != 0 && day == 29) throw new ArgumentException("dayの指定が不正です");
            }
            else if((month == 4 || month == 6 || month == 9 || month == 11) && day == 31)
            {
                throw new ArgumentException("dayの指定が不正です");
            }


            var h8 = seed >> 24;
            var u24 = seed & 0xFFFFFF;

            h8 = (h8 - month * day) & 0xFF;
            if (h8 > 118)
            {
                // 極論、フレームカウンタの加算で賄えばいいのです.
                u24 += (h8 - 118) << 24;
                h8 = 118;
            }

            //
            // 時刻計算
            //

            // 時
            var hour = u24 > 0x180000 ? 23 : (u24 >> 16);
            u24 -= hour << 16;

            // 待機フレーム
            var frame = blank + u24 - year;

            // 起動から初期seed決定までが間に合わなくて待機時間を伸ばしたい場合.
            if (blank + u24 < year || frame < 0x10000 && extendsFrame)
            {
                // 時から借りてこれるときは借りてくる.
                if (hour != 0)
                {
                    hour--;
                    frame += 0x10000;
                }
                // 借りてこれないときはもっと上から借りてくる.
                else if (h8 != 0)
                {
                    h8--;
                    frame += 0x1000000;
                }
                // 全部フレームで賄う. あほでしょ.
                else
                {
                    // (上位8bit) == month * dayが保証されている.
                    // min + sec + (frame >> 24) == 0になればいい.
                    // frameを最小化するためにmin = sec = 59にする.
                    // つまりmin = sec = 59, (frame >> 24) = 138.
                    h8 = 118;
                    frame = ((138u << 24) | (seed & 0xFFFFFF)) + blank;
                }
            }

            // 実待機秒数
            var d_sec = (uint)((long)frame * 10000 / 598261);
            if ((uint)((long)(d_sec * 598261 + 9999) / 10000) != frame) d_sec++;

            //
            // 日付計算
            // 

            // 10秒起動になるように仮に置く.
            var sec = (10 + d_sec) % 60;

            // 10秒にするとオーバーする場合.
            // 決定時の秒数が0になるようにして起動時の秒数を最小化する.
            // (ただし0~9秒は間に合わない = 60秒待つものとして評価する).
            if (h8 < sec) return (new InnerClock(year, month, day, hour, h8, 0), frame - blank, d_sec);

            // 59分にしても足りない場合.
            // 59分で決め打ちして残りを秒数に振る.
            if (sec + 59 < h8) return (new InnerClock(year, month, day, hour, 59, (h8 + 1) % 60), frame - blank, d_sec);


            return (new InnerClock(year, month, day, hour, h8 - sec, sec), frame - blank, d_sec);
        }

    }
}
