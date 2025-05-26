using System;

namespace Solutions {
    public class Leap {
        public static bool IsLeapYear( int year ) {
            bool yearDivisibleBy100 = year % 100 == 0; // Check for divisibility by 100
            bool yearDivisibleBy400 = year % 400 == 0; // Check for divisibility by 400
            bool yearDivisibleBy4 = year % 4 == 0; // Check for divisibility by 4
            if (yearDivisibleBy4)
                if (yearDivisibleBy100 == false)
                    return true;
                else
                    return yearDivisibleBy400 == true;
            return false;
        }
    }
}