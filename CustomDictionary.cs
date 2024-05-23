namespace LogicalSolver
{
    public class CustomDictionary <TValue>
    {
        private List<string> keys = new List<string>();
        private List<TValue> values = new List<TValue>();

        public void Add(string key, TValue value)
        {
            keys.Add(key);
            values.Add(value);
        }
        public bool ContainsKey(string key) => keys.Contains(key);

        public TValue Get(string key)
        {
            int keyIndex = 0;
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i] == key)
                {
                    keyIndex = i;
                    break;
                }
            }

            return values[keyIndex];

        }
    }
}
