using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_ObjectsGameplayData")]
public partial class M_ObjectsGameplayData : BGEntityGo
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
			if(_metaDefault==null || _metaDefault.IsDeleted) _metaDefault=BGRepo.I.GetMeta<BansheeGz.BGDatabase.BGMetaRow>(new BGId(4658376294490806110UL,6950373328361257866UL));
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
	public System.Boolean f_ObjectFound
	{
		get
		{
			return _f_ObjectFound[Entity.Index];
		}
		set
		{
			_f_ObjectFound[Entity.Index] = value;
		}
	}
	public System.Boolean f_ObjectReturned
	{
		get
		{
			return _f_ObjectReturned[Entity.Index];
		}
		set
		{
			_f_ObjectReturned[Entity.Index] = value;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName __f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name
	{
		get
		{
			if(__f_name==null || __f_name.IsDeleted) __f_name=(BansheeGz.BGDatabase.BGFieldEntityName) MetaDefault.GetField(new BGId(4704137733006346466UL,6507642195071752856UL));
			return __f_name;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_ObjectFound;
	public static BansheeGz.BGDatabase.BGFieldBool _f_ObjectFound
	{
		get
		{
			if(__f_ObjectFound==null || __f_ObjectFound.IsDeleted) __f_ObjectFound=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(5418712494444133144UL,10507902628022803586UL));
			return __f_ObjectFound;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_ObjectReturned;
	public static BansheeGz.BGDatabase.BGFieldBool _f_ObjectReturned
	{
		get
		{
			if(__f_ObjectReturned==null || __f_ObjectReturned.IsDeleted) __f_ObjectReturned=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(4762361548636797324UL,11593070393711443599UL));
			return __f_ObjectReturned;
		}
	}
}
