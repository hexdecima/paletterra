using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Paletterra.Utils.UI;

namespace Paletterra.Core.UI {
  public delegate void PreDrawFn(ImageButton button);

  /// A simple button using the inventory/menu slot as frame and a given
  /// texture as image.
  public class ImageButton : UIElement {
    /// A texture to render as the frame, behind the image.
    public static readonly Texture2D Frame = 
      (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Inventory_Back8");
    /// A colour to use for the frame, while this button is not hovered.
    public static Color FrameColor = new Color(80, 80, 210);
    /// Same as above but for hovered state.
    public static Color HoverColor = new Color(50, 50, 175);
    /// A texture to use if `image` is not defined.
    private static Texture2D Placeholder { get => 
      (Texture2D)ModContent.Request<Texture2D>($"Terraria/Images/Item_{ItemID.AngelStatue}"); }

    /// A texture to render in this button.
    public Texture2D? image = null;

    public bool visible = true;

    public string? tooltip = null;
    /// Colour tint for this button's frame.
    public Color color = ImageButton.FrameColor;
    /// Inner padding.
    public Directions padding = Directions.Default;
    /// <summary>
    /// Per-instance logic to run before each draw call.
    /// </summary>
    public PreDrawFn? PreDraw;

    public ImageButton() {}
    public ImageButton(Texture2D? image) {
      this.image = image;
    }

    #region Hooks
    protected override void DrawSelf(SpriteBatch sb)
    {
      base.DrawSelf(sb);
      if (!this.visible) { return; }
      if (this.PreDraw != null) { this.PreDraw(this); };

      CalculatedStyle dims = this.GetDimensions();
      Vector2 pos = dims.Position();

      Rectangle frameRect = new Rectangle((int)pos.X,
          (int)pos.Y,
          (int)dims.Width,
          (int)dims.Height);
      Rectangle imgRect = new Rectangle(
          (int)(pos.X + this.padding.L),
          (int)pos.Y + (int)this.padding.T,
          (int)dims.Width - (int)this.padding.X,
          (int)dims.Height - (int)this.padding.Y
          );

      if (IsMouseHovering && this.tooltip != null) {
        Main.hoverItemName = this.tooltip;
      }

      sb.Draw(ImageButton.Frame, frameRect, this.color);
      sb.Draw(this.image ?? ImageButton.Placeholder
          , imgRect, Color.White);
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (!this.visible) { return; }

      if (IsMouseHovering) {
        Main.LocalPlayer.mouseInterface = true;

        this.color = ImageButton.HoverColor;
      } else {
        this.color = ImageButton.FrameColor;
      }
    }
    #endregion
  }
}
