﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

//https://www.hackster.io/wilderness-labs/working-with-graphics-on-a-st7789-display-using-meadow-e2295a

//https://www.hackster.io/wilderness-labs/working-with-graphics-on-a-st7789-display-using-meadow-e2295a

namespace MeadowClockGraphics
{

    public class LightGroup
    {
        public string name;
        public string groupId;
        public Color color;

        public LightGroup()
        {
        }
    }

    public class MeadowAppLights : App<F7Micro, MeadowAppTest>
    {
        St7789 st7789;
        GraphicsLibrary graphics;

        public MeadowAppLights()
        {

            var config = new SpiClockConfiguration(6000, SpiClockConfiguration.Mode.Mode3);

            st7789 = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240,
                height: 240
            );

            //var raw = LoadJson();
            //Console.WriteLine(raw);


            graphics = new GraphicsLibrary(st7789);
            //graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            graphics.Clear(true);
            for (var i = 0; i < 100; i++)
            {
                DrawTexts();
            }

        }

        public string LoadJson()
        {
            using (var reader = new StreamReader("meadow0/prog.json"))
            {
                string json = reader.ReadToEnd();
                //dynamic source = JsonConvert.DeserializeObject(json);
                return json;
             }
        }

        public ProgramManager Manager()
        {
            const manager = new ProgramManager();
            return manager;
        }

        Color RandomColor()
        {
            Random rand = new Random();
            return Color.FromRgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }


        void DrawTexts()
        {
  

            var groups = new Dictionary<int, LightGroup>()
            {
                { 1, new LightGroup() { name = "RED", color = RandomColor() } },
                { 2, new LightGroup() { name = "RfED", color = RandomColor() } },
                { 3, new LightGroup() { name = "REhhD", color = RandomColor() } },
                { 4, new LightGroup() { name = "RgED", color = RandomColor() } },
                { 5, new LightGroup() { name = "RdddED", color = RandomColor() } },
                { 6, new LightGroup() { name = "ReeeED", color = RandomColor() } },
            };


            graphics.CurrentFont = new Font12x16();


            int y = 20;
            foreach (var index in Enumerable.Range(1, 6))
            {
                var item = groups[index];
                //Console.WriteLine($"LightGroup {index} is {item.name} {item.color}");
                graphics.DrawText(40, y, item.name, item.color);
                y += 30;
            }

            graphics.Show();

            Thread.Sleep(50);
        }

    }
}

