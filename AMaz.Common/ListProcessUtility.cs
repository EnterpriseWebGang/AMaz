using System.Linq;
using System.Xml.Linq;
namespace AMaz.Common
{
    public static class ListProcessUtility<T> where T : class
    {
        public static async Task ProcessListByBatchAsync(List<T> collection, int batchSize, Func<List<T>, Task> callBack) //the callback will take the collection and the batch size to process
        {
            for (int i = 0; i < collection.Count; i += batchSize)
            {
                // Get the current batch of elements
                List<T> batch = collection.GetRange(i, Math.Min(batchSize, collection.Count - i));

                // Call the callback function with the current batch
                await callBack(batch);
            }
        }
    }
}
