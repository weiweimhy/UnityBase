namespace BaseFramework
{
    public class NewableCreater<T> : ICreater<T> where T : new()
    {
        public T Create()
        {
            return new T();
        }
    }
}