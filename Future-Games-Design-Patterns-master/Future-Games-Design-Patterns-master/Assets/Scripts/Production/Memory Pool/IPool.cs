using System;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tools
{
    public interface IPool<T>
    {
        T Rent(bool returnActive);
    }

    #region ComponentPool
    public class ComponentPool<T> : IDisposable, IPool<T> where T : Component
    {
        private bool m_Disposed;
        private readonly uint m_ExpandBy;
        private readonly Stack<T> m_Objects;
        private readonly List<T> m_Created;
        private readonly T m_Prefab;
        private readonly Transform m_Parent;

        public ComponentPool(uint initSize, uint expandBy, T prefab, Transform parent)
        {
            m_ExpandBy = expandBy;
            m_Prefab = prefab;
            m_Parent = parent;
            m_Objects = new Stack<T>();
            m_Created = new List<T>();
            Expand((uint)Mathf.Max(1, initSize));
        }

        private void Expand(uint expandBy)
        {
            for (int i = 0; i < expandBy; i++)
            {
                T instance = Object.Instantiate<T>(m_Prefab, m_Parent);

                instance.gameObject.AddComponent<EmittOnDisable>().OnDisableGameObject += UnRent;
            }
        }

        private void UnRent(GameObject gameObject)
        {
            if (m_Disposed == false)
            {
                m_Objects.Push(gameObject.GetComponent<T>());
            }
        }

        public void Clean()
        {
            foreach (T component in m_Created)
            {
                if (component != null)
                {
                    component.GetComponent<EmittOnDisable>().OnDisableGameObject -= UnRent;
                    Object.Destroy(component.gameObject);
                }
            }
            m_Created.Clear();
            m_Objects.Clear();
        }

        public void Dispose()
        {
            m_Disposed = true;
            Clean();
        }

        public T Rent(bool returnActive)
        {
            if (m_Objects.Count == 0)
            {
                Expand(m_ExpandBy);
            }

            T instance = m_Objects.Pop();
            return instance;
        }
    }
    #endregion ComponentPool

    public class GameObjectPool : IPool<GameObject>, IDisposable
     {
         private bool m_IsDisposed;
         private uint m_ExpandBy;
         private readonly GameObject m_Prefab;
         private readonly Transform m_Parent;
         
         readonly Stack<GameObject> m_Objects = new Stack<GameObject>();
         
         readonly List<GameObject> m_Created = new List<GameObject>();

         public GameObjectPool(uint initSize,  GameObject prefab, uint expandBy = 1, Transform parent  = null)
         {
             m_ExpandBy  = (uint)Mathf.Max(1,expandBy);
             m_Prefab = prefab;
             m_Parent = parent;
             m_Prefab.SetActive(false);
             Expand((uint)Mathf.Max(1, initSize));
         }

         private void Expand(uint amount)
         {
             for (int i = 0; i < amount; i++)
             {
                 GameObject instance = Object.Instantiate(m_Prefab, m_Parent);
                 EmittOnDisable emittOnDisable = instance.AddComponent<EmittOnDisable>();
                 emittOnDisable.OnDisableGameObject += UnRent;
                 m_Objects.Push(instance);
                 m_Created.Add(instance);
             }
         }

         public void Clear()
         {
             foreach (GameObject gameObject in m_Created )
             {
                 if (gameObject != null)
                 {
                     gameObject.GetComponent<EmittOnDisable>().OnDisableGameObject -= UnRent;
                     Object.Destroy(gameObject);    
                 }
             }
             m_Objects.Clear();
             m_Created.Clear();
         }

         public  GameObject Rent(bool returnActive)
         {
             if (m_IsDisposed)
             {
                 return null;
             }                
             if (m_Objects.Count == 0)
             {
                 Expand(m_ExpandBy);
             }
              
             GameObject instance = m_Objects.Pop();

             instance.SetActive(returnActive);

             return instance;
         }
         
         private void UnRent(GameObject gameObject)
         {
             if (m_IsDisposed == false)
             {
                 m_Objects.Push(gameObject);
             }
         }

         public void Dispose()
         {
             m_IsDisposed = true;
             Clear();
         }
     }
}