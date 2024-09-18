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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;database=space_shooters;uid=root;sslmode=none", ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<DefaultKeybind>(entity =>
        {
            entity.HasKey(e => e.KeybindId).HasName("PRIMARY");

            entity.ToTable("default_keybinds");

            entity.Property(e => e.KeybindId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("keybind_id");

            entity.HasOne(d => d.Keybind).WithOne(p => p.DefaultKeybind)
                .HasForeignKey<DefaultKeybind>(d => d.KeybindId)
                .HasConstraintName("default_keybinds_keybind_id");
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("PRIMARY");

            entity.ToTable("entities");

            entity.Property(e => e.EntityId)
                .HasColumnType("int(11)")
                .HasColumnName("entity_id");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.SpawnWave)
                .HasColumnType("int(11)")
                .HasColumnName("spawn_wave");
        });

        modelBuilder.Entity<EntityEquipment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("entity_equipment");

            entity.HasIndex(e => e.EntityId, "entity_id");

            entity.HasIndex(e => e.ItemId, "equip_items_itemid");

            entity.Property(e => e.EntityId)
                .HasColumnType("int(11)")
                .HasColumnName("entity_id");
            entity.Property(e => e.ItemId)
                .HasColumnType("int(11)")
                .HasColumnName("item_id");

            entity.HasOne(d => d.Entity).WithMany()
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("equip_entities_entityid");

            entity.HasOne(d => d.Item).WithMany()
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("equip_items_itemid");
        });

        modelBuilder.Entity<EntitySkin>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("PRIMARY");

            entity.ToTable("entity_skin");

            entity.Property(e => e.EntityId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("entity_id");
            entity.Property(e => e.Skin)
                .HasMaxLength(128)
                .HasDefaultValueSql("'Entity_Default'")
                .HasColumnName("skin");

            entity.HasOne(d => d.Entity).WithOne(p => p.EntitySkin)
                .HasForeignKey<EntitySkin>(d => d.EntityId)
                .HasConstraintName("skin_entities_entityid");
        });

        modelBuilder.Entity<EntityStat>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("PRIMARY");

            entity.ToTable("entity_stats");

            entity.HasIndex(e => e.EntityId, "entity_id").IsUnique();

            entity.Property(e => e.EntityId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("entity_id");
            entity.Property(e => e.BaseAttackSpeed)
                .HasDefaultValueSql("'1000'")
                .HasColumnType("int(11)")
                .HasColumnName("base_attack_speed");
            entity.Property(e => e.BaseDamage)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("base_damage");
            entity.Property(e => e.BaseSpeed)
                .HasDefaultValueSql("'25'")
                .HasColumnType("int(11)")
                .HasColumnName("base_speed");
            entity.Property(e => e.Health)
                .HasDefaultValueSql("'100'")
                .HasColumnType("int(11)")
                .HasColumnName("health");
            entity.Property(e => e.MaxLevel)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("max_level");
            entity.Property(e => e.MinLevel)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("min_level");

            entity.HasOne(d => d.Entity).WithOne(p => p.EntityStat)
                .HasForeignKey<EntityStat>(d => d.EntityId)
                .HasConstraintName("stats_entities_entityid");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("items");

            entity.Property(e => e.ItemId)
                .HasColumnType("int(11)")
                .HasColumnName("item_id");
            entity.Property(e => e.ItemType)
                .HasMaxLength(128)
                .HasColumnName("item_type");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
            entity.Property(e => e.RequiredLevel)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(128)")
                .HasColumnName("required_level");
            entity.Property(e => e.Skin)
                .HasMaxLength(128)
                .HasColumnName("skin");
            entity.Property(e => e.Worth)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("worth");
        });

        modelBuilder.Entity<ItemStat>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("item_stats");

            entity.Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("item_id");
            entity.Property(e => e.AttackSpeed)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(64)")
                .HasColumnName("attack_speed");
            entity.Property(e => e.Damage)
                .HasColumnType("int(64)")
                .HasColumnName("damage");
            entity.Property(e => e.Durability)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(64)")
                .HasColumnName("durability");
            entity.Property(e => e.Healing)
                .HasColumnType("int(64)")
                .HasColumnName("healing");
            entity.Property(e => e.Level)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("level");

            entity.HasOne(d => d.Item).WithOne(p => p.ItemStat)
                .HasForeignKey<ItemStat>(d => d.ItemId)
                .HasConstraintName("stats_items_itemid");
        });

        modelBuilder.Entity<Itemshop>(entity =>
        {
            entity.HasKey(e => e.ShopitemId).HasName("PRIMARY");

            entity.ToTable("itemshop");

            entity.HasIndex(e => e.ItemId, "itemshop_item_id");

            entity.Property(e => e.ShopitemId)
                .HasColumnType("int(11)")
                .HasColumnName("shopitem_id");
            entity.Property(e => e.ItemId)
                .HasColumnType("int(11)")
                .HasColumnName("item_id");
            entity.Property(e => e.Worth)
                .HasColumnType("int(11)")
                .HasColumnName("worth");

            entity.HasOne(d => d.Item).WithMany(p => p.Itemshops)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("itemshop_item_id");
        });

        modelBuilder.Entity<KeybindEnum>(entity =>
        {
            entity.HasKey(e => e.KeybindId).HasName("PRIMARY");

            entity.ToTable("keybind_enums");

            entity.HasIndex(e => new { e.Keybind, e.KeybindEnum1 }, "keybind");

            entity.Property(e => e.KeybindId)
                .HasColumnType("int(11)")
                .HasColumnName("keybind_id");
            entity.Property(e => e.Keybind)
                .HasMaxLength(128)
                .HasColumnName("keybind");
            entity.Property(e => e.KeybindEnum1)
                .HasColumnType("int(11)")
                .HasColumnName("keybind_enum");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.Admin)
                .HasColumnType("tinyint(4)")
                .HasColumnName("admin");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(128)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserAction>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PRIMARY");

            entity.ToTable("user_actions");

            entity.Property(e => e.ActionId)
                .HasColumnType("int(11)")
                .HasColumnName("action_id");
            entity.Property(e => e.Action)
                .HasMaxLength(128)
                .HasColumnName("action");
        });

        modelBuilder.Entity<UserEquipment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("user_equipment");

            entity.HasIndex(e => e.ItemId, "items_equip_itemid");

            entity.HasIndex(e => e.UserId, "users_equip_userid");

            entity.Property(e => e.ItemId)
                .HasColumnType("int(11)")
                .HasColumnName("item_id");
            entity.Property(e => e.Itemslot)
                .HasColumnType("int(11)")
                .HasColumnName("itemslot");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Item).WithMany()
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("items_equip_itemid");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("users_equip_userid");
        });

        modelBuilder.Entity<UserGameStat>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user_game_stats");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.AverageAccuracy)
                .HasColumnType("int(11)")
                .HasColumnName("average_accuracy");
            entity.Property(e => e.DamageDone)
                .HasColumnType("int(11)")
                .HasColumnName("damage_done");
            entity.Property(e => e.Deaths)
                .HasColumnType("int(11)")
                .HasColumnName("deaths");
            entity.Property(e => e.HitShots)
                .HasColumnType("int(11)")
                .HasColumnName("hit_shots");
            entity.Property(e => e.Kills)
                .HasColumnType("int(11)")
                .HasColumnName("kills");
            entity.Property(e => e.MissedShots)
                .HasColumnType("int(11)")
                .HasColumnName("missed_shots");
            entity.Property(e => e.WavePr)
                .HasColumnType("int(11)")
                .HasColumnName("wave_pr");

            entity.HasOne(d => d.User).WithOne(p => p.UserGameStat)
                .HasForeignKey<UserGameStat>(d => d.UserId)
                .HasConstraintName("game_stats_user_id");
        });

        modelBuilder.Entity<UserInventory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("user_inventory");

            entity.HasIndex(e => e.ItemId, "inv_items_itemid");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Amount)
                .HasColumnType("int(11)")
                .HasColumnName("amount");
            entity.Property(e => e.ItemId)
                .HasColumnType("int(11)")
                .HasColumnName("item_id");
            entity.Property(e => e.Level)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("level");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Item).WithMany()
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("inv_items_itemid");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("inv_users_userid");
        });

        modelBuilder.Entity<UserKeybind>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.KeybindId, e.ActionId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("user_keybinds");

            entity.HasIndex(e => e.ActionId, "action_id");

            entity.HasIndex(e => e.KeybindId, "keybind_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.KeybindId)
                .HasColumnType("int(11)")
                .HasColumnName("keybind_id");
            entity.Property(e => e.ActionId)
                .HasColumnType("int(11)")
                .HasColumnName("action_id");

            entity.HasOne(d => d.Action).WithMany(p => p.UserKeybinds)
                .HasForeignKey(d => d.ActionId)
                .HasConstraintName("keybinds_action_id");

            entity.HasOne(d => d.Keybind).WithMany(p => p.UserKeybinds)
                .HasForeignKey(d => d.KeybindId)
                .HasConstraintName("keybinds_keybind_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserKeybinds)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("keybinds_user_id");
        });

        modelBuilder.Entity<UserSkin>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user_skin");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.Skin)
                .HasMaxLength(128)
                .HasDefaultValueSql("'User_Default'")
                .HasColumnName("skin");

            entity.HasOne(d => d.User).WithOne(p => p.UserSkin)
                .HasForeignKey<UserSkin>(d => d.UserId)
                .HasConstraintName("skin_users_userid");
        });

        modelBuilder.Entity<UserStat>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user_stats");

            entity.HasIndex(e => e.UserId, "user_id").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.BaseAttackSpeed)
                .HasDefaultValueSql("'1000'")
                .HasColumnType("int(11)")
                .HasColumnName("base_attack_speed");
            entity.Property(e => e.BaseDamage)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("base_damage");
            entity.Property(e => e.BaseSpeed)
                .HasDefaultValueSql("'25'")
                .HasColumnType("int(11)")
                .HasColumnName("base_speed");
            entity.Property(e => e.Health)
                .HasDefaultValueSql("'100'")
                .HasColumnType("int(128)")
                .HasColumnName("health");
            entity.Property(e => e.Level)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("level");
            entity.Property(e => e.LevelPoints)
                .HasColumnType("int(11)")
                .HasColumnName("level_points");
            entity.Property(e => e.LevelProgression)
                .HasColumnType("int(11)")
                .HasColumnName("level_progression");

            entity.HasOne(d => d.User).WithOne(p => p.UserStat)
                .HasForeignKey<UserStat>(d => d.UserId)
                .HasConstraintName("stats_users_userid");
        });

        modelBuilder.Entity<UsersShop>(entity =>
        {
            entity.HasKey(e => e.UsersShopitemId).HasName("PRIMARY");

            entity.ToTable("users_shop");

            entity.HasIndex(e => e.UserId, "shop_user_id");

            entity.HasIndex(e => e.UserItemId, "shop_user_item_id");

            entity.Property(e => e.UsersShopitemId)
                .HasColumnType("int(11)")
                .HasColumnName("users_shopitem_id");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.UserItemId)
                .HasColumnType("int(11)")
                .HasColumnName("user_item_id");
            entity.Property(e => e.Worth)
                .HasColumnType("int(11)")
                .HasColumnName("worth");

            entity.HasOne(d => d.User).WithMany(p => p.UsersShops)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("shop_user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
