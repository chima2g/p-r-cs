using System;
using System.Collections;

namespace FirstProject
{
    public class CommissionLogger
    {
        int BASE_COMMISSION = 125; // The standard amount of commission each broker receives for a case
        int BONUS_AMOUNT = 10; //The amount of bonus a broker receives for hitting target

        //Constants used to indicate which bonus structure is being applied
        int BONUS_TYPE_1 = 1;
        int BONUS_TYPE_2 = 2;

        //The amount a case has to be above before a broker can begin to receive bonus for each bonus structure
        int THRESHOLD_AMOUNT_1 = 100000;
        int THRESHOLD_AMOUNT_2 = 250000;

        //The target brokers have to hit in order to receive bonus for each bonus structure
        int TARGET_AMOUNT_1 = 10000;
        int TARGET_AMOUNT_2 = 50000;

        const CURRENCY_LOOKUP = { $: 0.8 }; //Lookup object for converting foreign currencies into GBP

        private ArrayList convertCSVStrToArr (string csvStr) {
            string[] lines = csvStr.Split("\r\n"); //Get each line of the CSV string
            ArrayList dataArr = new ArrayList();

            //Convert each line to an array of values and add the array to the array
            foreach (string line in lines)
            {
                dataArr.Add(line.Split(","));
            }

            return dataArr;
        }


        private int bonusCalculator (string caseValue, decimal threshold, decimal target) {
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
