using TestTankProject.Runtime._Project._Develop.Runtime.Sounds;
using TestTankProject.Runtime.Bootstrap;

namespace TestTankProject.Runtime.Core.Sounds
{
    [Message]
    public readonly struct PlaySoundCommand 
    {
        public readonly SoundTypes SoundType;
        public readonly float Delay;

        public PlaySoundCommand(SoundTypes soundType, float delay = 0f)
        {
            SoundType = soundType;
            Delay = delay;
        }
    }
}