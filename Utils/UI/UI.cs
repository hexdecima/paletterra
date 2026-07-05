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
    public float top;
    public float right;
    public float bottom;
    public float left;
    public float T { get => top; }
    public float R { get => right; }
    public float B { get => bottom; }
    public float L { get => left; }
    public float X { get => left + right; }
    public float Y { get => top + bottom; }

    public static Directions Zero() {
      return new Directions(0, 0, 0, 0);
    }
    public static Directions Default => new Directions(6, 6, 6, 6);

    public Directions(float top, float right, float bottom, float left) {
      this.top = top;
      this.right = right;
      this.bottom = bottom;
      this.left = left;
    }
    
    public void SetEach(float n) {
      this.top = n;
      this.right = n;
      this.bottom = n;
      this.left = n;
    }
  }
}
