interface IDatabaseRepository<T> //Contains the base methods used later in CSVDatabase
{
    void SaveToFile(T item);

    List<T> ReadFromFile();

}