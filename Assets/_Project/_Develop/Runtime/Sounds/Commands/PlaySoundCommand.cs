using TestTankProject.Runtime._Project._Develop.Runtime.Sounds;

namespace TestTankProject.Runtime.Core.Sounds
{
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