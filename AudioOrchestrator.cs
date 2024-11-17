using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Comput_a_die_v2
{
  internal class AudioOrchestrator
  {
    private SoundEffect _pressSoundEffect, _rollingSoundEffect, _stoppingSoundEffect;
    private SoundEffectInstance _pressedInstance, _rollingInstance;

    private SoundState _prevPressedState = SoundState.Stopped;

    public AudioOrchestrator(SoundEffect pressedEffect, SoundEffect rollingEffect, SoundEffect stoppingEffect)
    {
      _pressSoundEffect = pressedEffect; 
      _rollingSoundEffect = rollingEffect;
      _stoppingSoundEffect = stoppingEffect;

      _pressedInstance = _pressSoundEffect.CreateInstance();
      _rollingInstance = _rollingSoundEffect.CreateInstance();
      _rollingInstance.IsLooped = true;
    }

    internal void StartSpin()
    {
      _pressedInstance.Play();
    }

    internal void StopSpin()
    {
      _pressedInstance.Stop();
      _rollingInstance.Stop();

      _stoppingSoundEffect.Play();
    }

    internal void Kill()
    {
      _pressedInstance.Stop();
      _rollingInstance.Stop();
    }

    public void Update(GameTime gameTime)
    {
      if (_prevPressedState == SoundState.Playing && _pressedInstance.State == SoundState.Stopped)
      {
        _rollingInstance.Play();
      }

      _prevPressedState = _pressedInstance.State;
    }
  }
}
