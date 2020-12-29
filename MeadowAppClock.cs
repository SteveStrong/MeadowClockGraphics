using System;
using System.Threading;

using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;

//https://www.hackster.io/wilderness-labs/working-with-graphics-on-a-st7789-display-using-meadow-e2295a

//https://www.hackster.io/wilderness-labs/working-with-graphics-on-a-st7789-display-using-meadow-e2295a

namespace MeadowClockGraphics
{
    public class MeadowAppClock : App<F7Micro, MeadowAppTest>
    {
        Color WatchBackgroundColor = Color.White;

        St7789 st7789;
        GraphicsLibrary graphics;
        int hour, minute, tick;

        public MeadowAppClock()
        {

            var config = new SpiClockConfiguration(6000, SpiClockConfiguration.Mode.Mode3);
            
            st7789 = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );

            graphics = new GraphicsLibrary(st7789);
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            for (int i = 1; i < 2; i++)
            {
                DrawShapes(250, 250);
                DrawTexts(250, 250);
            }

            DrawClock(250, 250);
        }

        Color RandomColor()
        {
            Random rand = new Random();
            return Color.FromRgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        void DrawShapes(int displayWidth, int displayHeight)
        {
            
            graphics.Clear(true);

            int radius = 10;
            int originX = displayWidth / 2;
            int originY = displayHeight / 2;
            for (int i = 1; i < 5; i++)
            {
                graphics.DrawCircle
                (
                    centerX: originX,
                    centerY: originY,
                    radius: radius,
                    color: this.RandomColor()
                );
                graphics.Show();
                radius += 30;
            }

            int sideLength = 30;
            for (int i = 1; i < 5; i++)
            {
                graphics.DrawRectangle
                (
                    xLeft: (displayWidth - sideLength) / 2,
                    yTop: (displayHeight - sideLength) / 2,
                    width: sideLength,
                    height: sideLength,
                    color: this.RandomColor()
                );
                graphics.Show();
                sideLength += 60;
            }

            graphics.DrawLine(0, originY, displayWidth, originY, this.RandomColor());
            graphics.DrawLine(originX, 0, originX, displayHeight, this.RandomColor());
            graphics.DrawLine(0, 0, displayWidth, displayHeight, this.RandomColor());
            graphics.DrawLine(0, displayHeight, displayWidth, 0, this.RandomColor());
            graphics.Show();

            Thread.Sleep(5000);
        }

        void DrawTexts(int displayWidth, int displayHeight)
        {
            graphics.Clear(true);

            int indent = 20;
            int spacing = 20;
            int y = 5;

            graphics.CurrentFont = new Font12x16();
            graphics.DrawText(indent, y, "Meadow F7 SPI ST7789!!");
            graphics.DrawText(indent, y += spacing, "Red", Color.Red);
            graphics.DrawText(indent, y += spacing, "Purple", Color.Purple);
            graphics.DrawText(indent, y += spacing, "BlueViolet", Color.BlueViolet);
            graphics.DrawText(indent, y += spacing, "Blue", Color.Blue);
            graphics.DrawText(indent, y += spacing, "Cyan", Color.Cyan);
            graphics.DrawText(indent, y += spacing, "LawnGreen", Color.LawnGreen);
            graphics.DrawText(indent, y += spacing, "GreenYellow", Color.GreenYellow);
            graphics.DrawText(indent, y += spacing, "Yellow", Color.Yellow);
            graphics.DrawText(indent, y += spacing, "Orange", Color.Orange);
            graphics.DrawText(indent, y += spacing, "Brown", Color.Brown);
            graphics.Show();

            Thread.Sleep(5000);
        }

        void DrawClock(int displayWidth, int displayHeight)
        {
            graphics.Clear(true);

            hour = 8;
            minute = 54;
            DrawWatchFace(displayWidth, displayHeight);

            while (true)
            {
                tick++;
                Thread.Sleep(1000);
                UpdateClock(displayWidth, displayHeight, second: tick % 60);
            }
        }

        void DrawWatchFace(int displayWidth, int displayHeight)
        {
            graphics.Clear();
            int hour = 12;
            int xCenter = displayWidth / 2;
            int yCenter = displayHeight / 2;
            int x, y;
            graphics.DrawRectangle(0, 0, displayWidth, displayHeight, Color.White);
            graphics.DrawRectangle(5, 5, displayWidth - 10, displayHeight - 10, Color.White);
            graphics.CurrentFont = new Font12x20();
            graphics.DrawCircle(xCenter, yCenter, 100, WatchBackgroundColor, true);
            for (int i = 0; i < 60; i++)
            {
                x = (int)(xCenter + 80 * Math.Sin(i * Math.PI / 30));
                y = (int)(yCenter - 80 * Math.Cos(i * Math.PI / 30));

                if (i % 5 == 0)
                {
                    graphics.DrawText(
                        hour > 9 ? x - 10 : x - 5, y - 5, hour.ToString(), Color.Black);

                    if (hour == 12) hour = 1; else hour++;
                }
            }
            graphics.Show();
        }

        void UpdateClock(int displayWidth, int displayHeight, int second = 0)
        {
            int xCenter = displayWidth / 2;
            int yCenter = displayHeight / 2;
            int x, y, xT, yT;
            if (second == 0)
            {
                minute++;
                if (minute == 60)
                {
                    minute = 0;
                    hour++;
                    if (hour == 12)
                    {
                        hour = 0;
                    }
                }
            }

            //remove previous hour
            int previousHour = (hour - 1) < -1 ? 11 : (hour - 1);
            x = (int)(xCenter + 43 * System.Math.Sin(previousHour * System.Math.PI / 6));
            y = (int)(yCenter - 43 * System.Math.Cos(previousHour * System.Math.PI / 6));
            xT = (int)(xCenter + 3 * System.Math.Sin((previousHour - 3) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((previousHour - 3) * System.Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);

            xT = (int)(xCenter + 3 * System.Math.Sin((previousHour + 3) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((previousHour + 3) * System.Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);

            //current hour
            x = (int)(xCenter + 43 * System.Math.Sin(hour * System.Math.PI / 6));
            y = (int)(yCenter - 43 * System.Math.Cos(hour * System.Math.PI / 6));
            xT = (int)(xCenter + 3 * System.Math.Sin((hour - 3) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((hour - 3) * System.Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);

            xT = (int)(xCenter + 3 * System.Math.Sin((hour + 3) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((hour + 3) * System.Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);

            //remove previous minute
            int previousMinute = minute - 1 < -1 ? 59 : (minute - 1);
            x = (int)(xCenter + 55 * System.Math.Sin(previousMinute * System.Math.PI / 30));
            y = (int)(yCenter - 55 * System.Math.Cos(previousMinute * System.Math.PI / 30));
            xT = (int)(xCenter + 3 * System.Math.Sin((previousMinute - 15) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((previousMinute - 15) * System.Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);

            xT = (int)(xCenter + 3 * System.Math.Sin((previousMinute + 15) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((previousMinute + 15) * System.Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);

            //current minute
            x = (int)(xCenter + 55 * System.Math.Sin(minute * System.Math.PI / 30));
            y = (int)(yCenter - 55 * System.Math.Cos(minute * System.Math.PI / 30));
            xT = (int)(xCenter + 3 * System.Math.Sin((minute - 15) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((minute - 15) * System.Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);

            xT = (int)(xCenter + 3 * System.Math.Sin((minute + 15) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((minute + 15) * System.Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);

            //remove previous second
            int previousSecond = second - 1 < -1 ? 59 : (second - 1);
            x = (int)(xCenter + 70 * System.Math.Sin(previousSecond * System.Math.PI / 30));
            y = (int)(yCenter - 70 * System.Math.Cos(previousSecond * System.Math.PI / 30));
            graphics.DrawLine(xCenter, yCenter, x, y, WatchBackgroundColor);

            //current second
            x = (int)(xCenter + 70 * System.Math.Sin(second * System.Math.PI / 30));
            y = (int)(yCenter - 70 * System.Math.Cos(second * System.Math.PI / 30));
            graphics.DrawLine(xCenter, yCenter, x, y, Color.Red);

            graphics.Show();
        }
    }
}


