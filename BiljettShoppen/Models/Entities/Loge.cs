using Models.Entities.Base;

namespace Models.Entities
{
    public class Loge : BookableSpace
    {
        public string Name { get; set; }
        public string LogeNumber { get; set; }
        public int NumberOfPeople { get; set; }

        public override string GetDescription()
        {
            return Name;
        }
    }

}
