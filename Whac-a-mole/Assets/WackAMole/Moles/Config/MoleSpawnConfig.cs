using System;
using System.Collections.Generic;
using UnityEngine;
using WackAMole.Moles.Behaviours.Factory;

namespace WackAMole.Moles.Config
{
    [CreateAssetMenu(fileName = "MoleSpawnConfig", menuName = "WackAMole/MoleSpawnConfig", order = 0)]
    public class MoleSpawnConfig : ScriptableObject
    {
        [Serializable]
        public class MoleSpawnRate
        {
            public MoleBehaviorFactory.MoleBehaviorType MoleBehavior;
            [Range(0, 100)] public float SpawnRate;
        }

        public List<MoleSpawnRate> MoleSpawnRates;
    }
}