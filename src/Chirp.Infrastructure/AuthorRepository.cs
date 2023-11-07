using Chirp.Core;

namespace Chirp.Infrastructure
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ChirpDBContext _db;


        public AuthorRepository(ChirpDBContext db)
        {
            _db = db;
        }

        public void AddAuthor(string name, string email)
        {
            _db.Add(new Author { Name = name, Cheeps = new List<Cheep>(), Email = email });
        }

        public AuthorDTO GetAuthorByID(int ID)
        {
            var author = _db.Authors.Where(author => author.AuthorId == ID).Select(authorDTO => new AuthorDTO
            {
                AuthorId = ID,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db)
            }).FirstOrDefault();
            if (author != null)
            {
                return author;
            }
            else
            {
                throw new ArgumentException("Author with ID " + ID + " does not exist");
            }
        }

        public AuthorDTO GetAuthorByName(string name)
        {
            var author = _db.Authors.Where(author => author.Name == name).Select(authorDTO => new AuthorDTO
            {
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db)
            }).FirstOrDefault();
            if (author != null)
            {
                return author;
            }
            else
            {
                throw new ArgumentException("Author with name " + name + " does not exist");
            }


        }

        public AuthorDTO GetAuthorByEmail(string email)
        {
            var author = _db.Authors.Where(author => author.Email == email).Select(authorDTO => new AuthorDTO
            {
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db)
            }).FirstOrDefault();

            if (author != null)
            {
                return author;
            }
            else
            {
                throw new ArgumentException("Author with email " + email + " does not exist");
            }
        }

        private static List<CheepDTO> GetAllCheepsFromAuthor(string author, ChirpDBContext _dbcontext)
        {

            List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

            var cheepsDTO = _dbcontext.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks).Select(CheepDTO => new CheepDTO
            {
                //Sets the properties of the Cheep
                AuthorId = CheepDTO.Author.AuthorId,
                Author = CheepDTO.Author.Name,
                Message = CheepDTO.Text,
                Timestamp = CheepDTO.TimeStamp
            }
            );

            cheepsToReturn.AddRange(cheepsDTO);

            return cheepsToReturn;
        }
    }
}