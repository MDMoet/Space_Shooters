using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Space_Shooters.classes.Game.Game_PlayerHandling.UserActions;
using static Space_Shooters.classes.Game.Game_EntityHandling.EntitySpawning;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using System.Windows;
using System.Reflection;
using Space_Shooters.classes.Game.Game_EntityHandling;
using Space_Shooters.Models;

namespace Space_Shooters.classes.Game.Game_CollisionHandling
{
    public class Collisions
    {
        internal static void CheckCollision(Grid mainGrid)
        {
            var bulletsToRemove = new List<Border>();
            var entitiesToRemove = new List<Grid>();

            foreach (var bullet in bullets.ToList())
            {
                foreach (var enemy in entities.ToList())
                {
                    if (IsColliding(mainGrid, bullet, enemy))
                    {
                        HandleCollision(mainGrid, bullet, enemy, bulletsToRemove, entitiesToRemove);
                    }
                }
            }

            // Remove bullets and entities after iteration
           
            EntityRemoval(entitiesToRemove, mainGrid);
            BulletRemoval(bulletsToRemove, mainGrid);

        }
        internal static void EntityRemoval(List<Grid> entitiesToRemove, Grid mainGrid)
        {
            foreach (var entity in entitiesToRemove)
            {
                if (mainGrid.Children.Contains(entity))
                {
                    mainGrid.Children.Remove(entity);
                    entities.Remove(entity);
                }
            }
        }
        internal static void BulletRemoval(List<Border> bulletsToRemove, Grid mainGrid)
        {
            UserGameStat userGameStat = new();
            foreach (var bullet in bulletsToRemove)
            {
                if (mainGrid.Children.Contains(bullet))
                {
                    mainGrid.Children.Remove(bullet);
                    bullets.Remove(bullet);
                    userGameStat.HitShots++;
                }
            }
        }

        private static bool IsColliding(Grid mainGrid, Border bullet, Grid enemy)
        {
            // Ensure the bullet and enemy are part of the visual tree
            if (!mainGrid.Children.Contains(bullet) || !mainGrid.Children.Contains(enemy))
            {
                return false;
            }

            // Get the absolute position of the bullet
            var bulletTransform = bullet.TransformToAncestor(mainGrid);
            var bulletPosition = bulletTransform.Transform(new Point(0, 0));
            Rect rect = new(bulletPosition.X, bulletPosition.Y, bullet.Width, bullet.Height);
            Rect bulletRect = rect;

            // Get the absolute position of the enemy
            var enemyTransform = enemy.TransformToAncestor(mainGrid);
            var enemyPosition = enemyTransform.Transform(new Point(0, 0));
            Rect enemyRect = new(enemyPosition.X, enemyPosition.Y, enemy.Width, enemy.Height);

            return bulletRect.IntersectsWith(enemyRect);
        }

        private static void HandleCollision(Grid mainGrid, Border bullet, Grid entity, List<Border> bulletsToRemove, List<Grid> entitiesToRemove)
        {
            foreach (ProgressBar health in entity.Children.OfType<ProgressBar>())
            {
                health.Value -= int.Parse(bullet.Tag.ToString());
                userGameStat.DamageDone += int.Parse(bullet.Tag.ToString());
                if (health.Value <= 0)
                {
                    entitiesToRemove.Add(entity);
                    EntityRemoval(entitiesToRemove, mainGrid);
                    userGameStat.Kills++;
                    WaveCheck.CheckForEntities(mainGrid, entity, EntityWave.entityWaveAmount);
                }
            }
            bulletsToRemove.Add(bullet);
        }
    }
}
