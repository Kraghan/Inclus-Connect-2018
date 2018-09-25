using System;
using UnityEngine;

namespace Scripting.GameManagers
{
    [Serializable]
    internal class Pool
    {
        [SerializeField]
        // Size of the pool
        internal int poolSize;

        [SerializeField]
        // Prefab to pool
        UnityEngine.Object prefab;

        // Index in the pool
        int poolIndex;

        // Pool container
        GameObject[] pool;

        // Container
        internal void Initialize(GameObject p_root)
        {
            poolIndex = 0;

            pool = new GameObject[poolSize];
            for(int i = 0 ; i < poolSize ; ++i)
            {
                Debug.Log("Initialized one fx");
                pool[i] = GameObject.Instantiate(prefab) as GameObject;
                pool[i].transform.SetParent(p_root.transform);
                pool[i].SetActive(false);
            }
        }

        /// Return next available fx, or null if any
        internal GameObject GetNext()
        {
            int index = poolIndex;

            do
            {
                if (pool[index].activeSelf == false)
                {
                    poolIndex = index;
                    return pool[index];
                }
            }while(++index % pool.Length != poolIndex);

            return null;
        }
    }

    internal enum EFXType
    {
        Dust,

        Count
    }

    internal class FXManager : MonoBehaviour
    {
        /// FX pools
        [SerializeField]
        Pool[] m_pools = new Pool[(int)EFXType.Count];

        /// Awake
        void Awake()
        {
            foreach(Pool p_pool in m_pools)
            {
                p_pool.Initialize(gameObject);
            }
        }

        /// Spawns an FX
        internal GameObject SpawnFX(EFXType p_type, Vector3 p_position)
        {
            int poolIndex = (int)p_type;
            
            if (poolIndex > m_pools.Length)
            {
                Debug.LogErrorFormat("FXManager.SpawnFX(): Type doesn't match any pool.");
                return null;
            }

            Pool pool = m_pools[poolIndex];
            GameObject fx = pool.GetNext();

            if (fx != null)
            {
                fx.transform.position = p_position;
                fx.gameObject.SetActive(true);
                return fx;
            }

            return null;
        }
    }
}