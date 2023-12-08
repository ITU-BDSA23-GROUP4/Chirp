using System.Data.Common;
using System.Net;
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

        public async Task AddAuthor(string name, string email)
        {
            await _db.Authors.AddAsync(new Author { AuthorId = Guid.NewGuid(), Name = name, Cheeps = new List<Cheep>(), Email = email });
            //_db.Add(new Author { Name = name, Cheeps = new List<Cheep>(), Email = email });
            _db.SaveChanges();
        }

        public AuthorDTO GetAuthorByID(Guid ID)
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

        public void RemoveFollowee(string _AuthorName, string _FolloweeName)
        {
            var Author = _db.Authors.Where(a => a.Name == _AuthorName)
                .Include(a => a.Followed)
                .FirstOrDefault();

            var Followee = _db.Authors.Where(a => a.Name == _FolloweeName)
                .Include(a => a.Followers)
                .FirstOrDefault();

            if (Author != null && Followee != null)
            {
                // Author.Followed.Remove(Followee);
                Followee.Followers.Remove(Author);
                _db.SaveChanges();

            }
            else
            {
                throw new NullReferenceException("FollowerRelationship is null");
            }
        }


        public void AddFollowee(string _AuthorName, string _FolloweeName)
        {
            Console.WriteLine("\n\n\nAutorID:" + _AuthorName + "\nAutorID:" + _FolloweeName);

            var Author = _db.Authors.Where(a => a.Name == _AuthorName)
                .Include(a => a.Followed)
                .FirstOrDefault();

            Console.WriteLine("\n\nAuthor\nID: "+Author.AuthorId+"\nName: "+ Author.Name + "\nEmail: "+Author.Email);

            var Followee = _db.Authors.Where(a => a.Name == _FolloweeName)
                .Include(a => a.Followers)
                .FirstOrDefault();

            Console.WriteLine("\n\nFollowee\nID: "+Followee.AuthorId+"\nName: "+ Followee.Name + "\nEmail: "+Followee.Email);

            Author.Followed ??= new List<Author>();
            Followee.Followers ??= new List<Author>();
            
            if (Author != null && Followee != null)
            {
                
                if (Author.Followed.Contains(Followee) || Followee.Followers.Contains(Author))
                {
                    throw new InvalidOperationException("Author: " + Author.Name + "already follows: " + Followee.Name);
                }
                Console.WriteLine("Addign followee");
                Author.Followed.Add(Followee);
                Console.WriteLine("Addign follower");
                Followee.Followers.Add(Author);

                _db.SaveChanges();
            }
            else
            {
                throw new NullReferenceException("Obejct _Author or _Followee of type Author is null");
            }
        }

        private static List<CheepDTO> GetAllCheepsFromAuthor(string Name, ChirpDBContext _dbcontext)
        {

            List<CheepDTO> cheepsToReturn = new List<CheepDTO>();
            try
            {
                var cheepsDTO = _dbcontext.Cheeps.ToList().OrderByDescending(c => c.TimeStamp.Ticks).Where(author => author.Author.Name == Name).Select(CheepDTO => new CheepDTO
                {
                    //Sets the properties of the Cheep
                    CheepId = CheepDTO.Author.AuthorId,
                    AuthorName = CheepDTO.Author.Name,
                    Message = CheepDTO.Text,
                    Timestamp = CheepDTO.TimeStamp
                }
                            );
                cheepsToReturn.AddRange(cheepsDTO);

                return cheepsToReturn;
            }
            catch
            {
                return new List<CheepDTO>();
            }
        }

        private static List<AuthorDTO> GetAllFollowedAuthors(Guid AuthorId, ChirpDBContext DBcontext)
        {
            List<AuthorDTO> followed = new List<AuthorDTO>();

            var Author = DBcontext.Authors.Where(a => a.AuthorId == AuthorId)
                .Include(a => a.Followed)
                .FirstOrDefault();

            if (Author != null)
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

        private static List<AuthorDTO> GetAllFollowers(Guid _AuthorId, ChirpDBContext DBcontext)
        {
            List<AuthorDTO> followers = new List<AuthorDTO>();

            var Authors = DBcontext.Authors.
                Where(a => a.AuthorId == _AuthorId)
                .Include(a => a.Followers)
                .FirstOrDefault();

            if (Authors != null)
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