namespace Columbus.Welkom.Client.Services.Interfaces
{
    public interface IBaseService<T>
    {
        T? GetStorage(int club, int year);
        void SetStorage(T item, int club, int year);
    }
}