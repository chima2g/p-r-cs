using System;
using System.Linq;
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

        int bonusCalculator(string caseValue, decimal threshold, decimal target)
        {
            decimal caseValueAsDec = decimal.Parse(caseValue.Substring(1)); //Convert caseValue into float format
            int totalBonus = 0;

            //If the broker has hit the bonus threshold, calculate their bonus
            if (caseValueAsDec > threshold)
                totalBonus =
                  (int)((caseValueAsDec - threshold) / target) * BONUS_AMOUNT;

            return totalBonus;
        }

        string[][] getCommissionData (string[][] brokerCases, int bonusCalculation)
        {
            string[][] gBPBrokerCases = convertCasesToGBP(brokerCases, "$", 0.8);
            string[][] commissionArr = new string[gBPBrokerCases.Length][];
            commissionArr[0] = new string[] { "BrokerName", "CaseId", "BaseCommission", "BonusCommission" };

            for (int i = 1; i < gBPBrokerCases.Length; i++) //Starts at index 1 to ignore header
            {
                string[] _case = gBPBrokerCases[i];
                string[] commissionObj = new string[4];

                commissionObj[0] = _case[0];
                commissionObj[1] = _case[1];
                commissionObj[2] = "" + BASE_COMMISSION;                

                if (bonusCalculation > 0)
                {
                    int bonus = bonusCalculator(_case[2], THRESHOLD_AMOUNT_1, TARGET_AMOUNT_1);

                    //Add the additional bonus on if this is for the second bonus structure
                    if (bonusCalculation == BONUS_TYPE_2)
                        bonus += bonusCalculator(_case[2], THRESHOLD_AMOUNT_2, TARGET_AMOUNT_2);

                    commissionObj[3] = "£" + bonus; //Add the bonus key value pair to the commission object
                }

                commissionArr[i] = commissionObj;
            }

            return commissionArr;
        }

        string[][] getCommissionSummaryData(string[][] brokerCases)
        {
            Dictionary<string, double> summaryObj = new Dictionary<string, double>();  //Lookup object to keep track of each broker's total commission

            for (int i = 1; i < brokerCases.Length; i++) //Starts at index 1 to ignore header
            {
                string[] _case = brokerCases[i];

                string brokerName = _case[0];
                string baseCommission = _case[2]; 
                string bonusCommission = _case[3];

                double currentTotal = 0;
                double result;

                //Get the broker's current total from the lookup object
                if (summaryObj.TryGetValue(brokerName, out result))
                    currentTotal = result;

                currentTotal += Double.Parse(baseCommission.Substring(1)) + Double.Parse(bonusCommission.Substring(1));

                if(summaryObj.ContainsKey(brokerName))
                    summaryObj.Remove(brokerName);

                summaryObj.Add(brokerName, currentTotal);
            }

            string[][] summaryArr = new string[summaryObj.Keys.Count + 1][];
            summaryArr[0] = new string[] { "BrokerName", "TotalCommission" };

            for (int i = 0; i < summaryObj.Count; i++)
            {
                string[] summary = new string[2];
                summary[0] = summaryObj.Keys.ElementAt(i);
                summary[1] = "£" + summaryObj[summaryObj.Keys.ElementAt(i)];

                summaryArr[i + 1] = summary;
            }

            return summaryArr;
        }

        static void Main(string[] args)
        {
            string[][] inputCaseA = new string[2][];
            inputCaseA [0] = new string[] { "BrokerName", "CaseId", "CaseValue" };
            inputCaseA [1] = new string[] { "David", "2", "£607947.84" };

            CommissionLogger commissioner = new CommissionLogger();

            string[][] output = commissioner.getCommissionData(inputCaseA, 1);

            for (int i = 0; i < output.Length; i++)
            {
                for (int j = 0; j < output[i].Length; j++)
                {
                    Console.WriteLine(output[i][j]);
                }
            }

            Console.WriteLine("");

            string[][] caseData = new string[3][];
            caseData[0] = new string[] { "BrokerName", "CaseId", "BaseCommission", "BonusCommission" };
            caseData[1] = new string[] { "Emmá", "12", "£125", "£0" };
            caseData[2] = new string[] { "Emmá", "12", "£125", "£60" };

            output = commissioner.getCommissionSummaryData(caseData);

            for (int i = 0; i < output.Length; i++)
            {
                for (int j = 0; j < output[i].Length; j++)
                {
                    Console.WriteLine(output[i][j]);
                }
            }
        }
    }
}
