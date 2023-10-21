using Chirp.Core;

namespace Chirp.Infrastructure
{
    public class AuthorRepository
    {
        private readonly ChirpDBContext db;

        private CheepRepository cheepRepo = new CheepRepository();

        public AuthorRepository()
        {
            db = new ChirpDBContext();
        }

        public AuthorDTO GetAuthorByID(int ID)
        {
            var author = db.Authors.Where(author => author.AuthorId == ID).FirstOrDefault();

            var authorDTO = new AuthorDTO
            {
                AuthorId = ID,
                Name = author.Name,
                Email = author.Email,
                Cheeps = cheepRepo.GetAllCheepsFromAuthor(author.Name)
            };

            return authorDTO;
        }
        public AuthorDTO GetAuthorByName(string name)
        {
            var author = db.Authors.Where(author => author.Name == name).FirstOrDefault();

            var authorDTO = new AuthorDTO
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                Email = author.Email,
                Cheeps = cheepRepo.GetAllCheepsFromAuthor(author.Name)
            };

            return authorDTO;
        }
        public AuthorDTO GetAuthorByEmail(string email)
        {
            var author = db.Authors.Where(author => author.Name == email).FirstOrDefault();

            var authorDTO = new AuthorDTO
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                Email = author.Email,
                Cheeps = cheepRepo.GetAllCheepsFromAuthor(author.Name)
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