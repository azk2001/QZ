using System;
using System.Collections.Generic;
namespace BattleServer
{
    class AccountDataManager
    {
        private static Dictionary<string, AccountData> accountDic = new Dictionary<string, AccountData>();

        public static void AddAccount(AccountData accountData)
        {
            accountDic[accountData.userName] = accountData;
        }

        public static void RemoveAccount(AccountData accountData)
        {
            RemoveAccount(accountData.userName);
        }

        public static void RemoveAccount(string userName)
        {
            if (accountDic.ContainsKey(userName))
            {
                accountDic.Remove(userName);
            }
        }

        public static AccountData GetAccount(string userName)
        {
            if (accountDic.ContainsKey(userName))
            {
                return accountDic[userName];
            }
            return null;
        }
        
    }
}
