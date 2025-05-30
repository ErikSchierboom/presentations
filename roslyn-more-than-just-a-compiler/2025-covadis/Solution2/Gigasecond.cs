using System;

namespace Solution2 {
    public static class Gigasecond {
        public static DateTime Add(DateTime birthDate) {
            ;
            DateTime returnValue=birthDate.AddSeconds(1_000_000_000);
            return returnValue;
        }
    }
}
