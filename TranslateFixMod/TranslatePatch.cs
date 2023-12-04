

using Klei.AI;
using static ModifierSet;
using static ResearchTypes;
using static STRINGS.ELEMENTS;

namespace TranslateFixMod
{
    public class TestTranslatePath
    {


        public static void Postfix()
        {
            var traits = Db.Get().traits;
            foreach (var trait in traits.resources)
            {
                LocString name = trait.Name;
                if (name.text == name.key.String) //怎么判断没有翻译? 源和译 都是英文那么算是没有翻译.
                {

                }

            }
        }
        public void testExample()
        {
         
            
            var ttt = STRINGS.DUPLICANTS.TRAITS.IRONGUT.NAME;
            var traitsActions =  TUNING.TRAITS.TRAIT_CREATORS;

            foreach (var trAction in traitsActions)
            {
               // trAction.
               // trait.name;
            }
            //ModifierSet.LoadTraits(); 核心代码是直接加载  //二次加载会怎样?
            TUNING.TRAITS.TRAIT_CREATORS.ForEach(delegate (System.Action action)
            {
                action();
            });
            //回调执行完成以后,直接在数据库中存在.

            //示例代码: 这里明显翻译没到位.
            Db.Get().CreateTrait("IronGut", STRINGS.DUPLICANTS.TRAITS.IRONGUT.NAME, STRINGS.DUPLICANTS.TRAITS.IRONGUT.DESC, null, true, null, false, true);
            // Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, disabled_chore_groups, positiveTrait, true);
            // var traits= ModifierSet.traits; //Db.Get().traits
            // var traitGroups = ModifierSet.traitGroups;Db.Get().traitGroups

            //直接调用:
        }
    }
}
