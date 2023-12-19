using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure
{
    /*
    <Summary>
    This is the AuthorRepository.
    It contains method to interact with the Author and AuthorAuthor entities.
    </Summary>
    */
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ChirpDBContext _db;
        public AuthorRepository(ChirpDBContext db)
        {
            _db = db;
        }

        public async Task AddAuthor(string name, string email)
        {
            try
            {
                await _db.Authors.AddAsync(new Author { AuthorId = Guid.NewGuid(), Name = name, Cheeps = new List<Cheep>(), Email = email });
                await _db.SaveChangesAsync();
            } 
            catch (Exception)
            {
                // Do nothing as the author already exists
            }
        }

        public async Task DeleteAuthor(Guid authorId){
            var author = _db.Authors.Where(author => author.AuthorId == authorId).FirstOrDefault();
            if (author != null)
            {
                _db.Remove(author);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Author with ID " + authorId + " does not exist");
            }
        }

        public async Task<AuthorDTO> GetAuthorByID(Guid ID)
        { 
            var author = await _db.Authors.Where(author => author.AuthorId == ID).Select(authorDTO => new AuthorDTO
            {
                AuthorId = ID,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db),
                Followed = GetAllFollowedAuthor(authorDTO.AuthorId, _db),
                Followers = GetAllFollowers(authorDTO.AuthorId, _db)
            }).FirstAsync();
            
            if (author != null)
            {
                return author;
            }
            else
            {
                throw new ArgumentException("Author with ID " + ID + " does not exist");
            }
        }

        public async Task<AuthorDTO> GetAuthorByName(string name)
        {
            var author = await _db.Authors.Where(author => author.Name == name).Select(authorDTO => new AuthorDTO
            {
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db),
                Followed = GetAllFollowedAuthor(authorDTO.AuthorId, _db),
                Followers = GetAllFollowers(authorDTO.AuthorId, _db)
            }).FirstAsync();
            if (author != null)
            {
                return author;
            }
            else
            {
                throw new ArgumentException("Author with name " + name + " does not exist");
            }
        }

        public async Task<AuthorDTO> GetAuthorByEmail(string email)
        {
            var author = await _db.Authors.Where(author => author.Email == email).Select(authorDTO => new AuthorDTO
            {
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db),
                Followed = GetAllFollowedAuthor(authorDTO.AuthorId, _db),
                Followers = GetAllFollowers(authorDTO.AuthorId, _db)
            }).FirstAsync();

            if (author != null)
            {
                return author;
            }
            else
            {
                throw new ArgumentException("Author with email " + email + " does not exist");
            }
        }

        public async Task RemoveFollowee(string _AuthorName, string _FolloweeName)
        {
            // Find current user in db
            var Author = await _db.Authors.Where(a => a.Name == _AuthorName)
                .Include(a => a.Followed)
                .FirstAsync();

            // Find the user we wish to unfollow in the db
            var Followee = await _db.Authors.Where(a => a.Name == _FolloweeName)
                .Include(a => a.Followers)
                .FirstAsync();

            if (Author.Followed != null && Followee != null)
            {
                /* Remove the user we wish to unfollow, from the current users list of followed users.
                This will also remove the current user, 
                from the list of followers of the user we wish to unfollow, 
                as the this list is represented by the same tuple in the db. */
                Author.Followed.Remove(Followee);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> DoesAuthorExist(string email)
        {
            try
            {
                var author = await _db.Authors.Where(author => author.Email == email).Select(authorDTO => new AuthorDTO
                {
                    AuthorId = authorDTO.AuthorId,
                    Name = authorDTO.Name,
                    Email = authorDTO.Email,
                    Cheeps = GetAllCheepsFromAuthor(authorDTO.Name, _db),
                    Followed = GetAllFollowedAuthor(authorDTO.AuthorId, _db),
                    Followers = GetAllFollowers(authorDTO.AuthorId, _db)
                }).FirstAsync();
                return true;
            } 
            catch 
            {
                return false;
            }
        }

        public async Task AddFollowee(string _AuthorName, string _FolloweeName)
        {
            // Constraint that a user can not follow itself
            if (_AuthorName == _FolloweeName)
            {
                throw new ArgumentException(_AuthorName + " can not follow " + _FolloweeName + ", as " + _AuthorName + " can not follow itself");
            }

            // Find the current user
            var Author = await _db.Authors.Where(a => a.Name == _AuthorName)
                .Include(a => a.Followed)
                .FirstAsync();

            // Find the user we wish to follow
            var Followee = await _db.Authors.Where(a => a.Name == _FolloweeName)
                .Include(a => a.Followers)
                .FirstAsync();
            
                Author.Followed ??= new List<Author>();
                Followee.Followers ??= new List<Author>();
            
            if (Author != null && Followee != null)
            {
                /* Adding a duplicate tuple to the db, already provokes and exeption.
                However, this gives us full control of what type of exception is thrown and with which message. */
                if (Author.Followed.Contains(Followee) || Followee.Followers.Contains(Author))
                {
                    throw new InvalidOperationException("Author: " + Author.Name + "already follows: " + Followee.Name);
                }
                /* Adds the user we wish to follow, to the current users list of followed users.
                This will also add the current user, 
                to the list of followers of the user we wish to follow, 
                as the this list is represented by the same tuple in the db. */
                Author.Followed.Add(Followee);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new NullReferenceException("Obejct _Author or _Followee of type Author is null");
            }
        }

        // Helper method: Retrieves all cheeps related to the given author from the db.
        private  static List<CheepDTO> GetAllCheepsFromAuthor(string Name, ChirpDBContext _dbcontext)
        {
            List<CheepDTO> cheepsToReturn = new List<CheepDTO>();
            try
            {
                var cheepsDTO =  _dbcontext.Cheeps.ToList().OrderByDescending(c => c.TimeStamp.Ticks).Where(author => author.Author.Name == Name).Select(CheepDTO => new CheepDTO
                { 
                    CheepId = CheepDTO.CheepId,
                    AuthorName = CheepDTO.Author.Name,
                    Message = CheepDTO.Text,
                    Likes = CheepDTO.Likes,
                    Timestamp = CheepDTO.TimeStamp
                });
                cheepsToReturn.AddRange(cheepsDTO);

                return cheepsToReturn;
            }
            catch
            {
                return new List<CheepDTO>();
            }
        }

        // Helper method: Retrieves all followed authors from the AuthorAuthor entity in the db.
        private static List<AuthorDTO> GetAllFollowedAuthor(Guid AuthorId, ChirpDBContext DBcontext)
        {
            List<AuthorDTO> followed = new List<AuthorDTO>();

            var Author = DBcontext.Authors.Where(a => a.AuthorId == AuthorId)
                .Include(a => a.Followed)
                .FirstOrDefault();

            if (Author != null && Author.Followed != null)
            {
                var AuthorDTOs = Author.Followed.Select(AuthorDTO => new AuthorDTO
                {
                    AuthorId = AuthorDTO.AuthorId,
                    Name = AuthorDTO.Name,
                    Email = AuthorDTO.Email
                }).ToList();

                if (AuthorDTOs != null)
                {
                    followed.AddRange(AuthorDTOs);
                }
            }

            return followed;
        }
        
        // Helper method: Retrieves all followers (authors) from the AuthorAuthor entity in the db.
        private static List<AuthorDTO> GetAllFollowers(Guid _AuthorId, ChirpDBContext _dbcontext) 
        { 
            List<AuthorDTO> followers = new List<AuthorDTO>();

            var Authors = _dbcontext.Authors.
                Where(a => a.AuthorId == _AuthorId)
                .Include(a => a.Followers)
                .FirstOrDefault();

            if (Authors != null && Authors.Followers != null)
            {
                var AuthorDTOs = Authors?.Followers.Select(AuthorDTO => new AuthorDTO()
                {
                    AuthorId = AuthorDTO.AuthorId,
                    Name = AuthorDTO.Name,
                    Email = AuthorDTO.Email
                })
                .ToList();

                if (AuthorDTOs != null)
                {
                    followers.AddRange(AuthorDTOs);
                }
            }

            return followers;
        }
    }
}