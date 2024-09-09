using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using static Space_Shooters.classes.Game.Game_EntityHandling.EntityData;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Space_Shooters.views;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;

namespace Space_Shooters.classes.Game.Game_EntityHandling
{
    internal class DetermineEntity
    {
        internal static void DetermineEntityType()
        {
            int wave = Wave;
            Dictionary<string, object> entityData = [];
            if (wave >= 1)
            {
                _entityName = "Newborn alien";
                entityData = GetEntityDataFromDB();
            }
            if (wave >= 3)
            {
                _entityName = "Alien child";
                entityData = GetEntityDataFromDB();
            }
            if (wave >= 5)
            {
                _entityName = "Mature alien";
                entityData = GetEntityDataFromDB();
            }
            if (entityData.TryGetValue("name", out _) && entityData.TryGetValue("min_level", out _) &&
    entityData.TryGetValue("max_level", out _) && entityData.TryGetValue("health", out _) &&
    entityData.TryGetValue("base_damage", out _) && entityData.TryGetValue("base_speed", out _) &&
    entityData.TryGetValue("base_attack_speed", out _) && entityData.TryGetValue("begin_spawn_wave", out _) &&
    entityData.TryGetValue("skin", out _))
            {
               EntityCreation _ = new(entityData["name"], entityData["min_level"], entityData["max_level"], entityData["health"], entityData["base_damage"], entityData["base_speed"], entityData["base_attack_speed"], entityData["begin_spawn_wave"], entityData["skin"]);
            }
            else
            {
                MessageBox.Show("Error: Entity data not found.");
            }
        }
    }
    internal class EntityCreation
    {
        public static Enemy? _Enemy;
        internal static Dictionary<string, Enemy> enemies = [];
        internal EntityCreation(object _name, object _miLvl, object _maLvl, object _hp, object _bd, object _bs, object _bas, object _bsw, object _skin)
        {
            Random random = new();
            object _lvl = random.Next((int)_miLvl, (int)_maLvl);
            enemies[(string)_name] = CreateEnemy(_name, _lvl, _hp, _bd, _bs, _bas, _bsw, _skin);
            _Enemy = enemies[_entityName];
        }
        internal static Enemy CreateEnemy(object _name, object _lvl, object _hp, object _bd, object _bs, object _bas, object _bsw, object _skin)
        {
            Enemy enemy = new()
            {
                Name = (string)_name,
                Level = (int)_lvl,
                Health = (int)_hp,
                Base_damage = (int)_bd,
                Base_speed = (int)_bs,
                Base_attack_speed = (int)_bas,
                Begin_spawn_wave = (int)_bsw,
                Skin = (string)_skin
            };
            return enemy;
        }
    }
}
