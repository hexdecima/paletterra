using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace Paletterra.Core.UI {
  public class Initial: Layout {
    // Quickly cycles forward through tools.
    public override ImageButton? Center()
    {
      PaletteSystem initialSys = ModContent.GetInstance<PaletteSystem>();

      ImageButton btn = new ImageButton(initialSys?.tool.AsTexture());
      btn.tooltip = $"Switch to {initialSys?.tool.NextAsString()}";

      btn.OnLeftClick += delegate(UIMouseEvent ev, UIElement el) {
        PaletteSystem rtSys = ModContent.GetInstance<PaletteSystem>();
        rtSys?.tool.CycleNext();
        btn.tooltip = $"Switch to {rtSys?.tool.NextAsString()}";
        btn.image = rtSys?.tool.AsTexture();
      };

      return btn;
    }
    // Switches to a dedicated tool selection menu.
    public override ImageButton? BottomCenter()
    {
      ImageButton btn = new ImageButton();
      btn.tooltip = "Open Paint Selection";

      // NOTE: This feels painfully inefficient.
      btn.PreDraw = delegate(ImageButton btn) {
        PaletteSystem sys = ModContent.GetInstance<PaletteSystem>();
        Texture2D icon = 
          (Texture2D)ModContent.Request<Texture2D>($"Terraria/Images/Item_{sys.paintTracker?.active ?? ItemID.AngelStatue}");
        btn.image = icon; 
      };
      btn.OnLeftClick += delegate(UIMouseEvent ev, UIElement el) {
        PaletteSystem sys = ModContent.GetInstance<PaletteSystem>();
        sys.ToggleBrowser();
      };

      return btn;
    }
  }
}
