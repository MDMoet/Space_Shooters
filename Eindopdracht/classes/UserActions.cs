using System;
using System.Windows.Controls;
using System.Windows;
using System.Drawing;
using System.Threading.Tasks;
using static Eindopdracht.classes.GameTick;
using static Eindopdracht.classes.UserGameStats;

namespace Eindopdracht.classes
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

        // Direction enum to specify movement direction
        public enum Direction
        {
            Left,
            Right
        }

        // Move method to handles both left and right movement
        public void Move(Border user, Direction direction)
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
            Border Laser1_HitBox = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.Red,
                BorderThickness = new Thickness(1),
                Width = 15,
                Height = 35,
                Margin = new Thickness(user.Margin.Left + 35, 0, 0, -430),
                Name = "Laser1_HitBox",
                Tag = "20"
            };
            Border Laser2_HitBox = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.Red,
                BorderThickness = new Thickness(1),
                Width = 15,
                Height = 35,
                Margin = new Thickness(user.Margin.Left - 35, 0, 0, -430),
                Name = "Laser2_HitBox",
                Tag = "20"
            };
            Border Laser1 = new Border
            {
                Background = System.Windows.Media.Brushes.Red,
                Width = 5,
                Height = 20
            };
            Border Laser2 = new Border
            {
                Background = System.Windows.Media.Brushes.Red,
                Width = 5,
                Height = 20,
            };
            Laser1_HitBox.Child = Laser1;
            Laser2_HitBox.Child = Laser2;
            MainGrid.Children.Add(Laser1_HitBox);
            MainGrid.Children.Add(Laser2_HitBox);

            MoveAttack(Laser1_HitBox, Laser2_HitBox, MainGrid);
        }

        public async void MoveAttack(Border Laser1, Border Laser2, Grid MainGrid)
        {
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
                missed_shots++;
            }
            if (MainGrid.Children.Contains(Laser2))
            {
                MainGrid.Children.Remove(Laser2);
                missed_shots++;
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
