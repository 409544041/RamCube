
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QbismDataSheets{
	//Document URL: https://spreadsheets.google.com/feeds/worksheets/1AEkQ7oaG668N5cdC0A2lIITDqjFWsbqrod7--U14EAI/public/basic?alt=json-in-script

	//Sheet SheetLevelData
	public static QbismDataSheetsTypes.SheetLevelData levelData = new QbismDataSheetsTypes.SheetLevelData();
	static QbismDataSheets(){
		levelData.Init(); 
	}
}


namespace QbismDataSheetsTypes{
	public class LevelData{
		public string pin_ID;
		public int lVL_Index;
		public string lVL_Unlock_1;
		public string lVL_Unlock_2;
		public string level_Name;
		public string lock_Unlock;
		public bool serp_Seg;
		public int pin_Text;

		public LevelData(){}

		public LevelData(string pin_ID, int lVL_Index, string lVL_Unlock_1, string lVL_Unlock_2, string level_Name, string lock_Unlock, bool serp_Seg, int pin_Text){
			this.pin_ID = pin_ID;
			this.lVL_Index = lVL_Index;
			this.lVL_Unlock_1 = lVL_Unlock_1;
			this.lVL_Unlock_2 = lVL_Unlock_2;
			this.level_Name = level_Name;
			this.lock_Unlock = lock_Unlock;
			this.serp_Seg = serp_Seg;
			this.pin_Text = pin_Text;
		}
	}
	public class SheetLevelData: IEnumerable{
		public System.DateTime updated = new System.DateTime(2021,1,20,17,7,54);
		public readonly string[] labels = new string[]{"Pin_ID","LVL_Index","LVL_Unlock_1","LVL_Unlock_2","Level_Name","Lock_Unlock","Serp_Seg","Pin_Text"};
		private LevelData[] _rows = new LevelData[38];
		public void Init() {
			_rows = new LevelData[]{
					new LevelData("a_01",0,"a_02","","Introduction","",true,1),
					new LevelData("a_02",1,"a_03","a_05","TinyZigZag","",false,2),
					new LevelData("a_03",2,"a_04","","SevenStepZigZag","",false,3),
					new LevelData("a_04",3,"","","LaserIntro","",false,4),
					new LevelData("a_05",4,"","","JustTwoLasers","",false,5),
					new LevelData("a_06",5,"","","BoostIntro","",false,6),
					new LevelData("a_07",6,"","","ChooseBoostMoment","",false,7),
					new LevelData("a_08",7,"","","BoostSelection","",false,8),
					new LevelData("a_09",8,"","","DoubleBoostWithChoice","",false,9),
					new LevelData("a_10",9,"","","StaticIntro","",false,10),
					new LevelData("a_11",10,"","","StaticEasy","",false,11),
					new LevelData("a_12",11,"","","BoostToPosition","",false,12),
					new LevelData("a_13",12,"","","SmallStatic","",false,13),
					new LevelData("a_14",13,"","","CircleAroundLasers","",false,14),
					new LevelData("a_15",14,"","","TinySquareWithBoost","",false,15),
					new LevelData("b_01",15,"","","TwoIslands","",false,16),
					new LevelData("b_02",16,"","","FourBoostsOneFake","",false,17),
					new LevelData("b_03",17,"","","TurnIntro","",false,18),
					new LevelData("b_04",18,"","","ChooseTurnPosition","",false,19),
					new LevelData("b_05",19,"","","DoubleTurn","",false,20),
					new LevelData("b_06",20,"","","SquareAroundTurn","",false,21),
					new LevelData("b_07",21,"","","TurnBackTrack","",false,22),
					new LevelData("b_08",22,"","","TurnAndBoost","",false,23),
					new LevelData("b_09",23,"","","BoostAfterTurning","",false,24),
					new LevelData("b_10",24,"","","TurnBoostTurn","",false,25),
					new LevelData("b_11",25,"","","MoveableIntro","",false,26),
					new LevelData("b_12",26,"","","MoveablePushAlong","",false,27),
					new LevelData("b_13",27,"","","MoveableSimpleBridge","",false,28),
					new LevelData("b_14",28,"","","SimpleSelectBridge","",false,29),
					new LevelData("b_15",29,"","","SelectBridgeLocation","",false,30),
					new LevelData("c_01",30,"","","MoveableCreateBlockage","",false,31),
					new LevelData("c_02",31,"","","DoubleCubeBridge","",false,32),
					new LevelData("c_03",32,"","","BoostIntoMoveableEasy","",false,33),
					new LevelData("c_04",33,"","","BoostIntoMoveable","",false,34),
					new LevelData("c_05",34,"","","SlalomMoveableBoosts","",false,35),
					new LevelData("c_06",35,"","","BoostMoveableIntoMoveable","",false,36),
					new LevelData("c_07",36,"","","VeryOpenWithLasers","",false,37),
					new LevelData("c_08",37,"","","MoveableTurnBoostBridge","",false,38)
				};
		}
			
