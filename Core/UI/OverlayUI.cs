using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Paletterra.Utils.UI;

namespace Paletterra.Core.UI {
  /// <summary>
  /// Draws a little icon with the currently selected tool and/or paint near the cursor,
  /// as an overlay (i.e. no actual interactable elements).
  /// </summary>
  class OverlayUI: UIState {
    public static readonly ScalingVec2 DEFAULT_OFFSET = new ScalingVec2(6, 6);

    public ScalingVec2 offset = OverlayUI.DEFAULT_OFFSET;

    private Indicator? indc = null;

    public override void OnActivate()
    {
      this.indc = new Indicator(this.offset);
      this.Append(indc);
    }
    public override void OnDeactivate()
    {
      this.RemoveAllChildren();
      this.indc = null;
    }
  }

  class Indicator: UIElement {
    public static readonly Scaling DEFAULT_SIZE = new Scaling(20);

    private ScalingVec2 offset;
    public Scaling size = Indicator.DEFAULT_SIZE;

    public Indicator(ScalingVec2 offset) {
      this.offset = offset;
    }

    protected override void DrawSelf(SpriteBatch sb)
    {
      PaletteSystem sys = ModContent.GetInstance<PaletteSystem>(); 
      if (sys.paintTracker == null) return;

      Texture2D toolText = sys.tool.AsTexture();
      Texture2D? paintText = sys.paintTracker.AsTexture();

      Vector2 center = Main.MouseScreen;

      Rectangle toolRect = new Rectangle(
          (int)(center.X + this.offset.X.Scaled),
          (int)(center.Y + this.offset.Y.Scaled),
          (int)this.size.Scaled,
          (int)this.size.Scaled
        );
      sb.Draw(toolText, toolRect, Color.White);
      if (sys.tool.active == Tool.Scraper) return;

      if (paintText != null) {
        Rectangle paintRect = new Rectangle(
            (int)(center.X + this.offset.X.Scaled + (this.size.Scaled * .6f)),
            (int)(center.Y + this.offset.Y.Scaled + (this.size.Scaled * .6f)),
            (int)(this.size.Scaled * .6f),
            (int)(this.size.Scaled * .6f)
            );
        sb.Draw(paintText, paintRect, Color.White);
      }
    }
  }
}
