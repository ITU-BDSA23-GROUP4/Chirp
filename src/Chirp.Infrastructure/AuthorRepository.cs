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

        public void RemoveFollowee(int _AuthorId, int _FolloweeId) {
            //I as a chirp author remover Chirp author by "name" from my Followed and remove myself from their followers list
            
            var FollowerRelationship = _db.Follows.Where(f => f.AuthorId == _AuthorId && f.AuthorId == _FolloweeId).FirstOrDefault();
            if (FollowerRelationship != null) {
                _db.Follows.Remove(FollowerRelationship);
                _db.SaveChanges();
            }
        }

        private Author? GetRealAuthorByID(int ID){
            var author = _db.Authors.FirstOrDefault(author => author.AuthorId == ID);
            return author ;
        }

        public void AddFollowee(int _AuthorId, int _FolloweeId) {
            //I as a chirp author add Chirp author by "name" to my Folled and add myself  to their followers list
            Author? _Author = GetRealAuthorByID(_AuthorId);
            Author? _Followee = GetRealAuthorByID(_FolloweeId);

            if (_Author != null && _Followee != null)
            { 
                _db.Follows.Add(new Follow 
                {
                    AuthorId = _AuthorId, 
                    FolloweeId = _FolloweeId, 
                    Follower = _Author, 
                    Author = _Followee
                });
                _db.SaveChanges();            
            } 
            else 
            {
                throw new NullReferenceException("The RealAuthor or followee could not b");
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
            var authorDTOs = DBcontext.Follows.ToList().Where(f => f.Follower.AuthorId == AuthorId)
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
            var authorDTOs = DBcontext.Follows.ToList().Where(f => f.Author.AuthorId == AuthorId)
                .Select(AuthorDTO => new AuthorDTO 
                {
                    AuthorId = AuthorDTO.Author.AuthorId,
                    Name = AuthorDTO.Author.Name,
                    Email = AuthorDTO.Author.Email
                }
            );

            followers.AddRange(authorDTOs);

            return followers;
        }
    }
}