		public IEnumerator GetEnumerator(){
			return new SheetEnumerator(this);
		}
		private class SheetEnumerator : IEnumerator{
			private int idx = -1;
			private SheetLevelData t;
			public SheetEnumerator(SheetLevelData t){
				this.t = t;
			}
			public bool MoveNext(){
				if (idx < t._rows.Length - 1){
					idx++;
					return true;
				}else{
					return false;
				}
			}
			public void Reset(){
				idx = -1;
			}
			public object Current{
				get{
					return t._rows[idx];
				}
			}
		}
		public int Length{ get{ return _rows.Length; } }
		public LevelData this[int index]{
			get{
				return _rows[index];
			}
		}
		public LevelData this[string id]{
			get{
				for (int i = 0; i < _rows.Length; i++) {
					if( _rows[i].pin_ID == id){ return _rows[i]; }
				}
				return null;
			}
		}
		public bool ContainsKey(string key){
			for (int i = 0; i < _rows.Length; i++) {
				if( _rows[i].pin_ID == key){ return true; }
			}
			return false;
		}
		public LevelData[] ToArray(){
			return _rows;
		}
		public LevelData Random() {
			return _rows[ UnityEngine.Random.Range(0, _rows.Length) ];
		}

		public LevelData a_01{	get{ return _rows[0]; } }
		public LevelData a_02{	get{ return _rows[1]; } }
		public LevelData a_03{	get{ return _rows[2]; } }
		public LevelData a_04{	get{ return _rows[3]; } }
		public LevelData a_05{	get{ return _rows[4]; } }
		public LevelData a_06{	get{ return _rows[5]; } }
		public LevelData a_07{	get{ return _rows[6]; } }
		public LevelData a_08{	get{ return _rows[7]; } }
		public LevelData a_09{	get{ return _rows[8]; } }
		public LevelData a_10{	get{ return _rows[9]; } }
		public LevelData a_11{	get{ return _rows[10]; } }
		public LevelData a_12{	get{ return _rows[11]; } }
		public LevelData a_13{	get{ return _rows[12]; } }
		public LevelData a_14{	get{ return _rows[13]; } }
		public LevelData a_15{	get{ return _rows[14]; } }
		public LevelData b_01{	get{ return _rows[15]; } }
		public LevelData b_02{	get{ return _rows[16]; } }
		public LevelData b_03{	get{ return _rows[17]; } }
		public LevelData b_04{	get{ return _rows[18]; } }
		public LevelData b_05{	get{ return _rows[19]; } }
		public LevelData b_06{	get{ return _rows[20]; } }
		public LevelData b_07{	get{ return _rows[21]; } }
		public LevelData b_08{	get{ return _rows[22]; } }
		public LevelData b_09{	get{ return _rows[23]; } }
		public LevelData b_10{	get{ return _rows[24]; } }
		public LevelData b_11{	get{ return _rows[25]; } }
		public LevelData b_12{	get{ return _rows[26]; } }
		public LevelData b_13{	get{ return _rows[27]; } }
		public LevelData b_14{	get{ return _rows[28]; } }
		public LevelData b_15{	get{ return _rows[29]; } }
		public LevelData c_01{	get{ return _rows[30]; } }
		public LevelData c_02{	get{ return _rows[31]; } }
		public LevelData c_03{	get{ return _rows[32]; } }
		public LevelData c_04{	get{ return _rows[33]; } }
		public LevelData c_05{	get{ return _rows[34]; } }
		public LevelData c_06{	get{ return _rows[35]; } }
		public LevelData c_07{	get{ return _rows[36]; } }
		public LevelData c_08{	get{ return _rows[37]; } }

	}
}