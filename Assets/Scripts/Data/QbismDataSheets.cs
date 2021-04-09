
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
		public string biome;
		public int lVL_Index;
		public string lVL_Unlock_1;
		public string lVL_Unlock_2;
		public string level_Name;
		public int locks;
		public bool serp_Seg;
		public int pin_Text;

		public LevelData(){}

		public LevelData(string pin_ID, string biome, int lVL_Index, string lVL_Unlock_1, string lVL_Unlock_2, string level_Name, int locks, bool serp_Seg, int pin_Text){
			this.pin_ID = pin_ID;
			this.biome = biome;
			this.lVL_Index = lVL_Index;
			this.lVL_Unlock_1 = lVL_Unlock_1;
			this.lVL_Unlock_2 = lVL_Unlock_2;
			this.level_Name = level_Name;
			this.locks = locks;
			this.serp_Seg = serp_Seg;
			this.pin_Text = pin_Text;
		}
	}
	public class SheetLevelData: IEnumerable{
		public System.DateTime updated = new System.DateTime(2021,4,9,9,14,52);
		public readonly string[] labels = new string[]{"Pin_ID","Biome","LVL_Index","LVL_Unlock_1","LVL_Unlock_2","Level_Name","Locks","Serp_Seg","Pin_Text"};
		private LevelData[] _rows = new LevelData[39];
		public void Init() {
			_rows = new LevelData[]{
					new LevelData("a_01","Biome01",3,"a_02","","Introduction",0,true,1),
					new LevelData("a_02","Biome01",4,"a_03","a_05","TinyZigZag",0,false,2),
					new LevelData("a_03","Biome01",5,"a_04","b_01","SevenStepZigZag",0,false,3),
					new LevelData("a_04","Biome01",6,"a_05","","LaserIntro",0,false,4),
					new LevelData("a_05","Biome01",7,"a_06","","JustTwoLasers",0,false,5),
					new LevelData("a_06","Biome01",8,"b_01","","BoostIntro",0,false,6),
					new LevelData("a_07","Biome01",9,"","","ChooseBoostMoment",0,false,7),
					new LevelData("a_08","Biome01",10,"","","BoostSelection",0,false,8),
					new LevelData("a_09","Biome01",11,"","","DoubleBoostWithChoice",0,false,9),
					new LevelData("a_10","Biome01",12,"","","StaticIntro",0,false,10),
					new LevelData("a_11","Biome01",13,"","","StaticEasy",0,false,11),
					new LevelData("a_12","Biome01",14,"","","BoostToPosition",0,false,12),
					new LevelData("a_13","Biome01",15,"","","SmallStatic",0,false,13),
					new LevelData("a_14","Biome01",16,"","","CircleAroundLasers",0,false,14),
					new LevelData("a_15","Biome01",17,"","","TinySquareWithBoost",0,false,15),
					new LevelData("b_01","Biome02",18,"b_02","","TwoIslands",2,false,16),
					new LevelData("b_02","Biome02",19,"","","FourBoostsOneFake",0,false,17),
					new LevelData("b_03","Biome02",20,"","","TurnIntro",0,false,18),
					new LevelData("b_04","Biome02",21,"","","ChooseTurnPosition",0,false,19),
					new LevelData("b_05","Biome02",22,"","","DoubleTurn",0,false,20),
					new LevelData("b_06","Biome02",23,"","","SquareAroundTurn",0,false,21),
					new LevelData("b_07","Biome02",24,"","","TurnBackTrack",0,false,22),
					new LevelData("b_08","Biome02",25,"","","TurnAndBoost",0,false,23),
					new LevelData("b_09","Biome02",26,"","","BoostAfterTurning",0,false,24),
					new LevelData("b_10","Biome02",27,"","","TurnBoostTurn",0,false,25),
					new LevelData("b_11","Biome02",28,"","","MoveableIntro",0,false,26),
					new LevelData("b_12","Biome02",29,"","","MoveablePushAlong",0,false,27),
					new LevelData("b_13","Biome02",30,"","","MoveableSimpleBridge",0,false,28),
					new LevelData("b_14","Biome02",31,"","","SimpleSelectBridge",0,false,29),
					new LevelData("b_15","Biome02",32,"","","SelectBridgeLocation",0,false,30),
					new LevelData("c_01","Biome03",33,"","","MoveableCreateBlockage",0,false,31),
					new LevelData("c_02","Biome03",34,"","","DoubleCubeBridge",0,false,32),
					new LevelData("c_03","Biome03",35,"","","BoostIntoMoveableEasy",0,false,33),
					new LevelData("c_04","Biome03",36,"","","BoostIntoMoveable",0,false,34),
					new LevelData("c_05","Biome03",37,"","","SlalomMoveableBoosts",0,false,35),
					new LevelData("c_06","Biome03",38,"","","BoostMoveableIntoMoveable",0,false,36),
					new LevelData("c_07","Biome03",39,"","","VeryOpenWithLasers",0,false,37),
					new LevelData("c_08","Biome03",40,"","","MoveableTurnBoostBridge",0,false,38),
					new LevelData("empty","",0,"","","",0,false,0)
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
		public LevelData empty{	get{ return _rows[38]; } }

	}
}