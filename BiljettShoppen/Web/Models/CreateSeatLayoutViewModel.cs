namespace Web.Models
{
    public class CreateSeatLayoutViewModel
    {
        public int ArenaId { get; set; }
        public string ArenaName { get; set; }
        public string LayoutName { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfCols { get; set; }
        public decimal ChairBasePrice { get; set; }
        public decimal BenchBasePrice { get; set; }
    }
}
