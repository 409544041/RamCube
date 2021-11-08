using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_LevelGameplayData")]
public partial class M_LevelGameplayData : BGEntityGo
{
	public override BGMetaEntity MetaConstraint
	{
		get
		{
			return MetaDefault;
		}
	}
	private static BansheeGz.BGDatabase.BGMetaRow _metaDefault;
	public static BansheeGz.BGDatabase.BGMetaRow MetaDefault
	{
		get
		{
			if(_metaDefault==null || _metaDefault.IsDeleted) _metaDefault=BGRepo.I.GetMeta<BansheeGz.BGDatabase.BGMetaRow>(new BGId(4723279784818553777UL,9330121963571183804UL));
			return _metaDefault;
		}
	}
	public static BansheeGz.BGDatabase.BGRepoEvents Events
	{
		get
		{
			return BGRepo.I.Events;
		}
	}
	public System.String f_name
	{
		get
		{
			return _f_name[Entity.Index];
		}
		set
		{
			_f_name[Entity.Index] = value;
		}
	}
	public E_Pin f_Pin
	{
		get
		{
			return (E_Pin) _f_Pin[Entity.Index];
		}
		set
		{
			_f_Pin[Entity.Index] = value;
		}
	}
	public System.Int32 f_LocksLeft
	{
		get
		{
			return _f_LocksLeft[Entity.Index];
		}
		set
		{
			_f_LocksLeft[Entity.Index] = value;
		}
	}
	public System.Boolean f_DottedAnimPlayed
	{
		get
		{
			return _f_DottedAnimPlayed[Entity.Index];
		}
		set
		{
			_f_DottedAnimPlayed[Entity.Index] = value;
		}
	}
	public System.Boolean f_Unlocked
	{
		get
		{
			return _f_Unlocked[Entity.Index];
		}
		set
		{
			_f_Unlocked[Entity.Index] = value;
		}
	}
	public System.Boolean f_UnlockAnimPlayed
	{
		get
		{
			return _f_UnlockAnimPlayed[Entity.Index];
		}
		set
		{
			_f_UnlockAnimPlayed[Entity.Index] = value;
		}
	}
	public System.Boolean f_Completed
	{
		get
		{
			return _f_Completed[Entity.Index];
		}
		set
		{
			_f_Completed[Entity.Index] = value;
		}
	}
	public System.Boolean f_PathDrawn
	{
		get
		{
			return _f_PathDrawn[Entity.Index];
		}
		set
		{
			_f_PathDrawn[Entity.Index] = value;
		}
	}
	public System.Boolean f_LockIconDisabled
	{
		get
		{
			return _f_LockIconDisabled[Entity.Index];
		}
		set
		{
			_f_LockIconDisabled[Entity.Index] = value;
		}
	}
	public System.Boolean f_wallDown
	{
		get
		{
			return _f_wallDown[Entity.Index];
		}
		set
		{
			_f_wallDown[Entity.Index] = value;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName __f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name
	{
		get
		{
			if(__f_name==null || __f_name.IsDeleted) __f_name=(BansheeGz.BGDatabase.BGFieldEntityName) MetaDefault.GetField(new BGId(5598081807989236421UL,1501239811640041904UL));
			return __f_name;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldRelationSingle __f_Pin;
	public static BansheeGz.BGDatabase.BGFieldRelationSingle _f_Pin
	{
		get
		{
			if(__f_Pin==null || __f_Pin.IsDeleted) __f_Pin=(BansheeGz.BGDatabase.BGFieldRelationSingle) MetaDefault.GetField(new BGId(5611101511638644096UL,7289200097385794695UL));
			return __f_Pin;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldInt __f_LocksLeft;
	public static BansheeGz.BGDatabase.BGFieldInt _f_LocksLeft
	{
		get
		{
			if(__f_LocksLeft==null || __f_LocksLeft.IsDeleted) __f_LocksLeft=(BansheeGz.BGDatabase.BGFieldInt) MetaDefault.GetField(new BGId(5345850841205835300UL,16220438378230291107UL));
			return __f_LocksLeft;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_DottedAnimPlayed;
	public static BansheeGz.BGDatabase.BGFieldBool _f_DottedAnimPlayed
	{
		get
		{
			if(__f_DottedAnimPlayed==null || __f_DottedAnimPlayed.IsDeleted) __f_DottedAnimPlayed=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(5267107265309508871UL,4080065285184299438UL));
			return __f_DottedAnimPlayed;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_Unlocked;
	public static BansheeGz.BGDatabase.BGFieldBool _f_Unlocked
	{
		get
		{
			if(__f_Unlocked==null || __f_Unlocked.IsDeleted) __f_Unlocked=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(4644558649005386762UL,13966429524211724976UL));
			return __f_Unlocked;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_UnlockAnimPlayed;
	public static BansheeGz.BGDatabase.BGFieldBool _f_UnlockAnimPlayed
	{
		get
		{
			if(__f_UnlockAnimPlayed==null || __f_UnlockAnimPlayed.IsDeleted) __f_UnlockAnimPlayed=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(5477728311005227544UL,2327989079783061125UL));
			return __f_UnlockAnimPlayed;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_Completed;
	public static BansheeGz.BGDatabase.BGFieldBool _f_Completed
	{
		get
		{
			if(__f_Completed==null || __f_Completed.IsDeleted) __f_Completed=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(5552126076174291767UL,11347803035636501888UL));
			return __f_Completed;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_PathDrawn;
	public static BansheeGz.BGDatabase.BGFieldBool _f_PathDrawn
	{
		get
		{
			if(__f_PathDrawn==null || __f_PathDrawn.IsDeleted) __f_PathDrawn=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(5562002716607721452UL,9876705488166721464UL));
			return __f_PathDrawn;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_LockIconDisabled;
	public static BansheeGz.BGDatabase.BGFieldBool _f_LockIconDisabled
	{
		get
		{
			if(__f_LockIconDisabled==null || __f_LockIconDisabled.IsDeleted) __f_LockIconDisabled=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(5041402763999311649UL,14533767244693604751UL));
			return __f_LockIconDisabled;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_wallDown;
	public static BansheeGz.BGDatabase.BGFieldBool _f_wallDown
	{
		get
		{
			if(__f_wallDown==null || __f_wallDown.IsDeleted) __f_wallDown=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(5421608937216315096UL,9085621000404421508UL));
			return __f_wallDown;
		}
	}
}