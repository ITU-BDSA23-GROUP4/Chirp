using Chirp.Core;
using Microsoft.EntityFrameworkCore;

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
            _db.SaveChanges();
        }

        public AuthorDTO GetAuthorByID(int ID)
        {
            var author = _db.Authors.Where(author => author.AuthorId == ID).Select(authorDTO => new AuthorDTO
            {
                AuthorId = ID,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db),
                Followed = GetAllFollowedAuthors(authorDTO.AuthorId, _db),
                Followers = GetAllFollowers(authorDTO.AuthorId, _db)
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
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db),
                Followed = GetAllFollowedAuthors(authorDTO.AuthorId, _db),
                Followers = GetAllFollowers(authorDTO.AuthorId, _db)
            }).FirstOrDefault();
            if (author != null){
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
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db),
                Followed = GetAllFollowedAuthors(authorDTO.AuthorId, _db),
                Followers = GetAllFollowers(authorDTO.AuthorId, _db)
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

        private static List<CheepDTO> GetAllCheepsFromAuthor(string Name, ChirpDBContext _dbcontext)
        {

            List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

            var cheepsDTO = _dbcontext.Cheeps.ToList().OrderByDescending(c => c.TimeStamp.Ticks).Where(author => author.Author.Name == Name).Select(CheepDTO => new CheepDTO
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

        private static List<AuthorDTO> GetAllFollowedAuthors(int AuthorId, ChirpDBContext DBcontext) 
        {   
            List<AuthorDTO> followed = new List<AuthorDTO>();

            // pull out followed authors from a table not yet existing mapping between follower (foreign key to author) and author (foreign key to author)
            var authorDTOs = DBcontext.Follows.Where(f => f.Follower.AuthorId == AuthorId)
                .Select(AuthorDTO => new AuthorDTO 
                {
                    AuthorId = AuthorDTO.Follower.AuthorId,
                    Name = AuthorDTO.Follower.Name,
                    Email = AuthorDTO.Follower.Email
                }
            );

            followed.AddRange(authorDTOs);

            return followed;
        }

        private static List<AuthorDTO> GetAllFollowers(int AuthorId, ChirpDBContext DBcontext) 
        {   
            List<AuthorDTO> followers = new List<AuthorDTO>();

            // pull out followed authors from a table not yet existing mapping between author (foreign key to author) and follower (foreign key to author)
            // pull out followed authors from a table not yet existing mapping between follower (foreign key to author) and author (foreign key to author)
            var authorDTOs = DBcontext.Follows.Where(f => f.Followee.AuthorId == AuthorId)
                .Select(AuthorDTO => new AuthorDTO 
                {
                    AuthorId = AuthorDTO.Followee.AuthorId,
                    Name = AuthorDTO.Followee.Name,
                    Email = AuthorDTO.Followee.Email
                }
            );

            followers.AddRange(authorDTOs);

            return followers;
        }
    }
}