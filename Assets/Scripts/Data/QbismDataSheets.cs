
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QbismDataSheets{
	//Document URL: https://spreadsheets.google.com/feeds/worksheets/1AEkQ7oaG668N5cdC0A2lIITDqjFWsbqrod7--U14EAI/public/basic?alt=json-in-script

	//Sheet SheetWorld_Map_Data
	public static QbismDataSheetsTypes.SheetWorld_Map_Data world_Map_Data = new QbismDataSheetsTypes.SheetWorld_Map_Data();
	static QbismDataSheets(){
		world_Map_Data.Init(); 
	}
}


namespace QbismDataSheetsTypes{
	public class World_Map_Data{
		public string pin_ID;
		public int lVL_Index;
		public string lVL_Unlock_1;
		public string lVL_Unlock_2;
		public string level_Name;
		public string lock_Unlock;
		public bool serp_Seg;
		public int pin_Text;

		public World_Map_Data(){}

		public World_Map_Data(string pin_ID, int lVL_Index, string lVL_Unlock_1, string lVL_Unlock_2, string level_Name, string lock_Unlock, bool serp_Seg, int pin_Text){
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
	public class SheetWorld_Map_Data: IEnumerable{
		public System.DateTime updated = new System.DateTime(2021,1,20,15,9,6);
		public readonly string[] labels = new string[]{"Pin_ID","LVL_Index","LVL_Unlock_1","LVL_Unlock_2","Level_Name","Lock_Unlock","Serp_Seg","Pin_Text"};
		private World_Map_Data[] _rows = new World_Map_Data[38];
		public void Init() {
			_rows = new World_Map_Data[]{
					new World_Map_Data("a_01",0,"a_02","","Introduction","",true,1),
					new World_Map_Data("a_02",1,"a_03","a_05","TinyZigZag","",false,2),
					new World_Map_Data("a_03",2,"a_04","","SevenStepZigZag","",false,3),
					new World_Map_Data("a_04",3,"","","LaserIntro","",false,0),
					new World_Map_Data("a_05",4,"","","JustTwoLasers","",false,0),
					new World_Map_Data("a_06",5,"","","BoostIntro","",false,0),
					new World_Map_Data("a_07",6,"","","ChooseBoostMoment","",false,0),
					new World_Map_Data("a_08",7,"","","BoostSelection","",false,0),
					new World_Map_Data("a_09",8,"","","DoubleBoostWithChoice","",false,0),
					new World_Map_Data("a_10",9,"","","StaticIntro","",false,0),
					new World_Map_Data("a_11",10,"","","StaticEasy","",false,0),
					new World_Map_Data("a_12",11,"","","BoostToPosition","",false,0),
					new World_Map_Data("a_13",12,"","","SmallStatic","",false,0),
					new World_Map_Data("a_14",13,"","","CircleAroundLasers","",false,0),
					new World_Map_Data("a_15",14,"","","TinySquareWithBoost","",false,0),
					new World_Map_Data("b_01",15,"","","TwoIslands","",false,0),
					new World_Map_Data("b_02",16,"","","FourBoostsOneFake","",false,0),
					new World_Map_Data("b_03",17,"","","TurnIntro","",false,0),
					new World_Map_Data("b_04",18,"","","ChooseTurnPosition","",false,0),
					new World_Map_Data("b_05",19,"","","DoubleTurn","",false,0),
					new World_Map_Data("b_06",20,"","","SquareAroundTurn","",false,0),
					new World_Map_Data("b_07",21,"","","TurnBackTrack","",false,0),
					new World_Map_Data("b_08",22,"","","TurnAndBoost","",false,0),
					new World_Map_Data("b_09",23,"","","BoostAfterTurning","",false,0),
					new World_Map_Data("b_10",24,"","","TurnBoostTurn","",false,0),
					new World_Map_Data("b_11",25,"","","MoveableIntro","",false,0),
					new World_Map_Data("b_12",26,"","","MoveablePushAlong","",false,0),
					new World_Map_Data("b_13",27,"","","MoveableSimpleBridge","",false,0),
					new World_Map_Data("b_14",28,"","","SimpleSelectBridge","",false,0),
					new World_Map_Data("b_15",29,"","","SelectBridgeLocation","",false,0),
					new World_Map_Data("c_01",30,"","","MoveableCreateBlockage","",false,0),
					new World_Map_Data("c_02",31,"","","DoubleCubeBridge","",false,0),
					new World_Map_Data("c_03",32,"","","BoostIntoMoveableEasy","",false,0),
					new World_Map_Data("c_04",33,"","","BoostIntoMoveable","",false,0),
					new World_Map_Data("c_05",34,"","","SlalomMoveableBoosts","",false,0),
					new World_Map_Data("c_06",35,"","","BoostMoveableIntoMoveable","",false,0),
					new World_Map_Data("c_07",36,"","","VeryOpenWithLasers","",false,0),
					new World_Map_Data("c_08",37,"","","MoveableTurnBoostBridge","",false,0)
				};
		}
			
		public IEnumerator GetEnumerator(){
			return new SheetEnumerator(this);
		}
		private class SheetEnumerator : IEnumerator{
			private int idx = -1;
			private SheetWorld_Map_Data t;
			public SheetEnumerator(SheetWorld_Map_Data t){
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
		public World_Map_Data this[int index]{
			get{
				return _rows[index];
			}
		}
		public World_Map_Data this[string id]{
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
		public World_Map_Data[] ToArray(){
			return _rows;
		}
		public World_Map_Data Random() {
			return _rows[ UnityEngine.Random.Range(0, _rows.Length) ];
		}

		public World_Map_Data a_01{	get{ return _rows[0]; } }
		public World_Map_Data a_02{	get{ return _rows[1]; } }
		public World_Map_Data a_03{	get{ return _rows[2]; } }
		public World_Map_Data a_04{	get{ return _rows[3]; } }
		public World_Map_Data a_05{	get{ return _rows[4]; } }
		public World_Map_Data a_06{	get{ return _rows[5]; } }
		public World_Map_Data a_07{	get{ return _rows[6]; } }
		public World_Map_Data a_08{	get{ return _rows[7]; } }
		public World_Map_Data a_09{	get{ return _rows[8]; } }
		public World_Map_Data a_10{	get{ return _rows[9]; } }
		public World_Map_Data a_11{	get{ return _rows[10]; } }
		public World_Map_Data a_12{	get{ return _rows[11]; } }
		public World_Map_Data a_13{	get{ return _rows[12]; } }
		public World_Map_Data a_14{	get{ return _rows[13]; } }
		public World_Map_Data a_15{	get{ return _rows[14]; } }
		public World_Map_Data b_01{	get{ return _rows[15]; } }
		public World_Map_Data b_02{	get{ return _rows[16]; } }
		public World_Map_Data b_03{	get{ return _rows[17]; } }
		public World_Map_Data b_04{	get{ return _rows[18]; } }
		public World_Map_Data b_05{	get{ return _rows[19]; } }
		public World_Map_Data b_06{	get{ return _rows[20]; } }
		public World_Map_Data b_07{	get{ return _rows[21]; } }
		public World_Map_Data b_08{	get{ return _rows[22]; } }
		public World_Map_Data b_09{	get{ return _rows[23]; } }
		public World_Map_Data b_10{	get{ return _rows[24]; } }
		public World_Map_Data b_11{	get{ return _rows[25]; } }
		public World_Map_Data b_12{	get{ return _rows[26]; } }
		public World_Map_Data b_13{	get{ return _rows[27]; } }
		public World_Map_Data b_14{	get{ return _rows[28]; } }
		public World_Map_Data b_15{	get{ return _rows[29]; } }
		public World_Map_Data c_01{	get{ return _rows[30]; } }
		public World_Map_Data c_02{	get{ return _rows[31]; } }
		public World_Map_Data c_03{	get{ return _rows[32]; } }
		public World_Map_Data c_04{	get{ return _rows[33]; } }
		public World_Map_Data c_05{	get{ return _rows[34]; } }
		public World_Map_Data c_06{	get{ return _rows[35]; } }
		public World_Map_Data c_07{	get{ return _rows[36]; } }
		public World_Map_Data c_08{	get{ return _rows[37]; } }

	}
}