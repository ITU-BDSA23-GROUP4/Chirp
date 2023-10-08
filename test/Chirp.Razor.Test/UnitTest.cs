public class UnitTest
{
    [Fact]
    public void TestQuery() {
        string exptected = "0 10 They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me. 1690895677";
        string actual = "";
        DB db = DB.GetInstance();
        
        using (var reader = db.Query(
            "SELECT * FROM message LIMIT 1"
        )) {
            if (reader != null) {
                while(reader.Read()) {
                    actual = reader.GetString(0) + " " +
                        reader.GetString(1) + " " +
                        reader.GetString(2) + " " +
                        reader.GetString(3);
                }
            }
        }

        Assert.Equal(exptected, actual);

    }

    [Fact]
    public void TestUnixTimeStampToDateTimeString()
    {
        string expected = "05/05/01 0:00:00";

        Assert.Equal(expected, CheepService.UnixTimeStampToDateTimeString(989020800));
    }
}