using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using STRINGS;

namespace PacuMorphs
{
    public class BabyBetaPacuConfig : IEntityConfig
    {
        public const string ID = "PacuBetaBaby";

        public static string NAME = UI.FormatAsLink("Beta Fry", BetaPacuConfig.ID.ToUpper());
        public static string DESCRIPTION = $"A wriggly little Beta Fry.\n\nIn time it will mature into an adult {UI.FormatAsLink("Beta Fish", BetaPacuConfig.ID.ToUpper())}.";

        public GameObject CreatePrefab()
        {
            GameObject pacu = BetaPacuConfig.CreatePacu(ID,
                NAME,
                DESCRIPTION,
                "custombaby_pacu_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(pacu, BetaPacuConfig.ID, null);
            return pacu;
        }

        public string GetDlcId()
        {
            return DlcManager.VANILLA_ID;
        }
        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject prefab)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}