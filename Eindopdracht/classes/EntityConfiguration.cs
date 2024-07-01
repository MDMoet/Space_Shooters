using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Eindopdracht.classes.EntityData;

namespace Eindopdracht.classes
{
    internal class EntityData
    {
        internal static string _entityName;
        internal static Dictionary<string, Enemy> enemies = new Dictionary<string, Enemy>();
        internal static Dictionary<string, object> GetEntityDataFromDB()
        {
            string connStr = ConfigurationManager.ConnectionStrings["SpaceShooters"].ConnectionString;
            string query = $"SELECT name, entity_stats.min_level, entity_stats.max_level, entity_stats.health, entity_stats.base_damage, entity_stats.base_speed, entity_stats.base_attack_speed, entity_stats.begin_spawn_wave, entity_skin.skin FROM ((entities INNER JOIN entity_stats ON entities.entity_id = entity_stats.entity_id) INNER JOIN entity_skin ON entities.entity_id = entity_skin.entity_id) WHERE entities.name = @name;";

            Dictionary<string, object> _entityStats = new Dictionary<string, object>();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", _entityName);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            object _statValue = reader.GetValue(i);
                            string _statName = reader.GetName(i);
                            _entityStats[_statName] = _statValue;
                        }
                    }
                }
            }
            return _entityStats;
        }
    }
    public abstract class Entity
    {
        internal string Name { get; set; }
        internal int Level { get; set; }
        internal int Health { get; set; }
        internal int Base_damage { get; set; }
        internal int Base_speed { get; set; }
        internal int Base_attack_speed { get; set; }
        internal int Begin_spawn_wave { get; set; }
        internal string Skin { get; set; }
    }

    // Concrete subclass
    internal class Enemy : Entity
    {

        public Enemy(object _name, object _lvl, object _hp, object _bd, object _bs, object _bas, object _bsw, object _skin)
        {
            CreateEntity(_name, _lvl, _hp, _bd, _bs, _bas, _bsw, _skin);
        }
        internal void CreateEntity(object _name, object _lvl, object _hp, object _bd, object _bs, object _bas, object _bsw, object _skin)
        {
            Name = (string)_name;
            Level = (int)_lvl;
            Health = (int)_hp;
            Base_damage = (int)_bd;
            Base_speed = (int)_bs;
            Base_attack_speed = (int)_bas;
            Begin_spawn_wave = (int)_bsw;
            Skin = (string)_skin;
        }
    }
}
