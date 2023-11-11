using KSerialization;
using UnityEngine;

namespace ForceMuate
{

    [SerializationConfig(MemberSerialization.OptIn)]
    class ForceMutateButton : KMonoBehaviour, ISaveLoadable
    {
        [Serialize] public bool isMutating = false;

        protected override void OnPrefabInit()
        {
            Subscribe((int)GameHashes.RefreshUserMenu, (object data) => OnRefreshUserMenu());
            Subscribe((int)GameHashes.CopySettings, (object data) => OnCopySettings(data));
        }

        private void OnRefreshUserMenu()
        {
            if (isMutating)
            {
                string iconName = "action_building_disabled";
                string text = BUTTONS.DISABLELEARN.NAME;
                void on_click()
                {
                    isMutating = false;
                    UpdateState();
                }
                string tooltipText = BUTTONS.DISABLELEARN.TOOLTIP;
                //STRINGS.UI.UISIDESCREENS.PLANTERSIDESCREEN.MUTATIONS_HEADER

                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
                return;
            }
            else
            {
                string iconName = "action_building_disabled";
                string text = BUTTONS.ENABLELEARN.NAME;
                void on_click()
                {
                    isMutating = true;
                    UpdateState();
                }
                string tooltipText = BUTTONS.ENABLELEARN.TOOLTIP;
                // Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
                return;
            }
        }
        public static void OnRefreshUserMenuPlanB(MutantPlant mutant)
        {

            if (mutant != null)
            {
                string iconName = "action_building_disabled";
                string text = "+ForceMutate";
                string tooltipText = BUTTONS.ENABLELEARN.TOOLTIP;
                void on_click()
                {
                    mutant.Mutate();
                    mutant.Analyze();
                    PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(mutant.SubSpeciesID);
                    DetailsScreen.Instance.Trigger((int)GameHashes.UIRefreshData, null);
                }
                // Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
                Game.Instance.userMenu.AddButton(mutant.gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
            }
        }

        private void OnCopySettings(object sourceObj)
        {
            if ((sourceObj as GameObject).GetComponent<ForceMutateButton>() is var comp)
                isMutating = comp.isMutating;
        }

        protected void UpdateState()
        {
            var plant = gameObject;
            MutantPlant mp = plant.GetComponent<MutantPlant>();
            if (mp != null)
            {
                //MutantPlant mutantPlantGa = gameObject.AddOrGet<MutantPlant>();
                // string name2 = Db.Get().PlantMutations.GetRandomMutation(gameObject.PrefabID().Name).Id;
                mp.Mutate();
                mp.Analyze();
            }

            //Debug.LogWarning($"UpdateState  _---> ssssssss.......{plant} ..{mp}");

            //暂时不做any
        }
    }
}


