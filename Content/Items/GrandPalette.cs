using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Paletterra.Core.UI;
using Paletterra.Utils;
using Microsoft.Xna.Framework;

namespace Paletterra.Content.Items
{
  enum ExceptionPaint {
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
      Item.useStyle = ItemUseStyleID.HiddenAnimation;
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

          // Illuminant and Echo coatings are applied very differently from the rest.
          ExceptionPaint? except = null;
          if (paintId == ItemID.GlowPaint) except = ExceptionPaint.Illuminant;
          else if (paintId == ItemID.EchoCoating) except = ExceptionPaint.Echo;

          bool usedPaint = except == null ?
            this.HandleRegularPaint(t, paintId.Value, tool) :
            this.HandleExceptionPaint(t, except.Value, tool);

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
    // Almost all paints just require setting the paint byte of the tile
    // to its respective one.
    private bool HandleRegularPaint(Tile t, short paintId, Tool tool) {
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
    // The two exceptions work by changing specific properties of the tile,
    // as a tile can both have a paint colour AND be invisible.
    private bool HandleExceptionPaint(Tile t, ExceptionPaint paint, Tool tool) {
      if (tool == Tool.Brush) {
        switch (paint) {
          case ExceptionPaint.Echo:
            if (t.IsTileInvisible) return false;
            else t.IsTileInvisible = true;
            break;
          case ExceptionPaint.Illuminant:
            if (t.IsTileFullbright) return false;
            else t.IsTileFullbright = true;
            break;
        }
      } else {
        switch (paint) {
          case ExceptionPaint.Echo:
            if (t.IsWallInvisible) return false;
            else t.IsWallInvisible = true;
            break;
          case ExceptionPaint.Illuminant:
            if (t.IsWallFullbright) return false;
            else t.IsWallFullbright = true;
            break;
        }
      }

      return true;
    }
  }
}
