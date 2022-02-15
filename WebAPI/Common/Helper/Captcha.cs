namespace WebAPI.Common.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebAPI.Common.Models;

    public class Captcha
    {
        public static Task<CaptchaResult> GenerateCaptchaImageAsync()
        {
            int mathResult = 0;
            string expression = null;

            Random rnd = new Random();

            //生成3个10以内的整数，用来运算
            int operator1 = rnd.Next(0, 10);
            int operator2 = rnd.Next(0, 10);

            //随机组合运算顺序，只做 + 和 * 运算
            switch (rnd.Next(0, 3))
            {
                case 0:
                    mathResult = operator1 + operator2;
                    expression = string.Format(" {0} + {1}  = ?", operator1, operator2);
                    break;
                case 1:
                    mathResult = operator1 * operator2;
                    expression = string.Format(" {0} * {1}  = ?", operator1, operator2);
                    break;
                default:
                    mathResult = operator2 - operator1;
                    expression = string.Format(" {0} - {1}  = ?", operator2, operator1);
                    break;
            }
            var ms = new MemoryStream();

            using (Bitmap bmp = new Bitmap(111, 36))
            {
                using (Graphics graph = Graphics.FromImage(bmp))
                {
                    graph.Clear(Color.FromArgb(232, 238, 247)); //背景色，可自行设置

                    /* //画噪点
                     for (int i = 0; i <= 128; i++)
                     {
                         graph.DrawRectangle(
                             new Pen(Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255))),
                             rnd.Next(2, 128),
                             rnd.Next(2, 38),
                             1,
                             1);
                     }*/

                    //输出表达式
                    for (int i = 0; i < expression.Length; i++)
                    {
                        graph.DrawString(expression.Substring(i, 1),
                            new Font(FontFamily.GenericSansSerif, 18, FontStyle.Bold),
                            new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(128), rnd.Next(255))),
                            5 + i * 8,
                            rnd.Next(5, 10));
                    }

                    //画边框，不需要可以注释掉
                    graph.DrawRectangle(new Pen(Color.Firebrick), 0, 0, 111 - 1, 36 - 1);
                }

                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);

                return Task.FromResult(new CaptchaResult
                {
                    CaptchaCode = mathResult.ToString(),
                    CaptchaMemoryStream = ms
                });                
            }
        }
    }
}
