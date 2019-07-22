using System;

namespace FirstProject
{
    public class CommissionLogger
    {
        int BASE_COMMISSION = 125; // The standard amount of commission each broker receives for a case
        int BONUS_AMOUNT = 10; //The amount of bonus a broker receives for hitting target

        //Constants used to indicate which bonus structure is being applied
        int BONUS_TYPE_1 = 1;
        int BONUS_TYPE_2 = 2;

        private int getBonus (string caseValue, decimal threshold, decimal target) {
            decimal caseValueAsDec = decimal.Parse(caseValue.Substring(1)); //Convert caseValue into float format
            int totalBonus = 0;

            //If the broker has hit the bonus threshold, calculate their bonus
            if (caseValueAsDec > threshold)
                totalBonus =
                  (int) ((caseValueAsDec - threshold) / target) * BONUS_AMOUNT;

            return totalBonus;
        }



        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
