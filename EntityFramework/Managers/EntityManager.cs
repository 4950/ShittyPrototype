﻿using System;
using System.Collections.Generic;


namespace EntityFramework.Managers
{
    public class EntityManager
    {
        // Dictionary all the things!    
        private TagManager<uint> tagManager;
        private GroupManager<uint> groupManager; 
        private Dictionary<uint, Entity> entities;
        private Dictionary<uint, BitVector> componentBitVectors;
        private Dictionary<uint, BitVector> systemBitVectors;
        private Dictionary<uint, Dictionary<uint, Component>> entityComponents;
        private TypeMapper<Component> componentTypeMapper;
        private World world;
        

        public TagManager<uint> TagManager
        {
            get
            {
                return tagManager;
            }
        }

        public GroupManager<uint> GroupManager
        {
            get
            {
                return groupManager;
            }
        } 

        internal EntityManager(World world)
        {
            tagManager = new TagManager<uint>();
            groupManager = new GroupManager<uint>();
            entities = new Dictionary<uint, Entity>();
            entityComponents = new Dictionary<uint, Dictionary<uint, Component>>();         
            componentTypeMapper = Mappers.ComponentTypeMapper;
            componentBitVectors = new Dictionary<uint, BitVector>();
            systemBitVectors = new Dictionary<uint, BitVector>();
            this.world = world;
        }

        /// <summary>
        /// Creates a new Entity but does NOT add it to the world until the entitie's Refresh() method is called
        /// </summary>
        /// <returns>New Entity</returns>
        public Entity CreateEntity()
        {
            Entity e = new Entity(this);                        
            entities.Add(e.Id, e);
            return e;
        }

        public void Refresh(uint id)
        {          
            world.Refresh(entities[id]);
        }

        public void RemoveEntity(uint id)
        {
            if (!entities.ContainsKey(id))
            {
                return;
            }            
            entities.Remove(id);
            groupManager.RemoveFromAllGroups(id);
            tagManager.RemoveTag(id);
            componentBitVectors.Remove(id);
            systemBitVectors.Remove(id);
        }

        public Entity GetEntity(uint id)
        {
            if (!entities.ContainsKey(id))
            {
                return null;
            }
            return entities[id];
        }    

        public void AddComponent(uint id, Component c)
        {
            if (!entityComponents.ContainsKey(id))
            {
                entityComponents.Add(id, new Dictionary<uint, Component>());
            }
            entityComponents[id].Add(c.Id, c);
            GetComponentBitVector(id).AddBit(c.Bit);      
        }
        
        public bool HasComponent(uint id, Type type)
        {
             if (entityComponents.ContainsKey(id))
            {                
                if(entityComponents[id].ContainsKey(componentTypeMapper.GetValue(type)))
                {
                    return true;
                }
            }
            return false;
        }

        public Component GetComponent(uint id, Type type)
        {
            if (!HasComponent(id, type))
            {
                return null;
            }
            return  entityComponents[id][componentTypeMapper.GetValue(type)];
        }

        public IEnumerable<Component> GetComponents(uint id)
        {
            if (entityComponents.ContainsKey(id))
            {
                return entityComponents[id].Values;
            }
            return new List<Component>();
        } 

        public void RemoveComponent(uint id, Type type)
        {
            if (!HasComponent(id, type))
            {
                return;
            }
            entityComponents[id].Remove(componentTypeMapper.GetValue(type));
            GetComponentBitVector(id).RemoveBitByOffset((int)componentTypeMapper.GetValue(type));
        }

        public BitVector GetComponentBitVector(uint id)
        {
            if (!componentBitVectors.ContainsKey(id))
            {
                componentBitVectors[id] = new BitVector();
            }
            return componentBitVectors[id];
        }

        public BitVector GetSystemBitVector(uint id)
        {
            if (!systemBitVectors.ContainsKey(id))
            {
                systemBitVectors[id] = new BitVector();
            }
            return systemBitVectors[id];
        }
    }
}