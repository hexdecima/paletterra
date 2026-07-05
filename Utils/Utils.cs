using Terraria.ID;
using System.Collections.Generic;

namespace Paletterra.Utils {
  /// <summary>
  /// Helper for listing and mapping paint items and their paint bytes.
  /// </summary>
  public class Paints {
    /// <summary>
    /// Lists all paint item IDs.
    /// </summary>
    public static short[] ALL_ITEMS => new short[] {
      ItemID.RedPaint,
      ItemID.OrangePaint,
      ItemID.YellowPaint,
      ItemID.LimePaint,
      ItemID.GreenPaint,
      ItemID.TealPaint,
      ItemID.CyanPaint,
      ItemID.SkyBluePaint,
      ItemID.BluePaint,
      ItemID.PurplePaint,
      ItemID.VioletPaint,
      ItemID.PinkPaint,
      ItemID.BlackPaint,
      ItemID.GrayPaint,
      ItemID.WhitePaint,
      ItemID.BrownPaint,
      ItemID.DeepRedPaint,
      ItemID.DeepOrangePaint,
      ItemID.DeepYellowPaint,
      ItemID.DeepLimePaint,
      ItemID.DeepGreenPaint,
      ItemID.DeepTealPaint,
      ItemID.DeepCyanPaint,
      ItemID.DeepSkyBluePaint,
      ItemID.DeepBluePaint,
      ItemID.DeepPurplePaint,
      ItemID.DeepVioletPaint,
      ItemID.DeepPinkPaint,
      ItemID.ShadowPaint,
      ItemID.NegativePaint,
      // These two behave differently.
      ItemID.GlowPaint,
      ItemID.EchoCoating
    };
    /// <summary>
    /// Lists all paint bytes.
    /// </summary>
    public static byte[] ALL_PAINTS => new byte[] {
      PaintID.RedPaint,
      PaintID.OrangePaint,
      PaintID.YellowPaint,
      PaintID.LimePaint,
      PaintID.GreenPaint,
      PaintID.TealPaint,
      PaintID.CyanPaint,
      PaintID.SkyBluePaint,
      PaintID.BluePaint,
      PaintID.PurplePaint,
      PaintID.VioletPaint,
      PaintID.PinkPaint,
      PaintID.BlackPaint,
      PaintID.GrayPaint,
      PaintID.WhitePaint,
      PaintID.BrownPaint,
      PaintID.DeepRedPaint,
      PaintID.DeepOrangePaint,
      PaintID.DeepYellowPaint,
      PaintID.DeepLimePaint,
      PaintID.DeepGreenPaint,
      PaintID.DeepTealPaint,
      PaintID.DeepCyanPaint,
      PaintID.DeepSkyBluePaint,
      PaintID.DeepBluePaint,
      PaintID.DeepPurplePaint,
      PaintID.DeepVioletPaint,
      PaintID.DeepPinkPaint,
      PaintID.ShadowPaint,
      PaintID.NegativePaint,
      // Fillers to keep symmetry. Not actually used.
      PaintID.IlluminantPaint,
      PaintID.IlluminantPaint,
    };
    /// <summary>
    /// Lists all paint items and bytes as pairs.
    /// </summary>
    public static Dictionary<short, byte> Pairs() {
      short[] items = Paints.ALL_ITEMS;
      byte[] paints = Paints.ALL_PAINTS;

      Dictionary<short, byte> pairs = new Dictionary<short, byte> {};

      // WARN: This assumes items and paints are symmetrical.
      for (int i = 0; i < items.Length; i++) {
        pairs.Add(items[i], paints[i]);
      }

      return pairs;
    }
    /// <summary>
    /// Essentially a safe indexing of paints by their item IDs.
    /// </summary>
    public static byte? MapItemToPaint(short itemId) {
      Dictionary<short, byte> pairs = Paints.Pairs();
      return pairs.ContainsKey(itemId) ? pairs[itemId] : null;
    }
  }
}
