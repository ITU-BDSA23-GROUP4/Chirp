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
        /// <summary>
        /// Adds an author to the database, is called after an author adds a cheep.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task AddAuthor(string name, string email)
        {
            try
            {
                await _db.Authors.AddAsync(new Author { Name = name, Cheeps = new List<Cheep>(), Email = email });
                await _db.SaveChangesAsync();
            } 
            catch (Exception)
            {
                //Do nothing as the author already exists
            }
        }
        /// <summary>
        /// Finds an Author in the database based in ID 
        /// 
        /// Should be debrecated
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<AuthorDTO> GetAuthorByID(int ID)
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
        /// <summary>
        /// Finds an Author in the Databse based on Name needs to be exact. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
        /// Examines the database for if an Author with the given Email 
        /// If it is found it returns True if it is found in the database.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A boolean, True if found</returns>
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
    
        public async Task RemoveFollowee(int _AuthorId, int _FollowerId) {
            //I as a chirp author remover Chirp author by "name" from my Followed and remove myself from their followers list
            
            var FollowerRelationship = await _db.Follows.Where(f => f.FolloweeId == _AuthorId && f.AuthorId == _FollowerId).FirstAsync();
            
            if (FollowerRelationship != null) {
                _db.Follows.Remove(FollowerRelationship);
                await _db.SaveChangesAsync();
            }
            else 
            {
                throw new NullReferenceException("FollowerRelationship is null");
            }
        }

        private async Task<Author?> GetRealAuthorByID(int ID){
            var author = await _db.Authors.FirstAsync(author => author.AuthorId == ID);
            System.Threading.Thread.Sleep(5000);
            return author;
        }
        //Should be async?
        /// <summary>
        /// Makes an Author Follow another follower Followee<\br> 
        /// This is done through the SelfRelecting AuthorAuthor relation.
        /// </summary>
        /// <param name="_AuthorId"></param>
        /// <param name="_FolloweeId"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task AddFollowee(int _AuthorId, int _FolloweeId) {
            //I as a chirp author add Chirp author by "name" to my Folled and add myself  to their followers list
            Author? _Author =  await GetRealAuthorByID(_AuthorId);
            Author? _Followee = await GetRealAuthorByID(_FolloweeId);

            if (_Author != null && _Followee != null)
            { 
                _db.Follows.Add(new Follow 
                {
                    AuthorId = _AuthorId, 
                    FolloweeId = _FolloweeId, 
                    Author = _Author, 
                    Follower = _Followee
                });
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
                    AuthorId = CheepDTO.Author.AuthorId,
                    Author = CheepDTO.Author.Name,
                    Message = CheepDTO.Text,
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

        private static Task<List<AuthorDTO>> GetAllFollowedAuthorsAsync(int AuthorId, ChirpDBContext _dbcontext) {
            return Task.Run(() => GetAllFollowedAuthor(AuthorId, _dbcontext));
        }

        private static List<AuthorDTO> GetAllFollowedAuthor(int AuthorId, ChirpDBContext _dbcontext) 
        {   
            List<AuthorDTO> followed = new List<AuthorDTO>();

            // pull out followed authors from a table not yet existing mapping between follower (foreign key to author) and author (foreign key to author)
            var authorDTOs = _dbcontext.Follows.ToList().Where(f => f.FolloweeId == AuthorId)
                .Select(AuthorDTO => new AuthorDTO 
                {
                    AuthorId = AuthorDTO.AuthorId,
                    Name = AuthorDTO.Author.Name,
                    Email = AuthorDTO.Author.Email
                }
            );

            followed.AddRange(authorDTOs);

            return followed;
        }

        private static Task<List<AuthorDTO>> GetAllFollowersAsync(int AuthorId, ChirpDBContext _dbcontext) {
            return Task.Run(() => GetAllFollowers(AuthorId, _dbcontext));
        }
        private static List<AuthorDTO> GetAllFollowers(int AuthorId, ChirpDBContext _dbcontext) 
        {   
            List<AuthorDTO> followers = new List<AuthorDTO>();

            // pull out followed authors from a table not yet existing mapping between author (foreign key to author) and follower (foreign key to author)
            // pull out followed authors from a table not yet existing mapping between follower (foreign key to author) and author (foreign key to author)
            var authorDTOs = _dbcontext.Follows.ToList().Where(f => f.AuthorId == AuthorId)
                .Select(AuthorDTO => new AuthorDTO 
                {
                    AuthorId = AuthorDTO.FolloweeId,
                    Name = AuthorDTO.Follower.Name,
                    Email = AuthorDTO.Follower.Email
                }
            );

            followers.AddRange(authorDTOs);

            return followers;
        }
    }
}
