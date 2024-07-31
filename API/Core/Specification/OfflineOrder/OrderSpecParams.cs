namespace Core.Specification.OfflineOrderSpec
{
    public class OrderSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex {get; set;} = 1;
        private int _pagesize = 20;
        public int PageSize { 
            get => _pagesize; 
            set => _pagesize = (value > MaxPageSize) ? MaxPageSize : value; 
        }

        public string Sort {get; set;}
        private string _search;
        public string Search { 
            get => _search; 
            set => _search = value?.ToLower(); 
        }
    }
}