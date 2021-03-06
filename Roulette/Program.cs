﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RouletteLogic;
using RouletteLogic.BettingStrategies;
using RouletteLogic.Formatters;

namespace Roulette
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var results = RunGame(100, 1, 20, new MartingaleStrategy());
            PrintResult(results, new ResultTabFormatter());
            Console.Read();
        }
        
        private static IEnumerable<ResultDataItem> RunGame(int bankRoll, int minimumBet, int iterations, RouletteBettingStrategy strategy)
        {
            var resultDataItems = new List<ResultDataItem>(); 
            
            var rouletteTable = new RouletteTable(200);
            Result lastResult = null;

            for (int i = 1; i <= iterations; i++)
            {
                var bet = strategy.DetermineBet(bankRoll,minimumBet, rouletteTable.TableLimit, lastResult);

                rouletteTable.PlaceBet(bet);
                var results = rouletteTable.PlayGames();

                lastResult = results.First();

                bankRoll = Helper.TallyResult(bankRoll, bet, lastResult);
                
                resultDataItems.Add(new ResultDataItem() {Bankroll = bankRoll, Bet = bet, Result = lastResult});
            }
            
            return resultDataItems;
        }

        private static void PrintResult(IEnumerable<ResultDataItem> items, IResultFormatter formatter)
        {
            //header
            Console.WriteLine(formatter.GetHeader());

            //detail
            foreach (var item in items)
            {
                Console.WriteLine(formatter.GetDetail(item));
            }
            
        }
    }
}