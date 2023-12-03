using HarmonyLib;

namespace ForceMuate
{
    [HarmonyPatch(typeof(GlobalAssets), "OnPrefabInit")]
    public class BUILDING
    {
        static void Postfix() => LocString.CreateLocStringKeys(typeof(BUILDING));

        public class STATUSITEMS
        {
            public class REQUIRESATTRIBUTERANGE
            {
                public static LocString NAME = "Attribute-Required Operation";
                public static LocString TOOLTIP = "Only duplicants with a base Science Attribute from {Attributes} can learn from this building.";
            }
        }
    }

    public class DUPLICANTS
    {
        public class STATUSITEMS
        {
            public class LEARNING
            {
                public static LocString NAME = "Learning";
                public static LocString TOOLTIP = "This Duplicant is intently studying to improve their Science Attribute.";
            }
        }

        public class CHORES
        {
            public class PRECONDITIONS
            {
                public class REQUIRES_ATTRIBUTE_RANGE
                {
                    public static LocString DESCRIPTION = "Science outside range";
                }
            }
        }
    }

    public class BUTTONS
    {
        public class DISABLELEARN
        {
            public static LocString NAME = "ForceMutate";
            public static LocString TOOLTIP = "add mutations";
        }

        public class ENABLELEARN
        {
            public static LocString NAME = "ForceMutate+";
            public static LocString TOOLTIP = "refresh add mutations.";
        }
    }
}
