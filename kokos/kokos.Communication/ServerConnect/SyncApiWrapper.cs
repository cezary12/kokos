﻿using kokos.Abstractions;
using kokos.Communication.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace kokos.Communication.ServerConnect
{
    public class SyncApiWrapper
    {
        private static readonly Server Server = Servers.DEMO;
        private SyncAPIConnector _connector;

        public List<Symbol> SymbolRecords { get; private set; } 

        public static readonly SyncApiWrapper Instance = new SyncApiWrapper();

        private string _userId;
        private SecureString _password;

        private SyncApiWrapper()
        {
        }

        public void Login(string userId, SecureString password)
        {
            _userId = userId;
            _password = password;

            var loginResponse = ConnectToServer();
            ThrowIfNotSuccessful(loginResponse);

            var allSymbolsResponse = APICommandFactory.ExecuteAllSymbolsCommand(_connector, true);
            ThrowIfNotSuccessful(allSymbolsResponse);

            SymbolRecords = allSymbolsResponse.SymbolRecords.Select(x => new Symbol
            {
                Name = x.Symbol,
                CategoryName = x.CategoryName,
                Description = x.Description,
                Ask = x.Ask,
                Bid = x.Bid
            }).ToList();

            //var symbolResponse = APICommandFactory.ExecuteSymbolCommand(_connector, "EURUSD", true);
            //ThrowIfNotSuccessful(symbolResponse);
        }

        private LoginResponse ConnectToServer()
        {
            _connector = new SyncAPIConnector(Server);
            var credentials = new Credentials(_userId, _password.ToInsecureString(), "", "kokos");
            var loginResponse = APICommandFactory.ExecuteLoginCommand(_connector, credentials, true);

            return loginResponse;
        }

        public void Logout()
        {
            var logoutResponse = APICommandFactory.ExecuteLogoutCommand(_connector);
            ThrowIfNotSuccessful(logoutResponse);
        }

        public List<TickData> LoadData(string symbol, DataPeriod dataPeriod, DateTime? startDate = null, DateTime? endDate = null, long? ticks = null)
        {
            return TryExecute(() =>
            {
                var start = startDate == null ? (long?) null : startDate.Value.ToUnixMilliseconds();
                var end = endDate == null ? (long?) null : endDate.Value.ToUnixMilliseconds();
                var periodCode = MapToPeriodCode(dataPeriod);
                var chartRangeInfoRecord = new ChartRangeInfoRecord(symbol, periodCode, start, end, ticks);
                var range = APICommandFactory.ExecuteChartRangeCommand(_connector, chartRangeInfoRecord, true);

                ThrowIfNotSuccessful(range);

                var tickData = range.RateInfos.Convert(range.Digits ?? 1).ToList();

                return tickData;
            });
        }

        private PERIOD_CODE MapToPeriodCode(DataPeriod dataPeriod)
        {
            switch (dataPeriod)
            {
                case DataPeriod.D1: return PERIOD_CODE.PERIOD_D1;
                case DataPeriod.M5: return PERIOD_CODE.PERIOD_M5;
                case DataPeriod.H1: return PERIOD_CODE.PERIOD_H1;
                default: 
                    throw new NotSupportedException("Not supported period type: " + dataPeriod);
            }
        }

        private T TryExecute<T>(Func<T> execute, [CallerMemberName] string callerMemberName = null)
        {
            var retry = 0;
            Exception lastException;
            do
            {
                try
                {
                    return execute();
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    _connector.Dispose();
                    _connector = new SyncAPIConnector(Server);

                    var loginResponse = ConnectToServer();
                    ThrowIfNotSuccessful(loginResponse);
                }

            } while (++retry <= 3);

            throw new Exception("Unable to execute function " + callerMemberName, lastException);
        }

        private void ThrowIfNotSuccessful(BaseResponse response, [CallerMemberName] string callerMemberName = null)
        {
            if (response.Status != true)
                throw new Exception("Error while executing " + callerMemberName);
        }
    }
}