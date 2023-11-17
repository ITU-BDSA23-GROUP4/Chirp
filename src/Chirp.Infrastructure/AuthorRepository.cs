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
        public AuthorRepository(ChirpDBContext context) //If we want to use an existing db
        {
            db = context;
        }

        public void AddAuthor(string name, string email)
        {
            db.Add(new Author { Name = name, 
                Cheeps = new List<Cheep>(), 
                Email = email, 
                Following = new List<Author>(),
                Followers = new List<Author>(),
            });
        }

        public AuthorDTO GetAuthorByID(int ID)
        {
            var author = db.Authors.Where(author => author.AuthorId == ID).Select(authorDTO => new AuthorDTO
            {
                AuthorId = ID,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, db),
                Following = authorDTO.Following,
                Followers = authorDTO.Followers
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
            var author = db.Authors.Where(author => author.Name == name).Select(authorDTO => new AuthorDTO
            {
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, db)
            }).FirstOrDefault();
            if (author != null)
            {
                return author;
            }
            else
            {
                //Should change this logic later for more formal error handling of auhtor not existing
                //Implementing this so we can test the new User.Identity feature
                AddAuthor(name, name + "@chirp.com");
                db.SaveChanges();
                throw new ArgumentException("Author with name " + name + " does not exist");
            }


        }

        public AuthorDTO GetAuthorByEmail(string email)
        {
            var author = db.Authors.Where(author => author.Email == email).Select(authorDTO => new AuthorDTO
            {
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, db)
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
        
        private static List<CheepDTO> GetAllCheepsFromAuthor(string author, ChirpDBContext DBcontext)
        {

            List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

            var cheepsDTO = DBcontext.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks).Select(CheepDTO => new CheepDTO
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