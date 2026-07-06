using Terraria;

namespace Paletterra.Utils.UI {
  /// <summary>
  /// Wraps and explicitly marks a number that should be affected by
  /// the game's UI scale.
  /// </summary>
  public class Scaling {
    public float initial;

    public float Scaled => this.initial * Main.UIScale;

    public Scaling(float initial) {
      this.initial = initial;
    }
    public static Scaling operator+(Scaling lhs, float rhs) {
      return new Scaling(lhs.initial + rhs);
    }
    public static Scaling operator +(Scaling lhs, Scaling rhs) {
      return new Scaling(lhs.initial + rhs.initial);
    }
    public static bool operator >=(Scaling lhs, Scaling rhs) {
      return lhs.initial >= rhs.initial;
    }
    public static bool operator <=(Scaling lhs, Scaling rhs) {
      return lhs.initial <= rhs.initial;
    }
    public static bool operator ==(Scaling lhs, Scaling rhs) {
      return lhs.initial == rhs.initial;
    }
    public static bool operator !=(Scaling lhs, Scaling rhs) {
      return !(lhs == rhs);
    }
  }

  /// <summary>
  /// Same as `Vector2` but using `Scaling` instead of `float`.
  /// </summary>
  public class ScalingVec2 {
    public Scaling X;
    public Scaling Y;

    public ScalingVec2(float x, float y) {
      this.X = new Scaling(x);
      this.Y = new Scaling(y);
    }
    public static ScalingVec2 Zero() {
      return new ScalingVec2(0, 0);
    }
  }

  /// <summary>
  /// A more friend-shaped `Vector4`.
  /// </summary>
  public class Directions {
    public Scaling top;
    public Scaling right;
    public Scaling bottom;
    public Scaling left;
    public Scaling T { get => top; }
    public Scaling R { get => right; }
    public Scaling B { get => bottom; }
    public Scaling L { get => left; }
    public Scaling X { get => left + right; }
    public Scaling Y { get => top + bottom; }

    public static Directions Zero() {
      return new Directions(0, 0, 0, 0);
    }

    public Directions(float top, float right, float bottom, float left) {
      this.top = new Scaling(top);
      this.right = new Scaling (right);
      this.bottom = new Scaling(bottom);
      this.left = new Scaling(left);
    }
    public Directions(Scaling top, Scaling right, Scaling bottom, Scaling left) {
      this.top = top;
      this.right = right;
      this.bottom = bottom;
      this.left = left;
    }
    public void SetEach(float n) {
      this.top = new Scaling(n);
      this.right = new Scaling(n);
      this.bottom = new Scaling(n);
      this.left = new Scaling(n);
    }

    public static Directions operator +(Directions lhs, Directions rhs) {
      return new Directions(
          lhs.top + rhs.top,
          lhs.right + rhs.right,
          lhs.bottom + rhs.bottom,
          lhs.left + rhs.left
        );
    }
  }
}
