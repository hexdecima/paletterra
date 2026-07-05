using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Paletterra.Core.UI {
  public class Textures {
    public static Texture2D Brush { 
      get => (Texture2D)ModContent.Request<Texture2D>($"Terraria/Images/Item_{ItemID.Paintbrush}");
    }
    public static Texture2D Roller { 
      get => (Texture2D)ModContent.Request<Texture2D>($"Terraria/Images/Item_{ItemID.PaintRoller}");
    }
    public static Texture2D Scraper { 
      get => (Texture2D)ModContent.Request<Texture2D>($"Terraria/Images/Item_{ItemID.PaintScraper}");
    }
  }
}
