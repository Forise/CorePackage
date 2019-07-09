//Developed by Pavel Kravtsov.
namespace Core.Audio
{
    [System.Serializable]
    public class TimedNumber
    {
        public float Time;
        public int Number;

        public TimedNumber() { }

        public TimedNumber(float time, int number)
        {
            Time = time;
            Number = number;
        }
    }
}