using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Paletterra.Utils.UI;

namespace Paletterra.Core.UI {
  class MenuUI: UIState {
    /// <summary>
    /// The gap (in pixels) between the center button and those surrounding it.
    /// </summary>
    internal static readonly Scaling GAP = new Scaling(8);
    /// <summary>
    /// The size (in pixels) of each button in this UI.
    /// </summary>
    internal static readonly Scaling SIZE = new Scaling(30);

    /// <summary>
    /// Which button layout to render, if any.
    /// </summary>
    public Layout? layout;
    /// <summary>
    /// Point on screen to place this menu at.
    /// </summary>
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
    /// <summary>
    /// Clears all children and appends the current layout.
    /// </summary>
    public void ResetGrid() {
      this.RemoveAllChildren();
      this.PlaceGrid();
    }
    /// <summary>
    /// Returns the total sizes (in pixels) this UI takes.
    /// </summary>
    public Vector2 CalculateUISize() {
      int n = (int)((MenuUI.SIZE.Scaled * 3)
        + (MenuUI.GAP.Scaled * 2));

      return new Vector2(n, n);
    }
    /// <summary>
    /// Updates this element's sizes and positioning.
    /// </summary>
    private void UpdateDimensions() {
      Vector2 size = this.CalculateUISize();
      this.Width.Set(size.X, 0);
      this.Height.Set(size.Y, 0);

      this.Left.Set(pos.X, 0);
      this.Top.Set(pos.Y, 0);
    }
    /// <summary>
    /// Places all buttons from this layout into this element.
    ///
    /// Does not clear existing buttons.
    /// </summary>
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
