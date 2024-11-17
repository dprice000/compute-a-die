using System;

namespace Comput_a_die_v2.Utility
{
  internal class MomentumRandomizer
  {
    private const int PRECENT_CHANCE = 15;
    private static readonly Random _ran = new Random();


    public static bool IsDescreased() => _ran.Next(1, 101) <= PRECENT_CHANCE;
  }
}
