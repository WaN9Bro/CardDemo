using System;

namespace MyGame
{
    public static class UniqueIDGenerator
    {
        private static long lastTimestamp = DateTimeOffset.MinValue.ToUnixTimeMilliseconds();
        private static int sequence = 0;
        private static object lockObj = new object();

        public static long GenerateUniqueID()
        {
            lock (lockObj)
            {
                long currentTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (currentTimestamp < lastTimestamp)
                {
                    throw new Exception("Clock moved backwards. Refusing to generate id.");
                }

                if (currentTimestamp == lastTimestamp)
                {
                    sequence++;
                    if (sequence > 4095) // 12 bits for sequence number
                    {
                        // Wait until next millisecond
                        while (currentTimestamp <= lastTimestamp)
                        {
                            currentTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        }
                        sequence = 0;
                    }
                }
                else
                {
                    sequence = 0;
                }

                lastTimestamp = currentTimestamp;

                long uniqueId = (currentTimestamp << 12) | sequence;
                return uniqueId;
            }
        }
    }
}