using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Paletterra.Core.UI;
using Paletterra.Utils;
using Microsoft.Xna.Framework;

namespace Paletterra.Content.Items
{
  enum Coat {
    Illuminant,
    Echo,
  }

  class GrandPalette: ModItem {
    #region Hooks
    public override void SetStaticDefaults()
    {
      Item.ResearchUnlockCount = 1;

    }
    public override void SetDefaults()
    {
      Item.maxStack = 1;
      Item.consumable = false;
      Item.width = 32;
      Item.height = 32;
      Item.rare = ItemRarityID.Blue;
      Item.useAnimation = 15;
      Item.useTime = 15;
      Item.autoReuse = true;
      Item.useStyle = ItemUseStyleID.RaiseLamp;
    }
    public override void AddRecipes()
    {
      this.CreateRecipe()
        .AddIngredient(ItemID.Paintbrush)
        .AddIngredient(ItemID.PaintRoller)
        .AddIngredient(ItemID.PaintScraper)
        .AddIngredient(ItemID.RichMahogany, 16)
        .AddIngredient(ItemID.Bone, 6)
        .AddTile(TileID.TinkerersWorkbench)
        .Register();
    }
    public override bool AltFunctionUse(Player player) => true;
    public override bool? UseItem(Player player)
    {
      if (player.altFunctionUse == 2) {
        ModContent.GetInstance<PaletteSystem>()
          .ToggleMenu();
        return true;
      }

      this.Apply(player);
      return base.UseItem(player);
    }
    public override void HoldItem(Player player)
    {
      PaletteSystem sys = ModContent.GetInstance<PaletteSystem>();
      sys.isHoldingPalette = true;
    }
    #endregion

    /// <summary>
    /// Applies the selected tool to the tile currently being hovered.
    /// </summary>
    protected void Apply(Player player) {
      PaletteSystem sys = ModContent.GetInstance<PaletteSystem>();
        if (sys.itemDelay > 0) return;

        Point mousePos = Main.MouseWorld.ToTileCoordinates();
        Tile t = Main.tile[mousePos];

        // Aerophobia check.
        // Also why isn't `Tile.HasWall` a thing...
        if (!t.HasTile && t.WallType == 0) return;

        // About 16-blocks.
        float dist = player.Distance(Main.MouseWorld);
        if (dist > 260f) return;

        Tool tool = sys.tool.active;
        if (tool == Tool.Brush || tool == Tool.Roller) {
          short? paintId = sys.paintTracker?.active;
          if (paintId == null) return;

          Coat? coat = null;
          if (paintId == ItemID.GlowPaint) coat = Coat.Illuminant;
          else if (paintId == ItemID.EchoCoating) coat = Coat.Echo;

          bool usedPaint = coat == null ?
            this.HandlePaint(t, paintId.Value, tool) :
            this.HandleCoat(t, coat.Value, tool);

          if (usedPaint) {
            sys.ApplyItemDelay();
            player.ConsumeItem((int)paintId);
            sys.paintTracker?.Refresh();
          }
        } else if (tool == Tool.Scraper) {
          t.TileColor = PaintID.None;
          t.WallColor = PaintID.None;
          t.IsTileFullbright = false;
          t.IsWallFullbright = false;
          t.IsTileInvisible = false;
          t.IsWallInvisible = false;
        }
    }
    /// <summary>
    /// Applies paint to a tile or wall.
    ///
    /// Returns whether or not it was successfully applied.
    /// </summary>
    private bool HandlePaint(Tile t, short paintId, Tool tool) {
      byte? paint = Paints.MapItemToPaint(paintId);
      if (paint == null) return false;

      if (tool == Tool.Brush) {
        if (t.TileColor == paint.Value) return false;
        t.TileColor = paint.Value;
      }
      else {
        if (t.WallColor == paint.Value) return false;
        t.WallColor = paint.Value;
      }
      return true;
    }
    /// <summary>
    /// Applies a coat to a tile or wall.
    ///
    /// Returns whether or not it was successfully applied.
    /// </summary>
    private bool HandleCoat(Tile t, Coat paint, Tool tool) {
      if (tool == Tool.Brush) {
        switch (paint) {
          case Coat.Echo:
            if (t.IsTileInvisible) return false;
            else t.IsTileInvisible = true;
            break;
          case Coat.Illuminant:
            if (t.IsTileFullbright) return false;
            else t.IsTileFullbright = true;
            break;
        }
      } else {
        switch (paint) {
          case Coat.Echo:
            if (t.IsWallInvisible) return false;
            else t.IsWallInvisible = true;
            break;
          case Coat.Illuminant:
            if (t.IsWallFullbright) return false;
            else t.IsWallFullbright = true;
            break;
        }
      }

      return true;
    }
  }
}
