using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Paletterra.Utils.UI;

namespace Paletterra.Core.UI {
  class MenuUI: UIState {
    // The gap between the middle button and the ones to the sides, in pixels.
    internal static readonly Scaling GAP = new Scaling(8);
    // The size of each button in the UI.
    internal static readonly Scaling SIZE = new Scaling(30);

    // A 3x3 grid of buttons to render.
    public Layout? layout;
    public Vector2 pos;

    /// <summary>
    /// Updates position to be centered at a given `point`.
    /// </summary>
    public void Center(Vector2 point) {
      Vector2 size = this.CalculateUISize();
      this.pos = point - (size / 2);
      this.UpdateDimensions();
    }
    /// <summary>
    /// Updates the button layout being rendered and the interface.
    /// </summary>
    public void SetLayout(Layout lout) {
      this.layout = lout;
      this.ResetGrid();
    }
    public void ResetGrid() {
      this.RemoveAllChildren();
      this.PlaceGrid();
    }
    /// Returns a height and width for the UI.
    public Vector2 CalculateUISize() {
      int n = (int)((MenuUI.SIZE.Scaled * 3)
        + (MenuUI.GAP.Scaled * 2));

      return new Vector2(n, n);
    }
    /// Calculates the dimensions for this UI and applies it.
    ///
    /// Should be called after updating the center point.
    private void UpdateDimensions() {
      Vector2 size = this.CalculateUISize();
      this.Width.Set(size.X, 0);
      this.Height.Set(size.Y, 0);

      this.Left.Set(pos.X, 0);
      this.Top.Set(pos.Y, 0);
    }
    /// Places, and then appends each button as a child.
    private void PlaceGrid() {
      if (this.layout == null) { return; }

      int step = (int)(MenuUI.SIZE.Scaled + MenuUI.GAP.Scaled);
      ImageButton?[] btns = this.layout.Downcast();

      for (int i = 0; i <= 8; i++) {
        ref ImageButton? btn = ref btns[i];
        if (btn is null) { continue; }

        btn.Width.Set(MenuUI.SIZE.Scaled, 0);
        btn.Height.Set(MenuUI.SIZE.Scaled, 0);
        btn.Left.Set((i % 3) * step, 0);
        btn.Top.Set(float.Floor(i / 3) * step, 0);
        this.Append(btn);
      }
    }

    #region Hooks
    public override void OnActivate()
    {
      Vector2 cursorPos = Main.MouseScreen / Main.UIScale;
      this.Center(cursorPos);

      this.layout = Layout.Initial;
      this.ResetGrid();
    }
    public override void OnDeactivate()
    {
      this.RemoveAllChildren();
      this.layout = null;
    }
    #endregion
  }
}
