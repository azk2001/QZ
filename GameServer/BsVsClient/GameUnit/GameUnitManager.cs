using System.Collections.Generic;

namespace GameServer
{
    public class GameUnitManager
    {
        private static Dictionary<int, GameUnit> gameUnitList = new Dictionary<int, GameUnit>();
        private static Dictionary<int, List<GameUnit>> roomAllGameUnit = new Dictionary<int, List<GameUnit>>();

        public static GameUnit AddGameUnit(int uuid, int roomIndex)
        {
            GameUnit gameUnit = null;
            if (gameUnitList.ContainsKey(uuid) == false)
            {
                gameUnit = new GameUnit(uuid, roomIndex);
                gameUnitList[uuid] = gameUnit;
            }
            else
            {
                gameUnit = gameUnitList[uuid];
            }

            if (roomAllGameUnit.ContainsKey(roomIndex) == false)
            {
                roomAllGameUnit[roomIndex] = new List<GameUnit>();
            }

            roomAllGameUnit[roomIndex].Add(gameUnit);

            return gameUnit;
        }

        public static GameUnit GetGameUnit(int uuid)
        {
            GameUnit gameUnit = null;
            if (gameUnitList.ContainsKey(uuid) == true)
            {
                gameUnit = gameUnitList[uuid];
            }

            return gameUnit;
        }

        public static List<GameUnit> GetGameUnitList()
        {
            return new List<GameUnit>(gameUnitList.Values);
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
                    RemoveGameUnit(roomGameUnitList[i].uuid);
                }
            }

            roomAllGameUnit.Remove(roomIndex);
        }

        public static void RemoveGameUnit(int uuid)
        {
            GameUnit gameUnit = GetGameUnit(uuid);

            if (gameUnit != null)
            {
                gameUnitList.Remove(uuid);

                List<GameUnit> roomGameUnitList = GetRoomGameUnitList(gameUnit.roomIndex);

                for (int i = roomGameUnitList.Count - 1; i >= 0; i--)
                {
                    if (roomGameUnitList[i].uuid == uuid)
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
