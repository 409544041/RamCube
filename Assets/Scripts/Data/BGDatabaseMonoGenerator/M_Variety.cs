using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_Variety")]
public partial class M_Variety : BGEntityGo
{
	public override BGMetaEntity MetaConstraint
	{
		get
		{
			return MetaDefault;
		}
	}
	private static BansheeGz.BGDatabase.BGMetaNested _metaDefault;
	public static BansheeGz.BGDatabase.BGMetaNested MetaDefault
	{
		get
		{
			if(_metaDefault==null || _metaDefault.IsDeleted) _metaDefault=BGRepo.I.GetMeta<BansheeGz.BGDatabase.BGMetaNested>(new BGId(5737747710488173588UL,4786942112672858515UL));
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
	public E_Biome f_Biome
	{
		get
		{
			return (E_Biome) _f_Biome[Entity.Index];
		}
		set
		{
			_f_Biome[Entity.Index] = value;
		}
	}
	public UnityEngine.Object f_Object
	{
		get
		{
			return _f_Object[Entity.Index];
		}
	}
	public System.Collections.Generic.List<UnityEngine.Color> f_Color
	{
		get
		{
			return _f_Color[Entity.Index];
		}
		set
		{
			_f_Color[Entity.Index] = value;
		}
	}
	public System.Collections.Generic.List<UnityEngine.Color> f_ColorBG
	{
		get
		{
			return _f_ColorBG[Entity.Index];
		}
		set
		{
			_f_ColorBG[Entity.Index] = value;
		}
	}
	public System.Collections.Generic.List<UnityEngine.Color> f_ColorBG2
	{
		get
		{
			return _f_ColorBG2[Entity.Index];
		}
		set
		{
			_f_ColorBG2[Entity.Index] = value;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName __f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name
	{
		get
		{
			if(__f_name==null || __f_name.IsDeleted) __f_name=(BansheeGz.BGDatabase.BGFieldEntityName) MetaDefault.GetField(new BGId(5613765636979208794UL,4247337308855429289UL));
			return __f_name;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldRelationSingle __f_Biome;
	public static BansheeGz.BGDatabase.BGFieldRelationSingle _f_Biome
	{
		get
		{
			if(__f_Biome==null || __f_Biome.IsDeleted) __f_Biome=(BansheeGz.BGDatabase.BGFieldRelationSingle) MetaDefault.GetField(new BGId(4661050294717155902UL,2460429868659110058UL));
			return __f_Biome;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldUnityObject __f_Object;
	public static BansheeGz.BGDatabase.BGFieldUnityObject _f_Object
	{
		get
		{
			if(__f_Object==null || __f_Object.IsDeleted) __f_Object=(BansheeGz.BGDatabase.BGFieldUnityObject) MetaDefault.GetField(new BGId(4969397191281070135UL,512279267751597211UL));
			return __f_Object;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldListColor __f_Color;
	public static BansheeGz.BGDatabase.BGFieldListColor _f_Color
	{
		get
		{
			if(__f_Color==null || __f_Color.IsDeleted) __f_Color=(BansheeGz.BGDatabase.BGFieldListColor) MetaDefault.GetField(new BGId(5649979122024210993UL,10828038162112475050UL));
			return __f_Color;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldListColor __f_ColorBG;
	public static BansheeGz.BGDatabase.BGFieldListColor _f_ColorBG
	{
		get
		{
			if(__f_ColorBG==null || __f_ColorBG.IsDeleted) __f_ColorBG=(BansheeGz.BGDatabase.BGFieldListColor) MetaDefault.GetField(new BGId(5651051185295949394UL,13109017916707061413UL));
			return __f_ColorBG;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldListColor __f_ColorBG2;
	public static BansheeGz.BGDatabase.BGFieldListColor _f_ColorBG2
	{
		get
		{
			if(__f_ColorBG2==null || __f_ColorBG2.IsDeleted) __f_ColorBG2=(BansheeGz.BGDatabase.BGFieldListColor) MetaDefault.GetField(new BGId(5009535309938981592UL,2136018990551611826UL));
			return __f_ColorBG2;
		}
	}
}