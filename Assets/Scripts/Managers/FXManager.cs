using System;
using Scripting.FX;
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
                pool[i] = GameObject.Instantiate(prefab) as GameObject;
                pool[i].transform.SetParent(p_root.transform);
                pool[i].SetActive(false);
            }
        }

        /// Return next available fx, or null if any
        internal GameObject GetNext()
        {
            int index = poolIndex ;
            do
            {
                index = ++index % pool.Length;
                if (pool[index].activeSelf == false)
                {
                    poolIndex = index;
                    return pool[index];
                }
            }while(index != poolIndex);

            return null;
        }
    }

    internal enum EFXType
    {
        Dust,
        LandingSuccess,
        AttackConsequence,
        AttackSuccess,
        DefenseConsequence,
        DefenseSuccess,
        Shield,
        SimpleShield,
        Dash,
        Attack,
        Reward,

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
        internal GameObject SpawnFX(EFXType p_type, Vector3 p_position, GameObject p_targetToFollow = null)
        {
            //Debug.Log("Spawn FX !");
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

                Following f = fx.GetComponent<Following>();
                if (f != null)
                {
                    f.objectToFollow = p_targetToFollow;
                    f.offset = p_position - p_targetToFollow.transform.position;
                }

                return fx;
            }

            return null;
        }
    }
}