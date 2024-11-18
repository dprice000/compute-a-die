using Comput_a_die_v2.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Comput_a_die_v2.Controls
{
  internal class DieWheel
  {
    internal const int SPRITE_HEIGHT = 200, SPRITE_WIDTH = 200;
    private const int STARTING_MOMENTUM = 3;
    private const int MAX_MOMENTUM = 5;
    private const float MOMENTUM_THRESHOLD = 500;
    private const float STOPPING_THREHOLD = (float)(MOMENTUM_THRESHOLD * .15);
    private const float ANIMATION_THRESHOLD = 30;

    private Texture2D _spriteTexture;
    private readonly Vector2 _spritePosition;

    private readonly Rectangle _darkRectangle = new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
    private readonly Rectangle[] _spriteRectangles = new Rectangle[6];
    private readonly Point _spriteSize = new Point(SPRITE_WIDTH, SPRITE_HEIGHT);
    private float _animationTimer = 0;

    private int? _currentSprite = null;
    private int _currentMomentum = STARTING_MOMENTUM;
    private float _momentumTimer = 0;

    internal bool IsSpinning { get; private set; } = false;

    internal bool IsStopping { get; private set; } = false;

    internal DieWheel(Texture2D spriteTexture, Vector2 spritePosition)
    {
      _spriteTexture = spriteTexture;
      _spritePosition = spritePosition;

      InitializeSourceRectangles();
    }

    private void InitializeSourceRectangles()
    {
      const int spriteSheetHeight = 3, spriteSheetWidth = 3;
      int i = 0;

      for (int h = 1; h < spriteSheetHeight; h++)
      {
        for (int w = 0; w < spriteSheetWidth; w++)
        {
          _spriteRectangles[i] = new Rectangle(SPRITE_WIDTH * w, SPRITE_HEIGHT * h, SPRITE_WIDTH, SPRITE_HEIGHT);
          ++i;
        }
      }
    }

    private void EvaluateAnimation(GameTime gameTime)
    {
      if (!IsSpinning)
      {
        return;
      }

      _animationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

      if (_animationTimer >= ANIMATION_THRESHOLD)
      {
        _animationTimer = 0;
        _currentSprite += 1;

        if (_currentSprite >= _spriteRectangles.Length)
        {
          _currentSprite = 0;
        }
      }
    }

    internal void EvaluateMomentum(GameTime gameTime)
    {
      if (!IsSpinning)
      {
        return;
      }

      _momentumTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

      if (!IsStopping)
      {
        if (_momentumTimer >= MOMENTUM_THRESHOLD)
        {
          _momentumTimer = 0;

          if (_currentMomentum < MAX_MOMENTUM)
          {
            ++_currentMomentum;
          }
        }
      }

      if (IsStopping)
      {
        if (_currentMomentum > 0 && _momentumTimer >= STOPPING_THREHOLD && MomentumRandomizer.IsDescreased())
        {
          _momentumTimer = 0;
          --_currentMomentum;
        }

        if (_currentMomentum == 0)
        {
          Reset();
        }
      }
    }

    private void Reset()
    {
      IsSpinning = false;
      IsStopping = false;
      _animationTimer = 0;
      _momentumTimer = 0;
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
      if (!_currentSprite.HasValue)
      {
        spriteBatch.Draw(_spriteTexture, _spritePosition, _darkRectangle, Color.White);
        return;
      }

      spriteBatch.Draw(_spriteTexture, _spritePosition, _spriteRectangles[_currentSprite!.Value], Color.White);
    }

    internal void Update(GameTime gameTime)
    {
      EvaluateAnimation(gameTime);
      EvaluateMomentum(gameTime);
    }

    internal void StartSpin()
    {
      IsSpinning = true;
      IsStopping = false;
      _currentMomentum = STARTING_MOMENTUM;

      if (!_currentSprite.HasValue)
      {
        _currentSprite = 0;
      }
    }

    internal void StopSpin() 
    {
      IsStopping = true;
      _momentumTimer = 0;
    }
  }
}
