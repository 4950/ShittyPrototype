﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Managers;

namespace EntityFramework
{
    /// <summary>
    /// Lightweight object for grouping sets on components together
    /// </summary>
    public class Entity
    {
        #region Variables
        private EntityManager entityManager;
        private Guid guid;
        #endregion

        #region Properties
        public BitVector ComponentBits
        {
            get
            {
                return entityManager.GetComponentBitVector(Id);
            }
        }

        public BitVector SystemBits
        {
            get
            {
                return entityManager.GetSystemBitVector(Id);
            }
        }

        public uint Id
        {
            get;
            private set;
        }

        public string Tag
        {
            get
            {
                return entityManager.TagManager.GetTag(Id);
            }
            set
            {
                if (value == "")
                {
                    entityManager.TagManager.RemoveTag(Id);
                }
                else
                {
                    entityManager.TagManager.AddTag(Id, value);    
                }
                
            }
        }
       
        public IEnumerable<Component> Components
        {
            get
            {
                return entityManager.GetComponents(Id);
            }
        }

        public IEnumerable<string> Groups
        {
            get
            {
                return entityManager.GroupManager.GetGroups(Id);
            }
        }
        #endregion

        #region Constructors
        internal Entity(EntityManager em)
        {
            guid = Guid.NewGuid();
            Id = (uint)guid.GetHashCode();
            entityManager = em;
        }
        #endregion

        #region Functions
        public bool HasComponent(Type type)
        {
            return entityManager.HasComponent(Id, type);
        }

        public bool HasComponent<T>()
        {
            return HasComponent(typeof (T));
        }

        public T GetComponent<T>() where T : Component
        {
            return (T) entityManager.GetComponent(Id, typeof (T));
        }

        public void AddComponent(Component comp)
        {
            entityManager.AddComponent(Id, comp);
        }

        public void RemoveComponent<T>()
        {
            entityManager.RemoveComponent(Id, typeof(T));
        }

        public void AddToGroup(string group)
        {
            entityManager.GroupManager.AddToGroup(Id, group);
        }

        public void RemoveFromGroup(string group)
        {
            entityManager.GroupManager.RemoveFromGroup(Id, group);
        }

        public void RemoveTag()
        {
            Tag = "";
        }

        /// <summary>
        /// Updates the entity in the world it belongs to.
        /// This should be called after adding components to the 
        /// entity so that it can be distributed to its proper processing systems.
        /// </summary>
        public void Refresh()
        {
            entityManager.Refresh(Id);
        }
        #endregion
    }
}