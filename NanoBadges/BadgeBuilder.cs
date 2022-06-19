/*
    NanoBadge. An endpoint to create badges with MAUI.
    Copyright (C) 2022  Erick Fernando Mora Ramirez

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.

    mailto:erickfernandomoraramirez@gmail.com
*/

using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace NanoBadges
{
    public class BadgeBuilder
    {
        Font Font { get; set; }
        public float FontSize { get; set; } = 14;
        public float Padding { get; set; } = 6;
        public float CornerRadius { get; set; } = 4;
        public Color TitleBackground { get; set; }
        public Color ValueBackground { get; set; }
        public BadgeBuilder(Font font, Color? titleBackground = null, Color? textBackground = null)
        {
            Font = font;
            TitleBackground = titleBackground ?? new Color(30, 30, 30);
            ValueBackground = textBackground ?? new Color(200, 60, 0);
        }
        public byte[]? TitledBadge(string title, string value)
        {
            SizeF titleSize = Measure(title);
            RectF titleRectangle = new RectF(0, 0, titleSize.Width, titleSize.Height);
            RectF titleFixRectange = new RectF(titleSize.Width / 2, 0, titleSize.Width/2, titleSize.Height);
            
            SizeF valueSize = Measure(value);
            RectF valueRectangle = new RectF(titleSize.Width, 0, valueSize.Width, valueSize.Height);
            RectF valueFixRectangle = new RectF(titleSize.Width, 0, valueSize.Width / 2, titleSize.Height);;

            using BitmapExportContext bitmap = new SkiaBitmapExportContext(
                (int)(titleSize.Width + valueSize.Width), 
                (int)Math.Max(titleSize.Height, valueSize.Height), 
                1F);

            //Setting badge background
            bitmap.Canvas.FillColor = TitleBackground;
            bitmap.Canvas.FillRoundedRectangle(titleRectangle, CornerRadius);
            bitmap.Canvas.FillRectangle(titleFixRectange);
            bitmap.Canvas.FillColor = ValueBackground;
            bitmap.Canvas.FillRoundedRectangle(valueRectangle,CornerRadius);
            bitmap.Canvas.FillRectangle(valueFixRectangle);

            bitmap.Canvas.Font = Font;
            bitmap.Canvas.FontSize = FontSize;
            bitmap.Canvas.FontColor = Colors.White; //I don't believe in not white letter badges
            bitmap.Canvas.DrawString(title, titleRectangle, HorizontalAlignment.Center, VerticalAlignment.Center);
            bitmap.Canvas.DrawString(value, valueRectangle, HorizontalAlignment.Center, VerticalAlignment.Center);

            using MemoryStream memoryStream = new MemoryStream();
            bitmap.WriteToStream(memoryStream);
            return memoryStream.ToArray();
        }

        public SizeF Measure(string text)
        {
            using BitmapExportContext bitmap = new SkiaBitmapExportContext(1000, 1000, 1F);
            SizeF original = bitmap.Canvas.GetStringSize(text, Font, FontSize, HorizontalAlignment.Center, VerticalAlignment.Center);
            return new SizeF { 
                Height = original.Height + Padding*2, 
                Width = original.Width + Padding*2
            };
        }
    }
}
