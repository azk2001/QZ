ps -ef | grep "BattleServer.exe 战斗服"| grep -v grep | awk '{print $2}' | xargs -t -i kill -9 {}
ps -ef | grep "OverBattleServer.exe 跨服战斗服"| grep -v grep | awk '{print $2}' | xargs -t -i kill -9 {}
rm -rf bs.log
rm -rf obs.log
mono BattleServer.exe 战斗服 > bs.log&
mono OverBattleServer.exe 跨服战斗服 > obs.log&
