namespace Core.Specification
{
    public class ShopeeOrderSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex {get; set;} = 1;
        private int _pagesize = 8;
        public int PageSize { 
            get => _pagesize; 
            set => _pagesize = (value > MaxPageSize) ? MaxPageSize : value; 
        }
        public string Sort {get; set;}
        private string _search;
        public string Date { get; set; }
        public string Search { 
            get => _search; 
            set => _search = value.ToLower(); 
        }
    }
}