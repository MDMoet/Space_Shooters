using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Space_Shooters.Models;

namespace Space_Shooters.Context;

public partial class GameContext : DbContext
{
    public GameContext()
    {
    }

    public GameContext(DbContextOptions<GameContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DefaultKeybind> DefaultKeybinds { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<EntityEquipment> EntityEquipments { get; set; }

    public virtual DbSet<EntitySkin> EntitySkins { get; set; }

    public virtual DbSet<EntityStat> EntityStats { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemStat> ItemStats { get; set; }

    public virtual DbSet<Itemshop> Itemshops { get; set; }

    public virtual DbSet<KeybindEnum> KeybindEnums { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAction> UserActions { get; set; }

    public virtual DbSet<UserEquipment> UserEquipments { get; set; }

    public virtual DbSet<UserGameStat> UserGameStats { get; set; }

    public virtual DbSet<UserInventory> UserInventories { get; set; }

    public virtual DbSet<UserKeybind> UserKeybinds { get; set; }

    public virtual DbSet<UserSkin> UserSkins { get; set; }

    public virtual DbSet<UserStat> UserStats { get; set; }

    public virtual DbSet<UsersShop> UsersShops { get; set; }

    public virtual DbSet<WaveInformation> WaveInformations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;database=space_shooters;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<DefaultKeybind>(entity =>
        {
            entity.HasKey(e => e.KeybindId).HasName("PRIMARY");

            entity.Property(e => e.KeybindId).ValueGeneratedNever();

            entity.HasOne(d => d.Keybind).WithOne(p => p.DefaultKeybind).HasConstraintName("default_keybinds_keybind_id");
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("PRIMARY");
        });

        modelBuilder.Entity<EntityEquipment>(entity =>
        {
            entity.HasOne(d => d.Entity).WithMany().HasConstraintName("equip_entities_entityid");

            entity.HasOne(d => d.Item).WithMany().HasConstraintName("equip_items_itemid");
        });

        modelBuilder.Entity<EntitySkin>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("PRIMARY");

            entity.Property(e => e.EntityId).ValueGeneratedNever();
            entity.Property(e => e.Skin).HasDefaultValueSql("'Entity_Default'");

            entity.HasOne(d => d.Entity).WithOne(p => p.EntitySkin).HasConstraintName("skin_entities_entityid");
        });

        modelBuilder.Entity<EntityStat>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("PRIMARY");

            entity.Property(e => e.EntityId).ValueGeneratedNever();
            entity.Property(e => e.BaseAttackSpeed).HasDefaultValueSql("'1000'");
            entity.Property(e => e.BaseDamage).HasDefaultValueSql("'1'");
            entity.Property(e => e.BaseSpeed).HasDefaultValueSql("'25'");
            entity.Property(e => e.BeginSpawnWave).HasDefaultValueSql("'1'");
            entity.Property(e => e.Health).HasDefaultValueSql("'100'");
            entity.Property(e => e.MaxLevel).HasDefaultValueSql("'1'");
            entity.Property(e => e.MinLevel).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.Entity).WithOne(p => p.EntityStat).HasConstraintName("stats_entities_entityid");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.Property(e => e.RequiredLevel).HasDefaultValueSql("'1'");
            entity.Property(e => e.Worth).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<ItemStat>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.Property(e => e.ItemId).ValueGeneratedNever();
            entity.Property(e => e.AttackSpeed).HasDefaultValueSql("'1'");
            entity.Property(e => e.Durability).HasDefaultValueSql("'1'");
            entity.Property(e => e.Level).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.Item).WithOne(p => p.ItemStat).HasConstraintName("stats_items_itemid");
        });

        modelBuilder.Entity<Itemshop>(entity =>
        {
            entity.HasKey(e => e.ShopitemId).HasName("PRIMARY");

            entity.HasOne(d => d.Item).WithMany(p => p.Itemshops).HasConstraintName("itemshop_item_id");
        });

        modelBuilder.Entity<KeybindEnum>(entity =>
        {
            entity.HasKey(e => e.KeybindId).HasName("PRIMARY");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");
        });

        modelBuilder.Entity<UserAction>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PRIMARY");
        });

        modelBuilder.Entity<UserEquipment>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("items_equip_itemid");

            entity.HasOne(d => d.User).WithMany().HasConstraintName("users_equip_userid");
        });

        modelBuilder.Entity<UserGameStat>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithOne(p => p.UserGameStat).HasConstraintName("game_stats_user_id");
        });

        modelBuilder.Entity<UserInventory>(entity =>
        {
            entity.Property(e => e.Level).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.Item).WithMany().HasConstraintName("inv_items_itemid");

            entity.HasOne(d => d.User).WithMany().HasConstraintName("inv_users_userid");
        });

        modelBuilder.Entity<UserKeybind>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.KeybindId, e.ActionId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.HasOne(d => d.Action).WithMany(p => p.UserKeybinds).HasConstraintName("keybinds_action_id");

            entity.HasOne(d => d.Keybind).WithMany(p => p.UserKeybinds).HasConstraintName("keybinds_keybind_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserKeybinds).HasConstraintName("keybinds_user_id");
        });

        modelBuilder.Entity<UserSkin>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Skin).HasDefaultValueSql("'User_Default'");

            entity.HasOne(d => d.User).WithOne(p => p.UserSkin).HasConstraintName("skin_users_userid");
        });

        modelBuilder.Entity<UserStat>(entity =>
        {
            entity.Property(e => e.BaseAttackSpeed).HasDefaultValueSql("'1000'");
            entity.Property(e => e.BaseDamage).HasDefaultValueSql("'1'");
            entity.Property(e => e.Health).HasDefaultValueSql("'100'");
            entity.Property(e => e.Level).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.User).WithOne().HasConstraintName("stats_users_userid");
        });

        modelBuilder.Entity<UsersShop>(entity =>
        {
            entity.HasKey(e => e.UsersShopitemId).HasName("PRIMARY");

            entity.HasOne(d => d.User).WithMany(p => p.UsersShops).HasConstraintName("shop_user_id");
        });

        modelBuilder.Entity<WaveInformation>(entity =>
        {
            entity.HasKey(e => e.WaveId).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
