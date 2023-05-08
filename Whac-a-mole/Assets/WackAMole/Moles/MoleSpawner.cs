using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using WackAMole.Controllers;
using WackAMole.Moles.Behaviours;
using WackAMole.Moles.Behaviours.Factory;
using WackAMole.Moles.Config;

namespace WackAMole.Moles
{
    public class MoleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject molePrefab;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private float minSpawnInterval = 1f;
        [SerializeField] private float maxSpawnInterval = 3f;
        [SerializeField] private float wackedKillAnimationDelay = 1f;
        [SerializeField] private MoleSpawnConfig moleSpawnConfig;
        
        private List<Mole> molePool;
        private HashSet<int> occupiedSpawnPoints;
        private MoleGameController moleGameController;
        public void Init(MoleGameController moleGameController)
        {
            this.moleGameController = moleGameController;
            ResetSpawner();
            CreateMolePool();

            moleGameController.OnGameStarted += StartSpawning;
            moleGameController.OnGameEnded += HandleOnGameEnded;
        }

        public void ResetSpawner()
        {
            foreach (var mole in molePool)
            {
                moleGameController.UnRegisterMole(mole);
                Destroy(mole.gameObject);
            }

            molePool.Clear();
        }

        private void Start()
        {
            molePool = new List<Mole>(poolSize);
            occupiedSpawnPoints = new HashSet<int>();
        }

        private void OnDestroy()
        {
            if(moleGameController == null) return;
            
            moleGameController.OnGameStarted -= StartSpawning;
            moleGameController.OnGameEnded -= HandleOnGameEnded;
        }

        private void CreateMolePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var moleObject = Instantiate(molePrefab);
                var mole = moleObject.GetComponent<Mole>();
                IMoleBehavior moleBehavior = GetRandomMoleBehavior();
                mole.Initialize(moleBehavior);
                moleObject.SetActive(false);
                moleGameController.RegisterMole(mole);
                molePool.Add(mole);
            }
        }

        private void StartSpawning() => StartCoroutine(SpawnMoles());

        private void HandleOnGameEnded(int endScore)
        {
            foreach (var mole in molePool)
            {
                moleGameController.UnRegisterMole(mole);
            }

            StopAllCoroutines();
        }
        
        private IEnumerator SpawnMoles()
        {
            while (true)
            {
                var spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
                yield return new WaitForSeconds(spawnInterval);

                var spawnIndex = GetRandomAvailableSpawnPointIndex();
                if (spawnIndex != -1)
                {
                    SpawnMole(spawnIndex);
                }
            }
        }

        private int GetRandomAvailableSpawnPointIndex()
        {
            var availableSpawnPoints = new List<int>();
            for (var i = 0; i < spawnPoints.Count; i++)
            {
                if (!occupiedSpawnPoints.Contains(i))
                {
                    availableSpawnPoints.Add(i);
                }
            }

            if (availableSpawnPoints.Count == 0)
            {
                return -1;
            }

            var randomIndex = Random.Range(0, availableSpawnPoints.Count);
            return availableSpawnPoints[randomIndex];
        }

        private void SpawnMole(int spawnIndex)
        {
            var mole = GetAvailableMole();
            if (mole != null)
            {
                Transform spawnPoint = spawnPoints[spawnIndex];
                mole.transform.position = spawnPoint.position;
                mole.transform.rotation = spawnPoint.rotation;
                mole.gameObject.SetActive(true);
                mole.Show();

                occupiedSpawnPoints.Add(spawnIndex);
                StartCoroutine(HandleMoleDuration(mole, spawnIndex));
            }
        }
        
        private IMoleBehavior GetRandomMoleBehavior()
        {
            var totalRate = moleSpawnConfig.MoleSpawnRates.Sum(spawnRate => spawnRate.SpawnRate);

            var randomValue = Random.Range(0, totalRate);
            var accumulatedRate = 0f;

            foreach (var spawnRate in moleSpawnConfig.MoleSpawnRates)
            {
                accumulatedRate += spawnRate.SpawnRate;
                if (randomValue <= accumulatedRate)
                {
                    return MoleBehaviorFactory.CreateMoleBehavior(spawnRate.MoleBehavior);
                }
            }

            return new NormalMoleBehavior();
        }

        private Mole GetAvailableMole() => molePool.FirstOrDefault(mole => !mole.gameObject.activeInHierarchy);

        private IEnumerator HandleMoleDuration(Mole mole, int spawnIndex)
        {
            yield return new WaitForSeconds(mole.MoleBehavior.GetDisplayDuration());
            //Wait before returning it to the pool to do a proper animation.
            yield return new WaitForSeconds(wackedKillAnimationDelay);
            
            mole.gameObject.SetActive(false);
            occupiedSpawnPoints.Remove(spawnIndex);
        }
    }
}
