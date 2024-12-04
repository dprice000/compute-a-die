using Comput_a_die_v2.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Comput_a_die_v2
{
  public class Game1 : Game
  {
    private const int SPRITE_WIDTH = 200;
    private const int WHEEL_MARGIN = 60;
    private readonly Color BACKGROUND_COLOR = Color.Black;
    private readonly Vector2 STARTING_POINT = new Vector2(170, 60);
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _faceplateTexture;
    private readonly Vector2 _faceplatePosition = Vector2.Zero;

    private bool _prevIsSpinning;
    private AudioOrchestrator _audioOrchestrator;
    private DieWheel _dieWheel1, _dieWheel2;

    private bool _isPressedState, _prevPressedState;

    private bool IsSpinning => _dieWheel1.IsSpinning || _dieWheel2.IsSpinning;

    public Game1()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {

      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      var pressSoundEffect = Content.Load<SoundEffect>("Audio\\StartingSound");
      var rollingSoundEffect = Content.Load<SoundEffect>("Audio\\RollingSound");
      var stoppingSoundEffect = Content.Load<SoundEffect>("Audio\\StoppingSound");
      _audioOrchestrator = new AudioOrchestrator(pressSoundEffect, rollingSoundEffect, stoppingSoundEffect);

      // Create a new SpriteBatch, which can be used to draw textures.
      _spriteBatch = new SpriteBatch(GraphicsDevice);
      var spriteTexture = Content.Load<Texture2D>("Sprites\\FullDieSpriteSheet");
      _faceplateTexture = Content.Load<Texture2D>("Sprites\\Faceplate");

      var position1 = STARTING_POINT;
      _dieWheel1 = new DieWheel(spriteTexture, position1);
      var position2 = new Vector2(position1.X + SPRITE_WIDTH + WHEEL_MARGIN, position1.Y);
      _dieWheel2 = new DieWheel(spriteTexture, position2);
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      var gameFocused = this.IsActive;
      var mouseState = Mouse.GetState();
      var keyboardState = Keyboard.GetState();

      if (mouseState.LeftButton == ButtonState.Pressed
          || keyboardState.IsKeyDown(Keys.Space))
      {
          _isPressedState = true;
      }
      else if (mouseState.LeftButton == ButtonState.Released
              && keyboardState.IsKeyUp(Keys.Space))
      {
        _isPressedState = false;
      }

      if (gameFocused && _isPressedState != _prevPressedState)
      {
        if (_isPressedState)
        {
          _audioOrchestrator.StartSpin();
          _dieWheel1.StartSpin();
          _dieWheel2.StartSpin();
        }
        else
        {
          _audioOrchestrator.StopSpin();
          _dieWheel1.StopSpin();
          _dieWheel2.StopSpin();
        }
      }
      else if (!gameFocused || !IsSpinning)
      {
        _audioOrchestrator.Kill();
      }

      _audioOrchestrator.Update(gameTime);
      _dieWheel1.Update(gameTime);
      _dieWheel2.Update(gameTime);

      // TODO: Add your update logic here

      _prevIsSpinning = IsSpinning;
      _prevPressedState = _isPressedState;
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(BACKGROUND_COLOR);

      _spriteBatch.Begin();
      _spriteBatch.Draw(_faceplateTexture, _faceplatePosition, Color.White);
      _dieWheel1.Draw(_spriteBatch);
      _dieWheel2.Draw(_spriteBatch);
      _spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
