namespace Chirp.Infrastructure;
public class Author
{
    //As for now, all the variables need to be required since an Author needs to have a name, email and a list of cheeps, even if the list is empty.
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep> Cheeps = new List<Cheep>();
    public List<Follow> Followed { get; set;} = new List<Follow>();
    public List<Follow> Followers { get; set;} = new List<Follow>();
}

public class Follow
{   
    //ovner of the follow Relation
    public int AuthorId { get; set; }
    //The one who is followed
    public int FolloweeId { get; set; }
    //The author who follow me
    public required Author Follower { get; set; }
    //The author who is followed
    public required Author Author { get; set; }
}
