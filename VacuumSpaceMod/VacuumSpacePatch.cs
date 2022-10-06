using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace VacuumSpaceMod
{


    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    public static class BuildingComplete_OnSpawn_Patch
    {

        public static void Postfix(BuildingComplete __instance)
        {
            GameObject go = __instance.gameObject;
            if (__instance.name == "VacuumSpaceModComplete" )
            {
                Vector3 pos = go.transform.position;
                PrimaryElement element = go.GetComponent<PrimaryElement>();
                float temperature = element.Temperature;
                int cell = Grid.PosToCell(pos);

                //替换成石块
                SimMessages.ReplaceAndDisplaceElement(cell, element.ElementID,
                    null, 50f, temperature, byte.MaxValue, 0, -1); // spawn Natural Block
                                                                   //猜太空背景为
                replaceBuilding(__instance.name, cell);


                go.DeleteObject(); // remove Natural Tile
            }
        }
        /**
         * 使用模板来替换建筑.模板名放在模板目录下.
         */
        public static void replaceBuilding(string templateName, int cell)
        {
            TemplateContainer template = TemplateCache.GetTemplate(templateName);
            TemplateLoader.Stamp(template, Grid.CellToPos(cell), delegate
            {
                Console.WriteLine("替换打印了...");
            });
        }
    }
}