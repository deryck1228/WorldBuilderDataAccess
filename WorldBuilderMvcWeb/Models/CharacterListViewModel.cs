namespace WorldBuilderMvcWeb.Models
{
    public class CharacterListViewModel
    {
        public List<Character> Characters { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
