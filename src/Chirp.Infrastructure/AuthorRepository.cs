using Chirp.Core;

namespace Chirp.Infrastructure
{
    public class AuthorRepository
    {
        private readonly ChirpDBContext db;

        private CheepRepository CheepRepository = new CheepRepository();

        public AuthorRepository()
        {
            db = new ChirpDBContext();
        }

        public AuthorDTO GetAuthorByID(int ID)
        {
            var author = db.Authors.Where(author => author.AuthorId == ID).FirstOrDefault();

            if(author == null)
            {
                return null;
            }

            var authorDTO = new AuthorDTO
            {
                AuthorId = ID,
                Name = author.Name,
                Email = author.Email,
                Cheeps = CheepRepository.GetCheepsFromAuthor(author.Name)
            };

            return authorDTO;
        }

        public AuthorDTO GetAuthorByName(string name)
        {
            var author = db.Authors.Where(author => author.Name == name).FirstOrDefault();

            if(author == null)
            {
                return null;
            }
            
            var authorDTO = new AuthorDTO
            {
                AuthorId = author.AuthorId,
                Name = name,
                Email = author.Email,
                Cheeps = CheepRepository.GetCheepsFromAuthor(name)
            };

            return authorDTO;
        }

        public AuthorDTO GetAuthorByEmail(string name)
        {
            var author = db.Authors.Where(author => author.Name == name).FirstOrDefault();

            if(author == null)
            {
                return null;
            }

            var authorDTO = new AuthorDTO
            {
                AuthorId = author.AuthorId,
                Name = name,
                Email = author.Email,
                Cheeps = CheepRepository.GetCheepsFromAuthor(name)
            };

            return authorDTO;
        }

        public void AddAuthor(AuthorDTO author)
        {
            db.Add(new Author { Name = author.Name, Cheeps = new List<Cheep>(), Email = author.Email });
            db.SaveChanges();
        }

    }
}
