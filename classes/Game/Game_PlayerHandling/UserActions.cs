using System;
using System.Windows.Controls;
using System.Windows;
using System.Drawing;
using System.Threading.Tasks;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using System.Collections.Generic;
using Space_Shooters.Models;

namespace Space_Shooters.classes.Game.Game_PlayerHandling
{
    interface IUserActions
    {
        void MoveLeft(Border user);
        void MoveRight(Border user);
        void Attack1(Border user, Grid MainGrid);
        void Attack2();
        void Attack3();
    }
    public class UserActions : IUserActions
    {
        private readonly static int _userMovementSpeed = 25;
        private readonly static int _leftBoundary = -746;
        private readonly static int _rightBoundary = 736;
        public static List<Border> bullets = [];

        // Direction enum to specify movement direction
        public enum Direction
        {
            Left,
            Right
        }

        // Move method to handles both left and right movement
        private static void Move(Border user, Direction direction)
        {
            // Calculate the movement based on the direction

            double movement = direction == Direction.Left ? -_userMovementSpeed : _userMovementSpeed;
            // Calculate the new location based on the current margin
            double newLocation = user.Margin.Left + movement;

            // Clamp the new location within the boundaries
            newLocation = Math.Max(_leftBoundary, Math.Min(_rightBoundary, newLocation));

            // Apply the new margin to the user
            user.Margin = new Thickness(newLocation, user.Margin.Top, 0, 0);
        }

        public void MoveLeft(Border user)
        {
            // Move the user to the left
            Move(user, Direction.Left);
        }

        public void MoveRight(Border user)
        {
            // Move the user to the right
            Move(user, Direction.Right);
        }

        public void Attack1(Border user, Grid MainGrid)
        {
            // Attack 1 logic
            Border Laser1_HitBox = new()
            {
                BorderBrush = System.Windows.Media.Brushes.Transparent,
                BorderThickness = new Thickness(1),
                Width = 15,
                Height = 35,
                Margin = new Thickness(user.Margin.Left + 35, 0, 0, -430),
                Name = "Laser1_HitBox",
                Tag = userStat.BaseDamage.ToString()
            };
            Border Laser2_HitBox = new()
            {
                BorderBrush = System.Windows.Media.Brushes.Transparent,
                BorderThickness = new Thickness(1),
                Width = 15,
                Height = 35,
                Margin = new Thickness(user.Margin.Left - 35, 0, 0, -430),
                Name = "Laser2_HitBox",
                Tag = userStat.BaseDamage.ToString()
            };
            Border Laser1 = new()
            {
                Background = System.Windows.Media.Brushes.Red,
                Width = 5,
                Height = 20
            };
            Border Laser2 = new()
            {
                Background = System.Windows.Media.Brushes.Red,
                Width = 5,
                Height = 20,
            };
            Laser1_HitBox.Child = Laser1;
            Laser2_HitBox.Child = Laser2;
            MainGrid.Children.Add(Laser1_HitBox);
            MainGrid.Children.Add(Laser2_HitBox);

            bullets.Add(Laser1_HitBox);
            bullets.Add(Laser2_HitBox);

            MoveAttack(Laser1_HitBox, Laser2_HitBox, MainGrid);
        }

        private static async void MoveAttack(Border Laser1, Border Laser2, Grid MainGrid)
        {
            UserGameStat userGameStat = new();
            while (Laser1.Margin.Top > -1000 && Laser2.Margin.Top > -1000)
            {
                if (Paused)
                {
                    await Task.Delay(200);
                    continue;
                }
                await Task.Delay(tickMs);
                Laser1.Margin = new Thickness(Laser1.Margin.Left, Laser1.Margin.Top - 50, Laser1.Margin.Right, Laser1.Margin.Bottom);
                Laser2.Margin = new Thickness(Laser2.Margin.Left, Laser2.Margin.Top - 50, Laser2.Margin.Right, Laser2.Margin.Bottom);
            }
            if (MainGrid.Children.Contains(Laser1))
            {
                MainGrid.Children.Remove(Laser1);
                bullets.Remove(Laser1);
                userGameStat.MissedShots++;
            }
            if (MainGrid.Children.Contains(Laser2))
            {
                MainGrid.Children.Remove(Laser2);
                bullets.Remove(Laser2);
                userGameStat.MissedShots++;
            }
        }

        public void Attack2()
        {
            // Attack 2 logic
        }

        public void Attack3()
        {
            // Attack 3 logic
        }
    }
}
