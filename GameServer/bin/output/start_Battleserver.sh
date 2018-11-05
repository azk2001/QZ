ps -ef | grep "BattleServer.exe 战斗服"| grep -v grep | awk '{print $2}' | xargs -t -i kill -9 {}
rm -rf bs.log
mono BattleServer.exe 战斗服 > bs.log&
