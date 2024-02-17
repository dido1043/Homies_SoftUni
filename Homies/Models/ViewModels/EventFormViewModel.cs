namespace Homies.Models.ViewModels
{
    public class EventFormViewModel
    {

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Start { get; set; } = string.Empty;

        public string End { get; set; } = string.Empty;


        public int TypeId { get; set; }
        public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
    }
}
