using CheepRecord;
using SQLDB;
using Microsoft.Data.Sqlite;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public void AlterPage(int listSize, int change) ;
    public int GetPage();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    DB db = DB.GetInstance();

    private int page = 1;
    public int GetPage(){
        return page;
    }
    public void AlterPage(int listSize, int change){
        if ((page+change)>=1 && (page+change)<=(listSize/32+1)){
            page= page+change;    
        }
        
    }
    public List<CheepViewModel> GetCheeps()
    {      
        List<CheepViewModel> list = new List<CheepViewModel>();

        using (var reader = db.Query(
            @"SELECT U.username, M.text, M.pub_date 
            FROM user U, message M 
            WHERE U.user_id = M.author_id 
            ORDER BY M.pub_date DESC"))
        {
            list = ReaderToCheepViewModelList(reader);
        }

        return list;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        Console.WriteLine(author);
        List<CheepViewModel> list = new List<CheepViewModel>();
        
        using (var reader = db.Query(
            @"SELECT U.username, M.text, M.pub_date
            FROM user U, message M
            WHERE M.author_id = (
                SELECT user_id FROM User
                WHERE username = $author
            ) AND U.username = $author
            ORDER BY M.pub_date DESC"
            , new Dictionary<string, string>()
            {
	            {"$author", author}
            }
        )) {
            list = ReaderToCheepViewModelList(reader);
        }

        return list;
    }

    private static List<CheepViewModel> ReaderToCheepViewModelList(SqliteDataReader reader) {
        List<CheepViewModel> list = new List<CheepViewModel>();
        if (reader != null) {
            while (reader.Read()) {
                list.Add(new CheepViewModel(
                    reader.GetString(0), 
                    reader.GetString(1),
                    UnixTimeStampToDateTimeString(reader.GetDouble(2))
                ));
            }
        }
        return list;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
