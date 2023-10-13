using Chirp.Core;

namespace Chirp.Infrastructure
{
    public class AuthorRepository
    {
        private readonly ChirpDBContext db;

        public AuthorRepository()
        {
            db = new ChirpDBContext();
        }

        public void AddAuthor(string name, string email)
        {
            db.Add(new Author { Name = name, Cheeps = new List<Cheep>(), Email = email });
        }

        public AuthorDTO GetAuthor(int ID)
        {
            var author = db.Authors.Where(author => author.AuthorId == ID).Select(authorDTO => new AuthorDTO{
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = authorDTO.Cheeps.Select(cheepDTO => new CheepDTO{
                    AuthorId = ID,
                    Author = authorDTO.Name,
                    Message = cheepDTO.Text,
                    Timestamp = cheepDTO.TimeStamp.ToString()
                }).ToList()
            }).First();
            return author;
        }


    }
}
