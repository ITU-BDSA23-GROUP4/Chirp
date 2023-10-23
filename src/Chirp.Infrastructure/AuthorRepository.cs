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

        public AuthorDTO GetAuthorByID(int ID)
        {
            var author = db.Authors.Where(author => author.AuthorId == ID).Select(authorDTO => new AuthorDTO{
                AuthorId = ID,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, db)
            }).First();
            return author;
        }

        public AuthorDTO? GetAuthorByName(string name)
        {   
            try
            {
                var author = db.Authors.Where(author => author.Name == name).Select(authorDTO => new AuthorDTO{
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, db)
            }).First();
            return author;
            }
            catch (System.Exception)
            {
                
                return null;
            }
            
        }

        public AuthorDTO GetAuthorByEmail(string email)
        {
            var author = db.Authors.Where(author => author.Email == email).Select(authorDTO => new AuthorDTO{
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, db)
            }).First();
            return author;
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