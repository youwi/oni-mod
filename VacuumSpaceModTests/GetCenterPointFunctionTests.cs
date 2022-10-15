using Delaunay.Geo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VacuumSpaceMod.DebugViewClassPath.Tests
{
    [TestClass()]
    public class GetCenterPointFunctionTests
    {
        public object Vertices { get; private set; }

        [TestMethod()]
        public void GetCenterOfGravityPointTest()
        {
            List<Vector2> mPoints = new List<Vector2>() {
            new Vector2(135.18809509277344f, 205.34129333496094f),
            new Vector2(139.27040100097656f, 226.0731964111328f),
            new Vector2(133.2449951171875f, 236.0638885498047f),
            new Vector2(118.23139953613281f, 242.6313934326172f),
            new Vector2(102.55179595947266f, 238.3217010498047f),
            new Vector2(94.06069946289062f, 225.5251007080078f),
            new Vector2(96.63819885253906f, 209.00839233398438f),
            new Vector2(103.17119598388672f, 202.0290985107422f),
            new Vector2(114.10679626464844f, 198.60479736328125f),
             };

            object value = PolyUT.GetCenterOfGravityPoint(mPoints);
            Console.WriteLine(value);
            Console.WriteLine(PolyUT.GetMaxY(mPoints));

        }
        [TestMethod()]
        public void ClipTest()
        {
            Polygon poly = new Polygon();
            poly.Add(new Vector2(135.18809509277344f, 205.34129333496094f));
            poly.Add(new Vector2(139.27040100097656f, 226.0731964111328f));
            poly.Add(new Vector2(133.2449951171875f, 236.0638885498047f));
            poly.Add(new Vector2(118.23139953613281f, 242.6313934326172f));
            poly.Add(new Vector2(102.55179595947266f, 238.3217010498047f));
            poly.Add(new Vector2(94.06069946289062f, 225.5251007080078f));
            poly.Add(new Vector2(96.63819885253906f, 209.00839233398438f));
            poly.Add(new Vector2(103.17119598388672f, 202.0290985107422f));
            poly.Add(new Vector2(114.10679626464844f, 198.60479736328125f));

          //  poly.Clip(poly,ClipperLib.ClipType.ctXor);

        }
        [TestMethod()]
        public void RandTest()
        {
            SeededRandom rnd = new SeededRandom(2123121);
            Console.WriteLine(rnd.RandomRange(2, 5));
        }

        [TestMethod()]
        public void buildGeyserTest()
        {
            // GeyserGenericConfig
            // 获取所有的泉
            FieldInfo[] fieldInfos = typeof(GeyserGenericConfig).GetFields();
            string outStr = "name: pure_geyser" +
              "info:\n" +
              "  size:\n" +
              "    X: 6\n" +
              "    Y: 7\n" +
              "  min:\n" +
              "    X: -2\n" +
              "    Y: -3\n" +
              "  area: 38\n" +
              "cells: []\n" +
              "buildings: []\n" +
              "pickupables: []\n" +
              "elementalOres: []\n" +
              "otherEntities:\n";
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo field = fieldInfos[i];
                string value = (string)field.GetValue(field.Name);
                if (value == "GeyserGeneric") continue;
                outStr += string.Format("\n" +
                  "- id: GeyserGeneric_{0}\n" +
                  "  element: Obsidian\n" +
                  "  temperature: 2000\n" +
                  "  units: 1\n" +
                  "  type: Other\n" +
                  "  location_x: {1}\n", value, i * 2 + 1);

            }
            Console.WriteLine(outStr);

        }
    }
}