using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STRINGS;
namespace PipMorphs
{
    class STRINGS
    {
        public class CREATURES
        {
            public class SPECIES
            {
                public class SQUIRREL
                {
                    public static class VARIANT_AUTUMN
                    {
                        public static LocString NAME = UI.FormatAsLink("Autumn Pip", "SQUIRRELAUTUMN");

                        public static LocString DESC = string.Concat(new string[]
                        {
                            "The leaves on their bodies seem to start falling at anytime."
                        });

                        public static LocString EGG_NAME = UI.FormatAsLink("Autumn Pip Egg", "SQUIRRELAUTUMN");

                        public static class BABY
                        {
                            public static LocString NAME = UI.FormatAsLink("Autumn Pipsqueak", "SQUIRRELAUTUMN");

                            public static LocString DESC = "The leaves on their bodies resemble their cousins.";
                        }
                    }
                    public static class VARIANT_SPRING
                    {
                        public static LocString NAME = UI.FormatAsLink("Spring Pip", "SQUIRRELSPRING");

                        public static LocString DESC = string.Concat(new string[]
                        {
                            "The leaves on their bodies have bright colors."
                        });

                        public static LocString EGG_NAME = UI.FormatAsLink("Spring Pip Egg", "SQUIRRELSPRING");

                        public static class BABY
                        {
                            public static LocString NAME = UI.FormatAsLink("Spring Pipsqueak", "SQUIRRELSPRING");

                            public static LocString DESC = "The leaves on their bodies resemble their cousins.";
                        }
                    }
                    public static class VARIANT_WINTER
                    {
                        public static LocString NAME = UI.FormatAsLink("Winter Pip", "SQUIRRELWINTER");

                        public static LocString DESC = string.Concat(new string[]
                        {
                            "They prefer cold environments."
                        });

                        public static LocString EGG_NAME = UI.FormatAsLink("Winter Pip Egg", "SQUIRRELWINTER");

                        public static class BABY
                        {
                            public static LocString NAME = UI.FormatAsLink("Winter Pipsqueak", "SQUIRRELWINTER");

                            public static LocString DESC = "The leaves on their bodies resemble their cousins.";
                        }
                    }
                }
    
            }
        }

    }
}
