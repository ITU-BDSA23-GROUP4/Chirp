using Chirp.Core;
using Microsoft.EntityFrameworkCore;

/*
<Summary>
This is the authorRepository, where we can work with our authors
This is the repository that adheres to Authors, so this is were we can create, read, update and delete authors.
</Summary>
*/

namespace Chirp.Infrastructure
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ChirpDBContext _db;
        public AuthorRepository(ChirpDBContext db)
        {
            _db = db;
        }


        /// <summary>
        ///  Adds an Author to the data base with all required fields Note 
        ///  followed and followers list are not required
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task AddAuthor(string name, string email)
        {
            try
            {
                await _db.Authors.AddAsync(new Author { AuthorId = Guid.NewGuid(), Name = name, Cheeps = new List<Cheep>(), Email = email });
                await _db.SaveChangesAsync();
            } 
            catch (Exception)
            {
                //Do nothing as the author already exists
            }
        }

        public async Task DeleteAuthor(Guid authorId){
            //Gets the author from the database
            var author = _db.Authors.Where(author => author.AuthorId == authorId).FirstOrDefault();
            if (author != null) //Removes the author from the database
            {
                _db.Remove(author);
                await _db.SaveChangesAsync();
            }
            else //In case the author doesn't exist
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
        /// <summary>
        /// Finds an Author in the Databse based on Email needs to be exact. 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
        /// <summary>
        /// The author asking for the removal removes the requesting author from both Authors follow lists 
        /// </summary>
        /// <param name="_AuthorName"></param>
        /// <param name="_FolloweeName"></param>
        /// <returns></returns>
        public async Task RemoveFollowee(string _AuthorName, string _FolloweeName)
        {
            var Author = await _db.Authors.Where(a => a.Name == _AuthorName)
                .Include(a => a.Followed)
                .FirstAsync();

            var Followee = await _db.Authors.Where(a => a.Name == _FolloweeName)
                .Include(a => a.Followers)
                .FirstAsync();

            if (Author != null && Followee.Followers != null)
            {
                Followee.Followers.Remove(Author);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool?> DoesAuthorExist(string email)
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
            catch {
                return false;
            }
        }

         /// <summary>
        /// Makes an Author Follow another follower Followee<\br> 
        /// This is done through the SelfRelecting AuthorAuthor relation.
        /// </summary>
        /// <param name="_AuthorId"></param>
        /// <param name="_FolloweeId"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task AddFollowee(string _AuthorName, string _FolloweeName)
        {
            var Author = await _db.Authors.Where(a => a.Name == _AuthorName)
                .Include(a => a.Followed)
                .FirstAsync();


            var Followee = await _db.Authors.Where(a => a.Name == _FolloweeName)
                .Include(a => a.Followers)
                .FirstAsync();
            
                Author.Followed ??= new List<Author>();
                Followee.Followers ??= new List<Author>();
            
            if (Author != null && Followee != null)
            {

                if (Author.Followed.Contains(Followee) || Followee.Followers.Contains(Author))
                {
                    throw new InvalidOperationException("Author: " + Author.Name + "already follows: " + Followee.Name);
                }
                Author.Followed.Add(Followee);
                 Followee.Followers.Add(Author);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new NullReferenceException("Obejct _Author or _Followee of type Author is null");
            }
        }

        private static Task<List<CheepDTO>> GetAllCheepsFromAuthorAsync(string Name, ChirpDBContext _dbcontext) {
            return Task.Run(() => GetAllCheepsFromAuthor(Name, _dbcontext));
        }

        private  static List<CheepDTO> GetAllCheepsFromAuthor(string Name, ChirpDBContext _dbcontext)
        {
            List<CheepDTO> cheepsToReturn = new List<CheepDTO>();
            try
            {
                var cheepsDTO =  _dbcontext.Cheeps.ToList().OrderByDescending(c => c.TimeStamp.Ticks).Where(author => author.Author.Name == Name).Select(CheepDTO => new CheepDTO
                { 
                    //Sets the properties of the Cheep
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

        private static Task<List<AuthorDTO>> GetAllFollowedAuthorAsync(Guid AuthorId, ChirpDBContext _dbcontext) {
            return Task.Run(() => GetAllFollowedAuthor(AuthorId, _dbcontext));
        }

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

        private static Task<List<AuthorDTO>> GetAllFollowersAsync(Guid AuthorId, ChirpDBContext _dbcontext) {
            return Task.Run(() => GetAllFollowers(AuthorId, _dbcontext));
        }
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