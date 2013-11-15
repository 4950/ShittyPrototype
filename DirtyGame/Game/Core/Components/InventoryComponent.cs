﻿using EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirtyGame.game.Core.Components
{
    public class InventoryComponent : Component
    {
        private List<Entity> weapons;
        private Entity currentWeapon;

        public InventoryComponent()
        {
            weapons = new List<Entity>();
        }
        public List<Entity> WeaponList { get { return weapons; } }
        public void addWeapon(Entity weapon)
        {
            if (weapon.HasComponent<WeaponComponent>())
            {
                weapons.Add(weapon);
                if (weapons.Count == 1)
                    currentWeapon = weapon;
            }
        }
        public void setCurrentWeapon(Entity weapon)
        {
            if (weapons.Contains(weapon))
                currentWeapon = weapon;
        }
        public Entity CurrentWeapon { get { return currentWeapon; } }
        public void removeWeapon(Entity weapon)
        {
            weapons.Remove(weapon);
            if (currentWeapon == weapon)
            {
                if (weapons.Count > 0)
                    currentWeapon = weapons[0];
                else
                    currentWeapon = null;
            }
        }
    }
}