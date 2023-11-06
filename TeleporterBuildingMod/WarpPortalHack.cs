using TUNING;
using UnityEngine;

namespace TeleporterBuildingMod
{
    public class WarpPortalHack : IBuildingConfig
    {


        public GameObject CreatePrefab()
        {
            GameObject obj = EntityTemplates.CreatePlacedEntity("WarpPortal",
                STRINGS.BUILDINGS.PREFABS.WARPPORTAL.NAME,
                STRINGS.BUILDINGS.PREFABS.WARPPORTAL.DESC,
                2000f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0,
                noise: NOISE_POLLUTION.NOISY.TIER0,
                anim: Assets.GetAnim("warp_portal_sender_kanim"),
                initialAnim: "idle", sceneLayer: Grid.SceneLayer.Building, width: 3, height: 3);
            obj.AddTag(GameTags.NotRoomAssignable);
            obj.AddTag(GameTags.WarpTech);
            obj.AddTag(GameTags.Gravitas);
            PrimaryElement component = obj.GetComponent<PrimaryElement>();
            component.SetElement(SimHashes.Unobtanium);
            component.Temperature = 294.15f;
            obj.AddOrGet<Operational>();
            obj.AddOrGet<Notifier>();
            obj.AddOrGet<WarpPortal>();
            obj.AddOrGet<LoreBearer>();
            obj.AddOrGet<LoopingSounds>();
            obj.AddOrGet<Ownable>().tintWhenUnassigned = false;
            obj.AddOrGet<Prioritizable>();
            KBatchedAnimController kBatchedAnimController = obj.AddOrGet<KBatchedAnimController>();
            kBatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
            kBatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;

            return obj;
        }

        public void OnPrefabInit(GameObject inst)
        {
            inst.GetComponent<WarpPortal>().workLayer = Grid.SceneLayer.Building;
            inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.WarpPortal.Id;
            inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
            inst.GetComponent<Deconstructable>();
        }

        public void OnSpawn(GameObject inst)
        {
        }
        public override BuildingDef CreateBuildingDef()
        {
            string id = "WarpPortal1";

            int width = 3;
            int height = 3;
            string anim = "warp_portal_sender_new_kanim";//动画有bug,使用修改后的动画
                                                         //  string anim = "temporal_tear_opener_kanim";
            int hitpoints = 100;
            float construction_time = 120f;
            float[] tais1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
            EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
            string[] raw_METALS = MATERIALS.RAW_METALS;
            float melting_point = 2400f;
            BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tais1, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
            buildingDef.DefaultAnimState = "off";
            buildingDef.Entombable = false;
            buildingDef.Invincible = true;

            buildingDef.ShowInBuildMenu = true;
            //buildingDef.GetBuildingCell();

            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.GetComponent<Deconstructable>().allowDeconstruction = true;

        }
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            PrimaryElement component = go.GetComponent<PrimaryElement>();
            component.SetElement(SimHashes.Copper, true);
            component.Temperature = 294.15f;
            //go.get


            // TemporalTearOpener.Def def;= go.AddOrGetDef<WarpPortal.Def>();

        }

    }



}
