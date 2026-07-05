using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Paletterra.Utils.UI;

namespace Paletterra.Core.UI {
  /// <summary>
  /// Defines the paint browser panel.
  /// </summary>
  public class BrowserUI : UIState {
    private BrowserPanel? panel = null;
    private PaintEntry[] entries = new PaintEntry[0];

    public Vector2 pos;

    /// <summary>
    /// Initialises this UI at a given `point`.
    /// </summary>
    public static BrowserUI CreateAt(Vector2 point) {
      BrowserUI self = new BrowserUI();
      self.pos = point;

      return self;
    }

    /// <summary>
    /// Updates this browser's known paint entries, then refreshes the UI.
    /// </summary>
    public void UpdateEntries(PaintEntry[] updated) {
      this.entries = updated;

      if (this.panel != null) {
        ImageButton[] btns = this.MapEntriesToButtons();
        this.panel.UpdateEntries(btns);
      }
    }

    private void UpdateDimensions() {
      // WARN: The state (this class) should be where
      // dimensions are defined, not its child.
      ScalingVec2 size = BrowserPanel.PANEL_DIMENSIONS;

      this.Width.Set(size.X.Scaled, 0);
      this.Height.Set(size.Y.Scaled, 0);
      this.Left.Set(this.pos.X, 0);
      this.Top.Set(this.pos.Y, 0);
    }

    /// <summary>
    /// Returns the paint entries mapped to buttons.
    /// </summary>
    private ImageButton[] MapEntriesToButtons() {
      ImageButton[] btns = new ImageButton[this.entries.Length];

      for (int i = 0; i < this.entries.Length; i++) {
        PaintEntry entry = this.entries[i];
        // TODO: We're ignoring the stack count here. Don't.
        Texture2D text = (Texture2D)ModContent.Request<Texture2D>(
            $"Terraria/Images/Item_{entry.id}");
        ImageButton btn = new ImageButton(text);

        Item item = new Item();
        item.SetDefaults(entry.id);
        btn.tooltip = $"Select {item.Name}";
        btn.OnLeftClick += delegate(UIMouseEvent ev, UIElement el) {
          PaletteSystem sys = ModContent.GetInstance<PaletteSystem>();
          #nullable disable
          sys.paintTracker.active = entry.id;
        };
        btns[i] = btn;
      }

      return btns;
    }

    #region Hooks
    public override void OnActivate()
    {
      this.UpdateDimensions();
      this.panel = this.panel ?? new BrowserPanel();
      this.panel.pos = this.pos;
      this.panel.Reset();
      this.Append(this.panel);
    }
    public override void OnDeactivate()
    {
      this.RemoveAllChildren();
    }
    #endregion
  }
}
