using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.ID;

namespace Paletterra.Core.UI {
  enum Tool {
    Brush,
    Roller,
    Scraper
  }
  class ToolController {
    public static Tool Default = Tool.Brush;

    public Tool active;

    public ToolController() {
      this.active = ToolController.Default;
    }
    public void Select(Tool tool) {
      this.active = tool;
    }
    public void CycleNext() {
      switch (this.active) {
        case Tool.Brush:
          this.active = Tool.Roller;
          break;
        case Tool.Roller:
          this.active = Tool.Scraper;
          break;
        case Tool.Scraper:
          this.active = Tool.Brush;
          break;
      }
    }
    public string NextAsString() {
      switch (this.active) {
        case Tool.Brush:
          return "Roller";
        case Tool.Roller:
          return "Scraper";
        case Tool.Scraper:
          return "Brush";
        default:
          return "Unknown";
      }
    }
    public string AsString() {
      switch (this.active) {
        case Tool.Brush:
          return "Brush";
        case Tool.Roller:
          return "Roller";
        case Tool.Scraper:
          return "Scraper";
        default:
          return "Unknown";
      }
    }
    /// Returns the matching texture to use for this tool's button.
    public Texture2D AsTexture() {
      switch (this.active) {
        case Tool.Brush:
          return Textures.Brush;
        case Tool.Roller:
          return Textures.Roller;
        case Tool.Scraper:
          return Textures.Scraper;
        default:
          return (Texture2D)ModContent.Request<Texture2D>(
              $"Terraria/Images/Item_{ItemID.AngelStatue}");
      }
    }
  }

  enum Mode {
    // Paints one block at a time.
    Single,
    // Drag to paint over a rectangle.
    Bucket,
    // Drag to paint in a corrected line.
    Line,
    // Scrapes only tile paint (Scraper-only).
    ScrapeTiles,
    // Scrapes only wall paint (Scraper-only).
    ScrapeWalls
  }
  class ModeController {
    public static Mode Default = Mode.Single;

    public Mode active;

    public ModeController() {
      this.active = ModeController.Default;
    }

    public void Select(Mode mode) {
      this.active = mode;
    }
  }
}
