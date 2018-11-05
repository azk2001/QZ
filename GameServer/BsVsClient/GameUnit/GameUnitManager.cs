using System.Collections.Generic;

namespace BattleServer
{
    public class GameUnitManager
    {
        private static List<GameUnit> gameUnitList = new List<GameUnit>();
        private static Dictionary<int, List<GameUnit>> roomAllGameUnit = new Dictionary<int, List<GameUnit>>();

        public static GameUnit AddGameUnit(GameUnit gameUnit,int roomIndex)
        {
            if (gameUnitList.Contains(gameUnit) == false)
            {
                gameUnitList.Add(gameUnit);
            }

            if(roomAllGameUnit.ContainsKey(roomIndex) ==false)
            {
                roomAllGameUnit[roomIndex] = new List<GameUnit>();
            }

            roomAllGameUnit[roomIndex].Add(gameUnit);

            return gameUnit;
        }

        public static GameUnit GetGameUnit(int uid)
        {
            foreach (GameUnit gameUnit in gameUnitList)
            {
                if (gameUnit.uid == uid)
                    return gameUnit;
            }

            return null;
        }

        public static List<GameUnit> GetGameUnitList()
        {
            return gameUnitList;
        }

        public static List<GameUnit> GetRoomGameUnitList(int roomIndex)
        {
            if (roomAllGameUnit.ContainsKey(roomIndex) == true)
            {
                return roomAllGameUnit[roomIndex];
            }
            return new List<GameUnit>();
        }

        public static void RemoveRoomAllGameUnit(int roomIndex)
        {
            List<GameUnit> roomGameUnitList = GetRoomGameUnitList(roomIndex);

            for (int i = roomGameUnitList.Count - 1; i >= 0; i--)
            {
                if (roomGameUnitList.Count > 0)
                {
                    RemoveGameUnit(roomGameUnitList[i].uid);
                }
            }

            roomAllGameUnit.Remove(roomIndex);
        }

        public static void RemoveGameUnit(int uid)
        {
            GameUnit gameUnit = GetGameUnit(uid);

            if (gameUnit!=null)
            {
                gameUnitList.Remove(gameUnit);

                List<GameUnit> roomGameUnitList = GetRoomGameUnitList(gameUnit.roomIndex);

                for (int i = roomGameUnitList.Count - 1; i >=0; i--)
                {
                    if(roomGameUnitList[i].uid == uid)
                    {
                        roomGameUnitList.Remove(roomGameUnitList[i]);
                        break;
                    }
                }
            }
        }

        public static void RemoveAllGameUnit()
        {
            gameUnitList.Clear();

            roomAllGameUnit.Clear();
        }

    }
}
