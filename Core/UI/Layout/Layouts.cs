namespace Paletterra.Core.UI {
  public class Layout {
    public static Layout Initial { get => new Initial(); }

    public virtual ImageButton? TopLeft() {
      return null;
    }
    public virtual ImageButton? TopCenter() {
      return null;
    }
    public virtual ImageButton? TopRight() {
      return null;
    }
    public virtual ImageButton? CenterLeft() {
      return null;
    }
    public virtual ImageButton? Center() {
      return null;
    }
    public virtual ImageButton? CenterRight() {
      return null;
    }
    public virtual ImageButton? BottomLeft() {
      return null;
    }
    public virtual ImageButton? BottomCenter() {
      return null;
    }
    public virtual ImageButton? BottomRight() {
      return null;
    }
    public ImageButton?[] Downcast() {
      return new ImageButton?[] {
        TopLeft(),
        TopCenter(),
        TopRight(),
        CenterLeft(),
        Center(),
        CenterRight(),
        BottomLeft(),
        BottomCenter(),
        BottomRight()
      };
    }
  }
}
