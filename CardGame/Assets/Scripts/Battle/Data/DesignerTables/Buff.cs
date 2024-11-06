using System.Collections.Generic;

namespace MyGame.Data.DesignerTables
{
    public class Buff
    {
        public static Dictionary<string, BuffModel> Data = new Dictionary<string, BuffModel>
        {
            {"gongjitisheng",new BuffModel("gongjitisheng",0,1, new string[]{},0,HeroProperty.Default, HeroControlMod.Default,
                "",new object[]{},
                "",new object[]{},
                "",new object[]{},
                "",new object[]{},
                "",new object[]{},
                "",new object[]{},
                "",new object[]{},
                "",new object[]{}
                )}
        };
    }
}