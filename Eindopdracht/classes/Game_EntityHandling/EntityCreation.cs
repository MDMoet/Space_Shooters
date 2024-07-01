using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Windows.Controls;
using System.Windows;
using static Eindopdracht.classes.WaveNumber;
using static Eindopdracht.classes.EntityData;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Eindopdracht.views;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Eindopdracht.classes
{
    internal class DetermineEntity
    {
        int wave = _wave;

        internal void DetermineEntityType(Grid MainGrid)
        {
            EntityCreation entityCreation;
            Dictionary<string, object> entityData = new Dictionary<string, object>();
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
            entityCreation = new EntityCreation(MainGrid, entityData["name"], entityData["min_level"], entityData["max_level"], entityData["health"], entityData["base_damage"], entityData["base_speed"], entityData["base_attack_speed"], entityData["begin_spawn_wave"], entityData["skin"]);
        }
    }
    internal class EntityCreation
    {
        public static Enemy _Enemy;
        internal static Dictionary<string, Enemy> enemies = new Dictionary<string, Enemy>();
        internal EntityCreation(Grid MainGrid, object _name, object _miLvl, object _maLvl, object _hp, object _bd, object _bs, object _bas, object _bsw, object _skin) {
            Random random = new Random();
            object _lvl = random.Next((int)_miLvl, (int)_maLvl);
            enemies[(string)_name] = CreateEnemy(_name, _lvl, _hp, _bd, _bs, _bas, _bsw, _skin);
            _Enemy = enemies[_entityName];
        }
        internal Enemy CreateEnemy(object _name, object _lvl, object _hp, object _bd, object _bs, object _bas, object _bsw, object _skin)
        {
            Enemy enemy = new Enemy(_name,
                                    _lvl,
                                    _hp,
                                    _bd,
                                    _bs,
                                    _bas,
                                    _bsw,
                                    _skin);
            return enemy;   
        }
    }
}
