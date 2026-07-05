using Terraria.UI;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Paletterra.Utils.UI;

namespace Paletterra.Core.UI {
  /// <summary>
  /// A simple menu listing every paint item in the player's inventory
  /// as clickable buttons.
  /// </summary>
  public class BrowserPanel : UIElement {
    public static ScalingVec2 PANEL_DIMENSIONS = new ScalingVec2(224, 152);
    // NOTE: This can probably be a regular `Scaling`, 
    // as buttons will never not be equilateral.
    public static ScalingVec2 BUTTON_DIMENSIONS = new ScalingVec2(30, 30);
    private static Scaling PADDING = new Scaling(6);
    private static Scaling GAP = new Scaling(6);

    public Vector2 pos;
    /// <summary>
    /// Item buttons to render in this panel.
    /// Children dimensions are calculated by this panel element itself.
    /// </summary>
    public List<ImageButton> entries = new List<ImageButton> {};

    public UIPanel panel = new UIPanel();
    public bool visible = true;

    /// <summary>
    /// Updates rendered entry buttons and resets this element.
    /// </summary>
    public void UpdateEntries(IEnumerable<ImageButton> updated) {
      this.entries = updated.ToList();
      // TODO: Only reset the buttons, not the panel frame.
      this.Reset();
    }
    /// <summary>
    /// Redimensions children and refreshes them, as well as this panel's
    /// own dimensions.
    /// </summary>
    public void Reset() {
      this.RemoveAllChildren();

      this.Append(this.panel);

      this.Width.Set(PANEL_DIMENSIONS.X.Scaled, 0);
      this.Height.Set(PANEL_DIMENSIONS.Y.Scaled, 0);
      this.panel.Width.Set(PANEL_DIMENSIONS.X.Scaled, 0);
      this.panel.Height.Set(PANEL_DIMENSIONS.Y.Scaled, 0);
      this.SetPadding(0);
      this.panel.SetPadding(0);

      this.ResetButtons();
    }
    private void ResetButtons() {
      ScalingVec2 currentPoint = new ScalingVec2(PADDING.Scaled, PADDING.Scaled);

      this.panel.RemoveAllChildren();
      foreach (ImageButton btn in this.entries) {
        btn.Width.Set(BUTTON_DIMENSIONS.X.Scaled, 0);
        btn.Height.Set(BUTTON_DIMENSIONS.Y.Scaled, 0);

        btn.Left.Set(currentPoint.X.Scaled, 0);
        btn.Top.Set(currentPoint.Y.Scaled, 0);

        currentPoint.X += (GAP + BUTTON_DIMENSIONS.X);
        // Skips to the next "row" after running out of space.
        // WARN: This doesn't handle overflowing.
        if (currentPoint.X + (GAP + PADDING) >= PANEL_DIMENSIONS.X) {
          currentPoint.X = PADDING;
          currentPoint.Y += (GAP + BUTTON_DIMENSIONS.Y);
        }

        this.panel.Append(btn);
      }
    }

    # region Hooks
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
      if (!this.visible) { return; }
      base.DrawSelf(spriteBatch);

    }
    public override void Update(GameTime gameTime)
    {
      if (!this.visible) { return; }
      base.Update(gameTime);
    }
    #endregion
  }
}
