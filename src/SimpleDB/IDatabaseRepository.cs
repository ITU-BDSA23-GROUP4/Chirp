interface IDatabaseRepository<T> //Contains the base methods used later in CSVDatabase
{
    abstract static CSVDatabase<T> GetCSVDatabase();
    void SetPath(string path);
    void SaveToFile(T item);

    List<T> ReadFromFile();

}