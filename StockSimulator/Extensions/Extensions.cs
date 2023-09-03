namespace StockSimulator.Extensions
{
    public static class Extensions
    {
        public static async Task<TV> Then<T, TV>(this Task<T> task, Func<T, TV> then)
        {
            var result = await task;
            return then(result);
        }

        public static async Task Then<T>(this Task<T> task, Action<T> then)
        {
            var result = await task;
            then(result);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        //Create a ForEach method with index
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var counter = 0;
            foreach (var item in enumerable)
            {
                action(item, counter++);
            }
        }
    }
}