namespace BaseFramework
{
    public class NewableCreaterPool<T> : Pool<T> where T : IRecycleable, new()
    {
        public NewableCreaterPool(int initPoolSize = 0, int maxPoolSize = int.MaxValue)
        {
            creater = new NewableCreater<T>();
            InitPoolSize(initPoolSize, maxPoolSize);
        }
    }
}