using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Paletterra.Utils.UI;
using Terraria.GameContent.UI.Elements;

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
        ImageButton[] paintBtns = this.MapEntriesToButtons();
        ImageButton[] btns = [
          this.MkDeselectButton(),
          .. paintBtns
        ];

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
    /// Makes a button to deselect active paint.
    /// </summary>
    private ImageButton MkDeselectButton() {
      Texture2D icon = (Texture2D)ModContent.Request<Texture2D>(
          "Paletterra/Assets/Textures/EmptyPaint"
        );
      ImageButton btn = new ImageButton(icon);
      btn.tooltip = "Deselect";
      btn.OnLeftClick += delegate(UIMouseEvent ev, UIElement el) {
          PaletteSystem sys = ModContent.GetInstance<PaletteSystem>();
          #nullable disable
          sys.paintTracker.active = null;
      };

      return btn;
    }

    /// <summary>
    /// Returns the paint entries mapped to buttons.
    /// </summary>
    private ImageButton[] MapEntriesToButtons() {
      int cap = int.Clamp(this.entries.Length, 0, 23);
      ImageButton[] btns = new ImageButton[cap];

      for (int i = 0; i < cap; i++) {
        PaintEntry entry = this.entries[i];
        Texture2D text = (Texture2D)ModContent.Request<Texture2D>(
            $"Terraria/Images/Item_{entry.id}");
        ImageButton btn = new ImageButton(text);
        btn.padding = new Directions(2, 6, 6, 2);

        float fontScale = entry.stack <= 9999 ? .8f : .7f;
        string countText = entry.stack <= 9999 ? $"{entry.stack}" : "9999+";
        UIText count = new UIText(countText, fontScale);
        count.HAlign = .95f;
        count.VAlign = 1;
        btn.Append(count);

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
