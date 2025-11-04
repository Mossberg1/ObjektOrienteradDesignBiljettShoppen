using Models.Entities.Base;

namespace Models.Entities
{
    public class Loge : BookableSpace
    {
        private int _numberOfPeople;

        public string Name { get; set; }
        public string LogeNumber { get; set; }
        public int NumberOfPeople 
        { 
            get { return _numberOfPeople; }
            set 
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                _numberOfPeople = value;
            }
        }

        public Loge() { }

        public Loge(string name, string logeNumber, int numberOfPeople) 
        {
            Name = name;
            LogeNumber = logeNumber;
            NumberOfPeople = numberOfPeople;
        }

        public override string GetDescription()
        {
            return Name;
        }
    }

}
