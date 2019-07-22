using System;
using System.Collections;
using System.Collections.Generic;

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

//        const CURRENCY_LOOKUP = { $: 0.8 }; //Lookup object for converting foreign currencies into GBP

        private ArrayList convertCSVStrToArr (string csvStr)
        {
            string[] lines = csvStr.Split("\r\n"); //Get each line of the CSV string
            ArrayList dataArr = new ArrayList();

            //Convert each line to an array of values and add the array to the array
            foreach (string line in lines)
            {
                dataArr.Add(line.Split(","));
            }

            return dataArr;
        }

        string convertArrayToCSVStr ( string[] dataArr)
        {
            //Convert the CSV array data to a string and write it to file
            string csvStr = "";
            
            for (int i = 0; i < dataArr.Length; i++) {
                string csvLine = dataArr[i];
                csvStr += String.Join(",", csvLine);
                if (i != dataArr.Length - 1) csvStr += "\r\n";
            }

            return csvStr;
        }

        string[][] convertCasesToGBP (string[][] brokerCases, string currency, double conversionRate)
        {
            string[][] convertedCases = brokerCases;

            for (int i = 1; i < convertedCases.Length; i++) //Starts at index 1 to ignore header
            {
                string[] _case = convertedCases[i];
                string caseValue = _case[2];
                if (caseValue.StartsWith(currency))
                {
                    double _caseValue = Double.Parse(caseValue.Substring(1));
                    double gbpVal = Math.Round((_caseValue * conversionRate), 2);   //Calculate the converted amount
                    _case[2] = "£" + gbpVal;
                }

                convertedCases[i] = _case;
            }

            return convertedCases;
        }

        string[][] getCommissionData (string[][] brokerCases, int bonusCalculation)
        {
            string[][] gBPBrokerCases = convertCasesToGBP(brokerCases, "$", 0.8);

            string[][] commissionArr = new string[gBPBrokerCases.Length][];

            for (int i = 1; i < gBPBrokerCases.Length; i++) //Starts at index 1 to ignore header
            {
                string[] _case = gBPBrokerCases[i];
                string[] commissionObj = new string[3];

                commissionObj[0] = _case[0];
                commissionObj[1] = _case[1];

                if (bonusCalculation > 0)
                {
                    int bonus = bonusCalculator(_case[2], THRESHOLD_AMOUNT_1, TARGET_AMOUNT_1);

                    //Add the additional bonus on if this is for the second bonus structure
                    if (bonusCalculation == BONUS_TYPE_2)
                        bonus += bonusCalculator(_case[2], THRESHOLD_AMOUNT_2, TARGET_AMOUNT_2);

                    commissionObj[2] = "£" + bonus; //Add the bonus key value pair to the commission object
                }

                commissionArr[i] = commissionObj;
            }

            return commissionArr;
        }

        string[][] getCommissionSummaryData(string[][] brokerCases)
        {
            IDictionary<string, int> summaryObj = new Dictionary<string, int>();  //Lookup object to keep track of each broker's total commission

            string[][] summaryArr = new string[brokerCases.Length][];
//            summaryArr[0] = [["BrokerName", "TotalCommission"];

            for (int i = 1; i < brokerCases.Length; i++) //Starts at index 1 to ignore header
            {
                string[] _case = brokerCases[i];
                string[] commissionObj = new string[2];

                string baseCommission = _case[1]; 
                string bonusCommission = _case[2];

                int currentTotal = 0;
                int result;

                //Get the broker's current total from the lookup object
                if (summaryObj.TryGetValue(_case[0], out result))
                    currentTotal = result;

//                summaryObj[BrokerName] =
  //            currentTotal + Double.Parse(baseCommission.Substring(1)) + Double.Parse(bonusCommission.Substring(1));
                        
            }

            return summaryArr;
        }

        int bonusCalculator (string caseValue, decimal threshold, decimal target)
        {
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
