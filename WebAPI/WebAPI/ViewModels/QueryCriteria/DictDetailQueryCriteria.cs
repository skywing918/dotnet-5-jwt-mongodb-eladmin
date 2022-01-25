namespace WebAPI.ViewModels
{
    public class DictDetailQueryCriteria : Pageable
    {
        public string label { get; set; }
        public string dictName { get; set; }
    }
}
