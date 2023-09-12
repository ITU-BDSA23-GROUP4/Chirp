interface IDatabaseRepository<T>
{
    void SaveToFile(T item);

    List<T> ReadFromFile();

}