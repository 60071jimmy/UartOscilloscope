﻿using System;                                                                   //	使用System函式庫
using System.Collections.Generic;                                               //	使用System.Collections.Generic函式庫
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UartOscilloscope                                                      //	UartOscilloscope命名空間
{                                                                               //	進入UartOscilloscope命名空間
	class CommandDatabase                                                       //	CommandDatabase類別
	{                                                                           //	進入CommandDatabase類別
		List<CommandClass> CommandSet = new List<CommandClass>();               //	宣告CommandSet
		public CommandDatabase()                                                //	CommandDatabase建構子
		{                                                                       //	進入CommandDatabase建構子
			CommandSet.Add(new CommandClass(                                    //	新增指令
				1,                                                              //	指令編號
				"help",															//	指令名稱
				new System.Threading.Tasks.Task(() =>                           //	建立指令工作
				{                                                               //	進入指令工作內容
					
					
				})));                                                           //	結束指令工作內容
			CommandSet.Add(new CommandClass(                                    //	新增指令
				1,                                                              //	指令編號
				"lscom",                                                        //	指令名稱
				new System.Threading.Tasks.Task(() =>                           //	建立指令工作
				{                                                               //	進入指令工作內容
					UARTConnection UARTConnection1;                             //	宣告UARTConnection1物件
					UARTConnection1 = new UARTConnection(0, false);
					Console.WriteLine(UARTConnection1.GetComportList().ToString());
				})));                                                           //	結束指令工作內容
		}                                                                       //	結束CommandDatabase建構子
		public override string ToString()                                       //	覆寫ToString方法
		{                                                                       //	進入覆寫ToString方法
			string OutputStr = "";												//	宣告輸出字串
			foreach (CommandClass Item in this.CommandSet)						//	以foreach迴圈依序取出指令物件
			{                                                                   //	進入foreach敘述
				OutputStr = OutputStr + Item.ToString() + '\t';					//	生成輸出字串
			}                                                                   //	結束foreach敘述
			return OutputStr;													//	回傳輸出字串
		}                                                                       //	結束覆寫ToString方法
		public string[] GetCommandSet()                                         //	GetCommandSet方法
		{                                                                       //	進入GetCommandSet方法
			string[] OutputStrArray;                                            //	宣告輸出字串陣列
			OutputStrArray = new string[] { };									//	初始化字串陣列
			Array.Resize<string>(ref OutputStrArray, CommandSet.Count);			//	調整字串陣列大小
			for(int Index = 0; Index < CommandSet.Count; Index++)				//	以for迴圈依序將資料填入
			{																	//	進入for迴圈

			}																	//	結束for迴圈
			return OutputStrArray;												//	回傳字串陣列
		}                                                                       //	結束GetCommandSet方法
	}                                                                           //	結束CommandDatabase類別
}                                                                               //	結束UartOscilloscope命名空間
