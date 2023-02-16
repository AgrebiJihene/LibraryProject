using System.Collections.Generic;

namespace WPF.Reader.Model
{
    // A vous de completer ce qu'est un Livre !!
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public double Price { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
