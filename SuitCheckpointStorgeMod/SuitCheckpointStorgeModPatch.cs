using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;
using static STRINGS.INPUT_BINDINGS;

namespace SuitCheckpointStorgeMod
{

    [HarmonyPatch(typeof(SuitMarkerConfig), "DoPostConfigureComplete")]
    public class SuitCheckpointStorgeModPatch
    {
        public static List<Tag> SUIT = new List<Tag>
        {
            GameTags.AtmoSuit,
           // GameTags.Suit,
        };
        public static void Postfix(GameObject go)
        {
            Prioritizable.AddRef(go);
            Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
            storage.showInUI = true;
            storage.allowItemRemoval = true;
            storage.capacityKg = 5f;
            storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
            storage.showCapacityStatusItem = true;
            storage.showCapacityAsMainStatus = true;
            ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
            manualDeliveryKG.SetStorage(storage);
            manualDeliveryKG.RequestedItemTag = GameTags.AtmoSuit;
            manualDeliveryKG.capacity = 5f;
            manualDeliveryKG.refillMass = 5f;
            manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
            /*
            Storage storage = go.AddOrGet<Storage>();
            storage.showInUI = false;
            storage.allowItemRemoval = true;
            storage.showDescriptor = true;

            //List<Storage.StoredItemModifier> list = this.defaultStoredItemModifers;
            storage.storageFilters = SUIT;// STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
         
            // storage.
            storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
            storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
            storage.showCapacityStatusItem = true;
            storage.showCapacityAsMainStatus = true;
            storage.capacityKg = 5;//200kg一件 气压服,有bug,物品按个数计算

            storage.allowSettingOnlyFetchMarkedItems = false;//仅限打扫 全部.

            storage.allowClearable=true;

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
            var stk= go.AddOrGet<StorageLocker>();
          

            Prioritizable.AddRef(go);
            Storage storageB = BuildingTemplates.CreateDefaultStorage(go, false);
            storageB.showInUI = true;
            storageB.capacityKg = 50f;
            storageB.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
              */
            // storage.storageFilters =;

            //root.Subscribe(-OnStorageChange, OnStorageChanged);
            //root.Subscribe(-UserSettingsChanged, OnUserSettingsChanged);
            //var filterable = go.gameObject.FindOrAdd<TreeFilterable>();
            //TreeFilterable treeFilterable = filterable;
            //treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)
            //    Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>
            //    (OnFilterChanged));
            //storage = root.GetComponent<Storage>();
            //storage.Subscribe(GameHashes.OnlyFetchMarkedItemsSettingChanged, OnOnlyFetchMarkedItemsSettingChanged);
            //storage.Subscribe(GameHashes .FunctionalChanged, OnFunctionalChanged);

            //stk.filteredStorage.FilterChanged();
            //var sff= new FilteredStorage(null, null, this, use_logic_meter, fetch_chore_type);
            //sff.SetMeter
            Prioritizable.AddRef(go);//优先级
            go.AddOrGet<UserNameable>();

            go.AddOrGetDef<StorageController.Def>();
        }
    }
}
