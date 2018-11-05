using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayIDCollect
{
    public const int ad_login = 1;
    /// <summary>
    /// 背景音乐
    /// </summary>
    public const int ad_backMusic = 2;
    /// <summary>
    /// 普攻放泡泡
    /// </summary>
    public const int ad_attck = 201;
    /// <summary>
    /// 技能跳跃
    /// </summary>
    public const int ad_jump = 202;
    /// <summary>
    /// 技能踢球
    /// </summary>
    public const int ad_kick = 203;
    /// <summary>
    /// 技能无敌
    /// </summary>
    public const int ad_invincible = 204;
    /// <summary>
    /// 爆炸声音
    /// </summary>
    public const int ad_bomb = 205;
    /// <summary>
    /// 死亡
    /// </summary>
    public const int ad_death = 206;
    /// <summary>
    /// 救人/被救/自救
    /// </summary>
    public const int ad_rescue = 207;
    /// <summary>
    /// 点击确定
    /// </summary>
    public const int ad_click_OK = 401;
    /// <summary>
    /// 点击音
    /// </summary>
    public const int ad_click = 402;
    /// <summary>
    /// 飞机飞过的声
    /// </summary>
    public const int ad_aircraft = 403;
    /// <summary>
    /// 加入游戏音
    /// </summary>
    public const int ad_join = 404;
    /// <summary>
    /// 捡到道具声
    /// </summary>
    public const int ad_pickUp = 405;
    /// <summary>
    /// 结束倒计时
    /// </summary>
    public const int ad_cutDownTimer = 406;
    /// <summary>
    /// 开始倒计时
    /// </summary>
    public const int ad_beginTimer = 407;
    /// <summary>
    /// 界面点开声
    /// </summary>
    public const int ad_UIOpen = 408;
    /// <summary>
    /// 胜利or失败
    /// </summary>
    public static int[] ad_battleResult =
    {
        409,410,-1
    };

    /// <summary>
    /// 开始倒计时结束后游戏开始音
    /// </summary>
    public const int ad_start = 411;


    /// <summary>
    /// 战斗背景音乐
    /// </summary>
    public static int[] ad_battleMusic =
    {
        3,4,5,6
    };

	/// <summary>
	/// 说话
	/// </summary>
	public static int[] ad_SpeakList =
	{
		412,413,414
	};

}
