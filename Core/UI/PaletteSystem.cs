using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using Terraria;
using Paletterra.Utils;

namespace Paletterra.Core.UI {
  /// <summary>
  /// Wrapper bundling an interface and its state.
  /// </summary>
  sealed class PaletteElement<T> 
      where T: UIState, new() {
    public T state;
    public UserInterface iface { get; }
    public bool isEnabled { get => this.iface.CurrentState != null; }

    public PaletteElement() {
      this.state = new T();
      this.state.Activate();
      this.iface = new UserInterface();
    }
    public void Toggle() {
      if (this.isEnabled) {
        this.Disable();
      } else {
        this.Enable();
      }
    }
    public void Enable() {
      this.state.Activate();
      this.iface.SetState(this.state);
    }
    public void Disable() {
      this.iface.SetState(null);
      this.state.Deactivate();
    }
  }

  sealed public class PaintEntry {
    public short id;
    public int stack;

    public PaintEntry(short id, int stack) {
      this.id = id;
      this.stack = stack;
    }
  }

  sealed public class PaintTracker {
    /// <summary>
    /// Which paint item ID to apply when using tools.
    /// </summary>
    public short? active = null;
    /// <summary>
    /// Last seen list of paint items in the player's inventory.
    /// </summary>
    public List<PaintEntry> entries = new List<PaintEntry> {};

    /// <summary>
    /// Reads the player's inventory to update tracked paint items.
    /// </summary>
    public void Refresh() {
      Player p = Main.LocalPlayer;
      // NOTE: Maybe save this value within this instance after the first read?
      List<short> allPaintIDs = Paints.ALL_ITEMS.ToList();
      List<PaintEntry> items = new List<PaintEntry> {};
      bool enoughActive = false;

      foreach (short id in allPaintIDs) {
        int count = p.CountItem((int)id, 10000);
        if (count > 0) {
          items.Add(new PaintEntry(id, count));
          if (this.active != null && this.active == id) enoughActive = true;
        }
      }

      this.entries = items;
      if (!enoughActive) this.active = null;
    }
  }

  [Autoload(Side = ModSide.Client)]
  sealed class PaletteSystem: ModSystem {
    public static readonly string LayerName = "Paletterra: Palette";

    /// <summary>
    /// The main UI element, a 3x3 grid of buttons to configure the palette.
    /// </summary>
    private PaletteElement<MenuUI>? menu;
    /// <summary>
    /// A browser and picker for all paints in the player's inventory.
    /// </summary>
    private PaletteElement<BrowserUI>? browser;
    private GameTime? lastUpdated;
    private bool isBrowserVisible = false;
    /// <summary>
    /// Tracks paint items in the player's inventory.
    ///
    /// Used to manage paint usage when using brushes and
    /// to populate the Browser.
    /// </summary>
    public PaintTracker? paintTracker = new PaintTracker();

    /// <summary>
    /// Manages the currently active tool.
    /// </summary>
    public ToolController tool = new ToolController();
    /// <summary>
    /// Manages the currently active tool's mode.
    /// </summary>
    public ModeController mode = new ModeController();
    /// <summary>
    /// Simpler accessor to the menu layout.
    /// </summary>
    public Layout? layout { 
      get => this.menu?.state.layout; 
      set { if (this.menu != null) this.menu.state.layout = value; } 
    }
    /// <summary>
    /// Tracks the time (in ticks) between the palette's usages.
    /// </summary>
    public int itemDelay = 0;
    public bool isHoldingPalette = false;

    /// <summary>
    /// Hides the UI if visible, or shows it if not.
    /// </summary>
    public void ToggleMenu() {
      if (this.menu != null && this.menu.isEnabled) {
        this.menu.Disable();
        this.HideBrowser();
      } else {
        this.menu?.Enable();
      }
    }
    public void ShowBrowser() {
      if (this.menu == null 
          || this.browser == null 
          || this.paintTracker == null
        ) { return; }

      this.paintTracker.Refresh();

      // TODO: Refactor this abomination.
      this.browser.state.pos = this.menu.state.pos + 
        (new Vector2(-(10 * Main.UIScale), this.menu.state.CalculateUISize().Y + 20 * Main.UIScale));
      this.browser.state.UpdateEntries(
          this.paintTracker.entries.ToArray()
        );
      this.browser.Enable();

      this.isBrowserVisible = true;
    }
    public void HideBrowser() {
      this.browser?.Disable();
      this.isBrowserVisible = false;
    }
    public void ToggleBrowser() {
      if (this.isBrowserVisible) {
        this.HideBrowser();
      } else {
        this.ShowBrowser();
      }
    }
    /// <summary>
    /// To prevent palette items from going sicko mode.
    ///
    /// Called after successfully modifying a tile.
    /// </summary>
    public void ApplyItemDelay() {
      // 6 ticks = ~100ms
      this.itemDelay = 6;
    }

    /// <summary>
    /// We preload every texture used in the UI as the player loads into a world,
    /// to prevent some textures inconsistently just not loading in.
    /// </summary>
    private void PreloadTextures() {
      short[] toolIds = new short[] {
        ItemID.Paintbrush,
        ItemID.PaintRoller,
        ItemID.PaintScraper
      };
      foreach (short id in toolIds) {
        Main.instance.LoadItem(id);
      }

      short[] paintIds = Paints.ALL_ITEMS;
      foreach (short id in paintIds) {
        Main.instance.LoadItem(id);
      }
    }
    // Probably overkill at this point but oh well.
    private void Tick() {
      if (this.itemDelay > 0) {
        this.itemDelay--;
      }

      if (!this.isHoldingPalette) {
        if (this.menu != null && this.menu.isEnabled) {
          this.ToggleMenu();
        }
      }
      this.isHoldingPalette = false;
    }

    #region Hooks
    public override void OnWorldLoad()
    {
      this.paintTracker = new PaintTracker();
      this.menu = new PaletteElement<MenuUI>();
      this.browser = new PaletteElement<BrowserUI>();
      this.itemDelay = 0;
      this.PreloadTextures();
    }
    public override void OnWorldUnload()
    {
      this.paintTracker = null;
      this.menu = null;
      this.browser = null;
    }
    public override void UpdateUI(GameTime gameTime)
    {
      this.lastUpdated = gameTime;
      this.menu?.iface.Update(gameTime);
      this.browser?.iface.Update(gameTime);
      this.Tick();
    }
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
      int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
      if (mouseTextIndex != -1) {
        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
              PaletteSystem.LayerName,
              delegate {
                if (this.lastUpdated != null) {
                  if (this.menu != null && this.menu.isEnabled) {
                    this.menu.iface.Draw(Main.spriteBatch, this.lastUpdated);
                  }
                  if (this.browser != null && this.browser.isEnabled) {
                    this.browser.iface.Draw(Main.spriteBatch, this.lastUpdated);
                  }
                }

                return true;
              },
              InterfaceScaleType.UI
            ));
      }
    }
    #endregion
  }
}
